using AssetRipper.Assets;
using AssetRipper.Assets.Generics;
using AssetRipper.Export.Modules.Shaders.Extensions;
using AssetRipper.Export.UnityProjects;
using AssetRipper.Export.UnityProjects.Shaders;
using AssetRipper.IO.Files;
using AssetRipper.Primitives;
using AssetRipper.SourceGenerated.Classes.ClassID_48;
using AssetRipper.SourceGenerated.Subclasses.BufferBinding;
using AssetRipper.SourceGenerated.Subclasses.ConstantBuffer;
using AssetRipper.SourceGenerated.Subclasses.MatrixParameter;
using AssetRipper.SourceGenerated.Subclasses.ParserBindChannels;
using AssetRipper.SourceGenerated.Subclasses.SamplerParameter;
using AssetRipper.SourceGenerated.Subclasses.SerializedPass;
using AssetRipper.SourceGenerated.Subclasses.SerializedPlayerSubProgram;
using AssetRipper.SourceGenerated.Subclasses.SerializedProgram;
using AssetRipper.SourceGenerated.Subclasses.SerializedProgramParameters;
using AssetRipper.SourceGenerated.Subclasses.SerializedProperties;
using AssetRipper.SourceGenerated.Subclasses.SerializedProperty;
using AssetRipper.SourceGenerated.Subclasses.SerializedShader;
using AssetRipper.SourceGenerated.Subclasses.SerializedShaderRTBlendState;
using AssetRipper.SourceGenerated.Subclasses.SerializedShaderState;
using AssetRipper.SourceGenerated.Subclasses.SerializedShaderVectorValue;
using AssetRipper.SourceGenerated.Subclasses.SerializedStencilOp;
using AssetRipper.SourceGenerated.Subclasses.SerializedSubProgram;
using AssetRipper.SourceGenerated.Subclasses.SerializedSubShader;
using AssetRipper.SourceGenerated.Subclasses.SerializedTagMap;
using AssetRipper.SourceGenerated.Subclasses.SerializedTextureProperty;
using AssetRipper.SourceGenerated.Subclasses.StructParameter;
using AssetRipper.SourceGenerated.Subclasses.TextureParameter;
using AssetRipper.SourceGenerated.Subclasses.UAVParameter;
using AssetRipper.SourceGenerated.Subclasses.VectorParameter;
using AssetsTools.NET;
using K4os.Compression.LZ4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ruri.RipperHook.AR_ShaderDecompiler;

public sealed class ShaderDXDecompileExporter : ShaderExporterBase
{
    public override bool Export(IExportContainer container, IUnityObjectBase asset, string path, FileSystem fileSystem)
    {
        // This part remains unchanged
        using Stream fileStream = File.Create(path);

        var shader = (IShader)asset;

        var shaderName = shader.Name;
        AssetTypeValueField shaderData = ShaderAssetCreator.CreateAssetTypeValueField(shader);

        var shaderProcessor = new USCSandbox.Processor.ShaderProcessor(shaderData, asset.Collection.Version, USCSandbox.GPUPlatform.d3d11);
        string shaderText = shaderProcessor.Process();

        // For testing, you might want to write the shaderText to the file
        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(shaderText);
        }

        return true;
    }
}


public static class ShaderAssetCreator
{

    public static AssetTypeValueField CreateAssetTypeValueField(IShader shader)
    {
        var blobs = shader.ReadBlobs();

        byte[] masterBlob;
        List<IReadOnlyList<uint>> offsets;
        List<IReadOnlyList<uint>> compressedLengths;
        List<IReadOnlyList<uint>> decompressedLengths;

        if (blobs.Length == 0 && shader.Platforms != null && shader.Platforms.Count > 0)
        {
            // 处理Shader完全没有编译代码的情况 (e.g., Fallback shader)
            // 必须为每个平台生成一个最小化的、有效的、压缩过的blob。

            // 1. 创建一个代表 "count = 0" 的 decompressed blob。
            byte[] decompressedEmptyBlob = new byte[4]; // [0,0,0,0]

            // 2. 将其压缩。
            byte[] compressedEmptyBlob = new byte[LZ4Codec.MaximumOutputSize(decompressedEmptyBlob.Length)];
            int compressedSize = LZ4Codec.Encode(decompressedEmptyBlob, compressedEmptyBlob, LZ4Level.L00_FAST);
            Array.Resize(ref compressedEmptyBlob, compressedSize);

            // 3. 为每个平台复制这个压缩后的blob，并计算offsets。
            using var masterBlobStream = new MemoryStream();
            offsets = new List<IReadOnlyList<uint>>();
            compressedLengths = new List<IReadOnlyList<uint>>();
            decompressedLengths = new List<IReadOnlyList<uint>>();
            uint currentOffset = 0;

            for (int i = 0; i < shader.Platforms.Count; i++)
            {
                masterBlobStream.Write(compressedEmptyBlob, 0, compressedEmptyBlob.Length);

                offsets.Add(new uint[] { currentOffset });
                compressedLengths.Add(new uint[] { (uint)compressedEmptyBlob.Length });
                decompressedLengths.Add(new uint[] { (uint)decompressedEmptyBlob.Length }); // Decompressed length is 4

                currentOffset += (uint)compressedEmptyBlob.Length;
            }
            masterBlob = masterBlobStream.ToArray();
        }
        else if (blobs.Length == 0) // 没有平台信息，也没有blobs
        {
            masterBlob = Array.Empty<byte>();
            offsets = new List<IReadOnlyList<uint>>();
            compressedLengths = new List<IReadOnlyList<uint>>();
            decompressedLengths = new List<IReadOnlyList<uint>>();
        }
        else
        {
            // 如果有blob数据，则使用现有的、已修复的重建逻辑。
            (masterBlob, offsets, compressedLengths, decompressedLengths) = ReconstructAndCompressBlobs(shader, blobs);
        }

        AssetTypeValueField offsetsField;
        AssetTypeValueField compressedLengthsField;
        AssetTypeValueField decompressedLengthsField;

        // 根据shader版本决定offsets/lengths的结构
        if (shader.Has_Offsets_AssetList_AssetList_UInt32())
        {
            offsetsField = CreateVectorOfVectorOfUInts("offsets", offsets);
            compressedLengthsField = CreateVectorOfVectorOfUInts("compressedLengths", compressedLengths);
            decompressedLengthsField = CreateVectorOfVectorOfUInts("decompressedLengths", decompressedLengths);
        }
        else // 包括 Has_Offsets_AssetList_UInt32 和更早的情况
        {
            offsetsField = CreateVectorOfUIntsForOldUnity("offsets", offsets.Select(l => l.FirstOrDefault()).ToList());
            compressedLengthsField = CreateVectorOfUIntsForOldUnity("compressedLengths", compressedLengths.Select(l => l.FirstOrDefault()).ToList());
            decompressedLengthsField = CreateVectorOfUIntsForOldUnity("decompressedLengths", decompressedLengths.Select(l => l.FirstOrDefault()).ToList());
        }

        var children = new List<AssetTypeValueField>
    {
        ConvertSerializedShader("m_ParsedForm", shader.ParsedForm, shader.Collection.Version, shader),
        CreateVectorOfInts("platforms", shader.Platforms?.Select(p => (int)p).ToList()),
        offsetsField,
        compressedLengthsField,
        decompressedLengthsField,
        CreateBlobField("compressedBlob", masterBlob)
    };

        var shaderTemplate = new AssetTypeTemplateField
        {
            Name = "Shader",
            Type = "Shader",
            ValueType = AssetValueType.None,
            Children = children.Select(c => c.TemplateField).ToList()
        };

        return new AssetTypeValueField
        {
            TemplateField = shaderTemplate,
            Value = null,
            Children = children
        };
    }

