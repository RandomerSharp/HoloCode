using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/*
public class MyInputField : MonoBehaviour
{
    private TextMesh textMesh;
    [SerializeField] [Range(1, 20)] private int lineLimit = 20;
    private List<string> codeLine;
    private int posX, posY;
    private bool codeUpdate;

    private void Awake()
    {
        textMesh = GetComponent<TextMesh>();
        if (codeLine != null) codeLine.Clear();
        else codeLine = new List<string>();
        codeLine.Add(string.Empty);
        posX = 1;
        posY = 0;
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
        while (true)
        {
            if (Input.anyKey)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    codeLine.Insert(posX, string.Empty);
                    posX++;
                    posY = 0;
                }
                if (Input.GetKeyDown(KeyCode.Backspace))
                {
                    if (codeLine[posX].Length == 0)
                    {
                        codeLine.RemoveAt(posX);
                        posX--;
                        posY = 0;
                    }
                    else
                    {
                        if (posY >= codeLine[posX].Length)
                        {
                            codeLine[posX].Remove(posY - 1, 1);
                        }
                        else
                        {
                            codeLine[posX].Remove(posY, 1);
                        }
                        //codeLine[posX].Remove(posY, 1);
                        posY--;
                    }
                }
                if (Input.inputString.Length > 0)
                {
                    Debug.Log(Input.compositionCursorPos);
                    if (posY >= codeLine[posX].Length)
                    {
                        codeLine[posX] += Input.inputString;
                    }
                    else if (posY == 0)
                    {
                        codeLine[posX] = codeLine[posX].Insert(posY, Input.inputString);
                    }
                    else
                    {
                        codeLine[posX] = codeLine[posX].Insert(posY, Input.inputString);
                    }
                    posY += Input.inputString.Length;
                    if (codeLine[posX].Length > lineLimit)
                    {
                        string newLine = codeLine[posX].Substring(lineLimit + 1);
                        codeLine[posX] = codeLine[posX].Substring(0, lineLimit);
                        codeLine.Insert(posX, newLine);
                        posX++;
                    }
                }
                UpdateCode();
            }
            yield return null;
        }
    }

    private void UpdateCode()
    {
        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
        for (int i = 1; i < codeLine.Count; i++)
        //foreach (var item in codeLine)
        {
            //stringBuilder.AppendLine(item);
            stringBuilder.AppendLine(codeLine[i]);
        }
        textMesh.text = stringBuilder.ToString();
    }

    public void SetText(string codeText)
    {
        if (codeLine != null) codeLine.Clear();
        else codeLine = new List<string>();
        codeLine.Add(string.Empty);

        if (textMesh == null)
        {
            textMesh = GetComponent<TextMesh>();
        }
        textMesh.text = codeText;

        posX = 1;
        posY = 0;
        //System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
        foreach (var s in codeText.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
        {
            codeLine.Add(s);
        }
    }
}*/
public class MyInputField : MonoBehaviour
{
    private TextMeshPro textMesh;
    private int posX, posY;
    private bool codeUpdate;
    private List<string> codeLine;

    private float lineHeight = 4.22f;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        posX = 1;
        posY = 0;
        if (codeLine != null) codeLine.Clear();
        else codeLine = new List<string>();
        codeLine.Add(string.Empty);
    }

    private void OnEnable()
    {
        StartCoroutine(KeyboardTypeIn());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Update()
    {

    }

    private IEnumerator KeyboardTypeIn()
    {
        while (true)
        {
            if (Input.anyKey)
            {
                textMesh.text += Input.inputString;
            }
            yield return null;
        }
    }

    private void UpdateCode()
    {
        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
        for (int i = 1; i < codeLine.Count; i++)
        {
            stringBuilder.AppendLine(codeLine[i]);
        }
        textMesh.text = stringBuilder.ToString();
    }

    public void SetText(string codeText)
    {
        if (textMesh == null)
        {
            textMesh = GetComponent<TextMeshPro>();
        }
        textMesh.text = codeText;

        posX = 1;
        posY = 0;

        foreach (var s in codeText.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
        {
            codeLine.Add(s);
        }
    }
}
