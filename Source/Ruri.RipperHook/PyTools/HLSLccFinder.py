import os
import re

def replace_in_file_content():
    count = 1  # 初始化计数器
    max_count = 9999  # 最大替换计数

    # 获取当前文件夹下的所有文件
    entries = [entry for entry in os.listdir('.') if entry.endswith('.cpp')]

    # 遍历所有.cpp文件
    for entry in entries:
        # 读取文件内容
        with open(entry, 'r', encoding='utf-8') as file:
            content = file.read()
        
        # 使用正则表达式查找和替换内容中的 '{Ruri_}'
        # 这里我们使用一个lambda函数来在每次匹配时递增计数器
        def replacer(match):
            nonlocal count
            result = f"{{{count:04d}}}"
            count += 1
            return result
        
        new_content, n = re.subn(r'\{Ruri_\}', replacer, content)
        if n > 0:  # 如果发生了替换
            # 保存修改后的文件
            with open(entry, 'w', encoding='utf-8') as file:
                file.write(new_content)
            print(f"Updated {entry}: replaced {n} occurrences.")
            
            if count > max_count:
                print("Reached the maximum count, stopping...")
                break  # 如果计数器超过最大值，则停止

# 调用函数
replace_in_file_content()