    #region Structure Converters

    private static AssetTypeValueField ConvertSerializedShader(string name, ISerializedShader? parsedForm, UnityVersion version, IShader shader)
    {
        var children = new List<AssetTypeValueField>();
        if (parsedForm != null)
        {
            children.Add(CreateStringField("m_Name", parsedForm.Name_R));
            children.Add(ConvertSerializedProperties("m_PropInfo", parsedForm.PropInfo, version, shader));
            children.Add(CreateVectorOfComplex("m_SubShaders", parsedForm.SubShaders, (n, d) => ConvertSubShader(n, d, version)));
            children.Add(CreateStringField("m_FallbackName", parsedForm.FallbackName?.String ?? ""));
            children.Add(CreateStringVector("m_KeywordNames", parsedForm.KeywordNames?.Select(s => s.String).ToList() ?? new List<string>()));
        }

        var template = new AssetTypeTemplateField { Name = name, Type = "SerializedShader", ValueType = AssetValueType.None, Children = children.Select(c => c.TemplateField).ToList() };
        return new AssetTypeValueField { TemplateField = template, Value = null, Children = children };
    }

    private static AssetTypeValueField ConvertSerializedProperties(string name, ISerializedProperties? propInfo, UnityVersion version, IShader shader)
    {
        var children = new List<AssetTypeValueField>();
        if (propInfo != null)
        {
            children.Add(CreateVectorOfComplex("m_Props", propInfo.Props, (n, d) => ConvertSerializedProperty(n, d, version, shader)));
        }

        var template = new AssetTypeTemplateField { Name = name, Type = "SerializedProperties", ValueType = AssetValueType.None, Children = children.Select(c => c.TemplateField).ToList() };
        return new AssetTypeValueField { TemplateField = template, Value = null, Children = children };
    }

    private static AssetTypeValueField ConvertSubShader(string name, ISerializedSubShader subShader, UnityVersion version)
    {
        var children = new List<AssetTypeValueField>
            {
                CreateVectorOfComplex("m_Passes", subShader.Passes, (n, p) => ConvertPass(n, p, version)),
                ConvertTags("m_Tags", subShader.Tags),
                CreatePrimitiveField("m_LOD", "int", AssetValueType.Int32, subShader.LOD)
            };

        var template = new AssetTypeTemplateField { Name = name, Type = "SerializedSubShader", ValueType = AssetValueType.None, Children = children.Select(c => c.TemplateField).ToList() };
        return new AssetTypeValueField { TemplateField = template, Value = null, Children = children };
    }

    private static AssetTypeValueField ConvertPass(string name, ISerializedPass pass, UnityVersion version)
    {
        var children = new List<AssetTypeValueField>
            {
                ConvertState("m_State", pass.State, version),
                // CreatePrimitiveField("m_Type", "int", AssetValueType.Int32, pass.Type), // Not used by A
                CreateStringField("m_UseName", pass.UseName),
                ConvertNameIndices("m_NameIndices", pass.NameIndices),
                // CreateStringField("m_TextureName", pass.TextureName), // Not used by A
                ConvertSerializedProgram("progVertex", pass.ProgVertex, version),
                ConvertSerializedProgram("progFragment", pass.ProgFragment, version),
                ConvertSerializedProgram("progGeometry", pass.ProgGeometry, version),
                ConvertSerializedProgram("progHull", pass.ProgHull, version),
                ConvertSerializedProgram("progDomain", pass.ProgDomain, version),
                ConvertSerializedProgram("progRayTracing", pass.ProgRayTracing, version),
                // CreatePrimitiveField("m_HasInstancingVariant", "bool", AssetValueType.Bool, pass.HasInstancingVariant), // Not used by A
                // CreatePrimitiveField("m_HasProceduralInstancingVariant", "bool", AssetValueType.Bool, pass.HasProceduralInstancingVariant), // Not used by A
            };

        var template = new AssetTypeTemplateField { Name = name, Type = "SerializedPass", ValueType = AssetValueType.None, Children = children.Select(c => c.TemplateField).ToList() };
        return new AssetTypeValueField { TemplateField = template, Value = null, Children = children };
    }

