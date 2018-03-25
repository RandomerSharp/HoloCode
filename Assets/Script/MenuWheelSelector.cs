﻿using HoloToolkit.Unity.Controllers;
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
            if (angle > 45f && nodes.Count > 0)
            {
                currectSelect = (currectSelect + 1) % nodes.Count;
                lastSelectorPosition = selectorPosition;
                transform.Rotate(transform.up, -360f / 7f, Space.World);
            }
            else if (angle < -45f && nodes.Count > 0)
            {
                currectSelect = (currectSelect - 1 + nodes.Count) % nodes.Count;
                lastSelectorPosition = selectorPosition;
                transform.Rotate(transform.up, 360f / 7f, Space.World);
            }
            shellDialog.GetComponentInChildren<TextMesh>().text = nodes[currectSelect].name;
        }
        IPointingSource ips = null;
        FocusManager.Instance.TryGetSinglePointer(out ips);
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
             select item).Count() > 0) return;

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
        StartCoroutine(Generate1());
    }

    private IEnumerator Generate1()
    {
        var nodes = (from node in FindObjectsOfType<BaseNode>()
                     where node.gameObject.layer == 0
                     select node).ToArray();
        yield return null;
        StringBuilder script = new StringBuilder();
        script.AppendLine("command = TrainConvNet:Eval");
        script.AppendLine("makeMode = false ; traceLevel = 0; deviceId = \"auto\"");
        script.AppendLine("rootDir = \".\"; dataDir = \"$rootDir$\"; modelDir = \"$rootDir$/Models\"");
        script.AppendLine("modelPath = \"$modelDir$/test.cmf\"");
        script.AppendLine("TrainConvNet = {");
        script.AppendLine(" action = \"train\"");
        script.AppendLine(" BrainScriptNetworkBuilder = {");


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
            if (node.Last.Count == 0 && node is FeatureNode)
            {
                switch (((FeatureNode)node).readerType)
                {
                case FeatureNode.ReaderType.ImageShap:
                    inputFeature = "imageShap = 32:32:32";
                    break;
                case FeatureNode.ReaderType.Text:
                    inputFeature = "inputDim = 943";
                    break;
                default:
                    inputFeature = "imageShap = 32:32:32";
                    break;
                }
                model.AppendLine(inputFeature);
            }
            else if (node.Last.Count == 0 && node is LabelNode)
            {
                inputLabel = "labelDim = " + ((LabelNode)node).labelDim.ToString();
                model.AppendLine(inputLabel);
            }
            else if (node is ConvolutionalLayer)
            {
                var temp = (ConvolutionalLayer)node;
                string convParam = string.Format("{0}, ({1}:{1}), pad={2}, activation={3},init = \"{4}\", initValueScale = {5}",
                    temp.inSize, temp.kernelSize, temp.pad, temp.activation, temp.init, temp.initValueScale);
                string convLast = string.Empty;
                if (temp.Last.Count > 0)
                {
                    if (temp.Last[0] is FeatureNode)
                    {
                        convLast += "featNorm";
                    }
                    else
                    {
                        convLast += "p" + temp.Last[0].GetHashCode().ToString();
                    }
                    for (int i = 1; i < temp.Last.Count; i++)
                    {
                        if (temp.Last[i] is FeatureNode)
                        {
                            convLast += ", featNorm";
                            continue;
                        }
                        else if (temp.Last[i] is ConvolutionalLayer)
                        {
                            convLast += ", p" + temp.Last[i].GetHashCode().ToString();
                        }
                    }
                }

                string poolingParam = string.Format("({0}:{0}), stride = ({1}:{1})", temp.poolingSize, temp.stride);
                string poolingLast = string.Format("l{0}", node.GetHashCode());

                model.AppendLine(string.Format("l{0} = ConvolutionalLayer {{ {1} }}({2})", node.GetHashCode(), convParam, convLast));
                model.AppendLine(string.Format("p{0} = MaxPoolingLayer {{ {1} }}({2})", node.GetHashCode(), poolingParam, poolingLast));
            }
            else if (node is DenseLayer)
            {
                var temp = (DenseLayer)node;
                string param = string.Format("{0}, activation={1}, init=\"{2}\", initValueScale = {3} ",
                    temp.init, Enum.GetName(typeof(DenseLayer.Activation), temp.activation), Enum.GetName(typeof(DenseLayer.Init), temp.init),
                    temp.initValueScale);
                string last = string.Empty;
                if (temp.Last.Count > 0)
                {
                    if (temp.Last[0] is FeatureNode)
                    {
                        last += "featNorm";
                    }
                    else
                    {
                        last += "p" + temp.Last[0].GetHashCode().ToString();
                    }
                    for (int i = 1; i < temp.Last.Count; i++)
                    {
                        if (temp.Last[i] is FeatureNode)
                        {
                            last += ", featNorm";
                            continue;
                        }
                        else if (temp.Last[i] is ConvolutionalLayer)
                        {
                            last += ", p" + temp.Last[i].GetHashCode().ToString();
                        }
                    }
                }

                model.AppendLine(string.Format("d{0} = DenseLayer {{ {1} }}({2})", node.GetHashCode(), param, last));
            }
            else if (node is LinearLayer)
            {
                LinearLayer temp = (LinearLayer)node;
                string param = string.Format("{0}, init=\"{1}\", initValueScale = {2} ",
                    temp.init, Enum.GetName(typeof(DenseLayer.Init), temp.init), temp.initValueScale);
                string last = string.Empty;
                if (temp.Last.Count > 0)
                {
                    if (temp.Last[0] is DenseLayer)
                    {
                        last += "d" + temp.Last[0].GetHashCode().ToString();
                    }
                    else
                    {
                        last += "p" + temp.Last[0].GetHashCode().ToString();
                    }
                    for (int i = 1; i < temp.Last.Count; i++)
                    {
                        if (temp.Last[i] is DenseLayer)
                        {
                            last += ", d" + temp.Last[i].GetHashCode().ToString();
                            continue;
                        }
                        else if (temp.Last[i] is ConvolutionalLayer)
                        {
                            last += ", p" + temp.Last[i].GetHashCode().ToString();
                        }
                    }
                }

                model.AppendLine(string.Format("z = LinearLayer {{ {1} }}({2})", node.GetHashCode(), param, last));
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
        script.AppendLine(model.ToString() + ".z");
        script.AppendFormat(" features = Input {{ {0} }}" + Environment.NewLine, inputFeature.Substring(0, inputFeature.IndexOf('=')));
        script.AppendFormat(" labels = Input {{ {0} }}" + Environment.NewLine, inputLabel.Substring(0, inputLabel.IndexOf('=')));
        script.AppendLine(" z = model (features)");

        script.AppendLine(ce);
        script.AppendLine(errs);
        script.AppendLine(" featuresNodes = (features)");
        script.AppendLine(" labelNodes = (labels)");
        script.AppendLine(" criterionNodes = (ce)");
        script.AppendLine(" evaluationNodes = (errs)");
        script.AppendLine(" outputNodes = (z)");
        script.AppendLine("}");
        yield return null;

        codeScreen.GetComponentInChildren<TMPro.TextMeshPro>().text = script.ToString();
    }
}
