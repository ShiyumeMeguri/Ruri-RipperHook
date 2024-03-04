using AssetRipper.Assets.Generics;
using AssetRipper.IO.Endian;
using AssetRipper.Primitives;
using AssetRipper.SourceGenerated.Classes.ClassID_142;
using AssetRipper.SourceGenerated.Subclasses.AssetInfo;
using AssetRipper.SourceGenerated.Subclasses.PPtr_Object;

namespace Ruri.RipperHook.Houkai_7_1;

public partial class Houkai_7_1_Hook
{
    [RetargetMethod(typeof(AssetBundle_2017_3))]
    public void AssetBundle_2017_3_ReadRelease(ref EndianSpanReader reader)
    {
        var _this = (object)this as AssetBundle_2017_3;
        var type = typeof(AssetBundle_2017_3);

        _this.Name = reader.ReadRelease_Utf8StringAlign();
        SetAssetListField<PPtr_Object_5>(type, "m_PreloadTable", ref reader);

        AssetDictionary<Utf8String, AssetInfo_5> m_Container = new();
        m_Container.ReadRelease_Map_Utf8StringAlign_AssetInfo_5(ref reader);
        SetPrivateField(type, "m_Container", m_Container);

        _this.AssetBundleName = reader.ReadRelease_Utf8StringAlign();
        _this.Dependencies.ReadRelease_ArrayAlign_Utf8StringAlign(ref reader);
        _this.PathFlags = reader.ReadInt32();
    }
}