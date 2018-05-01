using MixedRealityToolkit.InputModule.EventData;
using MixedRealityToolkit.InputModule.Focus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardKey : FocusTarget
{
    public KeyCode key;
    [SerializeField]
    private Keyboard keyBoard;
    [SerializeField]
    private Color normalColor;
    [SerializeField]
    private Color focusColor;

    private bool enableClick;

    private void Awake()
    {
        GetComponent<Renderer>().material.color = normalColor;
        enableClick = false;
    }

    public override void OnFocusEnter(FocusEventData e)
    {
        GetComponent<Renderer>().material.color = focusColor;
        enableClick = true;
    }

    public override void OnFocusExit(FocusEventData e)
    {
        GetComponent<Renderer>().material.color = normalColor;
        enableClick = false;
    }

    /*public void OnInputClicked(InputClickedEventData eventData)
    {
        GetComponent<Renderer>().material.color = normalColor;
        enableClick = false;
        if (key == KeyCode.None)
        {
            return;
        }
        keyBoard.ReceiveKey(key);
        GetComponent<Renderer>().material.color = focusColor;
        enableClick = true;
    }*/
}
