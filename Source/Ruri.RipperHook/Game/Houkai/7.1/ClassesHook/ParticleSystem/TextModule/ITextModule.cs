using AssetRipper.Assets;using AssetRipper.Assets.Export.Yaml;using AssetRipper.Assets.IO.Writing;using AssetRipper.IO.Endian;using AssetRipper.SourceGenerated.Subclasses.PPtr_Camera;using AssetRipper.SourceGenerated.Subclasses.PPtr_GameObject;
namespace Ruri.RipperHook.Houkai_7_1.Classes.Subclasses.TextModule;
public interface ITextModule : IUnityAssetBase, IEndianSpanReadable, IAssetWritable, IYamlExportable
{
	bool Enabled { get; set; }
	PPtr_Camera_5_0_0 SceneCamera { get; set; }
	PPtr_GameObject_5_0_0 Canvas { get; set; }
	PPtr_GameObject_5_0_0 Font0 { get; set; }
	PPtr_GameObject_5_0_0 Font1 { get; set; }
	PPtr_GameObject_5_0_0 Font2 { get; set; }
	PPtr_GameObject_5_0_0 Font3 { get; set; }
	PPtr_GameObject_5_0_0 Font4 { get; set; }
	PPtr_GameObject_5_0_0 Font5 { get; set; }
	PPtr_GameObject_5_0_0 Font6 { get; set; }
	PPtr_GameObject_5_0_0 Font7 { get; set; }
	PPtr_GameObject_5_0_0 Font8 { get; set; }
	PPtr_GameObject_5_0_0 Font9 { get; set; }
	PPtr_GameObject_5_0_0 Font10 { get; set; }
	PPtr_GameObject_5_0_0 Font11 { get; set; }
	PPtr_GameObject_5_0_0 Font12 { get; set; }
	PPtr_GameObject_5_0_0 Font13 { get; set; }
	PPtr_GameObject_5_0_0 Font14 { get; set; }
	PPtr_GameObject_5_0_0 Font15 { get; set; }
	PPtr_GameObject_5_0_0 Font16 { get; set; }
	PPtr_GameObject_5_0_0 Font17 { get; set; }
	PPtr_GameObject_5_0_0 Font18 { get; set; }
	PPtr_GameObject_5_0_0 Font19 { get; set; }
	PPtr_GameObject_5_0_0 Font20 { get; set; }
	PPtr_GameObject_5_0_0 Font21 { get; set; }
	PPtr_GameObject_5_0_0 Font22 { get; set; }
	PPtr_GameObject_5_0_0 Font23 { get; set; }
	PPtr_GameObject_5_0_0 Font24 { get; set; }
	PPtr_GameObject_5_0_0 Font25 { get; set; }
	PPtr_GameObject_5_0_0 Font26 { get; set; }
	PPtr_GameObject_5_0_0 Font27 { get; set; }
	PPtr_GameObject_5_0_0 Font28 { get; set; }
	PPtr_GameObject_5_0_0 Font29 { get; set; }
	PPtr_GameObject_5_0_0 Font30 { get; set; }
	PPtr_GameObject_5_0_0 Font31 { get; set; }
}
