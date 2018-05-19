using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System.Text.RegularExpressions;

public class MyInputField : MonoBehaviour, IVKeyInput
{
    private TextMeshPro textMesh;
    private int posX, posY;
    private bool codeUpdate;
    private List<string> codeLine;
    private List<bool> revised;
    private int page;
    private int maxLine;

    private float lineHeight = 4.22f;
    private int spaceCount = 0;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        posX = 0;
        posY = 0;
        if (codeLine != null) codeLine.Clear();
        else codeLine = new List<string>();
        if (revised != null) revised.Clear();
        else revised = new List<bool>();
        //codeLine.Add(string.Empty);
        //revised.Add(false);
        page = 0;
        maxLine = 47;
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
                    if (posX >= maxLine) page++;
                    else posX++;
                    posY = 0;
                    spaceCount = 0;
                    foreach (var item in codeLine[posX + page - 1])
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
                    revised.Add(true);
                    posY = l.Length;
                }
                else if (Input.GetKeyDown(KeyCode.Backspace))
                {
                    posY--;
                    if (posY < 0)
                    {
                        posX--;
                        if (posX >= 0)
                        {
                            codeLine.RemoveAt(posX + page + 1);
                            revised.RemoveAt(posX + page + 1);
                            posY = codeLine[posX + page].Length;
                        }
                        else
                        {
                            posX++;
                            if (page > 0)
                            {
                                page--;
                            }
                            posY = 0;
                        }
                    }
                    else
                    {
                        codeLine[posX + page] = codeLine[posX].Remove(posY);
                        revised[posX + page] = true;
                    }
                }
                else if (Input.inputString.Length > 0)
                {
                    Debug.Log((int)Input.inputString[0]);

                    if (posY == 0)
                    {
                        codeLine[posX + page] = Input.inputString + codeLine[posX + page];
                        revised[posX + page] = true;
                    }
                    else if (posY == codeLine[posX + page].Length)
                    {
                        codeLine[posX + page] += Input.inputString;
                        revised[posX + page] = true;
                    }
                    else
                    {
                        codeLine[posX + page] = codeLine[posX + page].Insert(posY, Input.inputString);
                        revised[posX + page] = true;
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
        for (int i = 0; i < maxLine; i++)
        {
            if (i + page >= codeLine.Count) break;
            string dis = codeLine[i + page];//.Replace("#include", "<#990f0fff>#include</color>");
            /*dis = dis.Replace("int", "<#0707a0ff>int</color>");
            dis = dis.Replace("using", "<#0707a0ff>using</color>");
            dis = dis.Replace("namespace", "<#0707a0ff>namespace</color>");
            dis = dis.Replace("float", "<#0707a0ff>float</color>");
            dis = dis.Replace("var", "<#0707a0ff>var</color>");
            dis = dis.Replace("console", "<#07a007ff>console</color>");*/
            stringBuilder.AppendLine(dis);
        }
        textMesh.text = CodeColor.JavaScript(stringBuilder.ToString());
    }

    public void SetText(string codeText)
    {
        if (textMesh == null)
        {
            textMesh = GetComponent<TextMeshPro>();
        }

        if (codeLine == null) codeLine = new List<string>();
        if (revised == null) revised = new List<bool>();
        codeLine.Clear();
        revised.Clear();

        posX = 0;
        posY = 0; // 在第0个字符左面

        foreach (var s in codeText.Split(new string[] { Environment.NewLine, "\n" }, StringSplitOptions.None))
        {
            codeLine.Add(s);
            revised.Add(true);
            posX++;
        }
        if (posX > 0) posX--;
        posY = codeLine.LastOrDefault().Length;

        UpdateCode();
    }

    public string GetText()
    {
        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
        foreach (var l in codeLine)
        {
            stringBuilder.AppendLine(l);
        }
        return stringBuilder.ToString();
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
            //Debug.Log(spaceCount);
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
            //Debug.Log(key);

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

    public void PageUp()
    {
        page--;
        if (page <= 0) page = 0;
    }

    public void PageDown()
    {
        page++;
    }
}
