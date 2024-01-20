using AssetRipper.IO.Endian;
using AssetRipper.SourceGenerated.Classes.ClassID_43;
using AssetRipper.SourceGenerated.Subclasses.BoneWeights4;
using AssetRipper.SourceGenerated.Subclasses.SubMesh;
using AssetRipper.SourceGenerated.Subclasses.VertexData;

namespace Ruri.RipperHook.Houkai_3_8;
public partial class Houkai_3_8_Hook
{
	[RetargetMethod(typeof(Mesh_2017_3_0))]
	public void Mesh_2017_3_0_ReadRelease(ref EndianSpanReader reader)
	{
		var _this = (object)this as Mesh_2017_3_0;
		var type = typeof(Mesh_2017_3_0);

		_this.Name = reader.ReadRelease_Utf8StringAlign();
		SetAssetListField<SubMesh_2017_3_0>(type, "m_SubMeshes", ref reader);
		_this.Shapes.ReadRelease(ref reader);
		_this.BindPose.ReadRelease_ArrayAlign_Asset(ref reader);
		_this.BoneNameHashes.ReadRelease_ArrayAlign_UInt32(ref reader);
		_this.RootBoneNameHash = reader.ReadUInt32();
		_this.MeshCompression = reader.ReadByte();
		_this.IsReadable = reader.ReadBoolean();
		_this.KeepVertices = reader.ReadBoolean();
		_this.KeepIndices = reader.ReadRelease_BooleanAlign();
		_this.IndexFormat = reader.ReadInt32();
		_this.IndexBuffer = reader.ReadRelease_ArrayAlign_Byte();
		SetAssetListField<BoneWeights4_2017_1_0>(type, "m_Skin", ref reader);
		((VertexData_2017_1_0)_this.VertexData).ReadRelease_AssetAlign(ref reader);
		_this.CompressedMesh.ReadRelease(ref reader);
		_this.LocalAABB.ReadRelease(ref reader);
		_this.MeshUsageFlags = reader.ReadInt32();
		_this.BakedConvexCollisionMesh = reader.ReadRelease_ArrayAlign_Byte();
		_this.BakedTriangleCollisionMesh = reader.ReadRelease_ArrayAlign_Byte();
		bool CloseMeshDynamicCompression = reader.ReadRelease_BooleanAlign();
	}
}
