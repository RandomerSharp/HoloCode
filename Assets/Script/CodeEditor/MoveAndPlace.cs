using MixedRealityToolkit.Common;
using MixedRealityToolkit.InputModule;
using MixedRealityToolkit.InputModule.EventData;
using MixedRealityToolkit.InputModule.InputHandlers;
using MixedRealityToolkit.InputModule.InputSources;
using MixedRealityToolkit.InputModule.Utilities.Interactions;
using MixedRealityToolkit.UX.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.XR.WSA.Input;

public class MoveAndPlace : MonoBehaviour, IInputHandler//, ISourceStateHandler
{
    // Event that gets raised when user begins manipulating the object
    public event Action StartedManipulating;
    // Event that gets raised when the user ends manipulation
    public event Action StoppedManipulating;

    [SerializeField]
    [Tooltip("Transform that will be dragged. Defaults to the object of the component.")]
    private Transform HostTransform = null;


    [Flags]
    private enum State
    {
        Start = 0x000,
        Moving = 0x001,
        Scaling = 0x010,
        Rotating = 0x100,
        MovingScaling = 0x011,
        RotatingScaling = 0x110,
        MovingRotatingScaling = 0x111
    };

    private State currentState;
    private TwoHandMoveLogic m_moveLogic;
    // Maps input id -> position of hand
    private readonly Dictionary<uint, Vector3> m_handsPressedLocationsMap = new Dictionary<uint, Vector3>();
    // Maps input id -> input source. Then obtain position of input source using currentInputSource.TryGetGripPosition(currentInputSourceId, out inputPosition);
    private readonly Dictionary<uint, IInputSource> m_handsPressedInputSourceMap = new Dictionary<uint, IInputSource>();
    private uint inputSourceId = 0;

    private void Awake()
    {
        m_moveLogic = new TwoHandMoveLogic();
    }

    private void Start()
    {
        if (HostTransform == null)
        {
            HostTransform = transform;
        }
    }

    private void Update()
    {
        // Update positions of all hands
        foreach (var key in m_handsPressedInputSourceMap.Keys)
        {
            Vector3 inputPosition;
            var inputSource = m_handsPressedInputSourceMap[key];
            if (InteractionInputSources.Instance.TryGetGripPosition(inputSource.SourceId, out inputPosition))
            {
                m_handsPressedLocationsMap[key] = inputPosition;
            }
        }

        if (currentState != State.Start)
        {
            UpdateStateMachine();
        }
    }

    private Vector3 GetInputPosition(InputEventData eventData)
    {
        Vector3 result;
        InteractionInputSources.Instance.TryGetGripPosition(eventData.SourceId, out result);
        return result;
    }


    public void OnInputDown(InputEventData eventData)
    {
        if (eventData.used) return;
        // Add to hand map
        m_handsPressedLocationsMap[eventData.SourceId] = GetInputPosition(eventData);
        m_handsPressedInputSourceMap[eventData.SourceId] = eventData.InputSource;
        UpdateStateMachine();

        InteractionSourceKind sourceKind;
        InteractionInputSources.Instance.TryGetSourceKind(eventData.SourceId, out sourceKind);
        if (sourceKind == InteractionSourceKind.Controller || sourceKind == InteractionSourceKind.Hand)
        {
            inputSourceId = eventData.SourceId;
        }

        eventData.Use();
    }

    public void OnInputPressed(InputPressedEventData eventData) { }

    public void OnInputPositionChanged(InputPositionEventData eventData) { }

    public void OnInputUp(InputEventData eventData)
    {
        RemoveSourceIdFromHandMap(eventData.SourceId);
        UpdateStateMachine();
        eventData.Use();
    }

    /*public void OnSourceDetected(SourceStateEventData eventData)
    {
    }*/

    private void RemoveSourceIdFromHandMap(uint sourceId)
    {
        if (m_handsPressedLocationsMap.ContainsKey(sourceId))
        {
            m_handsPressedLocationsMap.Remove(sourceId);
        }

        if (m_handsPressedInputSourceMap.ContainsKey(sourceId))
        {
            m_handsPressedInputSourceMap.Remove(sourceId);
        }
    }

    /*public void OnSourceLost(SourceStateEventData eventData)
    {
        RemoveSourceIdFromHandMap(eventData.SourceId);
        UpdateStateMachine();
        eventData.Use();
    }

    public void OnSourcePositionChanged(SourcePositionEventData eventData) { }

    public void OnSourceRotationChanged(SourceRotationEventData eventData) { }*/

