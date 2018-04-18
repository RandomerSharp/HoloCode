using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public interface IMyDictationHandler
{
    void OnDictationHypothesis(string text);
    void OnDictationResult(string text, ConfidenceLevel confidence);
    void OnDictationError(string error, int hresult);
    void OnDictationComplete(DictationCompletionCause cause);
}
