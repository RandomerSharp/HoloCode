using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatureNode : BaseNode
{
    public enum ReaderType
    {
        ImageShap,
        Text
    }
    #region Param
    public ReaderType readerType;
    #endregion

    public override string GetParameters()
    {
        return string.Empty;
    }
}
