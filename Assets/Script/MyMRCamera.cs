using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMRCamera : MonoBehaviour, IControllerInputHandler
{
    public void OnInputPositionChanged(InputPositionEventData eventData)
    {
        if (eventData.PressType == InteractionSourcePressInfo.Thumbstick)
        {
        }
    }
}
