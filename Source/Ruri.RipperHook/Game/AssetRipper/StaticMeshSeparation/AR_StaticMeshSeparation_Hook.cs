using Ruri.RipperHook.HookUtils.ExportHandlerHook;

namespace Ruri.RipperHook.AR_StaticMeshSeparation;

public partial class AR_StaticMeshSeparation_Hook : RipperHook
{
    protected AR_StaticMeshSeparation_Hook()
    {
    }

    protected override void InitAttributeHook()
    {
        AddExtraHook(typeof(ExportHandlerHook).Namespace, () => { ExportHandlerHook.CustomAssetProcessors.Add(StaticMeshProcessor); });
        base.InitAttributeHook();
    }
}