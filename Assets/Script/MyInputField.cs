using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System.Text.RegularExpressions;
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
public class MyInputField : MonoBehaviour, IVKeyInput
{
    private TextMeshPro textMesh;
    private int posX, posY;
    private bool codeUpdate;
    private List<string> codeLine;

    private float lineHeight = 4.22f;
    private int spaceCount = 0;

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
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    posX++;
                    posY = 0;
                    spaceCount = 0;
                    foreach (var item in codeLine[posX - 1])
                    {
                        if (item == '\t') spaceCount += 4;
                        else if (item == ' ') spaceCount += 1;
                        else break;
                    }
                    Debug.Log(spaceCount);
                    string l = string.Empty;
                    for (int i = 0; i < spaceCount; i += 4)
                    {
                        l += '\t';
                    }
                    for (int i = 0; i < spaceCount % 4; i++)
                    {
                        l += ' ';
                    }
                    codeLine.Add(l);
                    posY = l.Length;
                }
                else if (Input.GetKeyDown(KeyCode.Backspace))
                {
                    posY--;
                    if (posY < 0)
                    {
                        posX--;
                        if (posX > 0)
                        {
                            codeLine.RemoveAt(posX + 1);
                            posY = codeLine[posX].Length;
                        }
                        else posX++;
                    }
                    else
                    {
                        codeLine[posX] = codeLine[posX].Remove(posY);
                    }
                }
                else if (Input.inputString.Length > 0)
                {
                    Debug.Log((int)Input.inputString[0]);

                    if (posY == 0)
                    {
                        codeLine[posX] = Input.inputString + codeLine[posX];
                    }
                    else if (posY == codeLine[posX].Length)
                    {
                        codeLine[posX] += Input.inputString;
                    }
                    else
                    {
                        codeLine[posX] = codeLine[posX].Insert(posY, Input.inputString);
                    }
                    posY += Input.inputString.Length;
                }
            }
            yield return null;
            UpdateCode();
        }
    }

    private void UpdateCode()
    {
        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
        for (int i = 1; i < codeLine.Count; i++)
        {
            string dis = codeLine[i].Replace("#include", "<#990f0fff>#include</color>");
            dis = dis.Replace("int", "<#0707a0ff>int</color>");
            dis = dis.Replace("using", "<#0707a0ff>using</color>");
            dis = dis.Replace("namespace", "<#0707a0ff>namespace</color>");
            dis = dis.Replace("float", "<#0707a0ff>float</color>");
            dis = dis.Replace("var", "<#0707a0ff>var</color>");
            dis = dis.Replace("console", "<#07a007ff>console</color>");
            stringBuilder.AppendLine(dis);
        }
        textMesh.text = stringBuilder.ToString();
    }

    public void SetText(string codeText)
    {
        if (textMesh == null)
        {
            textMesh = GetComponent<TextMeshPro>();
        }

        posX = 0;
        posY = 0; // 在第0个字符左面

        foreach (var s in codeText.Split(new string[] { Environment.NewLine, "\n" }, StringSplitOptions.None))
        {
            codeLine.Add(s);
            posX++;
        }
        posY = codeLine.LastOrDefault().Length;

        UpdateCode();
    }

    public void VKeyInput(KeyCode key)
    {
        if (key == KeyCode.Return)
        {
            posX++;
            posY = 0;
            spaceCount = 0;
            foreach (var item in codeLine[posX - 1])
            {
                if (item == '\t') spaceCount += 4;
                else if (item == ' ') spaceCount += 1;
                else break;
            }
            Debug.Log(spaceCount);
            string l = string.Empty;
            for (int i = 0; i < spaceCount; i += 4)
            {
                l += '\t';
            }
            for (int i = 0; i < spaceCount % 4; i++)
            {
                l += ' ';
            }
            codeLine.Add(l);
            posY = l.Length;
        }
        else if (key == KeyCode.Backspace)
        {
            posY--;
            if (posY < 0)
            {
                posX--;
                if (posX > 0)
                {
                    codeLine.RemoveAt(posX + 1);
                    posY = codeLine[posX].Length;
                }
                else posX++;
            }
            else
            {
                codeLine[posX] = codeLine[posX].Remove(posY);
            }
        }
        else if ((key <= KeyCode.Z && key >= KeyCode.A) || (key <= KeyCode.Alpha9 && key >= KeyCode.Alpha0))
        {
            Debug.Log(key);

            if (posY == 0)
            {
                codeLine[posX] = (char)key + codeLine[posX];
            }
            else if (posY == codeLine[posX].Length)
            {
                codeLine[posX] += (char)key;
            }
            else
            {
                codeLine[posX] = codeLine[posX].Insert(posY, ((char)key).ToString());
            }
            posY++;
        }
    }
}