    private static AssetTypeValueField ConvertState(string name, ISerializedShaderState state, UnityVersion version)
    {
        var children = new List<AssetTypeValueField>();
        if (state != null)
        {
            children.Add(CreateStringField("m_Name", state.Name_R));
            children.Add(CreatePrimitiveField("m_LOD", "int", AssetValueType.Int32, state.LOD));
            children.Add(ConvertTags("m_Tags", state.Tags));
            children.Add(CreatePrimitiveField("lighting", "bool", AssetValueType.Bool, state.Lighting));

            // RTBlend states
            children.Add(CreatePrimitiveField("rtSeparateBlend", "bool", AssetValueType.Bool, state.RtSeparateBlend));
            children.Add(ConvertRTBlendState("rtBlend0", state.RtBlend0));
            children.Add(ConvertRTBlendState("rtBlend1", state.RtBlend1));
            children.Add(ConvertRTBlendState("rtBlend2", state.RtBlend2));
            children.Add(ConvertRTBlendState("rtBlend3", state.RtBlend3));
            children.Add(ConvertRTBlendState("rtBlend4", state.RtBlend4));
            children.Add(ConvertRTBlendState("rtBlend5", state.RtBlend5));
            children.Add(ConvertRTBlendState("rtBlend6", state.RtBlend6));
            children.Add(ConvertRTBlendState("rtBlend7", state.RtBlend7));

            // Stencil
            children.Add(ConvertStencilOp("stencilOp", state.StencilOp));
            children.Add(ConvertStencilOp("stencilOpFront", state.StencilOpFront));
            children.Add(ConvertStencilOp("stencilOpBack", state.StencilOpBack));
            children.Add(CreateSerializedFloatField("stencilRef", state.StencilRef.Val));
            children.Add(CreateSerializedFloatField("stencilReadMask", state.StencilReadMask.Val));
            children.Add(CreateSerializedFloatField("stencilWriteMask", state.StencilWriteMask.Val));

            // Fog
            children.Add(CreatePrimitiveField("fogMode", "float", AssetValueType.Float, (float)state.FogMode)); // A-code reads AsFloat
            children.Add(ConvertFogColor("fogColor", state.FogColor));
            children.Add(CreateSerializedFloatField("fogDensity", state.FogDensity.Val));
            children.Add(CreateSerializedFloatField("fogStart", state.FogStart.Val));
            children.Add(CreateSerializedFloatField("fogEnd", state.FogEnd.Val));

            // Other states
            children.Add(CreateSerializedFloatField("alphaToMask", state.AlphaToMask.Val));
            children.Add(CreateSerializedFloatField("zClip", state.ZClip.Val));
            children.Add(CreateSerializedFloatField("zTest", (float)state.ZTest.Val));
            children.Add(CreateSerializedFloatField("zWrite", (float)state.ZWrite.Val));
            children.Add(CreateSerializedFloatField("culling", (float)state.Culling.Val));
            children.Add(CreateSerializedFloatField("offsetFactor", state.OffsetFactor.Val));
            children.Add(CreateSerializedFloatField("offsetUnits", state.OffsetUnits.Val));
        }

        var template = new AssetTypeTemplateField { Name = name, Type = "SerializedShaderState", ValueType = AssetValueType.None, Children = children.Select(c => c.TemplateField).ToList() };
        return new AssetTypeValueField { TemplateField = template, Value = null, Children = children };
    }

    private static AssetTypeValueField ConvertRTBlendState(string name, ISerializedShaderRTBlendState blendState)
    {
        var children = new List<AssetTypeValueField>
        {
            CreateSerializedFloatField("srcBlend", (float)blendState.SourceBlend.Val),
            CreateSerializedFloatField("destBlend", (float)blendState.DestinationBlend.Val),
            CreateSerializedFloatField("srcBlendAlpha", (float)blendState.SourceBlendAlpha.Val),
            CreateSerializedFloatField("destBlendAlpha", (float)blendState.DestinationBlendAlpha.Val),
            CreateSerializedFloatField("blendOp", (float)blendState.BlendOp.Val),
            CreateSerializedFloatField("blendOpAlpha", (float)blendState.BlendOpAlpha.Val),
            CreateSerializedFloatField("colMask", (float)blendState.ColMask.Val)
        };
        var template = new AssetTypeTemplateField { Name = name, Type = "SerializedShaderRTBlendState", ValueType = AssetValueType.None, Children = children.Select(c => c.TemplateField).ToList() };
        return new AssetTypeValueField { TemplateField = template, Value = null, Children = children };
    }

    private static AssetTypeValueField ConvertStencilOp(string name, ISerializedStencilOp stencilOp)
    {
        var children = new List<AssetTypeValueField>
        {
            CreateSerializedFloatField("pass", (float)stencilOp.Pass.Val),
            CreateSerializedFloatField("fail", (float)stencilOp.Fail.Val),
            CreateSerializedFloatField("zFail", (float)stencilOp.ZFail.Val),
            CreateSerializedFloatField("comp", (float)stencilOp.Comp.Val)
        };
        var template = new AssetTypeTemplateField { Name = name, Type = "SerializedStencilOp", ValueType = AssetValueType.None, Children = children.Select(c => c.TemplateField).ToList() };
        return new AssetTypeValueField { TemplateField = template, Value = null, Children = children };
    }

    private static AssetTypeValueField ConvertFogColor(string name, ISerializedShaderVectorValue fogColor)
    {
        var children = new List<AssetTypeValueField>
        {
            CreateSerializedFloatField("x", fogColor.X.Val),
            CreateSerializedFloatField("y", fogColor.Y.Val),
            CreateSerializedFloatField("z", fogColor.Z.Val),
            CreateSerializedFloatField("w", fogColor.W.Val)
        };
        var template = new AssetTypeTemplateField { Name = name, Type = "SerializedShaderVectorValue", ValueType = AssetValueType.None, Children = children.Select(c => c.TemplateField).ToList() };
        return new AssetTypeValueField { TemplateField = template, Value = null, Children = children };
    }

    private static AssetTypeValueField CreateSerializedFloatField(string name, float value)
    {
        var valField = CreatePrimitiveField("val", "float", AssetValueType.Float, value);
        var template = new AssetTypeTemplateField { Name = name, Type = "SerializedFloat", ValueType = AssetValueType.None, Children = new List<AssetTypeTemplateField> { valField.TemplateField } };
        return new AssetTypeValueField { TemplateField = template, Value = null, Children = new List<AssetTypeValueField> { valField } };
    }

    private static AssetTypeValueField ConvertSerializedProgram(string name, ISerializedProgram? program, UnityVersion version)
    {
        var children = new List<AssetTypeValueField>();
        if (program != null)
        {
            if (program.PlayerSubPrograms is not null && program.PlayerSubPrograms.Any(p => p.Count > 0))
            {
                children.Add(CreateVectorOfVectorOfComplex("m_PlayerSubPrograms", program.PlayerSubPrograms, (n, psp) => ConvertPlayerSubProgram(n, psp)));
                children.Add(CreateVectorOfVectorOfUInts("m_ParameterBlobIndices", program.ParameterBlobIndices));
            }
            else
            {
                // Add dummy fields to prevent "field not found" in A-code
                children.Add(CreateDummyVector("m_PlayerSubPrograms"));
                children.Add(CreateDummyVector("m_ParameterBlobIndices"));
            }

            children.Add(CreateVectorOfComplex("m_SubPrograms", program.SubPrograms, (n, sp) => ConvertSubProgram(n, sp, version)));

            children.Add(ConvertProgramParameters("m_CommonParameters", program.CommonParameters));
        }
        var template = new AssetTypeTemplateField { Name = name, Type = "SerializedProgram", ValueType = AssetValueType.None, Children = children.Select(c => c.TemplateField).ToList() };
        return new AssetTypeValueField { TemplateField = template, Children = children };
    }

