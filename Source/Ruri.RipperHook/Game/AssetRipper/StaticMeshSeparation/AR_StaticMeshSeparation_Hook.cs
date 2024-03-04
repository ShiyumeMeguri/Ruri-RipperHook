using Ruri.RipperHook.HookUtils.ExportHandlerHook;

namespace Ruri.RipperHook.AR_StaticMeshSeparation;

public partial class AR_StaticMeshSeparation_Hook : RipperHook
{
    protected AR_StaticMeshSeparation_Hook()
    {
        ExportHandlerHook.CustomAssetProcessors.Add(StaticMeshProcessor);
    }

    protected override void InitAttributeHook()
    {
        additionalNamespaces.Add(typeof(ExportHandlerHook).Namespace);
        base.InitAttributeHook();
    }
}