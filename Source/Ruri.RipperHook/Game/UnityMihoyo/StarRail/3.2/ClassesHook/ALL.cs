using AssetRipper.Assets;
using AssetRipper.IO.Endian;
using AssetRipper.SourceGenerated;
using StarRail320.SourceGenerated.Classes.ClassID_108;
using StarRail320.SourceGenerated.Classes.ClassID_115;
using StarRail320.SourceGenerated.Classes.ClassID_135;
using StarRail320.SourceGenerated.Classes.ClassID_136;
using StarRail320.SourceGenerated.Classes.ClassID_137;
using StarRail320.SourceGenerated.Classes.ClassID_143;
using StarRail320.SourceGenerated.Classes.ClassID_154;
using StarRail320.SourceGenerated.Classes.ClassID_198;
using StarRail320.SourceGenerated.Classes.ClassID_20;
using StarRail320.SourceGenerated.Classes.ClassID_215;
using StarRail320.SourceGenerated.Classes.ClassID_223;
using StarRail320.SourceGenerated.Classes.ClassID_30;
using StarRail320.SourceGenerated.Classes.ClassID_310;
using StarRail320.SourceGenerated.Classes.ClassID_331;
using StarRail320.SourceGenerated.Classes.ClassID_47;
using StarRail320.SourceGenerated.Classes.ClassID_48;
using StarRail320.SourceGenerated.Classes.ClassID_64;
using StarRail320.SourceGenerated.Classes.ClassID_65;
using StarRail320.SourceGenerated.Classes.ClassID_95;

namespace Ruri.RipperHook.StarRail_3_2;

