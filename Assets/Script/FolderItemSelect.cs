using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FolderItemSelect : MonoBehaviour, IInputClickHandler, IFocusable
{
    [SerializeField]
    private UnityEvent onClick;

    private bool isFocused;

    public void OnFocusEnter()
    {
        foreach (var child in GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = LayerMask.NameToLayer("Highlight");
            //Debug.Log(child.name);
        }
        isFocused = true;

        GetComponentInChildren<CubeRotate>().RotateSpeed = 3f;
    }

    public void OnFocusExit()
    {
        isFocused = false;
        foreach (var child in GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = LayerMask.NameToLayer("Default");
        }

        GetComponentInChildren<CubeRotate>().RotateSpeed = 0f;
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        onClick.Invoke();
    }

    public void OpenFolder()
    {
    }




    private void Update()
    {
        if (isFocused && Input.GetKeyUp(KeyCode.Return))
        {
            onClick.Invoke();
        }
    }
}
