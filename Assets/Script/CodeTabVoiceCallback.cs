using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class CodeTabVoiceCallback : MonoBehaviour, IMyDictationHandler
{
    public void OnDictationComplete(DictationCompletionCause cause)
    {

    }

    public void OnDictationError(string error, int hresult)
    {

    }

    public void OnDictationHypothesis(string text)
    {

    }

    public void OnDictationResult(string text, ConfidenceLevel confidence)
    {
        text = text.ToLower();
        switch (text)
        {
        case "close":
        {
            transform.Find("Close").GetComponent<ItemSelect>().Close();
            break;
        }
        case "save":
        {
            transform.Find("Save").GetComponent<ItemSelect>().SaveFile();
            break;
        }
        case "page up":
        {
            transform.Find("PageUp").GetComponent<ItemSelect>().PageUpClick();
            break;
        }
        case "page down":
        {
            transform.Find("PageDown").GetComponent<ItemSelect>().PageDownClick();
            break;
        }
        default:
            break;
        }
    }
}
