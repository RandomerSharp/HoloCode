using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Windows.Speech;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.CSharp;
using UnityEngine.SceneManagement;

public class VoiceCallback : MonoBehaviour, IMyDictationHandler
{
    [SerializeField]
    private GameObject speakor;

    //private bool isResording;

    public void StartRecord()
    {
        Debug.Log("Recording");
        //isResording = true;
    }

    public void Test(string r = "")
    {
        Debug.Log("测试: " + r);
    }

    public void OnDictationHypothesis(string text)
    {
        Debug.Log("Hypothesis");
        speakor.GetComponent<BotSpeak>().Show(text + "...");
    }

    public void OnDictationResult(string text, ConfidenceLevel confidence)
    {
        Debug.Log(text);
        if (SceneManager.GetActiveScene().name == "NewStart")
        {
            speakor.GetComponent<BotSpeak>().Say(text);
            if (text.ToLower() == "select workspace")
            {
                SceneManager.LoadScene("SelectWorkspace", LoadSceneMode.Single);
            }
            else if (text.ToLower() == "quit")
            {
                Application.Quit();
            }
        }
    }

    public void OnDictationComplete(DictationCompletionCause cause)
    {
        Debug.Log("Complete" + cause.ToString());
    }

    public void OnDictationError(string error, int hresult)
    {
        Debug.Log("Dictation error!");
    }
}
