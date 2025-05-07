using AssetRipper.IO.Endian;
using AssetRipper.SourceGenerated;
using AssetRipper.SourceGenerated.Classes.ClassID_108;
using AssetRipper.SourceGenerated.Classes.ClassID_115;
using AssetRipper.SourceGenerated.Classes.ClassID_120;
using AssetRipper.SourceGenerated.Classes.ClassID_135;
using AssetRipper.SourceGenerated.Classes.ClassID_136;
using AssetRipper.SourceGenerated.Classes.ClassID_137;
using AssetRipper.SourceGenerated.Classes.ClassID_143;
using AssetRipper.SourceGenerated.Classes.ClassID_154;
using AssetRipper.SourceGenerated.Classes.ClassID_1971053207;
using AssetRipper.SourceGenerated.Classes.ClassID_198;
using AssetRipper.SourceGenerated.Classes.ClassID_199;
using AssetRipper.SourceGenerated.Classes.ClassID_212;
using AssetRipper.SourceGenerated.Classes.ClassID_215;
using AssetRipper.SourceGenerated.Classes.ClassID_223;
using AssetRipper.SourceGenerated.Classes.ClassID_23;
using AssetRipper.SourceGenerated.Classes.ClassID_30;
using AssetRipper.SourceGenerated.Classes.ClassID_310;
using AssetRipper.SourceGenerated.Classes.ClassID_331;
using AssetRipper.SourceGenerated.Classes.ClassID_47;
using AssetRipper.SourceGenerated.Classes.ClassID_48;
using AssetRipper.SourceGenerated.Classes.ClassID_483693784;
using AssetRipper.SourceGenerated.Classes.ClassID_64;
using AssetRipper.SourceGenerated.Classes.ClassID_65;
using AssetRipper.SourceGenerated.Classes.ClassID_73398921;
using AssetRipper.SourceGenerated.Classes.ClassID_96;

namespace Ruri.RipperHook.StarRail_3_2;

public partial class StarRail_3_2_Hook
{
    [RetargetMethod(ClassIDType.BoxCollider, ClassHookVersion)]
    public void BoxCollider_ReadRelease(ref EndianSpanReader reader)
    {
        var _this = (object)this as BoxCollider_2018_3;
        var type = typeof(BoxCollider_2018_3);

        _this.GameObject.ReadRelease(ref reader);
        _this.Material.ReadRelease(ref reader);
        _this.IsTrigger = reader.ReadBoolean();
        bool m_IsWalkable = reader.ReadBoolean();
        _this.Enabled = reader.ReadRelease_BooleanAlign();
        _this.Size.ReadRelease(ref reader);
        _this.Center.ReadRelease(ref reader);
    }
    [RetargetMethod(ClassIDType.Canvas, ClassHookVersion)]
    public void Canvas_ReadRelease(ref EndianSpanReader reader)
    {
        var _this = (object)this as Canvas_2018_3;
        var type = typeof(Canvas_2018_3);

        _this.GameObject.ReadRelease(ref reader);
        _this.Enabled = reader.ReadRelease_ByteAlign();
        _this.RenderMode = reader.ReadInt32();
        bool m_OverrideRenderMode = reader.ReadRelease_BooleanAlign();
        _this.Camera.ReadRelease(ref reader);
        _this.PlaneDistance = reader.ReadSingle();
        _this.PixelPerfect = reader.ReadBoolean();
        _this.ReceivesEvents = reader.ReadBoolean();
        _this.OverrideSorting = reader.ReadBoolean();
        _this.OverridePixelPerfect = reader.ReadBoolean();
        _this.SortingBucketNormalizedSize = reader.ReadSingle();
        _this.AdditionalShaderChannelsFlag = reader.ReadRelease_Int32Align();
        _this.SortingLayerID_Int32 = reader.ReadInt32();
        _this.SortingOrder = reader.ReadInt16();
        _this.TargetDisplay = reader.ReadSByte();
    }
    [RetargetMethod(ClassIDType.CapsuleCollider, ClassHookVersion)]
    public void CapsuleCollider_ReadRelease(ref EndianSpanReader reader)
    {
        var _this = (object)this as CapsuleCollider_2018_3;
        var type = typeof(CapsuleCollider_2018_3);
    }

