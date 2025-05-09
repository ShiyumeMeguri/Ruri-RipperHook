using AssetRipper.Assets;
using AssetRipper.Assets.Generics;
using AssetRipper.IO.Endian;
using AssetRipper.Primitives;
using AssetRipper.SourceGenerated.Subclasses.AssetBundleFullName;
using AssetRipper.SourceGenerated.Subclasses.AssetBundleInfo;
using AssetRipper.SourceGenerated.Subclasses.AssetImporterHashKey;
using AssetRipper.SourceGenerated.Subclasses.AssetInfo;
using AssetRipper.SourceGenerated.Subclasses.AssetTimeStamp;
using AssetRipper.SourceGenerated.Subclasses.ColorRGBAf;
using AssetRipper.SourceGenerated.Subclasses.ComputeShaderKernel;
using AssetRipper.SourceGenerated.Subclasses.ConfigSetting;
using AssetRipper.SourceGenerated.Subclasses.DefaultPreset;
using AssetRipper.SourceGenerated.Subclasses.FastPropertyName;
using AssetRipper.SourceGenerated.Subclasses.GUID;
using AssetRipper.SourceGenerated.Subclasses.Hash128;
using AssetRipper.SourceGenerated.Subclasses.NonAlignedStruct;
using AssetRipper.SourceGenerated.Subclasses.PlatformSettingsData_Editor;
using AssetRipper.SourceGenerated.Subclasses.PlatformSettingsData_Plugin;
using AssetRipper.SourceGenerated.Subclasses.PPtr_AnimatorState;
using AssetRipper.SourceGenerated.Subclasses.PPtr_AnimatorStateMachine;
using AssetRipper.SourceGenerated.Subclasses.PPtr_AnimatorStateTransition;
using AssetRipper.SourceGenerated.Subclasses.PPtr_AnimatorTransition;
using AssetRipper.SourceGenerated.Subclasses.PPtr_Component;
using AssetRipper.SourceGenerated.Subclasses.PPtr_Object;
using AssetRipper.SourceGenerated.Subclasses.PPtr_Shader;
using AssetRipper.SourceGenerated.Subclasses.PPtr_SphereCollider;
using AssetRipper.SourceGenerated.Subclasses.PPtr_Texture;
using AssetRipper.SourceGenerated.Subclasses.PPtr_Texture2D;
using AssetRipper.SourceGenerated.Subclasses.PresetType;
using AssetRipper.SourceGenerated.Subclasses.SampleSettings;
using AssetRipper.SourceGenerated.Subclasses.SecondaryTextureSettings;
using AssetRipper.SourceGenerated.Subclasses.SerializedPlayerSubProgram;
using AssetRipper.SourceGenerated.Subclasses.SpriteAtlasData;
using AssetRipper.SourceGenerated.Subclasses.SpriteRenderData;
using AssetRipper.SourceGenerated.Subclasses.UnityTexEnv;
using AssetRipper.SourceGenerated.Subclasses.Vector2f;
using AssetRipper.SourceGenerated.Subclasses.Vector2Long;
using AssetRipper.SourceGenerated.Subclasses.VideoClipImporterTargetSettings;

namespace Ruri.RipperHook;
internal static class ReadReleaseMethods
{
    public static void ReadRelease_AssetAlign<T>(this T value, ref EndianSpanReader reader) where T : UnityAssetBase
    {
        value.ReadRelease(ref reader);
        reader.Align();
    }

