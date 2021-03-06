﻿using MixedRealityToolkit.Common;
using MixedRealityToolkit.InputModule.EventData;
using MixedRealityToolkit.InputModule.InputHandlers;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseNode : MonoBehaviour, IPointerHandler
{
    public enum DisplayModeEnum
    {
        InMenu,
        InHand,
        Hidden
    }

    public enum RandomInitialization
    {
        heNormal,
        heUniform,
        glorotNormal,
        glorotUniform,
        xavier,
        uniform,
        gaussian,
        zero
    }

    public enum ActivationFunction
    {
        Sigmoid,
        Tanh,
        ReLU,
        Softmax,
        LogSoftmax,
        Hardmax
    }

    public enum LossFunction
    {
        CrossEntropy,
        CrossEntropyWithSoftmax,
        Logistic,
        WeightedLogistic,
        ClassificationError
    }

    public enum MyBoolean
    {
        Flase,
        True
    }

    private DisplayModeEnum displayMode;
    private List<BaseNode> next;
    private List<BaseNode> last;

    private MenuWheelSelector nodeManager;

    protected string shortName;

    public DisplayModeEnum DisplayMode
    {
        set { displayMode = value; }
    }
    public List<BaseNode> Next
    {
        get
        {
            return next;
        }
    }
    public List<BaseNode> Last
    {
        get
        {
            return last;
        }
    }

    public string ShortName
    {
        get
        {
            return shortName;
        }
    }

    public BaseNode LastNode<T>()
    {
        if (last.Count == 0) return null;
        foreach (var item in last)
        {
            if (item is T)
            {
                return item;
            }
        }
        return null;
    }

    public BaseNode NextNode<T>()
    {
        if (next.Count == 0) return null;
        foreach (var item in next)
        {
            if (item is T)
            {
                return item;
            }
        }
        return null;
    }

    public void SetNext(BaseNode node)
    {
        next.Add(node);
    }

    public void SetLast(BaseNode node)
    {
        last.Add(node);
    }

    public void RemoveLast(BaseNode node)
    {
        if (last.Remove(node))
            Debug.Log("Remove last " + node.gameObject.name);
    }

    public void RemoveNext(BaseNode node)
    {
        if (next.Remove(node))
        {
            Debug.Log("Remove next " + node.name);
        }
    }

    protected virtual void Awake()
    {
        next = new List<BaseNode>();
        last = new List<BaseNode>();

        nodeManager = FindObjectsOfType<MenuWheelSelector>()[0];
    }

    public void CreateOne()
    {
        Vector3 forward = (GameObject.Find("DefaultCursor").transform.position - CameraCache.Main.transform.position).normalized;
        var obj = GameObject.Instantiate(gameObject, CameraCache.Main.transform.position + forward * 20, Quaternion.identity);
        obj.layer = 0;
    }

    public abstract string GetParameters();
    public abstract void SetInspector(GameObject inspector, GameObject signleLineInput, GameObject signleLineSelect);




    private bool isPressed;
    private bool isLongPressed;
    private float pressDelta;

    public void OnPointerUp(ClickEventData eventData)
    {
        if (isLongPressed)
        {
            var inspector = transform.Find("HUD/ParamInspector").gameObject;
            inspector.SetActive(true);
            var inspector1 = inspector.GetComponent<Inspector>();
            inspector1.TargetObject = gameObject;
            inspector1.TargetName = gameObject.name;
            SetInspector(inspector, inspector1.paramInput, inspector1.paramSelect);
        }
        isPressed = false;
        isLongPressed = false;
        pressDelta = 0f;
    }

    public void OnPointerDown(ClickEventData eventData)
    {
        if (!isPressed)
        {
            isPressed = true;
        }
    }

    public void OnPointerClicked(ClickEventData eventData)
    {
        var inventory = GameObject.Find("HUX/Inventory").GetComponent<InventoryManager>();
        inventory.NodeSelect(this);
    }

    private void Update()
    {
        if (isPressed)
        {
            pressDelta += Time.deltaTime;
            if (pressDelta > 1f)
            {
                isLongPressed = true;
            }
        }
    }
}