    private static AssetTypeValueField ConvertSubProgram(string name, ISerializedSubProgram subProgram, UnityVersion version)
    {
        var children = new List<AssetTypeValueField>
            {
                CreatePrimitiveField("m_BlobIndex", "unsigned int", AssetValueType.UInt32, subProgram.BlobIndex),
                ConvertParserBindChannels("m_Channels", subProgram.Channels),
                CreateVectorOfUShorts("m_KeywordIndices", subProgram.KeywordIndices),
                CreatePrimitiveField("m_GpuProgramType", "SInt8", AssetValueType.Int8, subProgram.GpuProgramType),
                ConvertProgramParameters("m_Parameters", subProgram.Parameters),
                // The following are not used by A-code but are part of the structure
                // CreatePrimitiveField("m_ShaderHardwareTier", "SInt8", AssetValueType.Int8, subProgram.ShaderHardwareTier),
                // CreatePrimitiveField("m_ShaderRequirements", "int", AssetValueType.Int32, subProgram.ShaderRequirements_Int32),
            };
        var template = new AssetTypeTemplateField { Name = name, Type = "SerializedSubProgram", ValueType = AssetValueType.None, Children = children.Select(c => c.TemplateField).ToList() };
        return new AssetTypeValueField { TemplateField = template, Children = children };
    }

    private static AssetTypeValueField ConvertPlayerSubProgram(string name, ISerializedPlayerSubProgram playerSubProgram)
    {
        var children = new List<AssetTypeValueField>
        {
            CreatePrimitiveField("m_BlobIndex", "unsigned int", AssetValueType.UInt32, playerSubProgram.BlobIndex),
            CreateVectorOfUShorts("m_KeywordIndices", playerSubProgram.KeywordIndices),
            CreatePrimitiveField("m_GpuProgramType", "SInt8", AssetValueType.Int8, playerSubProgram.GpuProgramType)
        };
        var template = new AssetTypeTemplateField { Name = name, Type = "SerializedPlayerSubProgram", ValueType = AssetValueType.None, Children = children.Select(c => c.TemplateField).ToList() };
        return new AssetTypeValueField { TemplateField = template, Children = children };
    }

    private static AssetTypeValueField ConvertProgramParameters(string name, ISerializedProgramParameters? parameters)
    {
        var children = new List<AssetTypeValueField>();
        // A-code expects these fields, so create empty vectors if parameters is null or individual lists are null
        children.Add(CreateVectorOfComplex("m_VectorParams", parameters?.VectorParams, (n, vp) => ConvertVectorParameter(n, vp)));
        children.Add(CreateVectorOfComplex("m_MatrixParams", parameters?.MatrixParams, (n, mp) => ConvertMatrixParameter(n, mp)));
        children.Add(CreateVectorOfComplex("m_TextureParams", parameters?.TextureParams, (n, tp) => ConvertTextureParameter(n, tp)));
        children.Add(CreateVectorOfComplex("m_BufferParams", parameters?.BufferParams, (n, bp) => ConvertBufferBinding(n, bp)));
        children.Add(CreateVectorOfComplex("m_ConstantBuffers", parameters?.ConstantBuffers, (n, cb) => ConvertConstantBuffer(n, cb)));
        children.Add(CreateVectorOfComplex("m_ConstantBufferBindings", parameters?.ConstantBufferBindings, (n, cbb) => ConvertBufferBinding(n, cbb)));
        children.Add(CreateVectorOfComplex("m_UAVParams", parameters?.UAVParams, (n, up) => ConvertUAVParameter(n, up)));
        children.Add(CreateVectorOfComplex("m_Samplers", parameters?.Samplers, (n, s) => ConvertSamplerParameter(n, s)));

        var template = new AssetTypeTemplateField { Name = name, Type = "SerializedProgramParameters", ValueType = AssetValueType.None, Children = children.Select(c => c.TemplateField).ToList() };
        return new AssetTypeValueField { TemplateField = template, Children = children };
    }

    private static AssetTypeValueField ConvertParserBindChannels(string name, IParserBindChannels channels)
    {
        // A-Code reads this information directly from the shader blob in 'ShaderSubProgram' constructor.
        // It does not use this field. An empty one is sufficient to avoid schema errors.
        var children = new List<AssetTypeValueField>();
        var template = new AssetTypeTemplateField { Name = name, Type = "ParserBindChannels", ValueType = AssetValueType.None, Children = children.Select(c => c.TemplateField).ToList() };
        return new AssetTypeValueField { TemplateField = template, Children = children };
    }

    private static AssetTypeValueField ConvertConstantBuffer(string name, IConstantBuffer cb)
    {
        var children = new List<AssetTypeValueField>
        {
            CreatePrimitiveField("m_NameIndex", "int", AssetValueType.Int32, cb.NameIndex),
            // A-Code's ConstantBuffer constructor reads both m_MatrixParams and m_VectorParams into its CBParams list.
            CreateVectorOfComplex("m_MatrixParams", cb.MatrixParams, (n, mp) => ConvertMatrixParameter(n, mp)),
            CreateVectorOfComplex("m_VectorParams", cb.VectorParams, (n, vp) => ConvertVectorParameter(n, vp)),
            // A-Code checks m_StructParams but throws if it's not empty. It reads structs from the blob.
            CreateVectorOfComplex("m_StructParams", cb.StructParams, (n, sp) => ConvertStructParameter(n, sp)),
            CreatePrimitiveField("m_Size", "int", AssetValueType.Int32, cb.Size),
            CreatePrimitiveField("m_IsPartialCB", "bool", AssetValueType.Bool, cb.IsPartialCB)
        };
        var template = new AssetTypeTemplateField { Name = name, Type = "ConstantBuffer", ValueType = AssetValueType.None, Children = children.Select(c => c.TemplateField).ToList() };
        return new AssetTypeValueField { TemplateField = template, Children = children };
    }

