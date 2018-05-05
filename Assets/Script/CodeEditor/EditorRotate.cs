using MixedRealityToolkit.InputModule.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class EditorRotate : MonoBehaviour
{
    public InteractionSourceHandedness Handedness { get { return handedness; } }

    [SerializeField]
    protected InteractionSourceHandedness handedness = InteractionSourceHandedness.Left;

    public bool IsAttached { get; private set; }

    protected MotionControllerInfo controller;

    private void OnEnable()
    {
        if (MotionControllerVisualizer.Instance.TryGetControllerModel(handedness, out controller))
        {
            AttachElementToController(controller);
        }

        MotionControllerVisualizer.Instance.OnControllerModelLoaded += AttachElementToController;
        MotionControllerVisualizer.Instance.OnControllerModelUnloaded += DetachElementFromController;
    }

    private void OnDisable()
    {
        if (MotionControllerVisualizer.IsInitialized)
        {
            MotionControllerVisualizer.Instance.OnControllerModelLoaded -= AttachElementToController;
            MotionControllerVisualizer.Instance.OnControllerModelUnloaded -= DetachElementFromController;
        }
    }

    private void OnDestroy()
    {
        if (MotionControllerVisualizer.IsInitialized)
        {
            MotionControllerVisualizer.Instance.OnControllerModelLoaded -= AttachElementToController;
            MotionControllerVisualizer.Instance.OnControllerModelUnloaded -= DetachElementFromController;
        }
    }

    private void AttachElementToController(MotionControllerInfo newController)
    {
        if (!IsAttached && newController.Handedness == handedness)
        {
            controller = newController;
            OnAttachToController();

            IsAttached = true;
        }
    }

    private void DetachElementFromController(MotionControllerInfo oldController)
    {
        if (IsAttached && oldController.Handedness == handedness)
        {
            OnDetachFromController();

            controller = null;
            transform.parent = null;

            IsAttached = false;
        }
    }


    private void OnAttachToController()
    {
        InteractionManager.InteractionSourceUpdated += InteractionSourceUpdated;
    }

    private void OnDetachFromController()
    {
        InteractionManager.InteractionSourceUpdated -= InteractionSourceUpdated;
    }

    private void InteractionSourceUpdated(InteractionSourceUpdatedEventArgs obj)
    {
        if (obj.state.source.handedness == handedness && obj.state.touchpadPressed)
        {
            Vector2 pos = Vector2.zero;
            try
            {
                pos = obj.state.touchpadPosition;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                return;
            }
            if (pos.x > 0.3f)
            {
                transform.Rotate(transform.up, 1f);
            }
            else if (pos.x < -0.3f)
            {
                transform.Rotate(transform.up, -1f);
            }
        }
    }
}