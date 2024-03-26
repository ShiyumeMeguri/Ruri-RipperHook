using Ruri.RipperHook.HookUtils.ExportHandlerHook;

namespace Ruri.RipperHook.AR_PrefabOutlining;

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