    private static AssetTypeValueField ConvertMatrixParameter(string name, IMatrixParameter mp)
    {
        var children = new List<AssetTypeValueField>
        {
            CreatePrimitiveField("m_NameIndex", "int", AssetValueType.Int32, mp.NameIndex),
            CreatePrimitiveField("m_Index", "int", AssetValueType.Int32, mp.Index),
            CreatePrimitiveField("m_ArraySize", "int", AssetValueType.Int32, mp.ArraySize),
            // CreatePrimitiveField("m_Type", "SInt8", AssetValueType.Int8, mp.Type), // Not directly used by A's CBP constructor
            // A-code's CBP constructor checks for m_RowCount to identify a matrix
            CreatePrimitiveField("m_RowCount", "SInt8", AssetValueType.Int8, mp.RowCount)
        };
        var template = new AssetTypeTemplateField { Name = name, Type = "MatrixParameter", ValueType = AssetValueType.None, Children = children.Select(c => c.TemplateField).ToList() };
        return new AssetTypeValueField { TemplateField = template, Children = children };
    }

    private static AssetTypeValueField ConvertVectorParameter(string name, IVectorParameter vp)
    {
        var children = new List<AssetTypeValueField>
        {
            CreatePrimitiveField("m_NameIndex", "int", AssetValueType.Int32, vp.NameIndex),
            CreatePrimitiveField("m_Index", "int", AssetValueType.Int32, vp.Index),
            CreatePrimitiveField("m_ArraySize", "int", AssetValueType.Int32, vp.ArraySize),
            // CreatePrimitiveField("m_Type", "SInt8", AssetValueType.Int8, vp.Type), // Not directly used by A's CBP constructor
            // A-code's CBP constructor uses "m_Dim" for vectors if m_RowCount is dummy.
            CreatePrimitiveField("m_Dim", "SInt8", AssetValueType.Int8, vp.Dim)
        };
        var template = new AssetTypeTemplateField { Name = name, Type = "VectorParameter", ValueType = AssetValueType.None, Children = children.Select(c => c.TemplateField).ToList() };
        return new AssetTypeValueField { TemplateField = template, Children = children };
    }

    private static AssetTypeValueField ConvertTextureParameter(string name, ITextureParameter tp)
    {
        var children = new List<AssetTypeValueField>
        {
            CreatePrimitiveField("m_NameIndex", "int", AssetValueType.Int32, tp.NameIndex),
            CreatePrimitiveField("m_Index", "int", AssetValueType.Int32, tp.Index),
            CreatePrimitiveField("m_SamplerIndex", "int", AssetValueType.Int32, tp.SamplerIndex),
            CreatePrimitiveField("m_MultiSampled", "bool", AssetValueType.Bool, tp.MultiSampled),
            CreatePrimitiveField("m_Dim", "SInt8", AssetValueType.Int8, tp.Dim)
        };
        var template = new AssetTypeTemplateField { Name = name, Type = "TextureParameter", ValueType = AssetValueType.None, Children = children.Select(c => c.TemplateField).ToList() };
        return new AssetTypeValueField { TemplateField = template, Children = children };
    }

    private static AssetTypeValueField ConvertBufferBinding(string name, IBufferBinding bb)
    {
        var children = new List<AssetTypeValueField>
        {
            CreatePrimitiveField("m_NameIndex", "int", AssetValueType.Int32, bb.NameIndex),
            CreatePrimitiveField("m_Index", "int", AssetValueType.Int32, bb.Index),
            CreatePrimitiveField("m_ArraySize", "int", AssetValueType.Int32, bb.ArraySize)
        };
        var template = new AssetTypeTemplateField { Name = name, Type = "BufferBinding", ValueType = AssetValueType.None, Children = children.Select(c => c.TemplateField).ToList() };
        return new AssetTypeValueField { TemplateField = template, Children = children };
    }

    private static AssetTypeValueField ConvertUAVParameter(string name, IUAVParameter up)
    {
        // Not directly used by A-code's Deserialization, but good for completeness
        var children = new List<AssetTypeValueField>
        {
            CreatePrimitiveField("m_NameIndex", "int", AssetValueType.Int32, up.NameIndex),
            CreatePrimitiveField("m_Index", "int", AssetValueType.Int32, up.Index),
            CreatePrimitiveField("m_OriginalIndex", "int", AssetValueType.Int32, up.OriginalIndex)
        };
        var template = new AssetTypeTemplateField { Name = name, Type = "UAVParameter", ValueType = AssetValueType.None, Children = children.Select(c => c.TemplateField).ToList() };
        return new AssetTypeValueField { TemplateField = template, Children = children };
    }

    private static AssetTypeValueField ConvertSamplerParameter(string name, ISamplerParameter sp)
    {
        // Not directly used by A-code's Deserialization, but good for completeness
        var children = new List<AssetTypeValueField>
        {
            CreatePrimitiveField("sampler", "unsigned int", AssetValueType.UInt32, sp.Sampler),
            CreatePrimitiveField("bindPoint", "int", AssetValueType.Int32, sp.BindPoint)
        };
        var template = new AssetTypeTemplateField { Name = name, Type = "SamplerParameter", ValueType = AssetValueType.None, Children = children.Select(c => c.TemplateField).ToList() };
        return new AssetTypeValueField { TemplateField = template, Children = children };
    }

    private static AssetTypeValueField ConvertStructParameter(string name, IStructParameter sp)
    {
        // A-Code reads StructParameter data from the binary blob, not from this serialized field.
        // The serialized version can therefore be empty.
        var children = new List<AssetTypeValueField>();
        var template = new AssetTypeTemplateField { Name = name, Type = "StructParameter", ValueType = AssetValueType.None, Children = new List<AssetTypeTemplateField>() };
        return new AssetTypeValueField { TemplateField = template, Children = children };
    }

    private static AssetTypeValueField ConvertNameIndices(string name, AssetDictionary<Utf8String, int>? nameIndices)
    {
        var pairList = new List<KeyValuePair<Utf8String, int>>();
        if (nameIndices != null)
        {
            pairList.AddRange(nameIndices);
        }

        return CreateVectorOfComplex(name, pairList, (n, p) => ConvertStringIntPair(p));
    }

    private static AssetTypeValueField ConvertStringIntPair(KeyValuePair<Utf8String, int> pair)
    {
        var children = new List<AssetTypeValueField>
            {
                CreateStringField("first", pair.Key),
                CreatePrimitiveField("second", "int", AssetValueType.Int32, pair.Value)
            };
        var template = new AssetTypeTemplateField { Name = "data", Type = "pair", ValueType = AssetValueType.None, Children = children.Select(c => c.TemplateField).ToList() };
        return new AssetTypeValueField { TemplateField = template, Children = children };
    }

