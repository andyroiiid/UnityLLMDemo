using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Microsoft.ML.OnnxRuntimeGenAI;
using UnityEngine.Events;

public class MyModel : MonoBehaviour
{
    [SerializeField, Multiline]
    private string systemPrompt = "You are a helpful AI assistant.";

    private Model _model;
    private Tokenizer _tokenizer;
    private TokenizerStream _tokenizerStream;
    private readonly List<Tuple<string, string>> _history = new();

    private void Awake()
    {
        var modelPath = Path.Join(Application.streamingAssetsPath,
            "Llama-3.2-1B-Instruct-ONNX/cpu_and_mobile/cpu-int4-rtn-block-32-acc-level-4");
        _model = new Model(modelPath);
        _tokenizer = new Tokenizer(_model);
        _tokenizerStream = _tokenizer.CreateStream();
    }

    private void OnDestroy()
    {
        _model?.Dispose();
        _tokenizer?.Dispose();
        _tokenizerStream?.Dispose();
    }

    public void ResetHistory()
    {
        _history.Clear();
    }

    private string GeneratePromptWithHistory(string input)
    {
        // See ONNXModel.__init__ in this script for history format references
        // https://github.com/microsoft/onnxruntime-genai/blob/rel-0.5.2/examples/chat_app/interface/hddr_llm_onnx_interface.py

        var sb = new StringBuilder();
        sb.Append("<|begin_of_text|>");
        sb.AppendLlama3Dialog("system", systemPrompt);

        foreach (var dialog in _history)
        {
            sb.AppendLlama3Dialog("user", dialog.Item1);
            sb.AppendLlama3Dialog("assistant", dialog.Item2);
        }

        sb.AppendLlama3Dialog("user", input);
        sb.Append("<|start_header_id|>assistant<|end_header_id|>");
        return sb.ToString();
    }

    public IEnumerator Generate(string input, UnityAction<string> onResponseUpdated)
    {
        var prompt = GeneratePromptWithHistory(input);

        using var sequences = _tokenizer.Encode(prompt);

        using var generatorParams = new GeneratorParams(_model);
        // Check json files in the model folder for available options
        generatorParams.SetSearchOption("max_length", 1024);
        generatorParams.SetInputSequences(sequences);

        using var generator = new Generator(_model, generatorParams);
        var sb = new StringBuilder();
        while (!generator.IsDone())
        {
            generator.ComputeLogits();
            generator.GenerateNextToken();
            using var outputLogits = generator.GetOutput("logits");
            var tokens = generator.GetSequence(0);
            var newToken = _tokenizerStream.Decode(tokens[^1]);
            sb.Append(newToken);
            onResponseUpdated(sb.ToString());
            yield return null;
        }

        _history.Add(new Tuple<string, string>(input, sb.ToString()));
    }
}