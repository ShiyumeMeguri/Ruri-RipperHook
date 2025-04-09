using Ruri.RipperHook.HookUtils.ExportHandlerHook;

namespace Ruri.RipperHook.AR_StaticMeshSeparation;

/// <summary>
/// 静态网格分离 用于修复Static模型分离为单个网格
/// </summary>
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