namespace Ruri.RipperHook.AR_SkipProcessingAnimation;

/// <summary>
/// 跳过动画恢复 因为Unity导出的动画Path全部被Hash化 只能暴力计算所有Hash结构才能还原
/// 当游戏>10G之后 这个成本就已经无法估量了 因此直接跳过能节省你的生命
/// </summary>
public partial class AR_SkipProcessingAnimation_Hook : RipperHook
{
}