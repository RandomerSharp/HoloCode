using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;
using UnityEngine.XR.WSA.Input;
using MRDL.Design;
using HoloToolkit.Unity;

public class Drag0 : MonoBehaviour
{
    /*private bool holding = false; public InteractionSourceHandedness Handedness { get { return handedness; } }

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
    }*/

    public event Action StartedDragging;
    public event Action StoppedDragging;
    public Transform HostTransform;

    public float DistanceScale = 2f;

    [Range(0.01f, 1.0f)]
    public float PositionLerpSpeed = 0.2f;

    [Range(0.01f, 1.0f)]
    public float RotationLerpSpeed = 0.2f;

    private bool isDragging;
    private bool isGazed;
    private Vector3 objRefForward;
    private Vector3 objRefUp;
    private float objRefDistance;
    private Quaternion gazeAngularOffset;
    private float handRefDistance;
    private Vector3 objRefGrabPoint;

    private Vector3 draggingPosition;
    private Quaternion draggingRotation;

    private IInputSource currentInputSource;
    private uint currentInputSourceId;

    private void Start()
    {
        if (HostTransform == null)
        {
            HostTransform = transform;
        }
    }

    private void OnDestroy()
    {
        if (isDragging)
        {
            StopDragging();
        }

        if (isGazed)
        {
            OnFocusExit();
        }
    }

    private void Update()
    {
        if (isDragging)
        {
            UpdateDragging();
        }
    }

    public void StartDragging(Vector3 initialDraggingPosition)
    {
        if (isDragging)
        {
            return;
        }
        InputManager.Instance.PushModalInputHandler(gameObject);

        isDragging = true;

        Transform cameraTransform = CameraCache.Main.transform;

        Vector3 inputPosition = Vector3.zero;
        InteractionSourceInfo sourceKind;
        currentInputSource.TryGetSourceKind(currentInputSourceId, out sourceKind);
        switch (sourceKind)
        {
        case InteractionSourceInfo.Hand:
            currentInputSource.TryGetGripPosition(currentInputSourceId, out inputPosition);
            break;
        case InteractionSourceInfo.Controller:
            currentInputSource.TryGetPointerPosition(currentInputSourceId, out inputPosition);
            break;
        }

        Vector3 objForward = HostTransform.forward;
        Vector3 objUp = HostTransform.up;
        objRefGrabPoint = cameraTransform.transform.InverseTransformDirection(HostTransform.position - initialDraggingPosition);


        objForward = cameraTransform.InverseTransformDirection(objForward);
        objUp = cameraTransform.InverseTransformDirection(objUp);

        objRefForward = objForward;
        objRefUp = objUp;

        draggingPosition = initialDraggingPosition;

        StartedDragging.RaiseEvent();
    }

    private void UpdateDragging()
    {
        //Transform cameraTransform = CameraCache.Main.transform;

        Vector3 inputPosition = Vector3.zero;
        Quaternion inputRotation = Quaternion.identity;
        InteractionSourceInfo sourceKind;
        currentInputSource.TryGetSourceKind(currentInputSourceId, out sourceKind);
        switch (sourceKind)
        {
        case InteractionSourceInfo.Hand:
            currentInputSource.TryGetGripPosition(currentInputSourceId, out inputPosition);
            break;
        case InteractionSourceInfo.Controller:
            currentInputSource.TryGetPointerPosition(currentInputSourceId, out inputPosition);
            currentInputSource.TryGetPointerRotation(currentInputSourceId, out inputRotation);
            break;
        }

        Vector3 newPosition = Vector3.Lerp(HostTransform.position, draggingPosition, PositionLerpSpeed);
        HostTransform.position = newPosition;
        Quaternion newRotation = Quaternion.Lerp(HostTransform.rotation, draggingRotation, RotationLerpSpeed);
        HostTransform.rotation = newRotation;
    }

    public void StopDragging()
    {
        if (!isDragging)
        {
            return;
        }

        InputManager.Instance.PopModalInputHandler();

        isDragging = false;
        currentInputSource = null;
        currentInputSourceId = 0;
        StoppedDragging.RaiseEvent();
    }

    public void OnFocusEnter()
    {
        if (isGazed)
        {
            return;
        }

        isGazed = true;
    }

    public void OnFocusExit()
    {
        if (!isGazed)
        {
            return;
        }

        isGazed = false;
    }

    public void OnInputUp(InputEventData eventData)
    {
        if (currentInputSource != null &&
            eventData.SourceId == currentInputSourceId)
        {
            eventData.Use();

            StopDragging();
        }
    }

    public void OnInputDown(InputEventData eventData)
    {
        if (isDragging)
        {
            return;
        }
        InteractionSourceInfo sourceKind;
        eventData.InputSource.TryGetSourceKind(eventData.SourceId, out sourceKind);
        if (sourceKind != InteractionSourceInfo.Hand)
        {
            if (!eventData.InputSource.SupportsInputInfo(eventData.SourceId, SupportedInputInfo.Position))
            {
                return;
            }
        }

        eventData.Use();

        currentInputSource = eventData.InputSource;
        currentInputSourceId = eventData.SourceId;

        FocusDetails? details = FocusManager.Instance.TryGetFocusDetails(eventData);

        Vector3 initialDraggingPosition = (details == null)
            ? HostTransform.position
            : details.Value.Point;

        StartDragging(initialDraggingPosition);
    }

    public void OnSourceDetected(SourceStateEventData eventData)
    {
    }

    public void OnSourceLost(SourceStateEventData eventData)
    {
        if (currentInputSource != null && eventData.SourceId == currentInputSourceId)
        {
            StopDragging();
        }
    }
}
