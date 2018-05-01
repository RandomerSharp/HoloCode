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
    public MyBoolean bias = MyBoolean.True;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        shortName = string.Format("den{0}", Mathf.Abs(GetHashCode()));
    }

    public override string GetParameters()
    {
        return string.Format("{0}, activation = {1}, init = \"{2}\", initValueScale = {3}, bias = {4}",
            outDim, activation.ToString(), init.ToString(), initValueScale, bias.ToString());
    }

    public override void SetInspector(GameObject inspector, GameObject singleInput, GameObject signleLineSelect)
    {
        Inspector inspector1 = inspector.GetComponent<Inspector>();
        inspector.transform.Find("Quad/NodeName").GetComponent<TextMesh>().text = "Dense Layer";

        var outDimObj = Instantiate(singleInput);
        inspector1.Add(outDimObj.transform);
        outDimObj.GetComponent<ParamTypein>().SetValue("outDim", outDim.ToString());

        var activationObj = Instantiate(signleLineSelect);
        inspector1.Add(activationObj.transform);
        activationObj.GetComponent<ParamSelect>().SetType(typeof(ActivationFunction), "Activation Function", (int)activation);

        var initObj = Instantiate(signleLineSelect);
        inspector1.Add(initObj.transform);
        initObj.GetComponent<ParamSelect>().SetType(typeof(RandomInitialization), "Init", (int)init);

        var initValueScaleObj = Instantiate(singleInput);
        inspector1.Add(initValueScaleObj.transform);
        initValueScaleObj.GetComponent<ParamTypein>().SetValue("initValueScale", initValueScale.ToString());

        var biasObj = Instantiate(signleLineSelect);
        inspector1.Add(biasObj.transform);
        biasObj.GetComponent<ParamSelect>().SetType(typeof(MyBoolean), "Bias", (int)bias);

        inspector1.OnSave = () =>
        {
            outDim = System.Convert.ToInt32(outDimObj.GetComponent<ParamTypein>().GetValue());
            activation = (ActivationFunction)activationObj.GetComponent<ParamSelect>().GetValue();
            init = (RandomInitialization)initObj.GetComponent<ParamSelect>().GetValue();
            initValueScale = System.Convert.ToSingle(initValueScaleObj.GetComponent<ParamTypein>().GetValue());
            bias = (MyBoolean)biasObj.GetComponent<ParamSelect>().GetValue();
        };
    }
}
