using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearLayer : BaseNode
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
    public int inSize = 10;
    public Init init = Init.gaussian;
    public float initValueScale = 12;
    #endregion
}
