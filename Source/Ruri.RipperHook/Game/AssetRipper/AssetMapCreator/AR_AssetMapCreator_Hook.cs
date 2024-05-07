using AssetRipper.SourceGenerated;
using System.Text;

namespace Ruri.RipperHook.AR_AssetMapCreator;

public partial class AR_AssetMapCreator_Hook : RipperHook
{
    public static Dictionary<string, HashSet<ClassIDType>> assetClassIDLookup = new Dictionary<string, HashSet<ClassIDType>>();
    public static Dictionary<string, HashSet<string>> assetDependenciesLookup = new Dictionary<string, HashSet<string>>();
}