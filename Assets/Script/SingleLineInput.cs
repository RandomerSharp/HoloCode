using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleLineInput : MonoBehaviour, IVKeyInput
{
    public Action<string> InputComplate;

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
            InputComplate.Invoke(tmpro.text);
            gameObject.SetActive(false);
            return;
        }
        if (key == KeyCode.Backspace)
        {
            if (tmpro.text.Length == 0) return;
            tmpro.text = tmpro.text.Substring(0, tmpro.text.Length - 1);
            return;
        }
        tmpro.text += (char)key;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            InputComplate.Invoke(tmpro.text);
            gameObject.SetActive(false);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (tmpro.text.Length == 0) return;
            tmpro.text = tmpro.text.Substring(0, tmpro.text.Length - 1);
            return;
        }
        tmpro.text += Input.inputString;
    }
}
