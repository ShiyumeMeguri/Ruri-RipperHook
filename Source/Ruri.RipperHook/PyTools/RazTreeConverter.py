import os
import re
import json

cwd = os.getcwd()
out_dir = os.path.join(cwd, 'output')
os.makedirs(out_dir, exist_ok=True)

for root_id in os.listdir(cwd):
    root_path = os.path.join(cwd, root_id)
    if not os.path.isdir(root_path) or root_id == 'output':
        continue

    for ver_folder in os.listdir(root_path):
        ver_path = os.path.join(root_path, ver_folder)
        info_path = os.path.join(ver_path, 'info.json')
        if not os.path.isfile(info_path):
            continue

        with open(info_path, 'r', encoding='utf-8') as f:
            data = json.load(f)

        orig = data.get('Version', '')
        m = re.match(r'^(\d+)\.(\d+)', orig)
        if m:
            major, minor = m.groups()
        else:
            parts = re.findall(r'\d+', orig)
            major, minor = parts[0], parts[1] if len(parts) > 1 else ('', '')

        digits = ''.join(filter(str.isdigit, ver_folder))
        new_version = f"{major}.{minor}.{digits}x{root_id}"
        data['Version'] = new_version

        out_file = os.path.join(out_dir, f"{new_version}.json")
        with open(out_file, 'w', encoding='utf-8') as f:
            json.dump(data, f, ensure_ascii=False, separators=(',',':'))

        print(f"[OK] {info_path} -> {out_file}")
