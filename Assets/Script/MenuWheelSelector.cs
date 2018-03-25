using HoloToolkit.Unity.Controllers;
using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
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
        if (obj.state.source.handedness == handedness && obj.state.selectPressed)
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
}