    private static AssetTypeValueField ConvertTags(string name, ISerializedTagMap? tagMap)
    {
        var pairList = new List<KeyValuePair<Utf8String, Utf8String>>();
        if (tagMap?.Tags != null)
        {
            pairList.AddRange(tagMap.Tags);
        }

        var tagsVector = CreateVectorOfComplex("tags", pairList, (n, p) => ConvertTagPair(p));

        var template = new AssetTypeTemplateField { Name = name, Type = "SerializedTagMap", ValueType = AssetValueType.None, Children = new List<AssetTypeTemplateField> { tagsVector.TemplateField } };
        return new AssetTypeValueField { TemplateField = template, Value = null, Children = new List<AssetTypeValueField> { tagsVector } };
    }

    private static AssetTypeValueField ConvertTagPair(KeyValuePair<Utf8String, Utf8String> pair)
    {
        var children = new List<AssetTypeValueField>
            {
                CreateStringField("first", pair.Key),
                CreateStringField("second", pair.Value)
            };
        var template = new AssetTypeTemplateField { Name = "data", Type = "pair", ValueType = AssetValueType.None, Children = children.Select(c => c.TemplateField).ToList() };
        return new AssetTypeValueField { TemplateField = template, Children = children };
    }

    private static AssetTypeValueField ConvertSerializedProperty(string name, ISerializedProperty prop, UnityVersion version, IShader shader)
    {
        var children = new List<AssetTypeValueField>
        {
            CreateStringField("m_Name", prop.Name_R),
            CreateStringField("m_Description", prop.Description),
            CreateStringVector("m_Attributes", prop.Attributes?.Select(s => s.String).ToList()),
            CreatePrimitiveField("m_Type", "int", AssetValueType.Int32, (int)prop.Type),
            CreatePrimitiveField("m_Flags", "unsigned int", AssetValueType.UInt32, (uint)prop.Flags),

            CreatePrimitiveField("m_DefValue[0]", "float", AssetValueType.Float, prop.DefValue_0_),
            CreatePrimitiveField("m_DefValue[1]", "float", AssetValueType.Float, prop.DefValue_1_),
            CreatePrimitiveField("m_DefValue[2]", "float", AssetValueType.Float, prop.DefValue_2_),
            CreatePrimitiveField("m_DefValue[3]", "float", AssetValueType.Float, prop.DefValue_3_),

            ConvertSerializedTextureProperty("m_DefTexture", prop.DefTexture, version)
        };

        var template = new AssetTypeTemplateField { Name = name, Type = "SerializedProperty", ValueType = AssetValueType.None, Children = children.Select(c => c.TemplateField).ToList() };
        return new AssetTypeValueField { TemplateField = template, Value = null, Children = children };
    }

    private static AssetTypeValueField ConvertSerializedTextureProperty(string name, ISerializedTextureProperty texProp, UnityVersion version)
    {
        var children = new List<AssetTypeValueField>
            {
                CreateStringField("m_DefaultName", texProp.DefaultName.String),
                CreatePrimitiveField("m_TexDim", "int", AssetValueType.Int32, (int)texProp.TexDim)
            };

        var template = new AssetTypeTemplateField { Name = name, Type = "SerializedTextureProperty", ValueType = AssetValueType.None, Children = children.Select(c => c.TemplateField).ToList() };
        return new AssetTypeValueField { TemplateField = template, Value = null, Children = children };
    }

    private static AssetTypeValueField CreateOffsetsField(string name, IShader shader)
    {
        if (shader.Offsets_AssetList_AssetList_UInt32 != null) return CreateVectorOfVectorOfUInts(name, shader.Offsets_AssetList_AssetList_UInt32);
        if (shader.Offsets_AssetList_UInt32 != null) return CreateVectorOfUIntsForOldUnity(name, shader.Offsets_AssetList_UInt32);
        return CreateDummyVector(name);
    }

    private static AssetTypeValueField CreateLengthsField(string name, IShader shader)
    {
        if (name == "compressedLengths")
        {
            if (shader.CompressedLengths_AssetList_AssetList_UInt32 != null) return CreateVectorOfVectorOfUInts(name, shader.CompressedLengths_AssetList_AssetList_UInt32);
            if (shader.CompressedLengths_AssetList_UInt32 != null) return CreateVectorOfUIntsForOldUnity(name, shader.CompressedLengths_AssetList_UInt32);
        }
        else // decompressedLengths
        {
            if (shader.DecompressedLengths_AssetList_AssetList_UInt32 != null) return CreateVectorOfVectorOfUInts(name, shader.DecompressedLengths_AssetList_AssetList_UInt32);
            if (shader.DecompressedLengths_AssetList_UInt32 != null) return CreateVectorOfUIntsForOldUnity(name, shader.DecompressedLengths_AssetList_UInt32);
        }
        return CreateDummyVector(name);
    }

    #endregion

    #region Field Creation Helpers

    private static AssetTypeValueField CreatePrimitiveField<T>(string name, string typeName, AssetValueType valueType, T value)
    {
        bool isAligned = valueType is AssetValueType.Bool or AssetValueType.Int8 or AssetValueType.UInt8 or AssetValueType.Int16 or AssetValueType.UInt16;
        var template = new AssetTypeTemplateField { Name = name, Type = typeName, ValueType = valueType, HasValue = true, IsAligned = isAligned, Children = new List<AssetTypeTemplateField>() };
        return new AssetTypeValueField { TemplateField = template, Value = new AssetTypeValue(valueType, value), Children = new List<AssetTypeValueField>() };
    }

    private static AssetTypeValueField CreateStringField(string name, Utf8String? uft8String) => CreateStringField(name, uft8String?.String);
    private static AssetTypeValueField CreateStringField(string name, string? value)
    {
        var dataTemplate = new AssetTypeTemplateField { Name = "data", Type = "char", ValueType = AssetValueType.Int8, HasValue = true };
        var sizeTemplate = new AssetTypeTemplateField { Name = "size", Type = "int", ValueType = AssetValueType.Int32, HasValue = true };
        var arrayTemplate = new AssetTypeTemplateField { Name = "Array", Type = "Array", IsArray = true, IsAligned = true, ValueType = AssetValueType.ByteArray, Children = new List<AssetTypeTemplateField> { sizeTemplate, dataTemplate } };
        var template = new AssetTypeTemplateField { Name = name, Type = "string", ValueType = AssetValueType.String, HasValue = true, IsAligned = true, Children = new List<AssetTypeTemplateField> { arrayTemplate } };
        var field = new AssetTypeValueField { TemplateField = template, Children = new List<AssetTypeValueField>() };
        field.Value = new AssetTypeValue(AssetValueType.String, value ?? "");
        return field;
    }

