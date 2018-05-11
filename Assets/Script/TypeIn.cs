using MixedRealityToolkit.InputModule.EventData;
using MixedRealityToolkit.InputModule.InputHandlers;
using System.Linq;
using UnityEngine;

/// <summary>
/// 控制输入状态和输入事件（例如快捷键）
/// </summary>
public class TypeIn : MonoBehaviour, IPointerHandler//IFocusable
{
    [SerializeField]
    private Color normalColor;
    [SerializeField]
    private Color highLightColor;
    private MeshRenderer background;
    private GameObject text;

    private bool isFocus;

    public bool IsFocus
    {
        get
        {
            return isFocus;
        }

        set
        {
            isFocus = value;
        }
    }

    /*public void OnInputClicked(InputClickedEventData eventData)
    {
        background.material.color = highLightColor;

        var typeins = (from i in FindObjectsOfType(typeof(TypeIn))
                       where i is TypeIn && ((TypeIn)i).IsFocus == true
                       select ((TypeIn)i)).ToArray();
        foreach (var item in typeins)
        {
            item.OnFocusExit();
        }

        text.GetComponent<MyInputField>().enabled = true;
        isFocus = true;
    }*/

    private void Awake()
    {
        background = transform.parent.Find("Title").GetComponent<MeshRenderer>();
        background.material.color = normalColor;
        //transform.GetChild(0).gameObject;
        text = transform.Find("TextMeshPro").gameObject;
        text.GetComponent<MyInputField>().enabled = false;
        //Debug.Log("sadasdsa" + text.name);

        //OnInputClicked(null);
        OnPointerClicked(null);
    }

    /*public void OnFocusEnter()
    {
        //Debug.Log(gameObject.name + ": On focus enter");
        background.material.color = highLightColor;
        text.GetComponent<MyInputField>().enabled = true;
    }*/

    /*public override void OnFocusExit(FocusEventData eventData)
    {
        //Debug.Log(gameObject.name + ": On focus exit");
        background.material.color = normalColor;
        text.GetComponent<MyInputField>().enabled = false;
        isFocus = false;
    }*/

    private void Update()
    {
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.S))
        {
            SaveFile();
        }
        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            text.GetComponent<MyInputField>().PageUp();
        }
        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            text.GetComponent<MyInputField>().PageDown();
        }
    }

    public void SaveFile()
    {
        FileAndDirectory.Instance.SaveFile(FileAndDirectory.Instance.FullFilePath(gameObject.name), GetComponentInChildren<MyInputField>().GetText());
    }

    public void OnPointerUp(ClickEventData eventData) { }

    public void OnPointerDown(ClickEventData eventData) { }

    public void OnPointerClicked(ClickEventData eventData)
    {
        background.material.color = highLightColor;

        var typeins = (from i in FindObjectsOfType(typeof(TypeIn))
                       where i is TypeIn && ((TypeIn)i).IsFocus == true
                       select ((TypeIn)i)).ToArray();
        foreach (var item in typeins)
        {
            //item.OnFocusExit(null);
            background.material.color = normalColor;
            text.GetComponent<MyInputField>().enabled = false;
            item.isFocus = false;
        }

        text.GetComponent<MyInputField>().enabled = true;
        isFocus = true;

        eventData?.Use();
    }
}
