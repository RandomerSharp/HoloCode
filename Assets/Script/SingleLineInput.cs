using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleLineInput : MonoBehaviour, IVKeyInput
{
    public delegate void InputComplateHandler();
    public event InputComplateHandler InputComplate;

    private TMPro.TextMeshPro tmpro;

    private void Awake()
    {
        tmpro = GetComponentInChildren<TMPro.TextMeshPro>();
    }

    private void OnDisable()
    {
        InputComplate = null;
    }

    public string GetContent()
    {
        return tmpro.text;
    }

    public void VKeyInput(KeyCode key)
    {
        if (key == KeyCode.Return)
        {
            InputComplate.Invoke();
            gameObject.SetActive(false);
            return;
        }
        if (key == KeyCode.Backspace)
        {
            tmpro.text = tmpro.text.Substring(0, tmpro.text.Length - 1);
            return;
        }
        tmpro.text += (char)key;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            InputComplate.Invoke();
            gameObject.SetActive(false);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            tmpro.text = tmpro.text.Substring(0, tmpro.text.Length - 1);
            return;
        }
        tmpro.text += Input.inputString;
    }
}
