using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class EnableInput : MonoBehaviour, IFocusable
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

    public void OnFocusEnter()
    {
        Debug.Log(gameObject.name + ": On focus enter");
        EventSystem.current.SetSelectedGameObject(inputField.gameObject);
        inputField.MoveTextEnd(false);
        background.color = highLightColor;
        StartCoroutine(CodeInput());
    }

    public void OnFocusExit()
    {
        Debug.Log(gameObject.name + ": On focus exit");
        EventSystem.current.SetSelectedGameObject(null);
        background.color = normalColor;
        StopAllCoroutines();
    }

    private void Update()
    {
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.S))
        {
        }
    }

    private IEnumerator CodeInput()
    {
        while (true)
        {
            if (Input.anyKey)
            {
                Debug.Log("Input: " + Input.inputString);
            }
            yield return null;
        }
    }
}
