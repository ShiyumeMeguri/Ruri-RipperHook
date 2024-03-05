namespace Ruri.RipperHook.GirlsFrontline2Common;

public partial class GirlsFrontline2Common_Hook
{
    public static bool CustomAssetBundlesCheck(FileInfo file)
    {
        return file.Extension == ".bundle";
    }
}