    private static AssetTypeValueField CreateVectorField(string name, List<AssetTypeValueField> children, AssetTypeTemplateField dataTemplate)
    {
        var sizeTemplate = new AssetTypeTemplateField { Name = "size", Type = "int", ValueType = AssetValueType.Int32, HasValue = true };
        var arrayTemplate = new AssetTypeTemplateField { Name = "Array", Type = "Array", IsArray = true, IsAligned = true, ValueType = AssetValueType.Array, Children = new List<AssetTypeTemplateField> { sizeTemplate, dataTemplate } };
        var vectorTemplate = new AssetTypeTemplateField { Name = name, Type = "vector", ValueType = AssetValueType.None, Children = new List<AssetTypeTemplateField> { arrayTemplate } };
        var arrayValueField = new AssetTypeValueField { TemplateField = arrayTemplate, Value = new AssetTypeValue(AssetValueType.Array, new AssetTypeArrayInfo { size = children.Count }), Children = children };
        return new AssetTypeValueField { TemplateField = vectorTemplate, Value = null, Children = new List<AssetTypeValueField> { arrayValueField } };
    }

    private static AssetTypeValueField CreateDummyVector(string name)
    {
        var dataTemplate = new AssetTypeTemplateField { Name = "data", Type = "int", ValueType = AssetValueType.Int32 }; // type doesnt matter
        var sizeTemplate = new AssetTypeTemplateField { Name = "size", Type = "int", ValueType = AssetValueType.Int32, HasValue = true };
        var arrayTemplate = new AssetTypeTemplateField { Name = "Array", Type = "Array", IsArray = true, IsAligned = true, ValueType = AssetValueType.Array, Children = new List<AssetTypeTemplateField> { sizeTemplate, dataTemplate } };
        var vectorTemplate = new AssetTypeTemplateField { Name = name, Type = "vector", ValueType = AssetValueType.None, Children = new List<AssetTypeTemplateField> { arrayTemplate } };

        // For A-code's !field.IsDummy check to work correctly, it must be a vector with an empty array.
        var arrayValueField = new AssetTypeValueField { TemplateField = arrayTemplate, Value = new AssetTypeValue(AssetValueType.Array, new AssetTypeArrayInfo { size = 0 }), Children = new List<AssetTypeValueField>() };
        return new AssetTypeValueField { TemplateField = vectorTemplate, Value = null, Children = new List<AssetTypeValueField> { arrayValueField } };
    }


    private static AssetTypeValueField CreateVectorOfInts(string name, IReadOnlyList<int>? data)
    {
        var dataTemplate = CreatePrimitiveField("data", "int", AssetValueType.Int32, default(int)).TemplateField;
        var children = new List<AssetTypeValueField>();
        if (data != null) children.AddRange(data.Select(u => CreatePrimitiveField("data", "int", AssetValueType.Int32, u)));
        return CreateVectorField(name, children, dataTemplate);
    }

    private static AssetTypeValueField CreateVectorOfUInts(string name, IReadOnlyList<uint>? data)
    {
        var dataTemplate = CreatePrimitiveField("data", "unsigned int", AssetValueType.UInt32, default(uint)).TemplateField;
        var children = new List<AssetTypeValueField>();
        if (data != null) children.AddRange(data.Select(u => CreatePrimitiveField("data", "unsigned int", AssetValueType.UInt32, u)));
        return CreateVectorField(name, children, dataTemplate);
    }

    private static AssetTypeValueField CreateVectorOfUShorts(string name, IReadOnlyList<ushort>? data)
    {
        var dataTemplate = CreatePrimitiveField("data", "unsigned short", AssetValueType.UInt16, default(ushort)).TemplateField;
        var children = new List<AssetTypeValueField>();
        if (data != null) children.AddRange(data.Select(u => CreatePrimitiveField("data", "unsigned short", AssetValueType.UInt16, u)));
        return CreateVectorField(name, children, dataTemplate);
    }

    private static AssetTypeValueField CreateVectorOfVectorOfUInts(string name, IReadOnlyList<IReadOnlyList<uint>>? data)
    {
        var innerVectorTemplate = CreateVectorOfUInts("data", null).TemplateField;
        var children = new List<AssetTypeValueField>();
        if (data != null) children.AddRange(data.Select(list => CreateVectorOfUInts("data", list)));
        return CreateVectorField(name, children, innerVectorTemplate);
    }

    private static AssetTypeValueField CreateVectorOfUIntsForOldUnity(string name, IReadOnlyList<uint>? data)
    {
        var dataTemplate = CreatePrimitiveField("data", "unsigned int", AssetValueType.UInt32, default(uint)).TemplateField;
        var sizeTemplate = new AssetTypeTemplateField { Name = "size", Type = "int", ValueType = AssetValueType.Int32, HasValue = true };
        var arrayTemplate = new AssetTypeTemplateField { Name = name, Type = "Array", IsArray = true, IsAligned = true, ValueType = AssetValueType.Array, Children = new List<AssetTypeTemplateField> { sizeTemplate, dataTemplate } };
        var children = new List<AssetTypeValueField>();
        if (data != null) children.AddRange(data.Select(u => CreatePrimitiveField("data", "unsigned int", AssetValueType.UInt32, u)));
        return new AssetTypeValueField { TemplateField = arrayTemplate, Value = new AssetTypeValue(AssetValueType.Array, new AssetTypeArrayInfo { size = children.Count }), Children = children };
    }

    private static AssetTypeValueField CreateStringVector(string name, IReadOnlyList<string>? data)
    {
        var dataTemplate = CreateStringField("data", "").TemplateField;
        var children = new List<AssetTypeValueField>();
        if (data != null) children.AddRange(data.Select(s => CreateStringField("data", s)));
        return CreateVectorField(name, children, dataTemplate);
    }

    private static AssetTypeValueField CreateVectorOfComplex<T>(string name, IReadOnlyList<T>? data, Func<string, T, AssetTypeValueField> elementCreator) where T : notnull
    {
        var children = new List<AssetTypeValueField>();
        AssetTypeTemplateField dataTemplate;
        if (data == null || data.Count == 0)
        {
            // Create a dummy template as a placeholder so the vector structure is valid
            dataTemplate = new AssetTypeTemplateField { Name = "data", Type = "dummy" };
        }
        else
        {
            dataTemplate = elementCreator("data", data[0]).TemplateField;
            children.AddRange(data.Select(item => elementCreator("data", item)));
        }
        return CreateVectorField(name, children, dataTemplate);
    }

