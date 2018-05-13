using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using HoloToolkit.Unity.InputModule;

public class ParamTypein : MonoBehaviour, IVKeyInput, IInputClickHandler// IPointerHandler
{
    private bool enableInput = false;
    private TextMesh text;

    public bool EnableInput
    {
        get
        {
            return enableInput;
        }

        set
        {
            enableInput = value;
        }
    }

    private void Awake()
    {
        text = transform.Find("InputField/Text").GetComponent<TextMesh>();
    }

    /*public void OnInputClicked(InputClickedEventData eventData)
    {
        foreach (var item in FindObjectsOfType<ParamTypein>())
        {
            item.enableInput = false;
        }
        transform.parent.parent.parent.GetComponentInChildren<Keyboard>().InputTarget = gameObject;
        enableInput = true;
    }*/

    public void VKeyInput(KeyCode key)
    {
        if (!enableInput) return;
        if (key == KeyCode.Return)
        {
            //transform.parent.parent.parent.GetComponentInChildren<Keyboard>().InputTarget = gameObject;
            GameObject.Find("HUD").GetComponentInChildren<Keyboard>().InputTarget = gameObject;
            enableInput = false;
            return;
        }
        if (key == KeyCode.Backspace)
        {
            if (text.text.Length == 0) return;
            text.text = text.text.Substring(0, text.text.Length - 1);
            return;
        }
        text.text += (char)key;
    }

    private void Update()
    {
        if (!enableInput) return;
        if (Input.GetKeyDown(KeyCode.Return))
        {
            transform.parent.parent.parent.GetComponentInChildren<Keyboard>().InputTarget = gameObject;
            enableInput = false;
            return;
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (text.text.Length == 0) return;
            text.text = text.text.Substring(0, text.text.Length - 1);
            return;
        }
        text.text += Input.inputString;
    }

    public string GetValue()
    {
        return text.text;
    }

    public void SetValue(string key, string value)
    {
        text.text = value;
        transform.Find("Param").GetComponent<TextMesh>().text = key;
    }

    /*public void OnPointerUp(ClickEventData eventData) { }
    public void OnPointerDown(ClickEventData eventData) { }

    public void OnPointerClicked(ClickEventData eventData)
    {
        foreach (var item in FindObjectsOfType<ParamTypein>())
        {
            item.enableInput = false;
        }
        //transform.parent.parent.parent.GetComponentInChildren<Keyboard>().InputTarget = gameObject;
        GameObject.Find("HUD").GetComponentInChildren<Keyboard>().InputTarget = gameObject;
        enableInput = true;
    }*/

    public void OnInputClicked(InputClickedEventData eventData)
    {
        foreach (var item in FindObjectsOfType<ParamTypein>())
        {
            item.enableInput = false;
        }
        //transform.parent.parent.parent.GetComponentInChildren<Keyboard>().InputTarget = gameObject;
        GameObject.Find("HUD").GetComponentInChildren<Keyboard>().InputTarget = gameObject;
        enableInput = true;
    }
}
