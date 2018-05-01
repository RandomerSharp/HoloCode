using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ConvolutionalLayer : BaseNode
{
    #region Parameters
    public uint numOutputChannels = 32;
    public uint filterShape = 5;
    public ActivationFunction activation = ActivationFunction.ReLU;
    public RandomInitialization init = RandomInitialization.gaussian;
    public float initValueScale = 1;
    public uint stride = 1;
    public MyBoolean pad = MyBoolean.Flase;
    public uint lowerPad = 0;
    public uint upperPad = 0;
    public MyBoolean bias = MyBoolean.True;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        shortName = string.Format("conv{0}", Mathf.Abs(GetHashCode()));
    }

    public override string GetParameters()
    {
        return string.Format("{0}, {1}, activation = {2}, init = \"{3}\", initValueScale = {4}, stride = {5}, pad = {6}, lowerPad = {7}, upperPad = {8}, bias = {9}",
            numOutputChannels, string.Format("({0}, {0})", filterShape), activation.ToString(), init.ToString(), initValueScale,
            stride, pad.ToString().ToLower(), lowerPad, upperPad, bias.ToString().ToLower());
    }

    public override void SetInspector(GameObject inspector, GameObject signleLineInput, GameObject signleLineSelect)
    {
        Inspector inspector1 = inspector.GetComponent<Inspector>();
        inspector.transform.Find("Quad/NodeName").GetComponent<TextMesh>().text = "Convolutional Node";

        var activationObj = Instantiate(signleLineSelect);
        inspector1.Add(activationObj.transform);
        activationObj.GetComponent<ParamSelect>().SetType(typeof(ActivationFunction), "Activation Function", (int)activation);

        var initObj = Instantiate(signleLineSelect);
        inspector1.Add(initObj.transform);
        initObj.GetComponent<ParamSelect>().SetType(typeof(RandomInitialization), "Init", (int)init);

        inspector1.OnSave = () =>
        {
            activation = (ActivationFunction)activationObj.GetComponent<ParamSelect>().GetValue();
            init = (RandomInitialization)initObj.GetComponent<ParamSelect>().GetValue();
        };
    }
}
