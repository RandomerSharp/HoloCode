using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationNode : BaseNode
{
    public enum Algorithm
    {
        ErrorPrediction,
        ClassificationError,
        None
    }
    #region Param
    public Algorithm readerType = Algorithm.ErrorPrediction;
    #endregion

    public override string GetParameters()
    {
        return string.Empty;
    }
}
