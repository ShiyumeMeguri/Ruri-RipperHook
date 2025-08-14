﻿using AssetRipper.IO.Files.BundleFiles;
using AssetRipper.IO.Files.BundleFiles.FileStream;
using AssetRipper.IO.Files.Streams;
using AssetRipper.IO.Files.Streams.Smart;
using System.Buffers;
using System.Reflection;

namespace Ruri.RipperHook.HookUtils.BundleFileBlockReaderHook;

public class BundleFileBlockReaderHook : CommonHook
{
    private const string TYPE = "AssetRipper.IO.Files.BundleFiles.FileStream.BundleFileBlockReader, AssetRipper.IO.Files";

    private static readonly MethodInfo CreateStream = Type.GetType(TYPE).GetMethod("CreateStream", ReflectionExtensions.PrivateStaticBindFlag());
    private static readonly MethodInfo CreateTemporaryStream = Type.GetType(TYPE).GetMethod("CreateTemporaryStream", ReflectionExtensions.PrivateStaticBindFlag());
    public delegate void BlockCompressionDelegate(FileStreamNode entry, Stream mStream, StorageBlock block, SmartStream cachedBlockStream, CompressionType compressType, int m_cachedBlockIndex);

    /// <summary>
    /// 针对StorageBlock压缩加密的回调 解决解压错误不支持的类型 5 这种错误的自定义解压处理
    /// </summary>
    public static BlockCompressionDelegate CustomBlockCompression;

    [RetargetMethod(TYPE, nameof(ReadEntry))]
    public SmartStream ReadEntry(FileStreamNode entry)
    {
        var type = Type.GetType(TYPE);
        var m_blocksInfo = (BlocksInfo)GetPrivateField(type, "m_blocksInfo");
        var m_dataOffset = (long)GetPrivateField(type, "m_dataOffset");
        var m_stream = (Stream)GetPrivateField(type, "m_stream");
        var m_cachedBlockIndex = (int)GetPrivateField(type, "m_cachedBlockIndex");
        var m_cachedBlockStream = (SmartStream)GetPrivateField(type, "m_cachedBlockStream");

        if ((bool)GetPrivateField(type, "m_isDisposed"))
        {
            throw new ObjectDisposedException(nameof(type));
        }

        // find block offsets
        int blockIndex;
        long blockCompressedOffset = 0;
        long blockDecompressedOffset = 0;
        for (blockIndex = 0; blockDecompressedOffset + m_blocksInfo.StorageBlocks[blockIndex].UncompressedSize <= entry.Offset; blockIndex++)
        {
            blockCompressedOffset += m_blocksInfo.StorageBlocks[blockIndex].CompressedSize;
            blockDecompressedOffset += m_blocksInfo.StorageBlocks[blockIndex].UncompressedSize;
        }
        long entryOffsetInsideBlock = entry.Offset - blockDecompressedOffset;

        using SmartStream entryStream = (SmartStream)CreateStream.Invoke(this, new object[] { entry.Size });
        long left = entry.Size;
        m_stream.Position = m_dataOffset + blockCompressedOffset;

        // copy data of all blocks used by current entry to new stream
        while (left > 0)
        {
            byte[]? rentedArray;

            long blockStreamOffset;
            Stream blockStream;
            StorageBlock block = m_blocksInfo.StorageBlocks[blockIndex];
            if (m_cachedBlockIndex == blockIndex)
            {
                // data of the previous entry is in the same block as this one
                // so we don't need to unpack it once again. Instead we can use cached stream
                blockStreamOffset = 0;
                blockStream = m_cachedBlockStream;
                rentedArray = null;
                m_stream.Position += block.CompressedSize;
            }
            else
            {
                CompressionType compressType = block.CompressionType;
                if (compressType is CompressionType.None)
                {
                    blockStreamOffset = m_dataOffset + blockCompressedOffset;
                    blockStream = m_stream;
                    rentedArray = null;
                }
                else
                {
                    blockStreamOffset = 0;
                    m_cachedBlockIndex = blockIndex;
                    object[] parameters = new object[] { block.UncompressedSize, null };
                    m_cachedBlockStream.Move((SmartStream)CreateTemporaryStream.Invoke(this, parameters));
                    rentedArray = (byte[]?)parameters[1];

                    // 回调自定义处理
                    CustomBlockCompression(entry, m_stream, block, m_cachedBlockStream, compressType, m_cachedBlockIndex);

                    blockStream = m_cachedBlockStream;
                }
            }

            // consider next offsets:
            // 1) block - if it is new stream then offset is 0, otherwise offset of this block in the bundle file
            // 2) entry - if this is first block for current entry then it is offset of this entry related to this block
            //			  otherwise 0
            long blockSize = block.UncompressedSize - entryOffsetInsideBlock;
            blockStream.Position = blockStreamOffset + entryOffsetInsideBlock;
            entryOffsetInsideBlock = 0;

            long size = Math.Min(blockSize, left);
            using PartialStream partialStream = new(blockStream, blockStream.Position, size);
            partialStream.CopyTo(entryStream);
            blockIndex++;

            blockCompressedOffset += block.CompressedSize;
            left -= size;

            if (rentedArray != null)
            {
                ArrayPool<byte>.Shared.Return(rentedArray);
            }
        }
        if (left < 0)
        {
            throw new Exception($"{entry.PathFixed}, {entry.Size}, {entry.Size - left}");
        }
        entryStream.Position = 0;
        return entryStream.CreateReference();
    }
}