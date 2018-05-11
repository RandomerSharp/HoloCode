using MixedRealityToolkit.InputModule.EventData;
using MixedRealityToolkit.InputModule.Focus;
using MixedRealityToolkit.InputModule.InputHandlers;
using MixedRealityToolkit.InputModule.InputSources;
using MixedRealityToolkit.InputModule.Pointers;
using MixedRealityToolkit.InputModule.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class EditorRotate : MonoBehaviour, IHoldHandler
{
    private IInputSource inputSource;
    private uint sourceId;

    private void Awake()
    {
        sourceId = uint.MaxValue;
    }

    public void OnHoldCanceled(InputEventData eventData)
    {
        if (eventData.SourceId == sourceId)
        {
            eventData.Use();
        }
    }

    public void OnHoldCompleted(InputEventData eventData)
    {
        if (eventData.SourceId == sourceId)
        {
            eventData.Use();
        }
    }

    public void OnHoldStarted(InputEventData eventData)
    {
        Debug.Log("Hold handler");
        if (eventData.selectedObject == null || eventData.selectedObject.layer == 11)
        {
            Debug.Log(eventData.Handedness);
            inputSource = eventData.InputSource;
            sourceId = eventData.SourceId;

            eventData.Use();
        }
    }
}