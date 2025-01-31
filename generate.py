import requests
import sys

path = sys.argv[1]
with open(path, 'r') as file:
    l = file.readlines()
    prompts = [line.strip() for line in l]

# ENTER API KEY
api_key = ""
url = "https://api.tripo3d.ai/v2/openapi/task"

headers = {
    "Content-Type": "application/json",
    "Authorization": f"Bearer {api_key}"
}

task_ids = []

for p in prompts:
    data = {
        "type": "text_to_model",
        "prompt": p,
        "texture_quality": "detailed"
    }
    response = requests.post(url, headers=headers, json=data)
    task_ids.append(response.json()["data"]["task_id"])

with open("task_id.txt", 'w') as file:
    for t in task_ids:
        file.write(t)
        file.write("\n")