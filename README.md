# Unity LLM Demo

The project is in 6000.0.29f1 but should work under any recent Unity version.

## How to clone

```shell
git clone --recursive https://github.com/andyroiiid/UnityLLMDemo.git
```

Or if you've already cloned without `--recursive`:

```shell
git submodule update --init
```

## Dependencies & Licenses

- [ONNX Runtime](https://github.com/microsoft/onnxruntime) - [MIT License](https://github.com/microsoft/onnxruntime/blob/main/LICENSE)
    - Assets/Microsoft.ML.OnnxRuntimeGenAI/onnxruntime.dll
    - Assets/Microsoft.ML.OnnxRuntimeGenAI/onnxruntime_providers_shared.dll
- [ONNX Runtime generate() API](https://github.com/microsoft/onnxruntime-genai) - [MIT License](https://github.com/microsoft/onnxruntime-genai/blob/main/LICENSE)
    - Assets/Microsoft.ML.OnnxRuntimeGenAI/*.cs
    - Assets/Microsoft.ML.OnnxRuntimeGenAI/onnxruntime-genai.dll
- [Llama 3.2 ONNX models](https://huggingface.co/onnx-community/Llama-3.2-1B-Instruct-ONNX) - [LLAMA 3.2 Community License](https://github.com/meta-llama/llama-models/blob/main/models/llama3_2/LICENSE)
    - Assets/StreamingAssets/Llama-3.2-1B-Instruct-ONNX/*

## Code explanation

### Assets/Scripts/MyModel.cs

The model interface and chat history implementation.

You can remove `_history`-related code if you just need an one-off completion.

Check https://github.com/microsoft/onnxruntime-genai/tree/main/examples for more examples.

### Assets/Scripts/MyChat.cs

A (very) basic UI controller.

### Assets/Scripts/StringBuilderExtensions.cs

Just one `StringBuilder` helper function.

## License

**Here's a reminder that this repository is GPL-licensed**.

The code is super simple, and you can write your own in less than an hour - just don't copy paste directly from this
repo.
