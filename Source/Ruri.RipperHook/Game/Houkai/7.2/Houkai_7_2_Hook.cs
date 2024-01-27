using Ruri.RipperHook.Houkai_7_1;

namespace Ruri.RipperHook.Houkai_7_2;

public partial class Houkai_7_2_Hook : Houkai_7_1_Hook
{
    protected override void InitAttributeHook()
    {
        excludedNamespaces.Add(GetType().Namespace);
        additionalNamespaces.Add("Ruri.RipperHook.Houkai_7_1");
        base.InitAttributeHook();
    }
}