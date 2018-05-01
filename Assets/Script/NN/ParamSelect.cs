using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamSelect : MonoBehaviour
{
    private List<string> allSelection;
    private string paramName;
    private int currentSelect;

    private void Awake()
    {
        allSelection = new List<string>();
    }

    public void SetType(Type type, string variable, int cur)
    {
        paramName = variable;
        transform.Find("Param").GetComponent<TextMesh>().text = paramName;
        currentSelect = cur;

        if (type.IsEnum)
        {
            allSelection.AddRange(type.GetEnumNames());
        }

        transform.Find("InputField/Text").GetComponent<TextMesh>().text = allSelection[cur];
    }

    public void Next()
    {
        currentSelect++;
        currentSelect %= allSelection.Count;
        transform.Find("InputField/Text").GetComponent<TextMesh>().text = allSelection[currentSelect];
    }

    public void Last()
    {
        currentSelect += allSelection.Count - 1;
        currentSelect %= allSelection.Count;
        transform.Find("InputField/Text").GetComponent<TextMesh>().text = allSelection[currentSelect];
    }

    public int GetValue()
    {
        return currentSelect;
    }
}