    public static void ReadRelease_Array_Asset<T>(this AssetList<T> value, ref EndianSpanReader reader) where T : UnityAssetBase, new()
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease(ref reader);
        }
    }

    public static void ReadRelease_ArrayAlign_Asset<T>(this AssetList<T> value, ref EndianSpanReader reader) where T : UnityAssetBase, new()
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease(ref reader);
        }
        reader.Align();
    }

    public static void ReadRelease_Pair_Asset_Asset<TKey, TValue>(this AssetPair<TKey, TValue> value, ref EndianSpanReader reader) where TKey : UnityAssetBase, new() where TValue : UnityAssetBase, new()
    {
        value.Key.ReadRelease(ref reader);
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_PairAlign_Asset_Asset<TKey, TValue>(this AssetPair<TKey, TValue> value, ref EndianSpanReader reader) where TKey : UnityAssetBase, new() where TValue : UnityAssetBase, new()
    {
        value.Key.ReadRelease(ref reader);
        value.Value.ReadRelease(ref reader);
        reader.Align();
    }

    public static void ReadRelease_Map_Asset_Asset<TKey, TValue>(this AssetDictionary<TKey, TValue> value, ref EndianSpanReader reader) where TKey : UnityAssetBase, new() where TValue : UnityAssetBase, new()
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Asset_Asset(ref reader);
        }
    }

    public static void ReadRelease_MapAlign_Asset_Asset<TKey, TValue>(this AssetDictionary<TKey, TValue> value, ref EndianSpanReader reader) where TKey : UnityAssetBase, new() where TValue : UnityAssetBase, new()
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Asset_Asset(ref reader);
        }
        reader.Align();
    }

    public static void ReadRelease_Array_Array_Vector2f(this AssetList<AssetList<Vector2f>> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Array_Asset(ref reader);
        }
    }

    public static void ReadRelease_Array_Array_Vector2Long(this AssetList<AssetList<Vector2Long>> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Array_Asset(ref reader);
        }
    }

    public static void ReadRelease_Array_Int32(this AssetList<int> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.Add(reader.ReadInt32());
        }
    }

    public static void ReadRelease_Array_Pair_Int32_PPtr_Component_3_5(this AssetList<AssetPair<int, PPtr_Component_3_5>> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Int32_PPtr_Component_3_5(ref reader);
        }
    }

    public static void ReadRelease_Array_Pair_Int32_PPtr_Component_5(this AssetList<AssetPair<int, PPtr_Component_5>> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Int32_PPtr_Component_5(ref reader);
        }
    }

    public static void ReadRelease_Array_Pair_Int32_Single(this AssetList<AssetPair<int, float>> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Int32_Single(ref reader);
        }
    }

    public static void ReadRelease_Array_Pair_Int32_UInt32(this AssetList<AssetPair<int, uint>> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Int32_UInt32(ref reader);
        }
    }

    public static void ReadRelease_Array_Pair_PPtr_SphereCollider_PPtr_SphereCollider(this AssetList<AssetPair<PPtr_SphereCollider, PPtr_SphereCollider>> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Asset_Asset(ref reader);
        }
    }

    public static void ReadRelease_Array_Pair_Utf8StringAlign_PPtr_Object_3_5(this AssetList<AssetPair<Utf8String, PPtr_Object_3_5>> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_PPtr_Object_3_5(ref reader);
        }
    }

    public static void ReadRelease_Array_Pair_Utf8StringAlign_PPtr_Object_5(this AssetList<AssetPair<Utf8String, PPtr_Object_5>> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_PPtr_Object_5(ref reader);
        }
    }

    public static void ReadRelease_Array_SByte(this AssetList<sbyte> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.Add(reader.ReadSByte());
        }
    }

    public static void ReadRelease_Array_Single(this AssetList<float> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.Add(reader.ReadSingle());
        }
    }

    public static void ReadRelease_Array_UInt16(this AssetList<ushort> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.Add(reader.ReadUInt16());
        }
    }

    public static void ReadRelease_Array_UInt32(this AssetList<uint> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.Add(reader.ReadUInt32());
        }
    }

    public static void ReadRelease_Array_Utf8StringAlign(this AssetList<Utf8String> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.Add(reader.ReadRelease_Utf8StringAlign());
        }
    }

    public static void ReadRelease_ArrayAlign_ArrayAlign_SerializedPlayerSubProgram(this AssetList<AssetList<SerializedPlayerSubProgram>> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_ArrayAlign_Asset(ref reader);
        }
        reader.Align();
    }

    public static void ReadRelease_ArrayAlign_ArrayAlign_UInt32(this AssetList<AssetList<uint>> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_ArrayAlign_UInt32(ref reader);
        }
        reader.Align();
    }

    public static void ReadRelease_ArrayAlign_ArrayAlign_Vector2f(this AssetList<AssetList<Vector2f>> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_ArrayAlign_Asset(ref reader);
        }
        reader.Align();
    }

    public static void ReadRelease_ArrayAlign_ArrayAlign_Vector2Long(this AssetList<AssetList<Vector2Long>> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_ArrayAlign_Asset(ref reader);
        }
        reader.Align();
    }

    public static void ReadRelease_ArrayAlign_Boolean(this AssetList<bool> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.Add(reader.ReadBoolean());
        }
        reader.Align();
    }

    public static byte[] ReadRelease_ArrayAlign_Byte(this ref EndianSpanReader reader)
    {
        int num = reader.ReadInt32();
        byte[] array = ReadByteArray(ref reader, num);
        reader.Align();
        return array;
    }

    public static void ReadRelease_ArrayAlign_Int16(this AssetList<short> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.Add(reader.ReadInt16());
        }
        reader.Align();
    }

    public static void ReadRelease_ArrayAlign_Int32(this AssetList<int> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.Add(reader.ReadInt32());
        }
        reader.Align();
    }

    public static void ReadRelease_ArrayAlign_Int64(this AssetList<long> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.Add(reader.ReadInt64());
        }
        reader.Align();
    }

    public static void ReadRelease_ArrayAlign_Pair_Pair_Int32_Int64_Utf8StringAlign(this AssetList<AssetPair<AssetPair<int, long>, Utf8String>> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Pair_Int32_Int64_Utf8StringAlign(ref reader);
        }
        reader.Align();
    }

    public static void ReadRelease_ArrayAlign_Pair_PPtr_SphereCollider_PPtr_SphereCollider(this AssetList<AssetPair<PPtr_SphereCollider, PPtr_SphereCollider>> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Asset_Asset(ref reader);
        }
        reader.Align();
    }

    public static void ReadRelease_ArrayAlign_Pair_Utf8StringAlign_Boolean(this AssetList<AssetPair<Utf8String, bool>> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_Boolean(ref reader);
        }
        reader.Align();
    }

    public static void ReadRelease_ArrayAlign_Pair_Utf8StringAlign_PPtr_Object_5(this AssetList<AssetPair<Utf8String, PPtr_Object_5>> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_PPtr_Object_5(ref reader);
        }
        reader.Align();
    }

    public static void ReadRelease_ArrayAlign_Pair_Utf8StringAlign_PPtr_Texture_3_5(this AssetList<AssetPair<Utf8String, PPtr_Texture_3_5>> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_PPtr_Texture_3_5(ref reader);
        }
        reader.Align();
    }

    public static void ReadRelease_ArrayAlign_Pair_Utf8StringAlign_PPtr_Texture_5(this AssetList<AssetPair<Utf8String, PPtr_Texture_5>> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_PPtr_Texture_5(ref reader);
        }
        reader.Align();
    }

    public static void ReadRelease_ArrayAlign_Pair_Utf8StringAlign_UInt32(this AssetList<AssetPair<Utf8String, uint>> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_UInt32(ref reader);
        }
        reader.Align();
    }

    public static void ReadRelease_ArrayAlign_Single(this AssetList<float> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.Add(reader.ReadSingle());
        }
        reader.Align();
    }

    public static void ReadRelease_ArrayAlign_UInt16(this AssetList<ushort> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.Add(reader.ReadUInt16());
        }
        reader.Align();
    }

    public static void ReadRelease_ArrayAlign_UInt32(this AssetList<uint> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.Add(reader.ReadUInt32());
        }
        reader.Align();
    }

    public static void ReadRelease_ArrayAlign_Utf8StringAlign(this AssetList<Utf8String> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.Add(reader.ReadRelease_Utf8StringAlign());
        }
        reader.Align();
    }

    public static bool ReadRelease_BooleanAlign(this ref EndianSpanReader reader)
    {
        bool flag = reader.ReadBoolean();
        reader.Align();
        return flag;
    }

    public static byte ReadRelease_ByteAlign(this ref EndianSpanReader reader)
    {
        byte b = reader.ReadByte();
        reader.Align();
        return b;
    }

    public static short ReadRelease_Int16Align(this ref EndianSpanReader reader)
    {
        short num = reader.ReadInt16();
        reader.Align();
        return num;
    }

    public static int ReadRelease_Int32Align(this ref EndianSpanReader reader)
    {
        int num = reader.ReadInt32();
        reader.Align();
        return num;
    }

    public static long ReadRelease_Int64Align(this ref EndianSpanReader reader)
    {
        long num = reader.ReadInt64();
        reader.Align();
        return num;
    }

    public static void ReadRelease_Map_AssetImporterHashKey_UInt32(this AssetDictionary<AssetImporterHashKey, uint> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_AssetImporterHashKey_UInt32(ref reader);
        }
    }

    public static void ReadRelease_Map_FastPropertyName_Single(this AssetDictionary<FastPropertyName, float> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_FastPropertyName_Single(ref reader);
        }
    }

    public static void ReadRelease_Map_GUID_Utf8StringAlign(this AssetDictionary<GUID, Utf8String> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_GUID_Utf8StringAlign(ref reader);
        }
    }

    public static void ReadRelease_Map_Hash128_5_Int32(this AssetDictionary<Hash128_5, int> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Hash128_5_Int32(ref reader);
        }
    }

    public static void ReadRelease_Map_Int32_AssetBundleFullName(this AssetDictionary<int, AssetBundleFullName> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Int32_AssetBundleFullName(ref reader);
        }
    }

    public static void ReadRelease_Map_Int32_AssetBundleInfo(this AssetDictionary<int, AssetBundleInfo> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Int32_AssetBundleInfo(ref reader);
        }
    }

    public static void ReadRelease_Map_Int32_Hash128_5(this AssetDictionary<int, Hash128_5> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Int32_Hash128_5(ref reader);
        }
    }

    public static void ReadRelease_Map_Int32_Int32(this AssetDictionary<int, int> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Int32_Int32(ref reader);
        }
    }

    public static void ReadRelease_Map_Int32_SampleSettings_2022_2_0_a17(this AssetDictionary<int, SampleSettings_2022_2_0_a17> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Int32_SampleSettings_2022_2_0_a17(ref reader);
        }
    }

    public static void ReadRelease_Map_Int32_SampleSettings_5(this AssetDictionary<int, SampleSettings_5> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Int32_SampleSettings_5(ref reader);
        }
    }

    public static void ReadRelease_Map_Int32_UInt32(this AssetDictionary<int, uint> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Int32_UInt32(ref reader);
        }
    }

    public static void ReadRelease_Map_Int32_Utf8StringAlign(this AssetDictionary<int, Utf8String> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Int32_Utf8StringAlign(ref reader);
        }
    }

    public static void ReadRelease_Map_Int32_VideoClipImporterTargetSettings(this AssetDictionary<int, VideoClipImporterTargetSettings> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Int32_VideoClipImporterTargetSettings(ref reader);
        }
    }

    public static void ReadRelease_Map_Int64_Utf8StringAlign(this AssetDictionary<long, Utf8String> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Int64_Utf8StringAlign(ref reader);
        }
    }

    public static void ReadRelease_Map_Pair_GUID_Int32_SpriteRenderData_4_3(this AssetDictionary<AssetPair<GUID, int>, SpriteRenderData_4_3> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Pair_GUID_Int32_SpriteRenderData_4_3(ref reader);
        }
    }

    public static void ReadRelease_Map_Pair_GUID_Int32_SpriteRenderData_4_5(this AssetDictionary<AssetPair<GUID, int>, SpriteRenderData_4_5> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Pair_GUID_Int32_SpriteRenderData_4_5(ref reader);
        }
    }

    public static void ReadRelease_Map_Pair_GUID_Int64_SpriteAtlasData_2017(this AssetDictionary<AssetPair<GUID, long>, SpriteAtlasData_2017> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Pair_GUID_Int64_SpriteAtlasData_2017(ref reader);
        }
    }

    public static void ReadRelease_Map_Pair_GUID_Int64_SpriteAtlasData_2017_2(this AssetDictionary<AssetPair<GUID, long>, SpriteAtlasData_2017_2> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Pair_GUID_Int64_SpriteAtlasData_2017_2(ref reader);
        }
    }

    public static void ReadRelease_Map_Pair_GUID_Int64_SpriteAtlasData_2017_2_0_b9(this AssetDictionary<AssetPair<GUID, long>, SpriteAtlasData_2017_2_0_b9> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Pair_GUID_Int64_SpriteAtlasData_2017_2_0_b9(ref reader);
        }
    }

    public static void ReadRelease_Map_Pair_GUID_Int64_SpriteAtlasData_2020_2(this AssetDictionary<AssetPair<GUID, long>, SpriteAtlasData_2020_2> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Pair_GUID_Int64_SpriteAtlasData_2020_2(ref reader);
        }
    }

    public static void ReadRelease_Map_Pair_GUID_Int64_SpriteRenderData_2017(this AssetDictionary<AssetPair<GUID, long>, SpriteRenderData_2017> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Pair_GUID_Int64_SpriteRenderData_2017(ref reader);
        }
    }

    public static void ReadRelease_Map_Pair_GUID_Int64_SpriteRenderData_2017_b4(this AssetDictionary<AssetPair<GUID, long>, SpriteRenderData_2017_1_0_b4> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Pair_GUID_Int64_SpriteRenderData_2017_b4(ref reader);
        }
    }

    public static void ReadRelease_Map_Pair_GUID_Int64_SpriteRenderData_2017_3(this AssetDictionary<AssetPair<GUID, long>, SpriteRenderData_2017_3> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Pair_GUID_Int64_SpriteRenderData_2017_3(ref reader);
        }
    }

    public static void ReadRelease_Map_Pair_GUID_Int64_SpriteRenderData_2018(this AssetDictionary<AssetPair<GUID, long>, SpriteRenderData_2018> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Pair_GUID_Int64_SpriteRenderData_2018(ref reader);
        }
    }

    public static void ReadRelease_Map_Pair_GUID_Int64_SpriteRenderData_2018_2(this AssetDictionary<AssetPair<GUID, long>, SpriteRenderData_2018_2> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Pair_GUID_Int64_SpriteRenderData_2018_2(ref reader);
        }
    }

    public static void ReadRelease_Map_Pair_GUID_Int64_SpriteRenderData_2019(this AssetDictionary<AssetPair<GUID, long>, SpriteRenderData_2019> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Pair_GUID_Int64_SpriteRenderData_2019(ref reader);
        }
    }

    public static void ReadRelease_Map_Pair_GUID_Int64_SpriteRenderData_5(this AssetDictionary<AssetPair<GUID, long>, SpriteRenderData_5> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Pair_GUID_Int64_SpriteRenderData_5(ref reader);
        }
    }

    public static void ReadRelease_Map_Pair_GUID_Int64_SpriteRenderData_5_2(this AssetDictionary<AssetPair<GUID, long>, SpriteRenderData_5_2> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Pair_GUID_Int64_SpriteRenderData_5_2(ref reader);
        }
    }

    public static void ReadRelease_Map_Pair_GUID_Int64_SpriteRenderData_5_4_6(this AssetDictionary<AssetPair<GUID, long>, SpriteRenderData_5_4_6> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Pair_GUID_Int64_SpriteRenderData_5_4_6(ref reader);
        }
    }

    public static void ReadRelease_Map_Pair_GUID_Int64_SpriteRenderData_5_5(this AssetDictionary<AssetPair<GUID, long>, SpriteRenderData_5_5> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Pair_GUID_Int64_SpriteRenderData_5_5(ref reader);
        }
    }

    public static void ReadRelease_Map_Pair_GUID_Int64_SpriteRenderData_5_5_3(this AssetDictionary<AssetPair<GUID, long>, SpriteRenderData_5_5_3> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Pair_GUID_Int64_SpriteRenderData_5_5_3(ref reader);
        }
    }

    public static void ReadRelease_Map_Pair_GUID_Int64_SpriteRenderData_5_6(this AssetDictionary<AssetPair<GUID, long>, SpriteRenderData_5_6> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Pair_GUID_Int64_SpriteRenderData_5_6(ref reader);
        }
    }

    public static void ReadRelease_Map_Pair_GUID_Int64_SpriteRenderData_5_6_0_b10(this AssetDictionary<AssetPair<GUID, long>, SpriteRenderData_5_6_0_b10> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Pair_GUID_Int64_SpriteRenderData_5_6_0_b10(ref reader);
        }
    }

    public static void ReadRelease_Map_Pair_GUID_Int64_SpriteRenderData_5_6_2(this AssetDictionary<AssetPair<GUID, long>, SpriteRenderData_5_6_2> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Pair_GUID_Int64_SpriteRenderData_5_6_2(ref reader);
        }
    }

    public static void ReadRelease_Map_Pair_UInt16_UInt16_Single(this AssetDictionary<AssetPair<ushort, ushort>, float> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Pair_UInt16_UInt16_Single(ref reader);
        }
    }

    public static void ReadRelease_Map_Pair_Utf8StringAlign_Utf8StringAlign_PlatformSettingsData_Plugin(this AssetDictionary<AssetPair<Utf8String, Utf8String>, PlatformSettingsData_Plugin> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Pair_Utf8StringAlign_Utf8StringAlign_PlatformSettingsData_Plugin(ref reader);
        }
    }

    public static void ReadRelease_Map_PPtr_AnimatorState_4_Array_PPtr_AnimatorStateTransition_4(this AssetDictionary<PPtr_AnimatorState_4, AssetList<PPtr_AnimatorStateTransition_4>> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_PPtr_AnimatorState_4_Array_PPtr_AnimatorStateTransition_4(ref reader);
        }
    }

    public static void ReadRelease_Map_PPtr_AnimatorStateMachine_5_Array_PPtr_AnimatorTransition(this AssetDictionary<PPtr_AnimatorStateMachine_5, AssetList<PPtr_AnimatorTransition>> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_PPtr_AnimatorStateMachine_5_Array_PPtr_AnimatorTransition(ref reader);
        }
    }

    public static void ReadRelease_Map_PPtr_AnimatorStateMachine_5_ArrayAlign_PPtr_AnimatorTransition(this AssetDictionary<PPtr_AnimatorStateMachine_5, AssetList<PPtr_AnimatorTransition>> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_PPtr_AnimatorStateMachine_5_ArrayAlign_PPtr_AnimatorTransition(ref reader);
        }
    }

    public static void ReadRelease_Map_PPtr_Shader_3_5_Utf8StringAlign(this AssetDictionary<PPtr_Shader_3_5, Utf8String> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_PPtr_Shader_3_5_Utf8StringAlign(ref reader);
        }
    }

    public static void ReadRelease_Map_PPtr_Shader_5_Utf8StringAlign(this AssetDictionary<PPtr_Shader_5, Utf8String> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_PPtr_Shader_5_Utf8StringAlign(ref reader);
        }
    }

    public static void ReadRelease_Map_PresetType_ArrayAlign_DefaultPreset_2019_3_0_a10(this AssetDictionary<PresetType, AssetList<DefaultPreset_2019_3_0_a10>> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_PresetType_ArrayAlign_DefaultPreset_2019_3_0_a10(ref reader);
        }
    }

    public static void ReadRelease_Map_PresetType_ArrayAlign_DefaultPreset_2020_1_0_a23(this AssetDictionary<PresetType, AssetList<DefaultPreset_2020_1_0_a23>> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_PresetType_ArrayAlign_DefaultPreset_2020_1_0_a23(ref reader);
        }
    }

    public static void ReadRelease_Map_UInt32_Utf8StringAlign(this AssetDictionary<uint, Utf8String> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_UInt32_Utf8StringAlign(ref reader);
        }
    }

    public static void ReadRelease_Map_Utf8StringAlign_Array_Utf8StringAlign(this AssetDictionary<Utf8String, AssetList<Utf8String>> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_Array_Utf8StringAlign(ref reader);
        }
    }

    public static void ReadRelease_Map_Utf8StringAlign_AssetInfo_3_5(this AssetDictionary<Utf8String, AssetInfo_3_5> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_AssetInfo_3_5(ref reader);
        }
    }

    public static void ReadRelease_Map_Utf8StringAlign_AssetInfo_5(this AssetDictionary<Utf8String, AssetInfo_5> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_AssetInfo_5(ref reader);
        }
    }

    public static void ReadRelease_Map_Utf8StringAlign_AssetTimeStamp(this AssetDictionary<Utf8String, AssetTimeStamp> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_AssetTimeStamp(ref reader);
        }
    }

    public static void ReadRelease_Map_Utf8StringAlign_ColorRGBAf(this AssetDictionary<Utf8String, ColorRGBAf> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_ColorRGBAf(ref reader);
        }
    }

    public static void ReadRelease_Map_Utf8StringAlign_ComputeShaderKernel_2020_1_0_a17(this AssetDictionary<Utf8String, ComputeShaderKernel_2020_1_0_a17> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_ComputeShaderKernel_2020_1_0_a17(ref reader);
        }
    }

    public static void ReadRelease_Map_Utf8StringAlign_ComputeShaderKernel_2020_1_0_a9(this AssetDictionary<Utf8String, ComputeShaderKernel_2020_1_0_a9> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_ComputeShaderKernel_2020_1_0_a9(ref reader);
        }
    }

    public static void ReadRelease_Map_Utf8StringAlign_ComputeShaderKernel_2020_2_0_a15(this AssetDictionary<Utf8String, ComputeShaderKernel_2020_2_0_a15> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_ComputeShaderKernel_2020_2_0_a15(ref reader);
        }
    }

    public static void ReadRelease_Map_Utf8StringAlign_ComputeShaderKernel_2020_3_2(this AssetDictionary<Utf8String, ComputeShaderKernel_2020_3_2> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_ComputeShaderKernel_2020_3_2(ref reader);
        }
    }

    public static void ReadRelease_Map_Utf8StringAlign_ComputeShaderKernel_2021(this AssetDictionary<Utf8String, ComputeShaderKernel_2021> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_ComputeShaderKernel_2021(ref reader);
        }
    }

    public static void ReadRelease_Map_Utf8StringAlign_ComputeShaderKernel_2021_1_0_b7(this AssetDictionary<Utf8String, ComputeShaderKernel_2021_1_0_b7> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_ComputeShaderKernel_2021_1_0_b7(ref reader);
        }
    }

    public static void ReadRelease_Map_Utf8StringAlign_ConfigSetting(this AssetDictionary<Utf8String, ConfigSetting> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_ConfigSetting(ref reader);
        }
    }

    public static void ReadRelease_Map_Utf8StringAlign_GUID(this AssetDictionary<Utf8String, GUID> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_GUID(ref reader);
        }
    }

    public static void ReadRelease_Map_Utf8StringAlign_Int32(this AssetDictionary<Utf8String, int> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_Int32(ref reader);
        }
    }

    public static void ReadRelease_Map_Utf8StringAlign_Int64(this AssetDictionary<Utf8String, long> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_Int64(ref reader);
        }
    }

    public static void ReadRelease_Map_Utf8StringAlign_PlatformSettingsData_Editor(this AssetDictionary<Utf8String, PlatformSettingsData_Editor> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_PlatformSettingsData_Editor(ref reader);
        }
    }

    public static void ReadRelease_Map_Utf8StringAlign_PlatformSettingsData_Plugin(this AssetDictionary<Utf8String, PlatformSettingsData_Plugin> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_PlatformSettingsData_Plugin(ref reader);
        }
    }

    public static void ReadRelease_Map_Utf8StringAlign_PPtr_Object_3_5(this AssetDictionary<Utf8String, PPtr_Object_3_5> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_PPtr_Object_3_5(ref reader);
        }
    }

    public static void ReadRelease_Map_Utf8StringAlign_PPtr_Object_5(this AssetDictionary<Utf8String, PPtr_Object_5> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_PPtr_Object_5(ref reader);
        }
    }

    public static void ReadRelease_Map_Utf8StringAlign_PPtr_Texture_5(this AssetDictionary<Utf8String, PPtr_Texture_5> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_PPtr_Texture_5(ref reader);
        }
    }

    public static void ReadRelease_Map_Utf8StringAlign_PPtr_Texture2D_3_5(this AssetDictionary<Utf8String, PPtr_Texture2D_3_5> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_PPtr_Texture2D_3_5(ref reader);
        }
    }

    public static void ReadRelease_Map_Utf8StringAlign_PPtr_Texture2D_5(this AssetDictionary<Utf8String, PPtr_Texture2D_5> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_PPtr_Texture2D_5(ref reader);
        }
    }

    public static void ReadRelease_Map_Utf8StringAlign_SampleSettings_2022_2_0_a17(this AssetDictionary<Utf8String, SampleSettings_2022_2_0_a17> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_SampleSettings_2022_2_0_a17(ref reader);
        }
    }

    public static void ReadRelease_Map_Utf8StringAlign_SecondaryTextureSettings_2020_2_0_a12(this AssetDictionary<Utf8String, SecondaryTextureSettings_2020_2_0_a12> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_SecondaryTextureSettings_2020_2_0_a12(ref reader);
        }
    }

    public static void ReadRelease_Map_Utf8StringAlign_SecondaryTextureSettings_2022_2_20(this AssetDictionary<Utf8String, SecondaryTextureSettings_2022_2_20> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_SecondaryTextureSettings_2022_2_20(ref reader);
        }
    }

    public static void ReadRelease_Map_Utf8StringAlign_SecondaryTextureSettings_2023(this AssetDictionary<Utf8String, SecondaryTextureSettings_2023> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_SecondaryTextureSettings_2023(ref reader);
        }
    }

    public static void ReadRelease_Map_Utf8StringAlign_SecondaryTextureSettings_2023_2_0_a12(this AssetDictionary<Utf8String, SecondaryTextureSettings_2023_2_0_a12> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_SecondaryTextureSettings_2023_2_0_a12(ref reader);
        }
    }

    public static void ReadRelease_Map_Utf8StringAlign_SecondaryTextureSettings_2023_3_0_a11(this AssetDictionary<Utf8String, SecondaryTextureSettings_2023_3_0_a11> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_SecondaryTextureSettings_2023_3_0_a11(ref reader);
        }
    }

    public static void ReadRelease_Map_Utf8StringAlign_Single(this AssetDictionary<Utf8String, float> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_Single(ref reader);
        }
    }

    public static void ReadRelease_Map_Utf8StringAlign_UnityTexEnv_5(this AssetDictionary<Utf8String, UnityTexEnv_5> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_UnityTexEnv_5(ref reader);
        }
    }

    public static void ReadRelease_Map_Utf8StringAlign_Utf8StringAlign(this AssetDictionary<Utf8String, Utf8String> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_Utf8StringAlign(ref reader);
        }
    }

    public static void ReadRelease_Map_Utf8StringAlign_VideoClipImporterTargetSettings(this AssetDictionary<Utf8String, VideoClipImporterTargetSettings> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_VideoClipImporterTargetSettings(ref reader);
        }
    }

    public static void ReadRelease_MapAlign_Utf8StringAlign_Boolean(this AssetDictionary<Utf8String, bool> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_Boolean(ref reader);
        }
        reader.Align();
    }

    public static void ReadRelease_MapAlign_Utf8StringAlign_Int32(this AssetDictionary<Utf8String, int> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_Int32(ref reader);
        }
        reader.Align();
    }

    public static void ReadRelease_MapAlign_Utf8StringAlign_NonAlignedStruct(this AssetDictionary<Utf8String, NonAlignedStruct> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_NonAlignedStruct(ref reader);
        }
        reader.Align();
    }

    public static void ReadRelease_MapAlign_Utf8StringAlign_SecondaryTextureSettings_2020_2(this AssetDictionary<Utf8String, SecondaryTextureSettings_2020_2> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_SecondaryTextureSettings_2020_2(ref reader);
        }
        reader.Align();
    }

    public static void ReadRelease_MapAlign_Utf8StringAlign_SecondaryTextureSettings_2020_2_0_a12(this AssetDictionary<Utf8String, SecondaryTextureSettings_2020_2_0_a12> value, ref EndianSpanReader reader)
    {
        value.Clear();
        int num = reader.ReadInt32();
        for (int i = 0; i < num; i++)
        {
            value.AddNew().ReadRelease_Pair_Utf8StringAlign_SecondaryTextureSettings_2020_2_0_a12(ref reader);
        }
        reader.Align();
    }

    public static void ReadRelease_Pair_AssetImporterHashKey_UInt32(this AssetPair<AssetImporterHashKey, uint> value, ref EndianSpanReader reader)
    {
        value.Key.ReadRelease(ref reader);
        value.Value = reader.ReadUInt32();
    }

    public static void ReadRelease_Pair_FastPropertyName_Single(this AssetPair<FastPropertyName, float> value, ref EndianSpanReader reader)
    {
        value.Key.ReadRelease(ref reader);
        value.Value = reader.ReadSingle();
    }

    public static void ReadRelease_Pair_GUID_Int32(this AssetPair<GUID, int> value, ref EndianSpanReader reader)
    {
        value.Key.ReadRelease(ref reader);
        value.Value = reader.ReadInt32();
    }

    public static void ReadRelease_Pair_GUID_Int64(this AssetPair<GUID, long> value, ref EndianSpanReader reader)
    {
        value.Key.ReadRelease(ref reader);
        value.Value = reader.ReadInt64();
    }

    public static void ReadRelease_Pair_GUID_Utf8StringAlign(this AssetPair<GUID, Utf8String> value, ref EndianSpanReader reader)
    {
        value.Key.ReadRelease(ref reader);
        value.Value = reader.ReadRelease_Utf8StringAlign();
    }

    public static void ReadRelease_Pair_Hash128_5_Int32(this AssetPair<Hash128_5, int> value, ref EndianSpanReader reader)
    {
        value.Key.ReadRelease(ref reader);
        value.Value = reader.ReadInt32();
    }

    public static void ReadRelease_Pair_Int32_AssetBundleFullName(this AssetPair<int, AssetBundleFullName> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadInt32();
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Int32_AssetBundleInfo(this AssetPair<int, AssetBundleInfo> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadInt32();
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Int32_Hash128_5(this AssetPair<int, Hash128_5> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadInt32();
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Int32_Int32(this AssetPair<int, int> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadInt32();
        value.Value = reader.ReadInt32();
    }

    public static void ReadRelease_Pair_Int32_Int64(this AssetPair<int, long> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadInt32();
        value.Value = reader.ReadInt64();
    }

    public static void ReadRelease_Pair_Int32_PPtr_Component_3_5(this AssetPair<int, PPtr_Component_3_5> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadInt32();
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Int32_PPtr_Component_5(this AssetPair<int, PPtr_Component_5> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadInt32();
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Int32_SampleSettings_2022_2_0_a17(this AssetPair<int, SampleSettings_2022_2_0_a17> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadInt32();
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Int32_SampleSettings_5(this AssetPair<int, SampleSettings_5> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadInt32();
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Int32_Single(this AssetPair<int, float> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadInt32();
        value.Value = reader.ReadSingle();
    }

    public static void ReadRelease_Pair_Int32_UInt32(this AssetPair<int, uint> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadInt32();
        value.Value = reader.ReadUInt32();
    }

    public static void ReadRelease_Pair_Int32_Utf8StringAlign(this AssetPair<int, Utf8String> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadInt32();
        value.Value = reader.ReadRelease_Utf8StringAlign();
    }

    public static void ReadRelease_Pair_Int32_VideoClipImporterTargetSettings(this AssetPair<int, VideoClipImporterTargetSettings> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadInt32();
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Int64_Utf8StringAlign(this AssetPair<long, Utf8String> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadInt64();
        value.Value = reader.ReadRelease_Utf8StringAlign();
    }

    public static void ReadRelease_Pair_Pair_GUID_Int32_SpriteRenderData_4_3(this AssetPair<AssetPair<GUID, int>, SpriteRenderData_4_3> value, ref EndianSpanReader reader)
    {
        value.Key.ReadRelease_Pair_GUID_Int32(ref reader);
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Pair_GUID_Int32_SpriteRenderData_4_5(this AssetPair<AssetPair<GUID, int>, SpriteRenderData_4_5> value, ref EndianSpanReader reader)
    {
        value.Key.ReadRelease_Pair_GUID_Int32(ref reader);
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Pair_GUID_Int64_SpriteAtlasData_2017(this AssetPair<AssetPair<GUID, long>, SpriteAtlasData_2017> value, ref EndianSpanReader reader)
    {
        value.Key.ReadRelease_Pair_GUID_Int64(ref reader);
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Pair_GUID_Int64_SpriteAtlasData_2017_2(this AssetPair<AssetPair<GUID, long>, SpriteAtlasData_2017_2> value, ref EndianSpanReader reader)
    {
        value.Key.ReadRelease_Pair_GUID_Int64(ref reader);
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Pair_GUID_Int64_SpriteAtlasData_2017_2_0_b9(this AssetPair<AssetPair<GUID, long>, SpriteAtlasData_2017_2_0_b9> value, ref EndianSpanReader reader)
    {
        value.Key.ReadRelease_Pair_GUID_Int64(ref reader);
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Pair_GUID_Int64_SpriteAtlasData_2020_2(this AssetPair<AssetPair<GUID, long>, SpriteAtlasData_2020_2> value, ref EndianSpanReader reader)
    {
        value.Key.ReadRelease_Pair_GUID_Int64(ref reader);
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Pair_GUID_Int64_SpriteRenderData_2017(this AssetPair<AssetPair<GUID, long>, SpriteRenderData_2017> value, ref EndianSpanReader reader)
    {
        value.Key.ReadRelease_Pair_GUID_Int64(ref reader);
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Pair_GUID_Int64_SpriteRenderData_2017_b4(this AssetPair<AssetPair<GUID, long>, SpriteRenderData_2017_1_0_b4> value, ref EndianSpanReader reader)
    {
        value.Key.ReadRelease_Pair_GUID_Int64(ref reader);
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Pair_GUID_Int64_SpriteRenderData_2017_3(this AssetPair<AssetPair<GUID, long>, SpriteRenderData_2017_3> value, ref EndianSpanReader reader)
    {
        value.Key.ReadRelease_Pair_GUID_Int64(ref reader);
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Pair_GUID_Int64_SpriteRenderData_2018(this AssetPair<AssetPair<GUID, long>, SpriteRenderData_2018> value, ref EndianSpanReader reader)
    {
        value.Key.ReadRelease_Pair_GUID_Int64(ref reader);
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Pair_GUID_Int64_SpriteRenderData_2018_2(this AssetPair<AssetPair<GUID, long>, SpriteRenderData_2018_2> value, ref EndianSpanReader reader)
    {
        value.Key.ReadRelease_Pair_GUID_Int64(ref reader);
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Pair_GUID_Int64_SpriteRenderData_2019(this AssetPair<AssetPair<GUID, long>, SpriteRenderData_2019> value, ref EndianSpanReader reader)
    {
        value.Key.ReadRelease_Pair_GUID_Int64(ref reader);
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Pair_GUID_Int64_SpriteRenderData_5(this AssetPair<AssetPair<GUID, long>, SpriteRenderData_5> value, ref EndianSpanReader reader)
    {
        value.Key.ReadRelease_Pair_GUID_Int64(ref reader);
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Pair_GUID_Int64_SpriteRenderData_5_2(this AssetPair<AssetPair<GUID, long>, SpriteRenderData_5_2> value, ref EndianSpanReader reader)
    {
        value.Key.ReadRelease_Pair_GUID_Int64(ref reader);
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Pair_GUID_Int64_SpriteRenderData_5_4_6(this AssetPair<AssetPair<GUID, long>, SpriteRenderData_5_4_6> value, ref EndianSpanReader reader)
    {
        value.Key.ReadRelease_Pair_GUID_Int64(ref reader);
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Pair_GUID_Int64_SpriteRenderData_5_5(this AssetPair<AssetPair<GUID, long>, SpriteRenderData_5_5> value, ref EndianSpanReader reader)
    {
        value.Key.ReadRelease_Pair_GUID_Int64(ref reader);
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Pair_GUID_Int64_SpriteRenderData_5_5_3(this AssetPair<AssetPair<GUID, long>, SpriteRenderData_5_5_3> value, ref EndianSpanReader reader)
    {
        value.Key.ReadRelease_Pair_GUID_Int64(ref reader);
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Pair_GUID_Int64_SpriteRenderData_5_6(this AssetPair<AssetPair<GUID, long>, SpriteRenderData_5_6> value, ref EndianSpanReader reader)
    {
        value.Key.ReadRelease_Pair_GUID_Int64(ref reader);
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Pair_GUID_Int64_SpriteRenderData_5_6_0_b10(this AssetPair<AssetPair<GUID, long>, SpriteRenderData_5_6_0_b10> value, ref EndianSpanReader reader)
    {
        value.Key.ReadRelease_Pair_GUID_Int64(ref reader);
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Pair_GUID_Int64_SpriteRenderData_5_6_2(this AssetPair<AssetPair<GUID, long>, SpriteRenderData_5_6_2> value, ref EndianSpanReader reader)
    {
        value.Key.ReadRelease_Pair_GUID_Int64(ref reader);
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Pair_Int32_Int64_Utf8StringAlign(this AssetPair<AssetPair<int, long>, Utf8String> value, ref EndianSpanReader reader)
    {
        value.Key.ReadRelease_Pair_Int32_Int64(ref reader);
        value.Value = reader.ReadRelease_Utf8StringAlign();
    }

    public static void ReadRelease_Pair_Pair_UInt16_UInt16_Single(this AssetPair<AssetPair<ushort, ushort>, float> value, ref EndianSpanReader reader)
    {
        value.Key.ReadRelease_Pair_UInt16_UInt16(ref reader);
        value.Value = reader.ReadSingle();
    }

    public static void ReadRelease_Pair_Pair_Utf8StringAlign_Utf8StringAlign_PlatformSettingsData_Plugin(this AssetPair<AssetPair<Utf8String, Utf8String>, PlatformSettingsData_Plugin> value, ref EndianSpanReader reader)
    {
        value.Key.ReadRelease_Pair_Utf8StringAlign_Utf8StringAlign(ref reader);
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_PPtr_AnimatorState_4_Array_PPtr_AnimatorStateTransition_4(this AssetPair<PPtr_AnimatorState_4, AssetList<PPtr_AnimatorStateTransition_4>> value, ref EndianSpanReader reader)
    {
        value.Key.ReadRelease(ref reader);
        value.Value.ReadRelease_Array_Asset(ref reader);
    }

    public static void ReadRelease_Pair_PPtr_AnimatorStateMachine_5_Array_PPtr_AnimatorTransition(this AssetPair<PPtr_AnimatorStateMachine_5, AssetList<PPtr_AnimatorTransition>> value, ref EndianSpanReader reader)
    {
        value.Key.ReadRelease(ref reader);
        value.Value.ReadRelease_Array_Asset(ref reader);
    }

    public static void ReadRelease_Pair_PPtr_AnimatorStateMachine_5_ArrayAlign_PPtr_AnimatorTransition(this AssetPair<PPtr_AnimatorStateMachine_5, AssetList<PPtr_AnimatorTransition>> value, ref EndianSpanReader reader)
    {
        value.Key.ReadRelease(ref reader);
        value.Value.ReadRelease_ArrayAlign_Asset(ref reader);
    }

    public static void ReadRelease_Pair_PPtr_Shader_3_5_Utf8StringAlign(this AssetPair<PPtr_Shader_3_5, Utf8String> value, ref EndianSpanReader reader)
    {
        value.Key.ReadRelease(ref reader);
        value.Value = reader.ReadRelease_Utf8StringAlign();
    }

    public static void ReadRelease_Pair_PPtr_Shader_5_Utf8StringAlign(this AssetPair<PPtr_Shader_5, Utf8String> value, ref EndianSpanReader reader)
    {
        value.Key.ReadRelease(ref reader);
        value.Value = reader.ReadRelease_Utf8StringAlign();
    }

    public static void ReadRelease_Pair_PresetType_ArrayAlign_DefaultPreset_2019_3_0_a10(this AssetPair<PresetType, AssetList<DefaultPreset_2019_3_0_a10>> value, ref EndianSpanReader reader)
    {
        value.Key.ReadRelease(ref reader);
        value.Value.ReadRelease_ArrayAlign_Asset(ref reader);
    }

    public static void ReadRelease_Pair_PresetType_ArrayAlign_DefaultPreset_2020_1_0_a23(this AssetPair<PresetType, AssetList<DefaultPreset_2020_1_0_a23>> value, ref EndianSpanReader reader)
    {
        value.Key.ReadRelease(ref reader);
        value.Value.ReadRelease_ArrayAlign_Asset(ref reader);
    }

    public static void ReadRelease_Pair_UInt16_UInt16(this AssetPair<ushort, ushort> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadUInt16();
        value.Value = reader.ReadUInt16();
    }

    public static void ReadRelease_Pair_UInt32_Utf8StringAlign(this AssetPair<uint, Utf8String> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadUInt32();
        value.Value = reader.ReadRelease_Utf8StringAlign();
    }

    public static void ReadRelease_Pair_Utf8StringAlign_Array_Utf8StringAlign(this AssetPair<Utf8String, AssetList<Utf8String>> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadRelease_Utf8StringAlign();
        value.Value.ReadRelease_Array_Utf8StringAlign(ref reader);
    }

    public static void ReadRelease_Pair_Utf8StringAlign_AssetInfo_3_5(this AssetPair<Utf8String, AssetInfo_3_5> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadRelease_Utf8StringAlign();
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Utf8StringAlign_AssetInfo_5(this AssetPair<Utf8String, AssetInfo_5> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadRelease_Utf8StringAlign();
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Utf8StringAlign_AssetTimeStamp(this AssetPair<Utf8String, AssetTimeStamp> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadRelease_Utf8StringAlign();
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Utf8StringAlign_Boolean(this AssetPair<Utf8String, bool> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadRelease_Utf8StringAlign();
        value.Value = reader.ReadBoolean();
    }

    public static void ReadRelease_Pair_Utf8StringAlign_ColorRGBAf(this AssetPair<Utf8String, ColorRGBAf> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadRelease_Utf8StringAlign();
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Utf8StringAlign_ComputeShaderKernel_2020_1_0_a17(this AssetPair<Utf8String, ComputeShaderKernel_2020_1_0_a17> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadRelease_Utf8StringAlign();
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Utf8StringAlign_ComputeShaderKernel_2020_1_0_a9(this AssetPair<Utf8String, ComputeShaderKernel_2020_1_0_a9> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadRelease_Utf8StringAlign();
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Utf8StringAlign_ComputeShaderKernel_2020_2_0_a15(this AssetPair<Utf8String, ComputeShaderKernel_2020_2_0_a15> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadRelease_Utf8StringAlign();
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Utf8StringAlign_ComputeShaderKernel_2020_3_2(this AssetPair<Utf8String, ComputeShaderKernel_2020_3_2> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadRelease_Utf8StringAlign();
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Utf8StringAlign_ComputeShaderKernel_2021(this AssetPair<Utf8String, ComputeShaderKernel_2021> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadRelease_Utf8StringAlign();
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Utf8StringAlign_ComputeShaderKernel_2021_1_0_b7(this AssetPair<Utf8String, ComputeShaderKernel_2021_1_0_b7> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadRelease_Utf8StringAlign();
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Utf8StringAlign_ConfigSetting(this AssetPair<Utf8String, ConfigSetting> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadRelease_Utf8StringAlign();
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Utf8StringAlign_GUID(this AssetPair<Utf8String, GUID> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadRelease_Utf8StringAlign();
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Utf8StringAlign_Int32(this AssetPair<Utf8String, int> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadRelease_Utf8StringAlign();
        value.Value = reader.ReadInt32();
    }

    public static void ReadRelease_Pair_Utf8StringAlign_Int64(this AssetPair<Utf8String, long> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadRelease_Utf8StringAlign();
        value.Value = reader.ReadInt64();
    }

    public static void ReadRelease_Pair_Utf8StringAlign_NonAlignedStruct(this AssetPair<Utf8String, NonAlignedStruct> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadRelease_Utf8StringAlign();
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Utf8StringAlign_PlatformSettingsData_Editor(this AssetPair<Utf8String, PlatformSettingsData_Editor> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadRelease_Utf8StringAlign();
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Utf8StringAlign_PlatformSettingsData_Plugin(this AssetPair<Utf8String, PlatformSettingsData_Plugin> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadRelease_Utf8StringAlign();
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Utf8StringAlign_PPtr_Object_3_5(this AssetPair<Utf8String, PPtr_Object_3_5> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadRelease_Utf8StringAlign();
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Utf8StringAlign_PPtr_Object_5(this AssetPair<Utf8String, PPtr_Object_5> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadRelease_Utf8StringAlign();
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Utf8StringAlign_PPtr_Texture_3_5(this AssetPair<Utf8String, PPtr_Texture_3_5> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadRelease_Utf8StringAlign();
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Utf8StringAlign_PPtr_Texture_5(this AssetPair<Utf8String, PPtr_Texture_5> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadRelease_Utf8StringAlign();
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Utf8StringAlign_PPtr_Texture2D_3_5(this AssetPair<Utf8String, PPtr_Texture2D_3_5> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadRelease_Utf8StringAlign();
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Utf8StringAlign_PPtr_Texture2D_5(this AssetPair<Utf8String, PPtr_Texture2D_5> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadRelease_Utf8StringAlign();
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Utf8StringAlign_SampleSettings_2022_2_0_a17(this AssetPair<Utf8String, SampleSettings_2022_2_0_a17> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadRelease_Utf8StringAlign();
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Utf8StringAlign_SecondaryTextureSettings_2020_2(this AssetPair<Utf8String, SecondaryTextureSettings_2020_2> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadRelease_Utf8StringAlign();
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Utf8StringAlign_SecondaryTextureSettings_2020_2_0_a12(this AssetPair<Utf8String, SecondaryTextureSettings_2020_2_0_a12> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadRelease_Utf8StringAlign();
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Utf8StringAlign_SecondaryTextureSettings_2022_2_20(this AssetPair<Utf8String, SecondaryTextureSettings_2022_2_20> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadRelease_Utf8StringAlign();
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Utf8StringAlign_SecondaryTextureSettings_2023(this AssetPair<Utf8String, SecondaryTextureSettings_2023> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadRelease_Utf8StringAlign();
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Utf8StringAlign_SecondaryTextureSettings_2023_2_0_a12(this AssetPair<Utf8String, SecondaryTextureSettings_2023_2_0_a12> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadRelease_Utf8StringAlign();
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Utf8StringAlign_SecondaryTextureSettings_2023_3_0_a11(this AssetPair<Utf8String, SecondaryTextureSettings_2023_3_0_a11> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadRelease_Utf8StringAlign();
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Utf8StringAlign_Single(this AssetPair<Utf8String, float> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadRelease_Utf8StringAlign();
        value.Value = reader.ReadSingle();
    }

    public static void ReadRelease_Pair_Utf8StringAlign_UInt32(this AssetPair<Utf8String, uint> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadRelease_Utf8StringAlign();
        value.Value = reader.ReadUInt32();
    }

    public static void ReadRelease_Pair_Utf8StringAlign_UnityTexEnv_5(this AssetPair<Utf8String, UnityTexEnv_5> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadRelease_Utf8StringAlign();
        value.Value.ReadRelease(ref reader);
    }

    public static void ReadRelease_Pair_Utf8StringAlign_Utf8StringAlign(this AssetPair<Utf8String, Utf8String> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadRelease_Utf8StringAlign();
        value.Value = reader.ReadRelease_Utf8StringAlign();
    }

    public static void ReadRelease_Pair_Utf8StringAlign_VideoClipImporterTargetSettings(this AssetPair<Utf8String, VideoClipImporterTargetSettings> value, ref EndianSpanReader reader)
    {
        value.Key = reader.ReadRelease_Utf8StringAlign();
        value.Value.ReadRelease(ref reader);
    }

    public static sbyte ReadRelease_SByteAlign(this ref EndianSpanReader reader)
    {
        sbyte b = reader.ReadSByte();
        reader.Align();
        return b;
    }

    public static float ReadRelease_SingleAlign(this ref EndianSpanReader reader)
    {
        float num = reader.ReadSingle();
        reader.Align();
        return num;
    }

    public static byte[] ReadRelease_TypelessDataAlign(this ref EndianSpanReader reader)
    {
        int num = reader.ReadInt32();
        byte[] array = ReadByteArray(ref reader, num);
        reader.Align();
        return array;
    }

    public static ushort ReadRelease_UInt16Align(this ref EndianSpanReader reader)
    {
        ushort num = reader.ReadUInt16();
        reader.Align();
        return num;
    }

    public static uint ReadRelease_UInt32Align(this ref EndianSpanReader reader)
    {
        uint num = reader.ReadUInt32();
        reader.Align();
        return num;
    }

    public static Utf8String ReadRelease_Utf8StringAlign(this ref EndianSpanReader reader)
    {
        Utf8String utf8String = reader.ReadUtf8String();
        reader.Align();
        return utf8String;
    }
    public static byte[] ReadByteArray(ref EndianSpanReader reader, int count)
    {
        return reader.ReadBytesExact(count).ToArray();
    }
}
