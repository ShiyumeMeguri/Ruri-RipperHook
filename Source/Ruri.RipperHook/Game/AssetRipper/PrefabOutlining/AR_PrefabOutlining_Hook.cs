using Ruri.RipperHook.HookUtils.ExportHandlerHook;

namespace Ruri.RipperHook.AR_PrefabOutlining;

/// <summary>
/// 这个功能可能被移除 因此作为Hook留着
/// 编译后的游戏通常不包含prefab的link信息 这个是还原用的
/// </summary>
public partial class AR_PrefabOutlining_Hook : RipperHook
{
    protected AR_PrefabOutlining_Hook()
    {
    }

    protected override void InitAttributeHook()
    {
        AddExtraHook(typeof(ExportHandlerHook).Namespace, () => { ExportHandlerHook.CustomAssetProcessors.Add(PrefabOutliningProcessor); });
        base.InitAttributeHook();
    }
}