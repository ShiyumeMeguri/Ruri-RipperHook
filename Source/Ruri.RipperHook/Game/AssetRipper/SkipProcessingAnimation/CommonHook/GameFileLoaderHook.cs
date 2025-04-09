using AssetRipper.GUI.Web;
using AssetRipper.Import.Logging;
using AssetRipper.Processing.AnimationClips;
using AssetRipper.SourceGenerated.Classes.ClassID_74;
using System.IO;

namespace Ruri.RipperHook.AR_SkipProcessingAnimation;

public partial class AR_SkipProcessingAnimation_Hook
{
    [RetargetMethod(typeof(AnimationClipConverter), nameof(Process), isBefore: true, isReturn: true)]
    public static void Process(IAnimationClip clip, PathChecksumCache checksumCache)
    {
        return;
    }
}