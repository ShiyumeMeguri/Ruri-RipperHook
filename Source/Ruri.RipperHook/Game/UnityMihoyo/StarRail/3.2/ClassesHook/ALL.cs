using AssetRipper.Assets;
using AssetRipper.IO.Endian;
using AssetRipper.Primitives;
using AssetRipper.SourceGenerated;
using StarRail.SourceGenerated.Classes.ClassID_108;
using StarRail.SourceGenerated.Classes.ClassID_115;
using StarRail.SourceGenerated.Classes.ClassID_135;
using StarRail.SourceGenerated.Classes.ClassID_136;
using StarRail.SourceGenerated.Classes.ClassID_137;
using StarRail.SourceGenerated.Classes.ClassID_143;
using StarRail.SourceGenerated.Classes.ClassID_154;
using StarRail.SourceGenerated.Classes.ClassID_198;
using StarRail.SourceGenerated.Classes.ClassID_20;
using StarRail.SourceGenerated.Classes.ClassID_215;
using StarRail.SourceGenerated.Classes.ClassID_223;
using StarRail.SourceGenerated.Classes.ClassID_30;
using StarRail.SourceGenerated.Classes.ClassID_310;
using StarRail.SourceGenerated.Classes.ClassID_331;
using StarRail.SourceGenerated.Classes.ClassID_47;
using StarRail.SourceGenerated.Classes.ClassID_48;
using StarRail.SourceGenerated.Classes.ClassID_64;
using StarRail.SourceGenerated.Classes.ClassID_65;
using StarRail.SourceGenerated.Classes.ClassID_95;

namespace Ruri.RipperHook.StarRail_3_2;

