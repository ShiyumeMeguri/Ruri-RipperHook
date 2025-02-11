import sys
import json
import os
import glob  # 用于找到匹配的文件路径

def parse_node(node, file, level=0):
    indent = '\t' * level
    align_comment = " // Align" if node["MetaFlag"] & 0x4000 else ""
    file.write(f"{indent}{node['TypeName']} {node['Name']};{align_comment}\n")

    if "SubNodes" in node:
        for subnode in node["SubNodes"]:
            parse_node(subnode, file, level + 1)

def parse_class(cls, output_dir):
    # 输出到以Class名称命名的txt文件
    output_file_name = os.path.join(output_dir, f"{cls['Name']}.txt")
    with open(output_file_name, 'w') as file:
        file.write(f"Class {cls['Name']}\n")
        if "ReleaseRootNode" in cls and cls["ReleaseRootNode"]:
            parse_node(cls["ReleaseRootNode"], file)
        file.write("\n")
    print(f"Output written to {output_file_name}")

def parse_classes(classes, output_dir):
    for cls in classes:
        parse_class(cls, output_dir)

def parse_json(json_file):
    with open(json_file, 'r') as file:
        data = json.load(file)

    if "Classes" in data:
        # 修改输出文件路径到TypeTreeOutput文件夹，并以json文件名作为子文件夹
        output_dir = os.path.join("TypeTreeOutput", os.path.splitext(os.path.basename(json_file))[0])
        os.makedirs(output_dir, exist_ok=True)  # 确保输出目录存在
        parse_classes(data["Classes"], output_dir)

if __name__ == "__main__":
    # 遍历TypeTreeDump目录下的所有json文件    
    for json_file in glob.glob('TypeTreeDump/*.json'):
        parse_json(json_file)

# 官方Dump库 https://github.com/AssetRipper/TypeTreeDumps/tree/main/InfoJson