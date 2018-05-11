using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationNode : BaseNode
{
    public enum Algorithm
    {
        ErrorPrediction
    }
    #region Param
    public Algorithm err = Algorithm.ErrorPrediction;
    #endregion

    public override string GetParameters()
    {
        return err.ToString();
    }

    public override void SetInspector(GameObject inspector, GameObject signleLineInput, GameObject signleLineSelect)
    {
        Inspector inspector1 = inspector.GetComponent<Inspector>();
        inspector.transform.Find("Quad/NodeName").GetComponent<TextMesh>().text = "Evaluation Node";

        var errObj = Instantiate(signleLineSelect);
        inspector1.Add(errObj.transform);
        errObj.GetComponent<ParamSelect>().SetType(typeof(Algorithm), "err", (int)err);

        inspector1.OnSave = () =>
        {
            Debug.Log(GetHashCode());
            err = (Algorithm)errObj.GetComponent<ParamSelect>().GetValue();
        };
    }
}
