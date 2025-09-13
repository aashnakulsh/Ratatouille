import base64
import openai
from openai import OpenAI

OPENAI_API_KEY = "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"

client = OpenAI(api_key = OPENAI_API_KEY)

def prompt_gpt(prompt, image_path):
    with open("image_path", "rb") as f:
        b64 = base64.b64encode(f.read()).decode("utf-8")
    data_url = f"data:image/jpeg;base64,{b64}"

    resp = client.responses.create(
        model="gpt-4o-mini",
        input=[{
            "role": "user",
            "content": [
                {"type": "input_text", "text": prompt},
                {"type": "input_image", "image_url": data_url}
            ]
        }]
    )
    print(resp.output_text)

def parse_response(response):
    resp = {}
    resp_list = response.split("\n")
    
    if len(resp_list) != 3: raise ValueError("wrong!")

    #integer
    rating = resp_list[0]
    digit = "".join(filter(str.isdigit, rating))

    if len(digit)<1: raise ValueError("wrong!")

    digit = int(digit)
    if digit > 5 or digit <1: raise ValueError("wrong rating!")

    resp["rating"] = digit

    #feedback:
    if len(resp_list[1])<1: raise ValueError("wrong!")
    if len(resp_list[2])<1: raise ValueError("wrong!")

    resp["pos"] = resp_list[1]
    resp["impr"] = resp_list[2]

    return resp
