using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriterionNode : BaseNode
{
    public enum Algorithm
    {
        CrossEntropyWithSoftmax,
        CrossEntropy,
        None
    }
    #region Param
    public Algorithm readerType = Algorithm.CrossEntropyWithSoftmax;
    #endregion

    public override string GetParameters()
    {
        return string.Empty;
    }
}
