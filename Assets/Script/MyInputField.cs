using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyInputField : MonoBehaviour
{
    private TextMesh textMesh;
    [SerializeField] [Range(1, 20)] private int lineLimit = 20;
    private List<string> codeLine;
    private int posX, posY;

    private void Awake()
    {
        textMesh = GetComponent<TextMesh>();
    }

    private void OnEnable()
    {
        StartCoroutine(KeyboardTypeIn());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator KeyboardTypeIn()
    {
        while (Input.anyKey)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                codeLine.Insert(posX, string.Empty);
            }
            yield return null;
        }
    }

    public void SetText(string codeText)
    {
        if (codeLine != null) codeLine.Clear();
        else codeLine = new List<string>();
        posX = posY = 0;
        string l = string.Empty;
        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
        foreach (var c in codeText)
        {
            if (c == '\r' || c == '\n')
            {
            }
            stringBuilder.Append(c);
            if (stringBuilder.Length >= lineLimit)
            {
                codeLine.Insert(posX, l);
            }
        }
    }
}
