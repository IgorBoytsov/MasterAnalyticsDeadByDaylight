from fastapi import FastAPI, UploadFile, File

app = FastAPI(title="Screenshot Extractor")

@app.get("/health")
async def health_check():
    return {"status": "ok"}

@app.post("/analyze")
async def analyze_screenshot(file: UploadFile = File(...)):
    content = await file.read()
    return {
        "message": "Screenshot received",
        "filename": file.filename,
        "size": len(content),
    }

if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="0.0.0.0", port=8001)