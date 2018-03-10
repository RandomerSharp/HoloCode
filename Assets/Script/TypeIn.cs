using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeIn : MonoBehaviour
{
    [SerializeField]
    private CodeText codeManager;

    [SerializeField]
    private Color normalColor;
    [SerializeField]
    private Color highLightColor;
    private MeshRenderer background;
    private GameObject text;

    private void Awake()
    {
        background = GetComponent<MeshRenderer>();
        background.material.color = normalColor;
        text = transform.GetChild(0).gameObject;
    }

    public void OnFocusEnter()
    {
        Debug.Log(gameObject.name + ": On focus enter");
        background.material.color = highLightColor;
        text.GetComponent<MyInputField>().enabled = true;
    }

    public void OnFocusExit()
    {
        Debug.Log(gameObject.name + ": On focus exit");
        background.material.color = normalColor;
        text.GetComponent<MyInputField>().enabled = false;
    }

    private void Update()
    {
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.S))
        {
        }
    }
}
