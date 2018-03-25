using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;
using UnityEngine.XR.WSA.Input;
using MRDL.Design;

public class Drag0 : MonoBehaviour
{
    private bool holding = false; public InteractionSourceHandedness Handedness { get { return handedness; } }

    [Header("AttachToController Elements")]
    [SerializeField]
    protected InteractionSourceHandedness handedness = InteractionSourceHandedness.Left;

    public MotionControllerInfo.ControllerElementEnum Element { get { return element; } }

    [SerializeField]
    protected MotionControllerInfo.ControllerElementEnum element = MotionControllerInfo.ControllerElementEnum.PointingPose;

    public bool SetChildrenInactiveWhenDetached = true;

    [SerializeField]
    protected Vector3 positionOffset = Vector3.zero;

    [SerializeField]
    protected Vector3 rotationOffset = Vector3.zero;

    [SerializeField]
    protected Vector3 scale = Vector3.one;

    [SerializeField]
    protected bool setScaleOnAttach = false;

    public bool IsAttached { get; private set; }

    private Transform elementTransform;
    public Transform ElementTransform { get; private set; }

    protected MotionControllerInfo controllerRight;

    private void Awake()
    {
    }

    protected void OnEnable()
    {
        if (MotionControllerVisualizer.Instance.TryGetControllerModel(handedness, out controllerRight))
        {
            AttachElementToController(controllerRight);
        }

        MotionControllerVisualizer.Instance.OnControllerModelLoaded += AttachElementToController;
        MotionControllerVisualizer.Instance.OnControllerModelUnloaded += DetachElementFromController;
    }

    protected void OnDisable()
    {
        if (MotionControllerVisualizer.IsInitialized)
        {
            MotionControllerVisualizer.Instance.OnControllerModelLoaded -= AttachElementToController;
            MotionControllerVisualizer.Instance.OnControllerModelUnloaded -= DetachElementFromController;
        }
    }

    protected void OnDestroy()
    {
        if (MotionControllerVisualizer.IsInitialized)
        {
            MotionControllerVisualizer.Instance.OnControllerModelLoaded -= AttachElementToController;
            MotionControllerVisualizer.Instance.OnControllerModelUnloaded -= DetachElementFromController;
        }
    }

    protected void OnDetachFromController()
    {
        InteractionManager.InteractionSourceUpdated -= InteractionSourceUpdated;
    }

    protected void OnAttachToController()
    {
        InteractionManager.InteractionSourceUpdated += InteractionSourceUpdated;
    }

    private void AttachElementToController(MotionControllerInfo newController)
    {
        if (!IsAttached && newController.Handedness == handedness)
        {
            if (!newController.TryGetElement(element, out elementTransform))
            {
                Debug.LogError("Unable to find element of type " + element + " under controller " + newController.ControllerParent.name + "; not attaching.");
                return;
            }
            controllerRight = newController;
            OnAttachToController();

            IsAttached = true;
        }
    }

    private void DetachElementFromController(MotionControllerInfo oldController)
    {
        if (IsAttached && oldController.Handedness == handedness)
        {
            OnDetachFromController();

            IsAttached = false;
        }
    }

    private void InteractionSourceUpdated(InteractionSourceUpdatedEventArgs obj)
    {
        if (obj.state.source.handedness == InteractionSourceHandedness.Right)
        {
            Vector3 v = new Vector3();
            Vector3 p = new Vector3();
            if (obj.state.sourcePose.TryGetForward(out v) && obj.state.sourcePose.TryGetPosition(out p))
            {
            }
        }
    }
}
