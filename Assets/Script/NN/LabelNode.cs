using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelNode : InputNode
{
    #region Param
    public int labelDim = 10;
    #endregion

    public override string GetFeatures()
    {
        return string.Empty;
    }

    public override string GetLabels()
    {
        return string.Empty;
    }

    public override string GetParameters()
    {
        return string.Empty;
    }

    public override void SetInspector(GameObject inspector, GameObject signleLineInput, GameObject signleLineSelect)
    {

    }
}
