using MixedRealityToolkit.InputModule.EventData;
using MixedRealityToolkit.InputModule.Focus;
using MixedRealityToolkit.InputModule.InputHandlers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardKey : FocusTarget, IPointerHandler
{
    public KeyCode key;
    public KeyCode shiftKey;
    [SerializeField]
    private bool hasShift;
    [SerializeField]
    private Keyboard keyBoard;
    [SerializeField]
    private Color normalColor;
    [SerializeField]
    private Color focusColor;

    //private bool enableClick;
    private bool shift;

    public void Shift()
    {
        shift = !shift;
        if (shift && hasShift)
        {
            if (shiftKey == KeyCode.None && key <= KeyCode.Z && key >= KeyCode.A)
            {
                shiftKey = key - (int)KeyCode.A + 'A';
            }
            GetComponentInChildren<TextMesh>().text = ((char)shiftKey).ToString();
        }
        else
        {
            if (hasShift)
            {
                GetComponentInChildren<TextMesh>().text = ((char)key).ToString();
            }
        }
    }

    private void Awake()
    {
        GetComponent<Renderer>().material.color = normalColor;
        //enableClick = false;
        shift = false;
    }

    public override void OnFocusEnter(FocusEventData e)
    {
        GetComponent<Renderer>().material.color = focusColor;
        //enableClick = true;
    }

    public override void OnFocusExit(FocusEventData e)
    {
        GetComponent<Renderer>().material.color = normalColor;
        //enableClick = false;
    }

    public void OnPointerUp(ClickEventData eventData) { }

    public void OnPointerDown(ClickEventData eventData) { }

    public void OnPointerClicked(ClickEventData eventData)
    {
        GetComponent<Renderer>().material.color = normalColor;
        //enableClick = false;
        if (shift && hasShift)
        {
            if (shiftKey == KeyCode.None)
            {
                if (key <= KeyCode.Z && key >= KeyCode.A)
                {
                    shiftKey = key - (int)KeyCode.A + 'A';
                }
                else
                {
                    return;
                }
            }
            keyBoard.ReceiveKey(shiftKey);
        }
        else
        {
            if (key == KeyCode.None)
            {
                return;
            }
            keyBoard.ReceiveKey(key);
        }
        GetComponent<Renderer>().material.color = focusColor;
        //enableClick = true;
    }
}
