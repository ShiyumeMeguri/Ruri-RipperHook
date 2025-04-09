using Ruri.RipperHook.HookUtils.ExportHandlerHook;

namespace Ruri.RipperHook.AR_BundledAssetsExportMode;

/// <summary>
/// 默认设置修改 直接以ExportDirect的方式导出
/// 避免Resource以 0ef580b41ba93b990b52a6edd07ca5d2e578af3f/arts/xxx这种名字导出
/// 导致的成千上万重复文件夹
/// </summary>
public partial class AR_BundledAssetsExportMode_Hook : RipperHook
{
    protected AR_BundledAssetsExportMode_Hook()
    {
    }

    protected override void InitAttributeHook()
    {
        base.InitAttributeHook();
    }
}