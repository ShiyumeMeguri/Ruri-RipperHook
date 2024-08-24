using HLSLccWrapper;

namespace HLSLccDecompiler
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("请提供一个文件路径作为参数.");
                return;
            }

            string inputFilePath = args[0];

            if (!File.Exists(inputFilePath))
            {
                Console.WriteLine($"文件 '{inputFilePath}' 不存在.");
                return;
            }

            try
            {
                // 读取文件数据
                byte[] fileData = File.ReadAllBytes(inputFilePath);

                List<byte[]> dxbcSections = new List<byte[]>();
                int offset = 0;

                while (offset < fileData.Length)
                {
                    // 查找 'DXBC' 段的开头
                    int start = FindNextDXBC(fileData, offset);
                    if (start == -1) break;

                    // 查找下一个 'DXBC' 段的开头
                    int end = FindNextDXBC(fileData, start + 4);
                    if (end == -1) end = fileData.Length;

                    // 提取 'DXBC' 段
                    int length = end - start;
                    byte[] dxbcSection = new byte[length];
                    Array.Copy(fileData, start, dxbcSection, 0, length);
                    dxbcSections.Add(dxbcSection);

                    // 更新偏移量
                    offset = end;
                }

                if (dxbcSections.Count == 0)
                {
                    Console.WriteLine("未找到任何 'DXBC' 段.");
                    return;
                }

                string outputDirectory = Path.Combine(Path.GetDirectoryName(inputFilePath), Path.GetFileNameWithoutExtension(inputFilePath) + "Output");
                Directory.CreateDirectory(outputDirectory);

                WrappedGlExtensions ext = new WrappedGlExtensions
                {
                    ARB_explicit_attrib_location = 1,
                    ARB_explicit_uniform_location = 1,
                    ARB_shading_language_420pack = 0,
                    OVR_multiview = 0,
                    EXT_shader_framebuffer_fetch = 0
                };

                // 逐个反编译找到的 'DXBC' 段
                for (int i = 0; i < dxbcSections.Count; i++)
                {
                    byte[] exportData = dxbcSections[i];
                    var shader = WrappedShader.TranslateFromMem(exportData, WrappedGLLang.LANG_DEFAULT, ext);

                    // 确定输出文件名格式为 *_index.hlsl
                    string outputFileName = $"{Path.GetFileNameWithoutExtension(inputFilePath)}_{i}.hlsl";
                    string outputFilePath = Path.Combine(outputDirectory, outputFileName);

                    using (StreamWriter writer = new StreamWriter(outputFilePath))
                    {
                        if (shader.OK == 0)
                        {
                            writer.Write("Failed to decompile");
                        }
                        else
                        {
                            writer.Write(shader.Text);
                        }
                    }

                    Console.WriteLine($"反编译完成，输出文件: {outputFilePath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"处理文件时发生错误: {ex.Message}");
            }
        }

        // 查找下一个 'DXBC' 段的方法
        static int FindNextDXBC(byte[] data, int startIndex)
        {
            for (int i = startIndex; i < data.Length - 3; i++)
            {
                if (data[i] == 'D' && data[i + 1] == 'X' && data[i + 2] == 'B' && data[i + 3] == 'C')
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
