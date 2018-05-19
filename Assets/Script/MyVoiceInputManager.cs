using MixedRealityToolkit.Common;
using MixedRealityToolkit.InputModule.InputSources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Windows.Speech;

public class MyVoiceInputManager : Singleton<MyVoiceInputManager>
{
    [System.Serializable]
    private struct StringAndUnityEvent
    {
        public string keyword;
        public UnityEvent action;
    }

    public SpeechInputSource.RecognizerStartBehavior recognizerStart;
    //public UnityEventString onDictationResult;

    private DictationRecognizer dictationRecognizer;
    [SerializeField]
    private StringAndUnityEvent[] keywords;

    private void Start()
    {
        dictationRecognizer = new DictationRecognizer();
        dictationRecognizer.DictationComplete += DictationRecognizer_DictationComplete;
        dictationRecognizer.DictationError += DictationRecognizer_DictationError;
        dictationRecognizer.DictationHypothesis += DictationRecognizer_DictationHypothesis;
        dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;

        if (recognizerStart == SpeechInputSource.RecognizerStartBehavior.AutoStart)
        {
            dictationRecognizer.Start();
        }
    }

    private void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)
    {
        Debug.Log("Result: " + text);
        foreach (var item in keywords)
        {
            if (text == item.keyword)
            {
                item.action.Invoke();
                return;
            }
        }

        var dictationHandlers = (from i in FindObjectsOfType(typeof(MonoBehaviour))
                                 where i is IMyDictationHandler
                                 select ((IMyDictationHandler)i)).ToArray();
        foreach (var i in dictationHandlers)
        {
            i.OnDictationResult(text, confidence);
        }
        //dictationRecognizer.Start();
    }

    private void DictationRecognizer_DictationHypothesis(string text)
    {
        Debug.Log("Hypothesis: " + text);
        var dictationHandlers = (from i in FindObjectsOfType(typeof(MonoBehaviour))
                                 where i is IMyDictationHandler
                                 select ((IMyDictationHandler)i)).ToArray();
        foreach (var i in dictationHandlers)
        {
            i.OnDictationHypothesis(text);
        }
    }

    private void DictationRecognizer_DictationError(string error, int hresult)
    {
        Debug.Log("Error: " + error);
        var dictationHandlers = (from i in FindObjectsOfType(typeof(MonoBehaviour))
                                 where i is IMyDictationHandler
                                 select ((IMyDictationHandler)i)).ToArray();
        foreach (var i in dictationHandlers)
        {
            i.OnDictationError(error, hresult);
        }
        dictationRecognizer.Start();
    }

    private void DictationRecognizer_DictationComplete(DictationCompletionCause cause)
    {
        Debug.Log("Complete: " + cause.ToString());
        var dictationHandlers = (from i in FindObjectsOfType(typeof(MonoBehaviour))
                                 where i is IMyDictationHandler
                                 select ((IMyDictationHandler)i)).ToArray();
        foreach (var i in dictationHandlers)
        {
            i.OnDictationComplete(cause);
        }
        dictationRecognizer.Start();
    }

    public void StartDictation()
    {
        if (dictationRecognizer.Status == SpeechSystemStatus.Stopped)
        {
            dictationRecognizer.Start();
        }
    }

    public void StopDictation()
    {
        dictationRecognizer.Stop();
        Debug.Log(dictationRecognizer.Status);
    }

    protected override void OnDestroy()
    {
        dictationRecognizer.DictationComplete -= DictationRecognizer_DictationComplete;
        dictationRecognizer.DictationError -= DictationRecognizer_DictationError;
        dictationRecognizer.DictationHypothesis -= DictationRecognizer_DictationHypothesis;
        dictationRecognizer.DictationResult -= DictationRecognizer_DictationResult;

        dictationRecognizer.Dispose();

        base.OnDestroy();
    }
}
