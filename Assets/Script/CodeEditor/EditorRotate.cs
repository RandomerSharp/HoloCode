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

public class EditorRotate : MonoBehaviour, IInputHandler
{
    //private IInputSource inputSource;
    private uint sourceId;
    private bool isDraging;

    private Vector3 lastPos = Vector3.zero;

    private void Awake()
    {
        sourceId = uint.MaxValue;
    }

    private void Update()
    {
        if (isDraging)
        {
            InteractionSourceKind source;
            InteractionInputSources.Instance.TryGetSourceKind(sourceId, out source);
            Vector3 pos = Vector3.zero;
            if (source == InteractionSourceKind.Controller)
            {
                if (!InteractionInputSources.Instance.TryGetPointerPosition(sourceId, out pos))
                {
                    return;
                }
            }
            else if (source == InteractionSourceKind.Other)
            {
                if (InteractionInputSources.Instance.TryGetPointerPosition(sourceId, out pos))
                {
                    return;
                }
            }

            transform.Rotate(Vector3.up, Vector3.SignedAngle(lastPos, pos, Vector3.up));
            lastPos = pos;
        }
    }

    public void OnInputUp(InputEventData eventData)
    {
        if (eventData.SourceId == sourceId && isDraging)
        {
            isDraging = false;
            eventData.Use();
        }
    }

    public void OnInputDown(InputEventData eventData)
    {
        if (eventData.selectedObject == null || eventData.selectedObject.layer == 11)
        {
            //inputSource = eventData.InputSource;
            sourceId = eventData.SourceId;
            isDraging = true;

            InteractionInputSources.Instance.TryGetPointerPosition(sourceId, out lastPos);
            eventData.Use();
        }
    }

    public void OnInputPressed(InputPressedEventData eventData) { }

    public void OnInputPositionChanged(InputPositionEventData eventData) { }
}