public partial class StarRail_3_2_Hook
{
    [RetargetMethod(ClassIDType.Animator, ClassHookVersion)]
    public void Animator_ReadRelease(ref EndianSpanReader reader)
    {
        var realThis = (object)this as IUnityObjectBase;
        var customThis = new Animator_2019(realThis.AssetInfo);

        customThis.ReadRelease(ref reader);
        ReflectionExtensions.ClassCopy(customThis, realThis);
    }
    [RetargetMethod(ClassIDType.Camera, ClassHookVersion)]
    public void Camera_ReadRelease(ref EndianSpanReader reader)
    {
        var realThis = (object)this as IUnityObjectBase;
        var customThis = new Camera_2019(realThis.AssetInfo);

        customThis.ReadRelease(ref reader);
        ReflectionExtensions.ClassCopy(customThis, realThis);
    }
    [RetargetMethod(ClassIDType.SkinnedMeshRenderer, ClassHookVersion)]
    public void SkinnedMeshRenderer_ReadRelease(ref EndianSpanReader reader)
    {
        var realThis = (object)this as IUnityObjectBase;
        var customThis = new SkinnedMeshRenderer_2019(realThis.AssetInfo);

        customThis.ReadRelease(ref reader);
        ReflectionExtensions.ClassCopy(customThis, realThis);
    }
    [RetargetMethod(ClassIDType.BoxCollider, ClassHookVersion)]
    public void BoxCollider_ReadRelease(ref EndianSpanReader reader)
    {
        var realThis = (object)this as IUnityObjectBase;
        var customThis = new BoxCollider_2019(realThis.AssetInfo);

        customThis.ReadRelease(ref reader);
        ReflectionExtensions.ClassCopy(customThis, realThis);
    }
    [RetargetMethod(ClassIDType.Canvas, ClassHookVersion)]
    public void Canvas_ReadRelease(ref EndianSpanReader reader)
    {
        var realThis = (object)this as IUnityObjectBase;
        var customThis = new Canvas_2019(realThis.AssetInfo);

        customThis.ReadRelease(ref reader);
        ReflectionExtensions.ClassCopy(customThis, realThis);
    }
    [RetargetMethod(ClassIDType.CapsuleCollider, ClassHookVersion)]
    public void CapsuleCollider_ReadRelease(ref EndianSpanReader reader)
    {
        var realThis = (object)this as IUnityObjectBase;
        var customThis = new CapsuleCollider_2019(realThis.AssetInfo);

        customThis.ReadRelease(ref reader);
        ReflectionExtensions.ClassCopy(customThis, realThis);
    }
    [RetargetMethod(ClassIDType.CharacterController, ClassHookVersion)]
    public void CharacterController_ReadRelease(ref EndianSpanReader reader)
    {
        var realThis = (object)this as IUnityObjectBase;
        var customThis = new CharacterController_2019(realThis.AssetInfo);

        customThis.ReadRelease(ref reader);
        ReflectionExtensions.ClassCopy(customThis, realThis);
    }
    [RetargetMethod(ClassIDType.GraphicsSettings, ClassHookVersion)]
    public void GraphicsSettings_ReadRelease(ref EndianSpanReader reader)
    {
        var realThis = (object)this as IUnityObjectBase;
        var customThis = new GraphicsSettings_2019(realThis.AssetInfo);

        customThis.ReadRelease(ref reader);
        ReflectionExtensions.ClassCopy(customThis, realThis);
    }
    [RetargetMethod(ClassIDType.Light, ClassHookVersion)]
    public void Light_ReadRelease(ref EndianSpanReader reader)
    {
        var realThis = (object)this as IUnityObjectBase;
        var customThis = new Light_2019(realThis.AssetInfo);

        customThis.ReadRelease(ref reader);
        ReflectionExtensions.ClassCopy(customThis, realThis);
    }
    [RetargetMethod(ClassIDType.MeshCollider, ClassHookVersion)]
    public void MeshCollider_ReadRelease(ref EndianSpanReader reader)
    {
        var realThis = (object)this as IUnityObjectBase;
        var customThis = new MeshCollider_2019(realThis.AssetInfo);

        customThis.ReadRelease(ref reader);
        ReflectionExtensions.ClassCopy(customThis, realThis);
    }
    [RetargetMethod(ClassIDType.MonoScript, ClassHookVersion)]
    public void MonoScript_ReadRelease(ref EndianSpanReader reader)
    {
        var realThis = (object)this as IUnityObjectBase;
        var customThis = new MonoScript_2019(realThis.AssetInfo);

        customThis.ReadRelease(ref reader);
        ReflectionExtensions.ClassCopy(customThis, realThis);
    }
    [RetargetMethod(ClassIDType.ParticleSystem, ClassHookVersion)]
    public void ParticleSystem_ReadRelease(ref EndianSpanReader reader)
    {
        var realThis = (object)this as IUnityObjectBase;
        var customThis = new ParticleSystem_2019(realThis.AssetInfo);

        customThis.ReadRelease(ref reader);
        ReflectionExtensions.ClassCopy(customThis, realThis);
    }
    [RetargetMethod(ClassIDType.QualitySettings, ClassHookVersion)]
    public void QualitySettings_ReadRelease(ref EndianSpanReader reader)
    {
        var realThis = (object)this as IUnityObjectBase;
        var customThis = new QualitySettings_2019(realThis.AssetInfo);

        customThis.ReadRelease(ref reader);
        ReflectionExtensions.ClassCopy(customThis, realThis);
    }
    [RetargetMethod(ClassIDType.ReflectionProbe, ClassHookVersion)]
    public void ReflectionProbe_ReadRelease(ref EndianSpanReader reader)
    {
        var realThis = (object)this as IUnityObjectBase;
        var customThis = new ReflectionProbe_2019(realThis.AssetInfo);

        customThis.ReadRelease(ref reader);
        ReflectionExtensions.ClassCopy(customThis, realThis);
    }
    [RetargetMethod(ClassIDType.Shader, ClassHookVersion)]
    public void Shader_ReadRelease(ref EndianSpanReader reader)
    {
        var realThis = (object)this as IUnityObjectBase;
        var customThis = new Shader_2019(realThis.AssetInfo);

        customThis.ReadRelease(ref reader);
        ReflectionExtensions.ClassCopy(customThis, realThis);
    }
    [RetargetMethod(ClassIDType.SphereCollider, ClassHookVersion)]
    public void SphereCollider_ReadRelease(ref EndianSpanReader reader)
    {
        var realThis = (object)this as IUnityObjectBase;
        var customThis = new SphereCollider_2019(realThis.AssetInfo);

        customThis.ReadRelease(ref reader);
        ReflectionExtensions.ClassCopy(customThis, realThis);
    }
    [RetargetMethod(ClassIDType.SpriteMask, ClassHookVersion)]
    public void SpriteMask_ReadRelease(ref EndianSpanReader reader)
    {
        var realThis = (object)this as IUnityObjectBase;
        var customThis = new SpriteMask_2019(realThis.AssetInfo);

        customThis.ReadRelease(ref reader);
        ReflectionExtensions.ClassCopy(customThis, realThis);
    }
    [RetargetMethod(ClassIDType.TerrainCollider, ClassHookVersion)]
    public void TerrainCollider_ReadRelease(ref EndianSpanReader reader)
    {
        var realThis = (object)this as IUnityObjectBase;
        var customThis = new TerrainCollider_2019(realThis.AssetInfo);

        customThis.ReadRelease(ref reader);
        ReflectionExtensions.ClassCopy(customThis, realThis);
    }
    [RetargetMethod(ClassIDType.UnityConnectSettings, ClassHookVersion)]
    public void UnityConnectSettings_ReadRelease(ref EndianSpanReader reader)
    {
        var realThis = (object)this as IUnityObjectBase;
        var customThis = new UnityConnectSettings_2019(realThis.AssetInfo);

        customThis.ReadRelease(ref reader);
        ReflectionExtensions.ClassCopy(customThis, realThis);
    }
}
