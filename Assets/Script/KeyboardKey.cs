using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class KeyboardKey : MonoBehaviour, IInputClickHandler, IFocusable
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

    public void OnFocusEnter()
    {
        GetComponent<Renderer>().material.color = focusColor;
        enableClick = true;
    }

    public void OnFocusExit()
    {
        GetComponent<Renderer>().material.color = normalColor;
        enableClick = false;
    }

    public void OnInputClicked(InputClickedEventData eventData)
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
    }
}
