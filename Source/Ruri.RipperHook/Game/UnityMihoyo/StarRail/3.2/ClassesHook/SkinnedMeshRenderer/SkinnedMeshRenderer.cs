using AssetRipper.IO.Endian;
using AssetRipper.SourceGenerated.Classes.ClassID_137;
using AssetRipper.SourceGenerated.Subclasses.PPtr_Transform;
using AssetRipper.SourceGenerated.Subclasses.Vector3f;
using System.Numerics;

namespace Ruri.RipperHook.StarRail_3_2;

public partial class StarRail_3_2_Hook
{
    [RetargetMethod(typeof(SkinnedMeshRenderer_2019_3_0_a6))]
    public void SkinnedMeshRenderer_2019_3_0_a6_ReadRelease(ref EndianSpanReader reader)
    {
        var _this = (object)this as SkinnedMeshRenderer_2019_3_0_a6;
        var type = typeof(SkinnedMeshRenderer_2019_3_0_a6);

        Renderer_2019_3_0_a6_ReadRelease(ref reader);

        _this.Quality = reader.ReadInt32();
        _this.UpdateWhenOffscreen = reader.ReadBoolean();
        _this.SkinnedMotionVectors = reader.ReadRelease_BooleanAlign();
        _this.Mesh.ReadRelease(ref reader);
        SetAssetListField<PPtr_Transform_5>(type, "m_Bones", ref reader);
        _this.BlendShapeWeights.ReadRelease_ArrayAlign_Single(ref reader);
        _this.RootBone.ReadRelease(ref reader);
        _this.AABB.ReadRelease(ref reader);
        _this.DirtyAABB = reader.ReadRelease_BooleanAlign();
        Vector3f m_BoundExpandingScale = new();
        m_BoundExpandingScale.ReadRelease(ref reader);
        var m_EnableSkinning = reader.ReadBoolean();
    }
}