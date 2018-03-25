using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriterionNode : BaseNode
{
    public enum Algorithm
    {
        CrossEntropyWithSoftmax,
        None
    }
    #region Param
    public Algorithm readerType = Algorithm.CrossEntropyWithSoftmax;
    #endregion
}
