import json
import os

def parse_node(node, file, level=0):
    # 根据层级添加制表符
    indent = '\t' * level
    # 检查MetaFlag以确定是否添加 // Align 注释
    align_comment = " // Align" if node["MetaFlag"] & 0x4000 else ""
    file.write(f"{indent}{node['TypeName']} {node['Name']};{align_comment}\n")

    # 递归解析子节点
    if "SubNodes" in node:
        for subnode in node["SubNodes"]:
            parse_node(subnode, file, level + 1)

def parse_classes(classes, file):
    for cls in classes:
        file.write(f"Class {cls['Name']}\n")
        if "ReleaseRootNode" in cls and cls["ReleaseRootNode"]:
            parse_node(cls["ReleaseRootNode"], file)
        file.write("\n")  # 每个类别后添加一个空行

def parse_json(json_file):
    with open(json_file, 'r') as file:
        data = json.load(file)

    if "Classes" in data:
        # 创建输出文件
        output_file_name = f"{os.path.splitext(json_file)[0]}.txt"
        with open(output_file_name, 'w') as output_file:
            parse_classes(data["Classes"], output_file)
        print(f"Output written to {output_file_name}")

if __name__ == "__main__":
    json_file = 'info.json'
    if len(sys.argv) > 1:
        json_file = sys.argv[1]

    parse_json(json_file)