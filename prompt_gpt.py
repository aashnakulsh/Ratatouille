import base64
import openai
from openai import OpenAI

OPENAI_API_KEY = "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"

client = OpenAI(api_key = OPENAI_API_KEY)

with open("/Users/maryzaher/rat/image.png", "rb") as f:
    b64 = base64.b64encode(f.read()).decode("utf-8")
data_url = f"data:image/jpeg;base64,{b64}"

resp = client.responses.create(
    model="gpt-4o-mini",
    input=[{
        "role": "user",
        "content": [
            {"type": "input_text", "text": "Is the butter starting to brown?"},
            {"type": "input_image", "image_url": data_url}
        ]
    }]
)
print(resp.output_text)
