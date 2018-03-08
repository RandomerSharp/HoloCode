using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodeText : MonoBehaviour
{
    [SerializeField]
    private InputField inputField;
    [SerializeField]
    private Text textOut;

    public void CodeSync()
    {
        textOut.text = inputField.text;
    }
}
