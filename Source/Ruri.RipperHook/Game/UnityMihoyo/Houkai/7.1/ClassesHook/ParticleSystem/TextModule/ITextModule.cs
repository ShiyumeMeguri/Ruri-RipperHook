using AssetRipper.Assets;
using AssetRipper.Assets.IO.Writing;
using AssetRipper.IO.Endian;
using AssetRipper.SourceGenerated.Subclasses.PPtr_Camera;
using AssetRipper.SourceGenerated.Subclasses.PPtr_GameObject;

namespace Ruri.RipperHook.Houkai_7_1.Classes.Subclasses.TextModule;

public interface ITextModule : IUnityAssetBase, IEndianSpanReadable, IAssetWritable
{
    bool Enabled { get; set; }
    PPtr_Camera_5 SceneCamera { get; set; }
    PPtr_GameObject_5 Canvas { get; set; }
    PPtr_GameObject_5 Font0 { get; set; }
    PPtr_GameObject_5 Font1 { get; set; }
    PPtr_GameObject_5 Font2 { get; set; }
    PPtr_GameObject_5 Font3 { get; set; }
    PPtr_GameObject_5 Font4 { get; set; }
    PPtr_GameObject_5 Font5 { get; set; }
    PPtr_GameObject_5 Font6 { get; set; }
    PPtr_GameObject_5 Font7 { get; set; }
    PPtr_GameObject_5 Font8 { get; set; }
    PPtr_GameObject_5 Font9 { get; set; }
    PPtr_GameObject_5 Font10 { get; set; }
    PPtr_GameObject_5 Font11 { get; set; }
    PPtr_GameObject_5 Font12 { get; set; }
    PPtr_GameObject_5 Font13 { get; set; }
    PPtr_GameObject_5 Font14 { get; set; }
    PPtr_GameObject_5 Font15 { get; set; }
    PPtr_GameObject_5 Font16 { get; set; }
    PPtr_GameObject_5 Font17 { get; set; }
    PPtr_GameObject_5 Font18 { get; set; }
    PPtr_GameObject_5 Font19 { get; set; }
    PPtr_GameObject_5 Font20 { get; set; }
    PPtr_GameObject_5 Font21 { get; set; }
    PPtr_GameObject_5 Font22 { get; set; }
    PPtr_GameObject_5 Font23 { get; set; }
    PPtr_GameObject_5 Font24 { get; set; }
    PPtr_GameObject_5 Font25 { get; set; }
    PPtr_GameObject_5 Font26 { get; set; }
    PPtr_GameObject_5 Font27 { get; set; }
    PPtr_GameObject_5 Font28 { get; set; }
    PPtr_GameObject_5 Font29 { get; set; }
    PPtr_GameObject_5 Font30 { get; set; }
    PPtr_GameObject_5 Font31 { get; set; }
}