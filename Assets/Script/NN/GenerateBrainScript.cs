using MixedRealityToolkit.InputModule.EventData;
using MixedRealityToolkit.InputModule.InputHandlers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GenerateBrainScript : MonoBehaviour, IPointerHandler
{
    [SerializeField]
    private GameObject codeScreen;

    private bool generating = false;

    public void OnPointerClicked(ClickEventData eventData)
    {
        if (!generating)
        {
            StartCoroutine(Generate());
        }
    }

    public void OnPointerDown(ClickEventData eventData) { }

    public void OnPointerUp(ClickEventData eventData) { }

    private IEnumerator Generate()
    {
        Debug.Log("Generate");
        generating = true;
        var nodes = (from node in FindObjectsOfType<BaseNode>()
                     where node.gameObject.layer == 0
                     select node).ToArray();
        yield return null;
        List<InputNode> input = new List<InputNode>();
        List<BaseNode> output = new List<BaseNode>();
        foreach (var node in nodes)
        {
            if (node.Last.Count == 0 && node is InputNode)
            {
                input.Add((InputNode)node);
            }
            else if (node.Next.Count == 0)
            {
                output.Add(node);
            }
        }
        StringBuilder script = new StringBuilder();
        script.AppendLine("BrainScriptNetworkBuilder = {");
        yield return null;
        StringBuilder model = new StringBuilder();
        model.Append("model(");
        for (int i = 0; i < input.Count; i++)
        {
            if (i == 0)
            {
                model.Append(((InputNode)input[i]).GetFeatures());
            }
            else
            {
                model.Append(',');
                model.Append(((InputNode)input[i]).GetFeatures());
            }
        }
        model.AppendLine(") = {");
        BaseNode ceNode = null;
        BaseNode errNode = null;
        yield return null;
        Queue<BaseNode> q = new Queue<BaseNode>();
        foreach (var i in input) q.Enqueue(i);
        while (q.Count > 0)
        {
            BaseNode node = q.Dequeue();
            if (node is InputNode) { }
            else if (node is EvaluationNode)
            {
                errNode = node;
            }
            else if (node is CriterionNode)
            {
                ceNode = node;
            }
            else
            {
                string layerParam = node.GetParameters();
                StringBuilder convLast = new StringBuilder();
                if (node.Last.Count > 0)
                {
                    if (node.Last[0] is InputNode)
                    {
                        convLast.Append(((InputNode)node.Last[0]).GetFeatureName());
                    }
                    else
                    {
                        convLast.Append(node.Last[0].ShortName);
                    }
                    for (int i = 1; i < node.Last.Count; i++)
                    {
                        if (node.Last[i] is InputNode)
                        {
                            convLast.AppendFormat(", {0}", ((InputNode)node.Last[0]).GetFeatureName());
                            continue;
                        }
                        else
                        {
                            convLast.AppendFormat(", {0}", node.Last[i].ShortName);
                        }
                    }
                }
                model.AppendFormat("\t{0} = {1}{{ {2} }}({3}){4}", node.ShortName, node.GetType().Name, layerParam, convLast.ToString(), Environment.NewLine);
            }
            foreach (var item in node.Next)
            {
                q.Enqueue(item);
            }
            yield return null;
        }
        model.AppendLine("}");
        script.AppendLine(model.ToString());
        foreach (var i in input)
        {
            script.AppendLine(i.GetFeatures());
            script.AppendLine(i.GetLabels());
        }
        string z = "\tz = model(";
        bool zflag = false;
        foreach (var node in output)
        {
            for (int i = 0; i < node.Last.Count; i++)
            {
                if (node.Last[i] is CriterionNode || node.Last[i] is EvaluationNode || node.Last[i] is InputNode) continue;
                if (zflag)
                {
                    z += string.Format(":{0}", node.Last[i].ShortName);
                }
                else
                {
                    z += node.Last[i].ShortName;
                    zflag = true;
                }
            }
            yield return null;
        }
        z += ')';
        script.AppendLine(z);

        if (ceNode == null || errNode == null)
        {
            Debug.LogError("Generate failed");
            yield break;
        }

        string featureNodes = "\tfeatureNodes = (";
        string labelNodes = "\tlabelNodes = (";
        for (int i = 0; i < input.Count; i++)
        {
            if (i == 0)
            {
                featureNodes += input[i].GetFeatureName();
                labelNodes += input[i].GetLabelsName();
            }
            else
            {
                featureNodes += ':' + input[i].GetFeatureName();
                labelNodes += ':' + input[i].GetLabelsName();
            }
        }
        featureNodes += ')';
        labelNodes += ')';

        string ce = string.Format("\tce = {0} (", ceNode.GetParameters());
        string errs = string.Format("\terrs = {0} (", errNode.GetParameters());
        for (int i = 0; i < input.Count; i++)
        {
            if (i == 0)
            {
                ce += (input[i].GetLabelsName());
                errs += (input[i].GetLabelsName());
            }
            else
            {
                ce += ',';
                ce += (input[i].GetLabelsName());
                errs += ',';
                errs += (input[i].GetLabelsName());
            }
        }
        ce += ')';
        errs += ')';
        script.AppendLine(featureNodes);
        script.AppendLine(labelNodes);
        script.AppendLine(ce);
        script.AppendLine(errs);
        script.AppendLine("\toutputNodes = (z)");
        script.AppendLine("}");
        yield return null;

        foreach (var item in input)
        {
            script.AppendLine("reader = {");
            script.AppendLine(item.GetParameters());
            script.AppendLine("}");
        }

        Debug.Log("Generate complate");

        codeScreen.GetComponentInChildren<MyInputField>().SetText(script.ToString());
        generating = false;
    }
}
