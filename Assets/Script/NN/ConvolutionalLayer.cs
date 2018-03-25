using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvolutionalLayer : BaseNode
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
    public Activation activation = Activation.ReLU;
    public int inSize = 32;
    public bool pad = true;
    public Init init = Init.gaussian;
    public float initValueScale = 0.0043f;
    public int kernelSize = 5;
    public int poolingSize = 3;
    public int stride = 2;
    #endregion
}
