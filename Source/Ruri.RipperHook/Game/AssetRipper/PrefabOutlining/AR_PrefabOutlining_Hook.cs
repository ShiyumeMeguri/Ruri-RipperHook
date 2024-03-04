using Ruri.RipperHook.HookUtils.ExportHandlerHook;

namespace Ruri.RipperHook.AR_PrefabOutlining;

public partial class AR_PrefabOutlining_Hook : RipperHook
{
    protected AR_PrefabOutlining_Hook()
    {
        ExportHandlerHook.CustomAssetProcessors.Add(PrefabOutliningProcessor);
    }

    protected override void InitAttributeHook()
    {
        additionalNamespaces.Add(typeof(ExportHandlerHook).Namespace);
        base.InitAttributeHook();
    }
}