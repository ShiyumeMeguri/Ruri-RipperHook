using AssetRipper.Export.Modules.Shaders;
using AssetRipper.Export.Modules.Shaders.Exporters;
using AssetRipper.Export.Modules.Shaders.Exporters.DirectX;
using AssetRipper.Export.Modules.Shaders.IO;
using AssetRipper.Export.Modules.Shaders.ShaderBlob;
using AssetRipper.Import.Logging;
using AssetRipper.Primitives;
using AssetRipper.SourceGenerated.Extensions.Enums.Shader;
using DXDecompiler.Util;
using HLSLccWrapper;
using System.Buffers.Binary;

namespace Ruri.RipperHook.AR_ShaderDecompiler.Exporters.DirectX;

public class ShaderHLSLccExporter : ShaderTextExporter
{
    protected readonly GPUPlatform m_graphicApi;

    public ShaderHLSLccExporter(GPUPlatform graphicApi)
    {
        m_graphicApi = graphicApi;
    }

    public override void Export(ShaderWriter writer, ref ShaderSubProgram subProgram)
    {
        byte[] exportData = subProgram.ProgramData;
        if (TryGetShaderText(writer, exportData, writer.Version, m_graphicApi, ref subProgram, out string? disassemblyText))
        {
            ExportListing(writer, "//RuriShaderDecompile\n" + (disassemblyText ?? ""));
        }
    }

    public bool TryGetShaderText(ShaderWriter writer, byte[] data, UnityVersion version, GPUPlatform gpuPlatform, ref ShaderSubProgram subProgram, [NotNullWhen(true)] out string? disassemblyText)
    {
        int dataOffset = GetDataOffset(data, version, gpuPlatform);
        return TryDisassemble(writer, data, dataOffset, version, ref subProgram, out disassemblyText);
    }

    private int GetDataOffset(byte[] data, UnityVersion version, GPUPlatform gpuPlatform)
    {
        if (DXDataHeader.HasHeader(gpuPlatform))
        {
            return GetDataOffset(version, gpuPlatform, data[0]);
        }
        else
        {
            return 0;
        }
    }

    public int GetDataOffset(UnityVersion version, GPUPlatform graphicApi, int headerVersion)
    {
        if (DXDataHeader.HasHeader(graphicApi))
        {
            int offset = 0;
            if (headerVersion >= 2)
            {
                offset += 0x20;
            }
            return offset;
        }
        else
        {
            return 0;
        }
    }

    public bool TryDisassemble(ShaderWriter writer, byte[] data, int offset, UnityVersion version, ref ShaderSubProgram subProgram, [NotNullWhen(true)] out string? disassemblyText)
        => TryDisassemble(writer, GetRelevantData(data, offset), version, ref subProgram, out disassemblyText);

    public bool TryDisassemble(ShaderWriter writer, byte[] data, UnityVersion version, ref ShaderSubProgram subProgram, [NotNullWhen(true)] out string? disassemblyText)
    {
        ArgumentNullException.ThrowIfNull(data);

        if (data.Length == 0)
        {
            throw new ArgumentException("inputData cannot have zero length", nameof(data));
        }

        try
        {
            DXProgramType programType = GetProgramType(data, version);
            switch (programType)
            {
                case DXProgramType.DXBC:
                    disassemblyText = null;
                    using (MemoryStream stream = new MemoryStream(data))
                    {
                        using (BinaryReader reader = new BinaryReader(stream))
                        {
                            DXDataHeader header = new DXDataHeader();
                            header.Read(reader, writer.Version);

                            // HACK: since we can't restore UAV info and HLSLcc requires it, process such shader with default exporter
                            if (header.UAVs > 0)
                            {
                                disassemblyText = "// Shader: Unsupported UAVs Export";
                                Logger.Error(disassemblyText);
                            }
                            else
                            {
                                // 因为高版本有可能偏移了
                                subProgram.ProgramData = data;

                                byte[] exportData1 = DXShaderProgramRestorer.RestoreProgramData(reader, writer.Version, ref subProgram);
                                WrappedGlExtensions ext = new WrappedGlExtensions();
                                ext.ARB_explicit_attrib_location = 1;
                                ext.ARB_explicit_uniform_location = 1;
                                ext.ARB_shading_language_420pack = 0;
                                ext.OVR_multiview = 0;
                                ext.EXT_shader_framebuffer_fetch = 0;
                                WrappedShader shader = WrappedShader.TranslateFromMem(exportData1, WrappedGLLang.LANG_DEFAULT, ext);
                                disassemblyText = shader.Text;
                            }
                        }
                    }
                    return !string.IsNullOrEmpty(disassemblyText);
                case DXProgramType.DX9:
                    disassemblyText = DXDecompiler.DX9Shader.AsmWriter.Disassemble(data);
                    return !string.IsNullOrEmpty(disassemblyText);
            }
        }
        catch (Exception ex)
        {
            Logger.Error(LogCategory.Export, $"DXDecompilerly threw an exception while attempting to disassemble a shader");
            Logger.Verbose(LogCategory.Export, ex.ToString());
        }

        disassemblyText = null;
        return false;
    }

    private DXProgramType GetProgramType(ReadOnlySpan<byte> data, UnityVersion version)
    {
        int additionalOffset = DXDataHeader.HasGSInputPrimitive(version) ? 6 : 5;

        if (data.Length < additionalOffset + 4)
        {
            return DXProgramType.Unknown;
        }
        uint dxbcHeader = BinaryPrimitives.ReadUInt32LittleEndian(data[additionalOffset..]);
        if (dxbcHeader == "DXBC".ToFourCc() || dxbcHeader == 0xFEFF2001)
        {
            return DXProgramType.DXBC;
        }
        DXDecompiler.DX9Shader.ShaderType dx9ShaderType = (DXDecompiler.DX9Shader.ShaderType)
            BinaryPrimitives.ReadUInt16LittleEndian(data[(additionalOffset + 2)..]);
        if (dx9ShaderType == DXDecompiler.DX9Shader.ShaderType.Vertex ||
            dx9ShaderType == DXDecompiler.DX9Shader.ShaderType.Pixel ||
            dx9ShaderType == DXDecompiler.DX9Shader.ShaderType.Effect)
        {
            return DXProgramType.DX9;
        }
        return DXProgramType.Unknown;
    }

    private byte[] GetRelevantData(ReadOnlySpan<byte> bytes, int offset)
    {
        if (offset < 0 || offset > bytes.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset));
        }
        return bytes[offset..].ToArray();
    }

    private enum DXProgramType
    {
        Unknown,
        DX9,
        DXBC
    }
}
