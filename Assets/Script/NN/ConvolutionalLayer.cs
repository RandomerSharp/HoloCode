using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ConvolutionalLayer : BaseNode
{
    #region Parameters
    public uint numOutputChannels = 32;
    public string filterShape = "5:5";
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
        return string.Format("{0}, ({1}), activation = {2}, init = \"{3}\", initValueScale = {4}, stride = {5}, pad = {6}, lowerPad = {7}, upperPad = {8}, bias = {9}",
            numOutputChannels, filterShape, activation.ToString(), init.ToString(), initValueScale,
            stride, pad.ToString().ToLower(), lowerPad, upperPad, bias.ToString().ToLower());
    }

    public override void SetInspector(GameObject inspector, GameObject singleLineInput, GameObject singleLineSelect)
    {
        Inspector inspector1 = inspector.GetComponent<Inspector>();
        inspector.transform.Find("Quad/NodeName").GetComponent<TextMesh>().text = "Convolutional Node";

        var numOutputChannelsObj = Instantiate(singleLineInput);
        inspector1.Add(numOutputChannelsObj.transform);
        numOutputChannelsObj.GetComponent<ParamTypein>().SetValue("Num Output Channels", numOutputChannels.ToString());

        var filterShapeObj = Instantiate(singleLineInput);
        inspector1.Add(filterShapeObj.transform);
        filterShapeObj.GetComponent<ParamTypein>().SetValue("Filter Shape", filterShape);

        var activationObj = Instantiate(singleLineSelect);
        inspector1.Add(activationObj.transform);
        activationObj.GetComponent<ParamSelect>().SetType(typeof(ActivationFunction), "Activation Function", (int)activation);

        var initObj = Instantiate(singleLineSelect);
        inspector1.Add(initObj.transform);
        initObj.GetComponent<ParamSelect>().SetType(typeof(RandomInitialization), "Init", (int)init);

        var padObj = Instantiate(singleLineSelect);
        inspector1.Add(padObj.transform);
        padObj.GetComponent<ParamSelect>().SetType(typeof(MyBoolean), "Pad", (int)pad);

        var biasObj = Instantiate(singleLineSelect);
        inspector1.Add(biasObj.transform);
        biasObj.GetComponent<ParamSelect>().SetType(typeof(MyBoolean), "Bias", (int)bias);

        inspector1.OnSave = () =>
        {
            Debug.Log(GetHashCode());
            numOutputChannels = System.Convert.ToUInt32(numOutputChannelsObj.GetComponent<ParamTypein>().GetValue());
            filterShape = filterShapeObj.GetComponent<ParamTypein>().GetValue();
            activation = (ActivationFunction)activationObj.GetComponent<ParamSelect>().GetValue();
            init = (RandomInitialization)initObj.GetComponent<ParamSelect>().GetValue();
            pad = (MyBoolean)padObj.GetComponent<ParamSelect>().GetValue();
            bias = (MyBoolean)biasObj.GetComponent<ParamSelect>().GetValue();
        };
    }
}
