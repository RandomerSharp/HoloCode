using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxPoolingLayer : BaseNode
{
    #region Parameters
    public uint poolShape = 32;
    public uint stride = 1;
    public MyBoolean pad = MyBoolean.Flase;
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

    public override void SetInspector(GameObject inspector, GameObject singleInput, GameObject singleSelect)
    {
        Inspector inspector1 = inspector.GetComponent<Inspector>();
        inspector.transform.Find("Quad/NodeName").GetComponent<TextMesh>().text = "Max Pooling";

        var poolShapeObj = Instantiate(singleInput);
        inspector1.Add(poolShapeObj.transform);
        poolShapeObj.GetComponent<ParamTypein>().SetValue("Pool Shape", poolShape.ToString());

        var strideObj = Instantiate(singleInput);
        inspector1.Add(strideObj.transform);
        strideObj.GetComponent<ParamTypein>().SetValue("Stride", stride.ToString());

        var padObj = Instantiate(singleSelect);
        inspector1.Add(padObj.transform);
        padObj.GetComponent<ParamSelect>().SetType(typeof(MyBoolean), "Pad", (int)pad);

        var lowerPadObj = Instantiate(singleInput);
        inspector1.Add(lowerPadObj.transform);
        lowerPadObj.GetComponent<ParamTypein>().SetValue("Lower Pad", lowerPad.ToString());

        var upperPadObj = Instantiate(singleInput);
        inspector1.Add(upperPadObj.transform);
        upperPadObj.GetComponent<ParamTypein>().SetValue("Upper Pad", upperPad.ToString());

        inspector1.OnSave = () =>
        {
            poolShape = Convert.ToUInt32(poolShapeObj.GetComponent<ParamTypein>().GetValue());
            stride = Convert.ToUInt32(strideObj.GetComponent<ParamTypein>().GetValue());
            pad = (MyBoolean)padObj.GetComponent<ParamSelect>().GetValue();
            lowerPad = Convert.ToUInt32(lowerPadObj.GetComponent<ParamTypein>().GetValue());
            upperPad = Convert.ToUInt32(upperPadObj.GetComponent<ParamTypein>().GetValue());
        };
    }
}
