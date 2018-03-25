using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DenseLayer : BaseNode
{
    public enum Activation
    {
        ReLU,
        None
    }
    public enum Init
    {
        gaussian,
        None
    }
    #region Param
    public int inSize = 64;
    public Activation activation = Activation.ReLU;
    public Init init = Init.gaussian;
    public float initValueScale = 12;
    #endregion
}
