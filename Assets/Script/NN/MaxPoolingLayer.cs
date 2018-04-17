using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxPoolingLayer : BaseNode
{
    #region Parameters
    public uint poolShape = 32;
    public uint stride = 1;
    public bool pad = false;
    public uint lowerPad = 0;
    public uint upperPad = 0;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        shortName = string.Format("maxp{0}", Mathf.Abs(GetHashCode()));
    }

    public override string GetParameters()
    {
        return string.Format("{0}, stride = {1}, pad = {2}, lowerPad = {3}, upperPad = {4}",
            string.Format("({0}, {0})", poolShape), stride, pad.ToString().ToLower(), lowerPad, upperPad);
    }
}
