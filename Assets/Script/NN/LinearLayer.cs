using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearLayer : BaseNode
{
    #region Param
    public int outDim = 64;
    public RandomInitialization init = RandomInitialization.gaussian;
    public float initValueScale = 1;
    public bool bias = true;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        shortName = string.Format("lin{0}", Mathf.Abs(GetHashCode()));
    }

    public override string GetParameters()
    {
        return string.Format("{0}, init = \"{1}\", initValueScale = {2}, bias = {3}",
            outDim, init.ToString(), initValueScale, bias.ToString());
    }
}
