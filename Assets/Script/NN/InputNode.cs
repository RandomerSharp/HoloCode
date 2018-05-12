using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputNode : BaseNode
{
    public enum ReaderType
    {
        CNTKTextFormatReader,
        HTKMLFReader,
        ImageReader,
        CNTKBinaryReader,
        LMSequenceReader,
        UCIFastReader
    }

    public abstract string GetFeatures();
    public abstract string GetFeatureName();
    public abstract string GetLabels();
    public abstract string GetLabelsName();
}