    private static AssetTypeValueField CreateVectorOfVectorOfComplex<T>(string name, IReadOnlyList<IReadOnlyList<T>>? data, Func<string, T, AssetTypeValueField> elementCreator) where T : notnull
    {
        var children = new List<AssetTypeValueField>();
        AssetTypeTemplateField innerVectorTemplate;
        if (data == null || !data.Any())
        {
            // A-code expects a vector of vectors. We need to create the outer vector with an inner template.
            var dummyTemplate = new AssetTypeTemplateField { Name = "data", Type = "dummy" };
            innerVectorTemplate = CreateVectorField("data", new List<AssetTypeValueField>(), dummyTemplate).TemplateField;
        }
        else
        {
            innerVectorTemplate = CreateVectorOfComplex("data", data.FirstOrDefault(), elementCreator).TemplateField;
            children.AddRange(data.Select(list => CreateVectorOfComplex("data", list, elementCreator)));
        }
        return CreateVectorField(name, children, innerVectorTemplate);
    }

    private static (byte[] MasterBlob, List<IReadOnlyList<uint>> Offsets, List<IReadOnlyList<uint>> CompressedLengths, List<IReadOnlyList<uint>> DecompressedLengths)
    ReconstructAndCompressBlobs(IShader shader, AssetRipper.Export.Modules.Shaders.ShaderBlob.ShaderSubProgramBlob[] blobs)
    {
        var allCompressedBlobs = new List<byte[]>();
        var finalOffsets = new List<IReadOnlyList<uint>>();
        var finalCompressedLengths = new List<IReadOnlyList<uint>>();
        var finalDecompressedLengths = new List<IReadOnlyList<uint>>();
        uint currentOffset = 0;

        var version = shader.Collection.Version;
        bool hasSegment = version.GreaterThanOrEquals(2019, 3);
        int entrySize = hasSegment ? 12 : 8;

        foreach (var blob in blobs)
        {
            byte[] finalDecompressedBlob;

            // *** 核心修改点 ***
            if (blob.Entries.Length == 0)
            {
                // 对于没有subprogram的平台，创建一个只包含 "count = 0" 的blob。
                // 这是一个4字节的数组，内容为 [0, 0, 0, 0]。
                finalDecompressedBlob = new byte[4];
                // BitConverter.GetBytes(0) 也可以，但 new byte[4] 默认就是全0。
            }
            else
            {
                // 对于有内容的平台，使用之前的重建逻辑。
                using var decompressedStream = new MemoryStream();
                using var writer = new BinaryWriter(decompressedStream);

                int headerSize = 4 + blob.Entries.Length * entrySize;
                writer.BaseStream.Position = headerSize;

                var segmentStartOffsets = new long[blob.m_decompressedBlobSegments.Count];
                for (int i = 0; i < blob.m_decompressedBlobSegments.Count; i++)
                {
                    segmentStartOffsets[i] = writer.BaseStream.Position;
                    writer.Write(blob.m_decompressedBlobSegments[i]);
                }

                writer.BaseStream.Position = 0;
                writer.Write(blob.Entries.Length);

                foreach (var entry in blob.Entries)
                {
                    long absoluteOffset = segmentStartOffsets[entry.Segment] + entry.Offset;
                    writer.Write((int)absoluteOffset);
                    writer.Write(entry.Length);
                    if (hasSegment)
                    {
                        writer.Write(entry.Segment);
                    }
                }
                finalDecompressedBlob = decompressedStream.ToArray();
            }
            // *** 修改结束 ***

            // 压缩这个平台的blob (无论是包含数据的还是只包含count=0的)
            byte[] compressedPlatformBlob = new byte[LZ4Codec.MaximumOutputSize(finalDecompressedBlob.Length)];
            int compressedSize = LZ4Codec.Encode(finalDecompressedBlob, compressedPlatformBlob, LZ4Level.L00_FAST);
            Array.Resize(ref compressedPlatformBlob, compressedSize);

            // 记录元数据
            allCompressedBlobs.Add(compressedPlatformBlob);
            finalOffsets.Add(new[] { currentOffset });
            finalCompressedLengths.Add(new[] { (uint)compressedPlatformBlob.Length });
            finalDecompressedLengths.Add(new[] { (uint)finalDecompressedBlob.Length });

            currentOffset += (uint)compressedPlatformBlob.Length;
        }

        using var masterBlobStream = new MemoryStream();
        foreach (var compressedBlob in allCompressedBlobs)
        {
            masterBlobStream.Write(compressedBlob, 0, compressedBlob.Length);
        }

        return (masterBlobStream.ToArray(), finalOffsets, finalCompressedLengths, finalDecompressedLengths);
    }

    private static AssetTypeValueField CreateBlobField(string name, byte[]? data)
    {
        // 定义构成 ByteArray 的模板
        var dataTemplate = new AssetTypeTemplateField { Name = "data", Type = "UInt8", ValueType = AssetValueType.UInt8, HasValue = true };
        var sizeTemplate = new AssetTypeTemplateField { Name = "size", Type = "int", ValueType = AssetValueType.Int32, HasValue = true };

        // 1. 创建名为 "Array" 的子字段，它是一个 ByteArray
        var arrayTemplate = new AssetTypeTemplateField
        {
            Name = "Array", // 关键：子字段的名字必须是 "Array"
            Type = "Array",
            IsArray = true,
            IsAligned = false, // 字节数组通常不按4字节对齐
            ValueType = AssetValueType.ByteArray,
            Children = new List<AssetTypeTemplateField> { sizeTemplate, dataTemplate }
        };

        // 2. 创建名为 "compressedBlob" 的父字段，它是一个 "vector"
        var vectorTemplate = new AssetTypeTemplateField
        {
            Name = name, // 这是 "compressedBlob"
            Type = "vector",
            ValueType = AssetValueType.None,
            IsAligned = true, // vector 结构本身是对齐的
            Children = new List<AssetTypeTemplateField> { arrayTemplate } // 它的子节点是上面那个 "Array"
        };

        // 3. 构建实际的字段值
        // 先构建内部的 ByteArray 字段
        var arrayValueField = new AssetTypeValueField
        {
            TemplateField = arrayTemplate,
            Value = new AssetTypeValue(AssetValueType.ByteArray, data ?? Array.Empty<byte>()),
            Children = new List<AssetTypeValueField>()
        };

        // 再构建外部的 vector 字段，并将 arrayValueField 作为其子节点
        return new AssetTypeValueField
        {
            TemplateField = vectorTemplate,
            Value = null,
            Children = new List<AssetTypeValueField> { arrayValueField }
        };
    }


    #endregion
}