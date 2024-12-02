using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MyChat : MonoBehaviour
{
    [SerializeField]
    private MyModel myModel;

    [SerializeField]
    private Text response;

    [SerializeField]
    private Button reset;

    [SerializeField]
    private InputField input;

    [SerializeField]
    private Button send;

    private bool _generating;

    private void Awake()
    {
        reset.onClick.AddListener(OnReset);
        send.onClick.AddListener(OnSend);
    }

    private void Start()
    {
        myModel.ResetHistory();
    }

    private void OnReset()
    {
        if (_generating)
        {
            return;
        }

        myModel.ResetHistory();
    }

    private void OnSend()
    {
        if (_generating)
        {
            return;
        }

        StartCoroutine(GenerateResponse(input.text));
    }

    private IEnumerator GenerateResponse(string message)
    {
        _generating = true;
        yield return myModel.Generate(message, OnResponseUpdated);
        _generating = false;
    }

    private void OnResponseUpdated(string updatedResponse)
    {
        response.text = updatedResponse;
    }
}