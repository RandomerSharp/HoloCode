﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ItemSelect : MonoBehaviour, IInputClickHandler, IFocusable
{
    [SerializeField]
    protected UnityEvent onClick;

    private bool isFocused;

    public virtual void OnFocusEnter()
    {
        foreach (var child in GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = LayerMask.NameToLayer("Highlight");
            //Debug.Log(child.name);
        }
        isFocused = true;
        //if (onFocusEnter != null) onFocusEnter.Invoke();
    }

    public virtual void OnFocusExit()
    {
        isFocused = false;
        foreach (var child in GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = LayerMask.NameToLayer("Default");
        }
        //if (onFocusExit != null) onFocusExit.Invoke();
    }

    public virtual void OnInputClicked(InputClickedEventData eventData)
    {
        onClick.Invoke();
    }




    public void SelectWorkspace()
    {
        SceneManager.LoadScene("SelectWorkspace");
    }

    public void GotoSettings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Return(string fromScene)
    {
        SceneManager.LoadScene(fromScene);
    }




    private void Update()
    {
        if (isFocused && Input.GetKeyUp(KeyCode.Return))
        {
            onClick.Invoke();
        }
    }
}
