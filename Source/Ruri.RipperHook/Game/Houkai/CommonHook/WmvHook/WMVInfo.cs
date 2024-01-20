namespace Ruri.RipperHook.HoukaiCommon;
public struct WMVInfo
{
    public struct UnitAssetInfo
    {
        public string FilePath { get; set; }
        public int Offset { get; set; }
        public int FileSize { get; set; }
    }
    public string FilePath { get; set; }
    public int FileSize { get; set; }
    public int FileCount { get; set; }
    public UnitAssetInfo[] UnitAssetArray { get; set; }
}
