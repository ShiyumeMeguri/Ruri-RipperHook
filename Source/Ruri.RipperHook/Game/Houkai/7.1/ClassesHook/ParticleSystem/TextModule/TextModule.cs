using AssetRipper.Assets;
using AssetRipper.Assets.Export.Yaml;using AssetRipper.Assets.IO.Writing;using AssetRipper.IO.Endian;using AssetRipper.SourceGenerated.Subclasses.PPtr_Camera;using AssetRipper.SourceGenerated.Subclasses.PPtr_GameObject;

namespace Ruri.RipperHook.Houkai_7_1.Classes.Subclasses.TextModule;
public sealed class TextModule : UnityAssetBase, ITextModule, IUnityAssetBase, IEndianSpanReadable, IAssetWritable, IYamlExportable
{
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
	public int FontSize { get; set; }
	public int FontStyle { get; set; }
	public bool EmitWithWorldPosition { get; set; }
	public PPtr_Camera_5_0_0 SceneCamera { get; set; } = new();
	public PPtr_GameObject_5_0_0 Canvas { get; set; } = new();
	public PPtr_GameObject_5_0_0 Font0 { get; set; } = new();
	public PPtr_GameObject_5_0_0 Font1 { get; set; } = new();
	public PPtr_GameObject_5_0_0 Font2 { get; set; } = new();
	public PPtr_GameObject_5_0_0 Font3 { get; set; } = new();
	public PPtr_GameObject_5_0_0 Font4 { get; set; } = new();
	public PPtr_GameObject_5_0_0 Font5 { get; set; } = new();
	public PPtr_GameObject_5_0_0 Font6 { get; set; } = new();
	public PPtr_GameObject_5_0_0 Font7 { get; set; } = new();
	public PPtr_GameObject_5_0_0 Font8 { get; set; } = new();
	public PPtr_GameObject_5_0_0 Font9 { get; set; } = new();
	public PPtr_GameObject_5_0_0 Font10 { get; set; } = new();
	public PPtr_GameObject_5_0_0 Font11 { get; set; } = new();
	public PPtr_GameObject_5_0_0 Font12 { get; set; } = new();
	public PPtr_GameObject_5_0_0 Font13 { get; set; } = new();
	public PPtr_GameObject_5_0_0 Font14 { get; set; } = new();
	public PPtr_GameObject_5_0_0 Font15 { get; set; } = new();
	public PPtr_GameObject_5_0_0 Font16 { get; set; } = new();
	public PPtr_GameObject_5_0_0 Font17 { get; set; } = new();
	public PPtr_GameObject_5_0_0 Font18 { get; set; } = new();
	public PPtr_GameObject_5_0_0 Font19 { get; set; } = new();
	public PPtr_GameObject_5_0_0 Font20 { get; set; } = new();
	public PPtr_GameObject_5_0_0 Font21 { get; set; } = new();
	public PPtr_GameObject_5_0_0 Font22 { get; set; } = new();
	public PPtr_GameObject_5_0_0 Font23 { get; set; } = new();
	public PPtr_GameObject_5_0_0 Font24 { get; set; } = new();
	public PPtr_GameObject_5_0_0 Font25 { get; set; } = new();
	public PPtr_GameObject_5_0_0 Font26 { get; set; } = new();
	public PPtr_GameObject_5_0_0 Font27 { get; set; } = new();
	public PPtr_GameObject_5_0_0 Font28 { get; set; } = new();
	public PPtr_GameObject_5_0_0 Font29 { get; set; } = new();
	public PPtr_GameObject_5_0_0 Font30 { get; set; } = new();
	public PPtr_GameObject_5_0_0 Font31 { get; set; } = new();

}
