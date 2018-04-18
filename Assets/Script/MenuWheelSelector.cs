using HoloToolkit.Unity.Controllers;
using HoloToolkit.Unity.InputModule;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class MenuWheelSelector : AttachToController
{
    private Vector2 lastSelectorPosition;
    private Vector2 selectorPosition;

    private List<BaseNode> nodes;
    private int currectSelect;

    private bool isPressed;
    private bool isLongPressed;
    private float pressDelta;

    [SerializeField]
    private GameObject shellDialog;
    [SerializeField]
    private GameObject line;

    private BaseNode node1;
    private BaseNode node2;

    [SerializeField]
    private GameObject codeScreen;

    private void Awake()
    {
        nodes = new List<BaseNode>();
        nodes.AddRange(GetComponentsInChildren<BaseNode>());
        Debug.Log(nodes.Count);
        currectSelect = 0;
        lastSelectorPosition = new Vector2();
        selectorPosition = new Vector2();
        isPressed = false;
    }

    protected override void OnAttachToController()
    {
        InteractionManager.InteractionSourceUpdated += InteractionSourceUpdated;
    }

    protected override void OnDetachFromController()
    {
        InteractionManager.InteractionSourceUpdated -= InteractionSourceUpdated;
    }

    private void InteractionSourceUpdated(InteractionSourceUpdatedEventArgs obj)
    {
        if (obj.state.source.handedness == handedness && obj.state.touchpadTouched)
        {
            selectorPosition = obj.state.touchpadPosition;
            float angle = Vector2.SignedAngle(selectorPosition, lastSelectorPosition);
            if (angle > 30f && nodes.Count > 0)
            {
                currectSelect = (currectSelect + 1) % nodes.Count;
                lastSelectorPosition = selectorPosition;
                transform.Rotate(transform.up, -360f / nodes.Count, Space.World);
            }
            else if (angle < -30f && nodes.Count > 0)
            {
                currectSelect = (currectSelect - 1 + nodes.Count) % nodes.Count;
                lastSelectorPosition = selectorPosition;
                transform.Rotate(transform.up, 360f / nodes.Count, Space.World);
            }
            shellDialog.GetComponentInChildren<TextMesh>().text = nodes[currectSelect].name;
        }
        IPointingSource ips = null;
        FocusManager.Instance.TryGetSinglePointer(out ips);
        Debug.Log(obj.state.selectPressedAmount);
        if (obj.state.source.handedness == handedness && obj.state.selectPressed && FocusManager.Instance.GetFocusedObject(ips) == null)
        {
            if (!isPressed)
            {
                nodes[currectSelect].CreateOne();
                isPressed = true;
            }
        }
        else
        {
            isPressed = false;
        }
        lastSelectorPosition = selectorPosition;
    }

    public void NodeSelect(BaseNode node)
    {
        if (node1 == null)
        {
            node1 = node;
            return;
        }
        node2 = node;
        if (node1 == node2)
        {
            node1 = node2 = null;
            return;
        }
        node1.SetNext(node2);
        node2.SetLast(node1);

        if ((from item in FindObjectsOfType<LineUpdate>()
             where item != null && item.TryDestory(node1.transform, node2.transform)
             select item).Count() > 0)
        {
            node1 = null;
            node2 = null;
            return;
        }

        var obj = Instantiate(line);
        obj.GetComponent<LineUpdate>().Start = node1.transform;
        obj.GetComponent<LineUpdate>().End = node2.transform;

        node1 = null;
        node2 = null;
    }

    private bool generating = false;

    public void Generate()
    {
        if (generating) return;
        generating = true;
        StartCoroutine(Generate2());
    }

    private IEnumerator Generate1()
    {
        Debug.Log("Generate1");
        var nodes = (from node in FindObjectsOfType<BaseNode>()
                     where node.gameObject.layer == 0
                     select node).ToArray();
        yield return null;
        StringBuilder script = new StringBuilder();
        script.AppendLine("command = TrainConvNet");
        script.AppendLine("makeMode = false ; traceLevel = 0; deviceId = \"auto\"");
        script.AppendLine("rootDir = \".\"; dataDir = \"$rootDir$\"; modelDir = \"$rootDir$/Models\"");
        script.AppendLine("modelPath = \"$modelDir$/test.cmf\"");
        script.AppendLine("TrainConvNet = {");
        script.AppendLine("\taction = \"train\"");
        script.AppendLine("\tBrainScriptNetworkBuilder = {");


        string inputFeature = string.Empty;
        string inputLabel = string.Empty;
        StringBuilder model = new StringBuilder();
        string ce = string.Empty;
        string errs = string.Empty;

        model.AppendLine("model(features) = {");

        /*
         * featureNodes = (features)
         * labelNodes = (labels)
         * criterionNodes = (ce)
         * evaluationNodes = (errs)
         * outputNodes = (z)
         */

        foreach (var node in nodes)
        {
            if (node.Last.Count == 0 && node.Next.Count == 0) continue;
            if (node.Last.Count == 0 && node is FeatureNode)
            {
                switch (((FeatureNode)node).readerType)
                {
                case FeatureNode.ReaderType.ImageShap:
                    inputFeature = string.Format("imageShap{0} = {1}", Mathf.Abs(node.GetHashCode()), " = 32:32:32");
                    break;
                case FeatureNode.ReaderType.Text:
                    inputFeature = string.Format("inputDim{0} = {1}", Mathf.Abs(node.GetHashCode()), " = 943");
                    break;
                default:
                    inputFeature = string.Format("imageShap{0} = {1}", Mathf.Abs(node.GetHashCode()), " = 32:32:32");
                    break;
                }
                script.AppendLine(inputFeature);
            }
            else if (node.Last.Count == 0 && node is LabelNode)
            {
                inputLabel = string.Format("labelDim{0} = {1}", Mathf.Abs(node.GetHashCode()), ((LabelNode)node).labelDim.ToString());
                script.AppendLine(inputLabel);
            }
            else if (node is ConvolutionalLayer)
            {
                //string convParam = string.Format("{0}, ({1}:{1}), pad={2}, activation={3},init = \"{4}\", initValueScale = {5}",
                //    temp.inSize, temp.kernelSize, temp.pad, temp.activation, temp.init, temp.initValueScale);
                string convParam = node.GetParameters();
                string convLast = string.Empty;
                if (node.Last.Count > 0)
                {
                    if (node.Last[0] is FeatureNode)
                    {
                        convLast += "featNorm";
                    }
                    else
                    {
                        convLast += "p" + Mathf.Abs(node.Last[0].GetHashCode()).ToString();
                    }
                    for (int i = 1; i < node.Last.Count; i++)
                    {
                        if (node.Last[i] is FeatureNode)
                        {
                            convLast += ", featNorm";
                            continue;
                        }
                        else if (node.Last[i] is ConvolutionalLayer)
                        {
                            convLast += ", p" + Mathf.Abs(node.Last[i].GetHashCode()).ToString();
                        }
                    }
                }

                //string poolingParam = string.Format("({0}:{0}), stride = ({1}:{1})", temp.poolingSize, temp.stride);
                //string poolingLast = string.Format("l{0}", Mathf.Abs(node.GetHashCode()));

                model.AppendLine(string.Format("l{0} = ConvolutionalLayer {{ {1} }}({2})", Mathf.Abs(node.GetHashCode()), convParam, convLast));
                //model.AppendLine(string.Format("p{0} = MaxPoolingLayer {{ {1} }}({2})", Mathf.Abs(node.GetHashCode()), poolingParam, poolingLast));
            }
            else if (node is DenseLayer)
            {
                string param = node.GetParameters();
                string last = string.Empty;
                if (node.Last.Count > 0)
                {
                    if (node.Last[0] is FeatureNode)
                    {
                        last += "featNorm";
                    }
                    else
                    {
                        last += "p" + Mathf.Abs(node.Last[0].GetHashCode()).ToString();
                    }
                    for (int i = 1; i < node.Last.Count; i++)
                    {
                        if (node.Last[i] is FeatureNode)
                        {
                            last += ", featNorm";
                            continue;
                        }
                        else if (node.Last[i] is ConvolutionalLayer)
                        {
                            last += ", p" + Mathf.Abs(node.Last[i].GetHashCode()).ToString();
                        }
                    }
                }

                model.AppendLine(string.Format("d{0} = DenseLayer {{ {1} }}({2})", Mathf.Abs(node.GetHashCode()), param, last));
            }
            else if (node is LinearLayer)
            {
                //LinearLayer temp = (LinearLayer)node;
                string param = node.GetParameters();
                string last = string.Empty;
                if (node.Last.Count > 0)
                {
                    if (node.Last[0] is DenseLayer)
                    {
                        last += "d" + Mathf.Abs(node.Last[0].GetHashCode()).ToString();
                    }
                    else
                    {
                        last += "p" + Mathf.Abs(node.Last[0].GetHashCode()).ToString();
                    }
                    for (int i = 1; i < node.Last.Count; i++)
                    {
                        if (node.Last[i] is DenseLayer)
                        {
                            last += ", d" + Mathf.Abs(node.Last[i].GetHashCode()).ToString();
                            continue;
                        }
                        else if (node.Last[i] is ConvolutionalLayer)
                        {
                            last += ", p" + Mathf.Abs(node.Last[i].GetHashCode()).ToString();
                        }
                    }
                }
                model.AppendLine(string.Format("z = LinearLayer {{ {1} }}({2})", Mathf.Abs(node.GetHashCode()), param, last));
            }
            else if (node is EvaluationNode)
            {
                errs = "errs = " + Enum.GetName(typeof(EvaluationNode.Algorithm), ((EvaluationNode)node).readerType) + "(labels, z)";
            }
            else if (node is CriterionNode)
            {
                ce = "ce = " + Enum.GetName(typeof(CriterionNode.Algorithm), ((CriterionNode)node).readerType) + "(labels, z)";
            }
            yield return null;
        }
        script.AppendLine(model.ToString() + "}.z");
        script.AppendFormat("\tfeatures = Input {{ {0} }}{1}", inputFeature.Substring(0, inputFeature.IndexOf('=')), Environment.NewLine);
        script.AppendFormat("\tlabels = Input {{ {0} }}{1}", inputLabel.Substring(0, inputLabel.IndexOf('=')), Environment.NewLine);
        script.AppendLine("\tz = model (features)");

        script.AppendLine(ce);
        script.AppendLine(errs);
        script.AppendLine("\tfeaturesNodes = (features)");
        script.AppendLine("\tlabelNodes = (labels)");
        script.AppendLine("\tcriterionNodes = (ce)");
        script.AppendLine("\tevaluationNodes = (errs)");
        script.AppendLine("\toutputNodes = (z)");
        script.AppendLine("}");
        script.Append("}");
        yield return null;

        Debug.Log("Generate complate");
        //Debug.Log(script.ToString());

        codeScreen.GetComponentInChildren<TMPro.TextMeshPro>().text = script.ToString();
        generating = false;
    }

    private IEnumerator Generate2()
    {
        Debug.Log("Generate2");
        var nodes = (from node in FindObjectsOfType<BaseNode>()
                     where node.gameObject.layer == 0
                     select node).ToArray();
        yield return null;
        StringBuilder script = new StringBuilder();
        script.AppendLine("command = TrainConvNet");
        script.AppendLine("makeMode = false ; traceLevel = 0; deviceId = \"auto\"");
        script.AppendLine("rootDir = \".\"; dataDir = \"$rootDir$\"; modelDir = \"$rootDir$/Models\"");
        script.AppendLine("modelPath = \"$modelDir$/test.cmf\"");
        script.AppendLine("TrainConvNet = {");
        script.AppendLine("\taction = \"train\"");
        script.AppendLine("\tBrainScriptNetworkBuilder = {");

        InputNode reader = null;
        StringBuilder model = new StringBuilder();
        string ce = string.Empty;
        string errs = string.Empty;

        model.AppendLine("model(features) = {");

        foreach (var node in nodes)
        {
            if (node.Last.Count == 0 && node.Next.Count == 0) continue;
            if (node.Last.Count == 0 && node is InputNode)
            {
                reader = node as InputNode;
            }
            else if (node is EvaluationNode)
            {
                errs = string.Format("errs = {0} ({1}, {2})", Enum.GetName(typeof(EvaluationNode.Algorithm), ((EvaluationNode)node).readerType), "labels", "z");
            }
            else if (node is CriterionNode)
            {
                ce = string.Format("ce = {0} ({1}, {2})", Enum.GetName(typeof(CriterionNode.Algorithm), ((CriterionNode)node).readerType), "labels", "z");
            }
            else
            {
                string layerParam = node.GetParameters();
                StringBuilder convLast = new StringBuilder();
                if (node.Last.Count > 0)
                {
                    if (node.Last[0] is InputNode)
                    {
                        convLast.Append("features");
                    }
                    else
                    {
                        convLast.Append(node.Last[0].ShortName);
                    }
                    for (int i = 1; i < node.Last.Count; i++)
                    {
                        if (node.Last[i] is InputNode)
                        {
                            convLast.Append(", features");
                            continue;
                        }
                        else
                        {
                            convLast.AppendFormat(", {0}", node.Last[i].ShortName);
                        }
                    }
                }
                model.AppendLine(string.Format("{0} = ConvolutionalLayer {{ {1} }}({2})", node.ShortName, layerParam, convLast.ToString()));
            }
            yield return null;
        }
        script.AppendLine(model.ToString() + "}.z");
        script.AppendFormat("\t{0}{1}", reader?.GetFeatures());
        script.AppendFormat("\t{0}{1}", reader?.GetLabels(), Environment.NewLine);
        script.AppendLine("\tz = model (features)");

        script.AppendLine(ce);
        script.AppendLine(errs);
        script.AppendLine("\tfeaturesNodes = (features)");
        script.AppendLine("\tlabelNodes = (labels)");
        script.AppendLine("\tcriterionNodes = (ce)");
        script.AppendLine("\tevaluationNodes = (errs)");
        script.AppendLine("\toutputNodes = (z)");
        script.AppendLine("}");
        script.AppendLine("reader = {");
        script.AppendLine(reader?.GetParameters());
        script.AppendLine("}");
        script.Append("}");
        yield return null;

        Debug.Log("Generate complate");
        //Debug.Log(script.ToString());

        codeScreen.GetComponentInChildren<TMPro.TextMeshPro>().text = script.ToString();
        generating = false;
    }
}
