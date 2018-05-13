using HoloToolkit.Unity.InputModule;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class EditorRotate : MonoBehaviour, IInputHandler
{
    //private IInputSource inputSource;
    private uint m_sourceId;
    private bool isDraging;

    private IInputSource m_inputSource;

    private Vector3 m_lastPos = Vector3.zero;

    private void Awake()
    {
        m_sourceId = uint.MaxValue;
    }

    private void Update()
    {
        if (isDraging)
        {
            InteractionSourceInfo source;
            //InteractionInputSources.Instance.TryGetSourceKind(m_sourceId, out source);
            m_inputSource.TryGetSourceKind(m_sourceId, out source);
            Vector3 pos = Vector3.zero;
            if (source == InteractionSourceInfo.Controller)
            {
                if (!m_inputSource.TryGetPointerPosition(m_sourceId, out pos))
                {
                    return;
                }
            }
            else if (source == InteractionSourceInfo.Other)
            {
                if (!m_inputSource.TryGetPointerPosition(m_sourceId, out pos))
                {
                    return;
                }
            }

            transform.Rotate(Vector3.up, Vector3.SignedAngle(m_lastPos, pos, Vector3.up));
            m_lastPos = pos;
        }
    }

    public void OnInputUp(InputEventData eventData)
    {
        if (eventData.SourceId == m_sourceId && isDraging)
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
            m_sourceId = eventData.SourceId;
            isDraging = true;

            //InteractionInputSources.Instance.TryGetPointerPosition(sourceId, out lastPos);
            m_inputSource = eventData.InputSource;
            m_inputSource.TryGetPointerPosition(m_sourceId, out m_lastPos);
            eventData.Use();
        }
    }

    //public void OnInputPressed(InputPressedEventData eventData) { }

    public void OnInputPositionChanged(InputPositionEventData eventData) { }
}