public partial class StarRail_3_2_Hook
{
    [RetargetMethod(ClassIDType.Animator, ClassHookVersion)]
    public void Animator_ReadRelease(ref EndianSpanReader reader)
    {
        var realThis = (object)this as IUnityObjectBase;
        var dummyThis = Animator.Create(realThis.AssetInfo, StarRailClassVersion);

        dummyThis.ReadRelease(ref reader);
        ReflectionExtensions.ClassCopy(dummyThis, realThis);
    }
    [RetargetMethod(ClassIDType.Camera, ClassHookVersion)]
    public void Camera_ReadRelease(ref EndianSpanReader reader)
    {
        var realThis = (object)this as IUnityObjectBase;
        var dummyThis = Camera.Create(realThis.AssetInfo, StarRailClassVersion);

        dummyThis.ReadRelease(ref reader);
        ReflectionExtensions.ClassCopy(dummyThis, realThis);
    }
    [RetargetMethod(ClassIDType.SkinnedMeshRenderer, ClassHookVersion)]
    public void SkinnedMeshRenderer_ReadRelease(ref EndianSpanReader reader)
    {
        var realThis = (object)this as IUnityObjectBase;
        var dummyThis = SkinnedMeshRenderer.Create(realThis.AssetInfo, StarRailClassVersion);

        dummyThis.ReadRelease(ref reader);
        ReflectionExtensions.ClassCopy(dummyThis, realThis);
    }
    [RetargetMethod(ClassIDType.BoxCollider, ClassHookVersion)]
    public void BoxCollider_ReadRelease(ref EndianSpanReader reader)
    {
        var realThis = (object)this as IUnityObjectBase;
        var dummyThis = BoxCollider.Create(realThis.AssetInfo, StarRailClassVersion);

        dummyThis.ReadRelease(ref reader);
        ReflectionExtensions.ClassCopy(dummyThis, realThis);
    }
    [RetargetMethod(ClassIDType.Canvas, ClassHookVersion)]
    public void Canvas_ReadRelease(ref EndianSpanReader reader)
    {
        var realThis = (object)this as IUnityObjectBase;
        var dummyThis = Canvas.Create(realThis.AssetInfo, StarRailClassVersion);

        dummyThis.ReadRelease(ref reader);
        ReflectionExtensions.ClassCopy(dummyThis, realThis);
    }
    [RetargetMethod(ClassIDType.CapsuleCollider, ClassHookVersion)]
    public void CapsuleCollider_ReadRelease(ref EndianSpanReader reader)
    {
        var realThis = (object)this as IUnityObjectBase;
        var dummyThis = CapsuleCollider.Create(realThis.AssetInfo, StarRailClassVersion);

        dummyThis.ReadRelease(ref reader);
        ReflectionExtensions.ClassCopy(dummyThis, realThis);
    }
    [RetargetMethod(ClassIDType.CharacterController, ClassHookVersion)]
    public void CharacterController_ReadRelease(ref EndianSpanReader reader)
    {
        var realThis = (object)this as IUnityObjectBase;
        var dummyThis = CharacterController.Create(realThis.AssetInfo, StarRailClassVersion);

        dummyThis.ReadRelease(ref reader);
        ReflectionExtensions.ClassCopy(dummyThis, realThis);
    }
    [RetargetMethod(ClassIDType.GraphicsSettings, ClassHookVersion)]
    public void GraphicsSettings_ReadRelease(ref EndianSpanReader reader)
    {
        var realThis = (object)this as IUnityObjectBase;
        var dummyThis = GraphicsSettings.Create(realThis.AssetInfo, StarRailClassVersion);

        dummyThis.ReadRelease(ref reader);
        ReflectionExtensions.ClassCopy(dummyThis, realThis);
    }
    [RetargetMethod(ClassIDType.Light, ClassHookVersion)]
    public void Light_ReadRelease(ref EndianSpanReader reader)
    {
        var realThis = (object)this as IUnityObjectBase;
        var dummyThis = Light.Create(realThis.AssetInfo, StarRailClassVersion);

        dummyThis.ReadRelease(ref reader);
        ReflectionExtensions.ClassCopy(dummyThis, realThis);
    }
    [RetargetMethod(ClassIDType.MeshCollider, ClassHookVersion)]
    public void MeshCollider_ReadRelease(ref EndianSpanReader reader)
    {
        var realThis = (object)this as IUnityObjectBase;
        var dummyThis = MeshCollider.Create(realThis.AssetInfo, StarRailClassVersion);

        dummyThis.ReadRelease(ref reader);
        ReflectionExtensions.ClassCopy(dummyThis, realThis);
    }
    [RetargetMethod(ClassIDType.MonoScript, ClassHookVersion)]
    public void MonoScript_ReadRelease(ref EndianSpanReader reader)
    {
        var realThis = (object)this as IUnityObjectBase;
        var dummyThis = MonoScript.Create(realThis.AssetInfo, StarRailClassVersion);

        dummyThis.ReadRelease(ref reader);
        ReflectionExtensions.ClassCopy(dummyThis, realThis);
    }
    [RetargetMethod(ClassIDType.ParticleSystem, ClassHookVersion)]
    public void ParticleSystem_ReadRelease(ref EndianSpanReader reader)
    {
        var realThis = (object)this as IUnityObjectBase;
        var dummyThis = ParticleSystem.Create(realThis.AssetInfo, StarRailClassVersion);

        dummyThis.ReadRelease(ref reader);
        ReflectionExtensions.ClassCopy(dummyThis, realThis);
    }
    [RetargetMethod(ClassIDType.QualitySettings, ClassHookVersion)]
    public void QualitySettings_ReadRelease(ref EndianSpanReader reader)
    {
        var realThis = (object)this as IUnityObjectBase;
        var dummyThis = QualitySettings.Create(realThis.AssetInfo, StarRailClassVersion);

        dummyThis.ReadRelease(ref reader);
        ReflectionExtensions.ClassCopy(dummyThis, realThis);
    }
    [RetargetMethod(ClassIDType.ReflectionProbe, ClassHookVersion)]
    public void ReflectionProbe_ReadRelease(ref EndianSpanReader reader)
    {
        var realThis = (object)this as IUnityObjectBase;
        var dummyThis = ReflectionProbe.Create(realThis.AssetInfo, StarRailClassVersion);

        dummyThis.ReadRelease(ref reader);
        ReflectionExtensions.ClassCopy(dummyThis, realThis);
    }
    [RetargetMethod(ClassIDType.Shader, ClassHookVersion)]
    public void Shader_ReadRelease(ref EndianSpanReader reader)
    {
        var realThis = (object)this as IUnityObjectBase;
        var dummyThis = Shader.Create(realThis.AssetInfo, StarRailClassVersion);

        dummyThis.ReadRelease(ref reader);
        ReflectionExtensions.ClassCopy(dummyThis, realThis);
    }
    [RetargetMethod(ClassIDType.SphereCollider, ClassHookVersion)]
    public void SphereCollider_ReadRelease(ref EndianSpanReader reader)
    {
        var realThis = (object)this as IUnityObjectBase;
        var dummyThis = SphereCollider.Create(realThis.AssetInfo, StarRailClassVersion);

        dummyThis.ReadRelease(ref reader);
        ReflectionExtensions.ClassCopy(dummyThis, realThis);
    }
    [RetargetMethod(ClassIDType.SpriteMask, ClassHookVersion)]
    public void SpriteMask_ReadRelease(ref EndianSpanReader reader)
    {
        var realThis = (object)this as IUnityObjectBase;
        var dummyThis = SpriteMask.Create(realThis.AssetInfo, StarRailClassVersion);

        dummyThis.ReadRelease(ref reader);
        ReflectionExtensions.ClassCopy(dummyThis, realThis);
    }
    [RetargetMethod(ClassIDType.TerrainCollider, ClassHookVersion)]
    public void TerrainCollider_ReadRelease(ref EndianSpanReader reader)
    {
        var realThis = (object)this as IUnityObjectBase;
        var dummyThis = TerrainCollider.Create(realThis.AssetInfo, StarRailClassVersion);

        dummyThis.ReadRelease(ref reader);
        ReflectionExtensions.ClassCopy(dummyThis, realThis);
    }
    [RetargetMethod(ClassIDType.UnityConnectSettings, ClassHookVersion)]
    public void UnityConnectSettings_ReadRelease(ref EndianSpanReader reader)
    {
        var realThis = (object)this as IUnityObjectBase;
        var dummyThis = UnityConnectSettings.Create(realThis.AssetInfo, StarRailClassVersion);

        dummyThis.ReadRelease(ref reader);
        ReflectionExtensions.ClassCopy(dummyThis, realThis);
    }
}
