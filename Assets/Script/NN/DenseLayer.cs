using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DenseLayer : BaseNode
{
    #region Param
    public int outDim = 64;
    public ActivationFunction activation = ActivationFunction.ReLU;
    public RandomInitialization init = RandomInitialization.gaussian;
    public float initValueScale = 1;
    public bool bias = true;
    #endregion

    public override string GetParameters()
    {
        return string.Format("{0}, activation = {1}, init = \"{2}\", initValueScale = {3}, bias = {4}",
            outDim, activation.ToString(), init.ToString(), initValueScale, bias.ToString());
    }
}
