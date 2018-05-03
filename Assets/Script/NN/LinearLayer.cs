using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearLayer : BaseNode
{
    #region Param
    public uint outDim = 64;
    public RandomInitialization init = RandomInitialization.gaussian;
    public float initValueScale = 1;
    public MyBoolean bias = MyBoolean.True;
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

    public override void SetInspector(GameObject inspector, GameObject singleInput, GameObject singleSelect)
    {
        Inspector inspector1 = inspector.GetComponent<Inspector>();
        inspector.transform.Find("Quad/NodeName").GetComponent<TextMesh>().text = "Linear Layer";

        var initObj = Instantiate(singleSelect);
        inspector1.Add(initObj.transform);
        initObj.GetComponent<ParamSelect>().SetType(typeof(RandomInitialization), "init", (int)init);

        var outDimObj = Instantiate(singleInput);
        inspector1.Add(outDimObj.transform);
        outDimObj.GetComponent<ParamTypein>().SetValue("outDim", outDim.ToString());

        var initValueScaleObj = Instantiate(singleInput);
        inspector1.Add(initValueScaleObj.transform);
        initValueScaleObj.GetComponent<ParamTypein>().SetValue("initValueScale", initValueScale.ToString());

        var biasObj = Instantiate(singleSelect);
        inspector1.Add(biasObj.transform);
        biasObj.GetComponent<ParamSelect>().SetType(typeof(MyBoolean), "Bias", (int)bias);

        inspector1.OnSave = () =>
        {
            Debug.Log(GetHashCode());
            init = (RandomInitialization)initObj.GetComponent<ParamSelect>().GetValue();
            outDim = System.Convert.ToUInt32(outDimObj.GetComponent<ParamTypein>().GetValue());
            initValueScale = System.Convert.ToSingle(initValueScaleObj.GetComponent<ParamTypein>().GetValue());
            bias = (MyBoolean)biasObj.GetComponent<ParamSelect>().GetValue();
        };
    }
}
