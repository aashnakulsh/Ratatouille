from fastapi import FastAPI, File, Form, UploadFile
import shutil
import tempfile
import recipe_builder
import prompt_gpt

app = FastAPI()

omlette = recipe_builder.build_omlette()
pbj = recipe_builder.build_pbj()

@app.post("/process/")
async def process_file(
    image: UploadFile = File(...),
    recipeNum: int = Form(...),
    step: int = Form(...)
):
    with tempfile.NamedTemporaryFile(delete=False, suffix=".jpg") as tmp:
        shutil.copyfileobj(image.file, tmp)
        tmp_path = tmp.name
    if recipeNum == 0:
        recipe = omlette
    else:
        recipe = pbj
    prompt = recipe.run_step(step)
    result_text = prompt_gpt.prompt_gpt(prompt, tmp_path)
    parsed = prompt_gpt.parse_response(result_text)
    return {"result": parsed}

