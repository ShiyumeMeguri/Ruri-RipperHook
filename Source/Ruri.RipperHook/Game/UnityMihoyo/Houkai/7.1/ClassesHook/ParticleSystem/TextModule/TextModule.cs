using AssetRipper.Assets;
using AssetRipper.Assets.IO.Writing;
using AssetRipper.IO.Endian;
using AssetRipper.SourceGenerated.Subclasses.PPtr_Camera;
using AssetRipper.SourceGenerated.Subclasses.PPtr_GameObject;

namespace Ruri.RipperHook.Houkai_7_1.Classes.Subclasses.TextModule;

public sealed class TextModule : UnityAssetBase, ITextModule, IUnityAssetBase, IEndianSpanReadable, IAssetWritable
{
    public int FontSize { get; set; }
    public int FontStyle { get; set; }
    public bool EmitWithWorldPosition { get; set; }

    public override void ReadRelease(ref EndianSpanReader reader)
    {
        Enabled = reader.ReadRelease_BooleanAlign();
        SceneCamera.ReadRelease(ref reader);
        Canvas.ReadRelease(ref reader);
        Font0.ReadRelease(ref reader);
        Font1.ReadRelease(ref reader);
        Font2.ReadRelease(ref reader);
        Font3.ReadRelease(ref reader);
        Font4.ReadRelease(ref reader);
        Font5.ReadRelease(ref reader);
        Font6.ReadRelease(ref reader);
        Font7.ReadRelease(ref reader);
        Font8.ReadRelease(ref reader);
        Font9.ReadRelease(ref reader);
        Font10.ReadRelease(ref reader);
        Font11.ReadRelease(ref reader);
        Font12.ReadRelease(ref reader);
        Font13.ReadRelease(ref reader);
        Font14.ReadRelease(ref reader);
        Font15.ReadRelease(ref reader);
        Font16.ReadRelease(ref reader);
        Font17.ReadRelease(ref reader);
        Font18.ReadRelease(ref reader);
        Font19.ReadRelease(ref reader);
        Font20.ReadRelease(ref reader);
        Font21.ReadRelease(ref reader);
        Font22.ReadRelease(ref reader);
        Font23.ReadRelease(ref reader);
        Font24.ReadRelease(ref reader);
        Font25.ReadRelease(ref reader);
        Font26.ReadRelease(ref reader);
        Font27.ReadRelease(ref reader);
        Font28.ReadRelease(ref reader);
        Font29.ReadRelease(ref reader);
        Font30.ReadRelease(ref reader);
        Font31.ReadRelease(ref reader);
        FontSize = reader.ReadInt32();
        FontStyle = reader.ReadRelease_Int32Align();
        EmitWithWorldPosition = reader.ReadRelease_BooleanAlign();
    }

    public bool Enabled { get; set; }
    public PPtr_Camera_5 SceneCamera { get; set; } = new();
    public PPtr_GameObject_5 Canvas { get; set; } = new();
    public PPtr_GameObject_5 Font0 { get; set; } = new();
    public PPtr_GameObject_5 Font1 { get; set; } = new();
    public PPtr_GameObject_5 Font2 { get; set; } = new();
    public PPtr_GameObject_5 Font3 { get; set; } = new();
    public PPtr_GameObject_5 Font4 { get; set; } = new();
    public PPtr_GameObject_5 Font5 { get; set; } = new();
    public PPtr_GameObject_5 Font6 { get; set; } = new();
    public PPtr_GameObject_5 Font7 { get; set; } = new();
    public PPtr_GameObject_5 Font8 { get; set; } = new();
    public PPtr_GameObject_5 Font9 { get; set; } = new();
    public PPtr_GameObject_5 Font10 { get; set; } = new();
    public PPtr_GameObject_5 Font11 { get; set; } = new();
    public PPtr_GameObject_5 Font12 { get; set; } = new();
    public PPtr_GameObject_5 Font13 { get; set; } = new();
    public PPtr_GameObject_5 Font14 { get; set; } = new();
    public PPtr_GameObject_5 Font15 { get; set; } = new();
    public PPtr_GameObject_5 Font16 { get; set; } = new();
    public PPtr_GameObject_5 Font17 { get; set; } = new();
    public PPtr_GameObject_5 Font18 { get; set; } = new();
    public PPtr_GameObject_5 Font19 { get; set; } = new();
    public PPtr_GameObject_5 Font20 { get; set; } = new();
    public PPtr_GameObject_5 Font21 { get; set; } = new();
    public PPtr_GameObject_5 Font22 { get; set; } = new();
    public PPtr_GameObject_5 Font23 { get; set; } = new();
    public PPtr_GameObject_5 Font24 { get; set; } = new();
    public PPtr_GameObject_5 Font25 { get; set; } = new();
    public PPtr_GameObject_5 Font26 { get; set; } = new();
    public PPtr_GameObject_5 Font27 { get; set; } = new();
    public PPtr_GameObject_5 Font28 { get; set; } = new();
    public PPtr_GameObject_5 Font29 { get; set; } = new();
    public PPtr_GameObject_5 Font30 { get; set; } = new();
    public PPtr_GameObject_5 Font31 { get; set; } = new();
}