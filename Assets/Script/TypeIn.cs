//using HoloToolkit.Unity.InputModule;
using MixedRealityToolkit.InputModule.EventData;
using MixedRealityToolkit.InputModule.InputHandlers;
using System.Linq;
using UnityEngine;

/// <summary>
/// 控制输入状态和输入事件（例如快捷键）
/// </summary>
public class TypeIn : MonoBehaviour, IInputHandler//IFocusable,IInputHandler
{
    [SerializeField]
    private Color normalColor;
    [SerializeField]
    private Color highLightColor;
    private MeshRenderer background;
    private GameObject text;

    private bool isFocus;

    private float downDelta = 0f;
    private bool down = false;

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

    private void Awake()
    {
        background = transform.parent.Find("Title").GetComponent<MeshRenderer>();
        background.material.color = normalColor;
        //transform.GetChild(0).gameObject;
        text = transform.Find("TextMeshPro").gameObject;
        text.GetComponent<MyInputField>().enabled = false;
        //Debug.Log("sadasdsa" + text.name);

        //OnInputClicked(null);
        OnPointerClicked();
    }

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
        if (down)
        {
            downDelta += Time.deltaTime;
            if (downDelta > 5f)
            {
                down = false;
                downDelta = 0f;
            }
        }
    }

    public void SaveFile()
    {
        string fullPath = FileAndDirectory.Instance.FullFilePath(transform.parent.gameObject.name);
        Debug.Log(fullPath);
        FileAndDirectory.Instance.SaveFile(fullPath, GetComponentInChildren<MyInputField>().GetText());
    }

    public void OnPointerClicked()
    {
        Debug.Log("My pointer clicked");

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

        background.material.color = highLightColor;
        text.GetComponent<MyInputField>().enabled = true;
        isFocus = true;
    }

    public void OnInputUp(InputEventData eventData)
    {
        if (downDelta < 0.75f)
        {
            OnPointerClicked();
        }
        down = false;
        downDelta = 0f;
        eventData.Use();
    }

    public void OnInputDown(InputEventData eventData)
    {
        down = true;
        eventData.Use();
    }

    public void OnInputPressed(InputPressedEventData eventData) { }

    public void OnInputPositionChanged(InputPositionEventData eventData) { }
}
