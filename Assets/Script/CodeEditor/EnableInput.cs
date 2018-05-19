using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using MixedRealityToolkit.InputModule.Focus;
using MixedRealityToolkit.InputModule.EventData;

public class EnableInput : FocusTarget
{
    [SerializeField]
    private InputField inputField;
    [SerializeField]
    private CodeText codeManager;

    [SerializeField]
    private Color normalColor;
    [SerializeField]
    private Color highLightColor;
    [SerializeField]
    private Image background;

    private void Awake()
    {
        background.color = normalColor;
    }

    public override void OnFocusEnter(FocusEventData e)
    {
        Debug.Log(gameObject.name + ": On focus enter");
        GetComponentInChildren<MyInputField>().enabled = true;
        background.color = highLightColor;
    }

    public override void OnFocusExit(FocusEventData e)
    {
        Debug.Log(gameObject.name + ": On focus exit");
        GetComponentInChildren<MyInputField>().enabled = false;
        background.color = normalColor;
    }

    /*public void OnFocusEnter()
    {
        Debug.Log(gameObject.name + ": On focus enter");
        GetComponentInChildren<MyInputField>().enabled = true;
        background.color = highLightColor;
    }

    public void OnFocusExit()
    {
        Debug.Log(gameObject.name + ": On focus exit");
        GetComponentInChildren<MyInputField>().enabled = false;
        background.color = normalColor;
    }*/

    private void Update()
    {
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.S))
        {
        }
    }

    /*private IEnumerator CodeInput()
    {
        while (true)
        {
            if (Input.anyKey)
            {
                Debug.Log("Input: " + Input.inputString);
            }
            yield return null;
        }
    }*/
}
