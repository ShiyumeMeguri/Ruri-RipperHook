using AssetRipper.IO.Endian;
using AssetRipper.SourceGenerated.Classes.ClassID_43;
using AssetRipper.SourceGenerated.Subclasses.SubMesh;
using AssetRipper.SourceGenerated.Subclasses.VertexData;

namespace Ruri.RipperHook.ExAstrisCommon;

public partial class ExAstrisCommon_Hook
{
    [RetargetMethod(typeof(Mesh_2020_1_0_a19))]
    public void Mesh_2020_1_0_a19_ReadRelease(ref EndianSpanReader reader)
    {
        var _this = (object)this as Mesh_2020_1_0_a19;
        var type = typeof(Mesh_2020_1_0_a19);

        _this.Name = reader.ReadRelease_Utf8StringAlign();
        SetAssetListField<SubMesh_2017_3>(type, "m_SubMeshes", ref reader);
        _this.Shapes.ReadRelease(ref reader);
        _this.BindPose.ReadRelease_ArrayAlign_Asset(ref reader);
        _this.BoneNameHashes.ReadRelease_ArrayAlign_UInt32(ref reader);
        _this.RootBoneNameHash = reader.ReadUInt32();
        _this.BonesAABB.ReadRelease_ArrayAlign_Asset(ref reader);
        _this.VariableBoneCountWeights.ReadRelease(ref reader);
        _this.MeshCompression = reader.ReadByte();
        _this.IsReadable = reader.ReadBoolean();
        _this.KeepVertices = reader.ReadBoolean();
        _this.KeepIndices = reader.ReadRelease_BooleanAlign();
        _this.IndexFormat = reader.ReadInt32();
        _this.IndexBuffer = reader.ReadRelease_ArrayAlign_Byte();
        ((VertexData_2019)_this.VertexData).ReadRelease_AssetAlign(ref reader);
        _this.CompressedMesh.ReadRelease(ref reader);
        _this.LocalAABB.ReadRelease(ref reader);
        var m_ColliderType = reader.ReadInt32();
        _this.MeshUsageFlags = reader.ReadInt32();
        _this.BakedConvexCollisionMesh = reader.ReadRelease_ArrayAlign_Byte();
        _this.BakedTriangleCollisionMesh = reader.ReadRelease_ArrayAlign_Byte();
        _this.MeshMetrics_0_ = reader.ReadSingle();
        _this.MeshMetrics_1_ = reader.ReadRelease_SingleAlign();
        _this.StreamData.ReadRelease(ref reader);
    }
}