using HUX.Buttons;
using HUX.Interaction;
using HUX.Receivers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CompoundButtonClick : InteractionReceiver
{
    [SerializeField]
    private UnityEvent onClick;

    protected override void OnTapped(GameObject obj, InteractionManager.InteractionEventArgs eventArgs)
    {
        base.OnTapped(obj, eventArgs);
        switch (obj.name)
        {
        case "SelectWorkspace":
        {
            Debug.Log("SelectWorkspace clicked");
            break;
        }
        case "Setting":
        {
            Debug.Log("Setting clicked");
            break;
        }
        case "Quit":
        {
            Debug.Log("Quit clicked");
            Application.Quit();
            break;
        }
        default:
            break;
        }
    }
}
