using System.Reflection;
using AssetRipper.IO.Files.BundleFiles;
using AssetRipper.IO.Files.BundleFiles.FileStream;
using AssetRipper.IO.Files.Extensions;
using AssetRipper.IO.Files.Streams.Smart;

namespace Ruri.RipperHook.HookUtils.BundleFileBlockReaderHook;

public class BundleFileBlockReaderHook : CommonHook
{
    private const string TYPE = "AssetRipper.IO.Files.BundleFiles.FileStream.BundleFileBlockReader, AssetRipper.IO.Files";

    private static readonly MethodInfo CreateStream = Type.GetType(TYPE).GetMethod("CreateStream", ReflectionExtensions.PrivateStaticBindFlag());
    
    public delegate void BlockCompressionDelegate(Stream mStream, StorageBlock block, SmartStream cachedBlockStream, CompressionType compressType, int m_cachedBlockIndex);
    
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
            throw new ObjectDisposedException(nameof(type));

        // find block offsets
        int blockIndex;
        long blockCompressedOffset = 0;
        long blockDecompressedOffset = 0;
        for (blockIndex = 0;
             blockDecompressedOffset + m_blocksInfo.StorageBlocks[blockIndex].UncompressedSize <= entry.Offset;
             blockIndex++)
        {
            blockCompressedOffset += m_blocksInfo.StorageBlocks[blockIndex].CompressedSize;
            blockDecompressedOffset += m_blocksInfo.StorageBlocks[blockIndex].UncompressedSize;
        }

        var entryOffsetInsideBlock = entry.Offset - blockDecompressedOffset;

        using var entryStream = (SmartStream)CreateStream.Invoke(null, new object[] { entry.Size });
        var left = entry.Size;
        m_stream.Position = m_dataOffset + blockCompressedOffset;

        // copy data of all blocks used by current entry to new stream
        while (left > 0)
        {
            long blockStreamOffset;
            Stream blockStream;
            StorageBlock block = m_blocksInfo.StorageBlocks[blockIndex];
            if (m_cachedBlockIndex == blockIndex)
            {
                // data of the previous entry is in the same block as this one
                // so we don't need to unpack it once again. Instead we can use cached stream
                blockStreamOffset = 0;
                blockStream = m_cachedBlockStream;
                m_stream.Position += block.CompressedSize;
            }
            else
            {
                var compressType = block.CompressionType;
                if (compressType is CompressionType.None)
                {
                    blockStreamOffset = m_dataOffset + blockCompressedOffset;
                    blockStream = m_stream;
                }
                else
                {
                    blockStreamOffset = 0;
                    m_cachedBlockIndex = blockIndex;
                    m_cachedBlockStream.Move((SmartStream)CreateStream.Invoke(null, new object[] { block.UncompressedSize }));

                    CustomBlockCompression(m_stream, block, m_cachedBlockStream, compressType, m_cachedBlockIndex);

                    blockStream = m_cachedBlockStream;
                }
            }

            // consider next offsets:
            // 1) block - if it is new stream then offset is 0, otherwise offset of this block in the bundle file
            // 2) entry - if this is first block for current entry then it is offset of this entry related to this block
            //			  otherwise 0
            var blockSize = block.UncompressedSize - entryOffsetInsideBlock;
            blockStream.Position = blockStreamOffset + entryOffsetInsideBlock;
            entryOffsetInsideBlock = 0;

            var size = Math.Min(blockSize, left);
            blockStream.CopyStream(entryStream, size);
            blockIndex++;

            blockCompressedOffset += block.CompressedSize;
            left -= size;
        }

        if (left < 0)
            Console.WriteLine("DecompressionFailedException.ThrowReadMoreThanExpected(entry.Size, entry.Size - left)");
        entryStream.Position = 0;
        return entryStream.CreateReference();
    }
}