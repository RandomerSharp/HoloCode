using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ImageReader : InputNode
{
    public enum CropType
    {
        Center, RandomSide, RandomArea, MultiView10
    }
    public enum JitterType
    {
        None, UniRatio
    }
    public enum Interpolations
    {
        nearest, linear, cubic, lanczos
    }

    public ReaderType readerType = ReaderType.ImageReader;

    #region Param
    public float verbosity = 0f;
    public bool? randomize = true;
    public string file = "$rootDir$/train_map.txt";

    public uint width = 32;
    public uint height = 32;
    public uint channel = 3;
    public CropType cropType = CropType.Center;
    public float hflip = 0;
    public float sideRatio = 0.875f;
    public JitterType jitterType = JitterType.UniRatio;
    public Interpolations interpolations = Interpolations.linear;
    public float aspectRatio = 0.75f;
    public float brightnessRadius = 0.0f;
    public float contrastRadius = 0.0f;
    public float saturationRadius = 0.0f;
    public string intensityFile = "$rootDir$/ImageNet1K_intensity.xml";
    public float intensityStdDev = 0.1f;
    public string meanFile = "$rootDir$/ImageNet1K_mean.xml";

    public uint labelDim = 10;
    #endregion

    public override string GetParameters()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(string.Format("readerType = {0}", readerType.ToString()));
        sb.AppendLine(string.Format("file = {0}", file));
        sb.AppendLine(string.Format("randomize = {0}", randomize?.ToString() ?? "Auto"));
        sb.AppendLine("features=[");
        sb.AppendLine(string.Format("\twidth = {0}", width));
        sb.AppendLine(string.Format("\theight = {0}", height));
        sb.AppendLine(string.Format("\tchannel = {0}", channel));
        sb.AppendLine(string.Format("\tcropType = {0}", cropType.ToString()));
        sb.AppendLine(string.Format("\thflip = {0}", hflip));
        sb.AppendLine(string.Format("\tsideRatio = {0}", sideRatio));
        sb.AppendLine(string.Format("\tjitterType = {0}", jitterType.ToString()));
        sb.AppendLine(string.Format("\tinterpolations = {0}", interpolations.ToString()));
        sb.AppendLine(string.Format("\taspectRatio = {0}", aspectRatio));
        sb.AppendLine(string.Format("\tbrightnessRadius = {0}", brightnessRadius));
        sb.AppendLine(string.Format("\tcontrastRadius = {0}", contrastRadius));
        sb.AppendLine(string.Format("\tsaturationRadius = {0}", saturationRadius));
        sb.AppendLine(string.Format("\tintensityFile = {0}", intensityFile));
        sb.AppendLine(string.Format("\tintensityStdDev = {0}", intensityStdDev));
        sb.AppendLine(string.Format("\tmeanFile = {0}", meanFile));
        sb.AppendLine("]");
        sb.AppendLine("labels = [");
        sb.AppendLine(string.Format("labelDim = {0}", labelDim));
        return sb.ToString();
    }

    public override string GetFeatures()
    {
        return string.Format("features = Input({0}, {1}, {2})", width, height, channel);
    }

    public override string GetLabels()
    {
        return string.Format("labels = [labelDim = {0}]", labelDim);
    }

    public override void SetInspector(GameObject inspector, GameObject signleLineInput, GameObject signleLineSelect)
    {

        Inspector inspector1 = inspector.GetComponent<Inspector>();
        inspector.transform.Find("Quad/NodeName").GetComponent<TextMesh>().text = "Imagine Reader";
    }
}
