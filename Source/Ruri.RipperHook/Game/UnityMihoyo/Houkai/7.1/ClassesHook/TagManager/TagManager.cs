using AssetRipper.Assets.Generics;
using AssetRipper.IO.Endian;
using AssetRipper.Primitives;
using AssetRipper.SourceGenerated.Classes.ClassID_78;
using AssetRipper.SourceGenerated.Subclasses.SortingLayerEntry;

namespace Ruri.RipperHook.Houkai_7_1;

public partial class Houkai_7_1_Hook
{
    [RetargetMethod(typeof(TagManager_2017))]
    public void TagManager_2017_ReadRelease(ref EndianSpanReader reader)
    {
        var _this = (object)this as TagManager_2017;
        var type = typeof(TagManager_2017);

        _this.Tags.ReadRelease_ArrayAlign_Utf8StringAlign(ref reader);
        _this.Layers.ReadRelease_ArrayAlign_Utf8StringAlign(ref reader);
        SetAssetListField<SortingLayerEntry_5>(type, "m_SortingLayers", ref reader);
        AssetList<Utf8String> renderLayers = new();
        renderLayers.ReadRelease_ArrayAlign_Utf8StringAlign(ref reader);
    }
}