using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamSelect : MonoBehaviour
{
    [SerializeField]
    private List<string> allSelection;
    private string paramName;
    private int currentSelect;

    private void Awake()
    {
        if (allSelection != null && allSelection.Count > 0)
        {
            currentSelect = 0;
            transform.Find("InputField/Text").GetComponent<TextMesh>().text = allSelection[currentSelect];
        }
    }

    public void SetType(Type type, string variable, int cur)
    {
        if (allSelection == null) allSelection = new List<string>();
        else allSelection.Clear();

        paramName = variable;
        transform.Find("Param").GetComponent<TextMesh>().text = paramName;
        currentSelect = cur;
#if UNITY_EDITOR
        if (type.IsEnum)
        {
            allSelection.AddRange(type.GetEnumNames());
        }
#else
        allSelection.AddRange(Enum.GetNames(type));
#endif
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

    public string GetValueName()
    {
        return allSelection[currentSelect];
    }
}
