using AssetRipper.Export.Modules.Shaders;
using AssetRipper.Export.Modules.Shaders.Exporters;
using AssetRipper.Export.Modules.Shaders.Exporters.DirectX;
using AssetRipper.Export.Modules.Shaders.IO;
using AssetRipper.Export.Modules.Shaders.ShaderBlob;
using AssetRipper.SourceGenerated.Extensions.Enums.Shader;
using System.Runtime.InteropServices;

namespace AssetRipper.RuriHook.ShaderDecompiler.Exporters.DirectX
{
	public partial class ShaderHLSLccExporter : ShaderTextExporter
	{
		[LibraryImport("hlslcc.dll")]
		private static partial IntPtr TranslateHLSLFromMemCSharp(byte[] shader, uint flags, GLLang language);

		public enum GLLang
		{
			LANG_DEFAULT, // Depends on the HLSL shader model.
			LANG_ES_100,
			LANG_ES_FIRST = LANG_ES_100,
			LANG_ES_300,
			LANG_ES_310,
			LANG_ES_LAST = LANG_ES_310,
			LANG_120,
			LANG_GL_FIRST = LANG_120,
			LANG_130,
			LANG_140,
			LANG_150,
			LANG_330,
			LANG_400,
			LANG_410,
			LANG_420,
			LANG_430,
			LANG_440,
			LANG_GL_LAST = LANG_440,
			LANG_METAL
		}
		protected readonly GPUPlatform m_graphicApi;
		public ShaderHLSLccExporter(GPUPlatform graphicApi)
		{
			m_graphicApi = graphicApi;
		}
		public override void Export(ShaderWriter writer, ref ShaderSubProgram subProgram)
		{
			using (MemoryStream stream = new MemoryStream(subProgram.ProgramData))
			{
				using (BinaryReader reader = new BinaryReader(stream))
				{
					DXDataHeader header = new DXDataHeader();
					header.Read(reader, writer.Version);

					// HACK: since we can't restore UAV info and HLSLcc requires it, process such shader with default exporter
					if (header.UAVs > 0)
					{
						base.Export(writer, ref subProgram);
					}
					else
					{
						byte[] exportData = DXShaderProgramRestorer.RestoreProgramData(reader, writer.Version, ref subProgram);
						// todo Fix Memory Leak
						uint flag = 0x1;
						GLLang language = GLLang.LANG_DEFAULT;
						IntPtr sourceCode = TranslateHLSLFromMemCSharp(exportData, flag, language);
						string sourceC = Marshal.PtrToStringAnsi(sourceCode);

						if (string.IsNullOrEmpty(sourceC))
						{
							base.Export(writer, ref subProgram);
						}
						else
						{
							ExportListing(writer, sourceC);
						}
					}
				}
			}
		}
	}
}