    private void UpdateStateMachine()
    {
        var handsPressedCount = m_handsPressedLocationsMap.Count;
        State newState = currentState;
        switch (currentState)
        {
        case State.Start:
        case State.Moving:
            if (handsPressedCount == 0)
            {
                newState = State.Start;
            }
            else if (handsPressedCount == 1)
            {
                newState = State.Moving;
            }
            break;
        case State.MovingRotatingScaling:
            // TODO: if < 2, make this go to start state ('drop it')
            if (handsPressedCount == 0)
            {
                newState = State.Start;
            }
            else if (handsPressedCount == 1)
            {
                newState = State.Moving;
            }
            break;
        default:
            throw new ArgumentOutOfRangeException();
        }
        InvokeStateUpdateFunctions(currentState, newState);
        currentState = newState;
    }

    private void InvokeStateUpdateFunctions(State oldState, State newState)
    {
        if (newState != oldState)
        {
            switch (newState)
            {
            case State.Moving:
                OnOneHandMoveStarted();
                break;
            case State.Start:
                OnManipulationEnded();
                break;
            default:
                break;
            }
            switch (oldState)
            {
            case State.Start:
                OnManipulationStarted();
                break;
            default:
                break;
            }
        }
        else
        {
            switch (newState)
            {
            case State.Moving:
                OnOneHandMoveUpdated();
                break;
            default:
                break;
            }
        }
    }

    /*private void OnTwoHandManipulationUpdated()
    {
        var targetRotation = HostTransform.rotation;
        var targetPosition = HostTransform.position;
        var targetScale = HostTransform.localScale;

        if ((currentState & State.Moving) > 0)
        {
            targetPosition = m_moveLogic.Update(GetHandsCentroid(), targetPosition);
        }

        HostTransform.position = targetPosition;
        HostTransform.rotation = targetRotation;
        HostTransform.localScale = targetScale;
    }*/
    private void OnOneHandMoveUpdated()
    {
        var targetPosition = m_moveLogic.Update(m_handsPressedLocationsMap.Values.First(), HostTransform.position);

        HostTransform.position = targetPosition;
    }

    /*private void OnTwoHandManipulationEnded()
    {
        Debug.Log("Ended");
        transform.parent = GameObject.Find("Editor")?.transform;
        GetComponentInParent<ObjectCollection>().UpdateCollection();
    }*/

    private Vector3 GetHandsCentroid()
    {
        Vector3 result = m_handsPressedLocationsMap.Values.Aggregate(Vector3.zero, (current, state) => current + state);
        return result / m_handsPressedLocationsMap.Count;
    }

    /*private void OnTwoHandManipulationStarted(State newState)
    {
        if ((newState & State.Moving) > 0)
        {
            m_moveLogic.Setup(GetHandsCentroid(), HostTransform);
        }
    }*/

    private void OnOneHandMoveStarted()
    {
        Assert.IsTrue(m_handsPressedLocationsMap.Count == 1);

        transform.parent = null;
        Debug.Log(GameObject.Find("Editor").transform.childCount);
        //GameObject.Find("Editor").GetComponent<ObjectCollection>().UpdateCollection();
        gameObject.AddComponent<MixedRealityToolkit.Utilities.Billboard>();

        m_moveLogic.Setup(m_handsPressedLocationsMap.Values.First(), HostTransform);
    }

    private void OnManipulationStarted()
    {
        StartedManipulating?.Invoke();
        InputManager.PushModalInputHandler(gameObject);
    }

    private void OnManipulationEnded()
    {
        StoppedManipulating?.Invoke();
        InputManager.PopModalInputHandler();

        transform.parent = GameObject.Find("Editor")?.transform;
        Destroy(GetComponent<MixedRealityToolkit.Utilities.Billboard>());
        Ray ray;
        InteractionInputSources.Instance.TryGetPointingRay(inputSourceId, out ray);
        var dir = (ray.direction + (ray.origin - CameraCache.Main.transform.position)).normalized;
        float minn = float.MaxValue;
        int index = 0;
        foreach (var t in GameObject.Find("Editor").GetComponentsInChildren<Transform>())
        {
            if (minn > Vector3.Angle(dir, t.position - CameraCache.Main.transform.position))
            {
                minn = Vector3.Angle(dir, t.position - CameraCache.Main.transform.position);
                index = t.GetSiblingIndex();
            }
        }
        transform.SetSiblingIndex(index);
        GetComponentInParent<ObjectCollection>().UpdateCollection();
    }
}
