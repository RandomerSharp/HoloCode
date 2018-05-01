using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriterionNode : BaseNode
{
    #region Param
    public LossFunction lossFunction = LossFunction.CrossEntropyWithSoftmax;
    #endregion

    public override string GetParameters()
    {
        return lossFunction.ToString();
    }

    public override void SetInspector(GameObject inspector, GameObject signleLineInput, GameObject signleLineSelect)
    {
        Inspector inspector1 = inspector.GetComponent<Inspector>();
        inspector.transform.Find("Quad/NodeName").GetComponent<TextMesh>().text = "Criterion Node";

        var lossFunctionObj = Instantiate(signleLineSelect);
        inspector1.Add(lossFunctionObj.transform);
        lossFunctionObj.GetComponent<ParamSelect>().SetType(typeof(LossFunction), "Loss Function", (int)lossFunction);

        inspector1.OnSave = () =>
        {
            lossFunction = (LossFunction)lossFunctionObj.GetComponent<ParamSelect>().GetValue();
        };
    }
}