    [RetargetMethod(ClassIDType.CharacterController, ClassHookVersion)]
    public void CharacterController_ReadRelease(ref EndianSpanReader reader)
    {
        var _this = (object)this as CharacterController_2018_3;
        var type = typeof(CharacterController_2018_3);
    }

    [RetargetMethod(ClassIDType.GraphicsSettings, ClassHookVersion)]
    public void GraphicsSettings_ReadRelease(ref EndianSpanReader reader)
    {
        var _this = (object)this as GraphicsSettings_2019_3_0_a8;
        var type = typeof(GraphicsSettings_2019_3_0_a8);
    }

    [RetargetMethod(ClassIDType.Light, ClassHookVersion)]
    public void Light_ReadRelease(ref EndianSpanReader reader)
    {
        var _this = (object)this as Light_2019_3_0_a10;
        var type = typeof(Light_2019_3_0_a10);
    }

    [RetargetMethod(ClassIDType.MeshCollider, ClassHookVersion)]
    public void MeshCollider_ReadRelease(ref EndianSpanReader reader)
    {
        var _this = (object)this as MeshCollider_2019_3_7;
        var type = typeof(MeshCollider_2019_3_7);
    }

    [RetargetMethod(ClassIDType.MonoScript, ClassHookVersion)]
    public void MonoScript_ReadRelease(ref EndianSpanReader reader)
    {
        var _this = (object)this as MonoScript_2018_3;
        var type = typeof(MonoScript_2018_3);
    }

    [RetargetMethod(ClassIDType.ParticleSystem, ClassHookVersion)]
    public void ParticleSystem_ReadRelease(ref EndianSpanReader reader)
    {
        var _this = (object)this as ParticleSystem_2019_2_0_a9;
        var type = typeof(ParticleSystem_2019_2_0_a9);
    }

    [RetargetMethod(ClassIDType.QualitySettings, ClassHookVersion)]
    public void QualitySettings_ReadRelease(ref EndianSpanReader reader)
    {
        var _this = (object)this as QualitySettings_2019_3_0_a6;
        var type = typeof(QualitySettings_2019_3_0_a6);
    }

    [RetargetMethod(ClassIDType.ReflectionProbe, ClassHookVersion)]
    public void ReflectionProbe_ReadRelease(ref EndianSpanReader reader)
    {
        var _this = (object)this as ReflectionProbe_2018_3;
        var type = typeof(ReflectionProbe_2018_3);
    }

    [RetargetMethod(ClassIDType.Shader, ClassHookVersion)]
    public void Shader_ReadRelease(ref EndianSpanReader reader)
    {
        var _this = (object)this as Shader_2019_3_0_b0;
        var type = typeof(Shader_2019_3_0_b0);
    }

    [RetargetMethod(ClassIDType.SphereCollider, ClassHookVersion)]
    public void SphereCollider_ReadRelease(ref EndianSpanReader reader)
    {
        var _this = (object)this as SphereCollider_2018_3;
        var type = typeof(SphereCollider_2018_3);
    }

    [RetargetMethod(ClassIDType.SpriteMask, ClassHookVersion)]
    public void SpriteMask_ReadRelease(ref EndianSpanReader reader)
    {
        var _this = (object)this as SpriteMask_2019_3_0_a6;
        var type = typeof(SpriteMask_2019_3_0_a6);
    }

    [RetargetMethod(ClassIDType.TerrainCollider, ClassHookVersion)]
    public void TerrainCollider_ReadRelease(ref EndianSpanReader reader)
    {
        var _this = (object)this as TerrainCollider_2018_3;
        var type = typeof(TerrainCollider_2018_3);
    }

    [RetargetMethod(ClassIDType.UnityConnectSettings, ClassHookVersion)]
    public void UnityConnectSettings_ReadRelease(ref EndianSpanReader reader)
    {
        var _this = (object)this as UnityConnectSettings_2018_3;
        var type = typeof(UnityConnectSettings_2018_3);
    }
}