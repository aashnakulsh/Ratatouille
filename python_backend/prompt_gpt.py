import base64
import openai
from openai import OpenAI

OPENAI_API_KEY = "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"

client = OpenAI(api_key = OPENAI_API_KEY)

def prompt_gpt(prompt, image_path):
    with open(image_path, "rb") as f:
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
    return (resp.output_text)

def parse_response(response):
    resp = {}
    
    try: 
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
    #if GPT outputs garbage we can output a generic response
    except:
        resp = {"rating":3, "pos": "I can see you put in a good effort here", "impr": "I fear not even a rat would eat this"}
    return resp
