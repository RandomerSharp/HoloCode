using MixedRealityToolkit.InputModule.EventData;
using MixedRealityToolkit.InputModule.Focus;
using MixedRealityToolkit.InputModule.InputHandlers;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ItemSelect : FocusTarget, IInputHandler//IPointerHandler//, IInputClickHandler, IFocusable
{
    [SerializeField]
    protected UnityEvent onClick;
    [SerializeField]
    private GameObject boundingBox;

    private bool isFocused;

    private float downDelta = 0f;
    private bool down = false;

    private void Awake()
    {
        if (boundingBox == null)
        {
            boundingBox = transform.Find("BoundingBox").gameObject;
        }
    }

    public override void OnFocusEnter(FocusEventData eventData)
    {
        base.OnFocusEnter(eventData);
        isFocused = true;
        boundingBox?.SetActive(true);
    }

    public override void OnFocusExit(FocusEventData eventData)
    {
        base.OnFocusExit(eventData);
        isFocused = false;
        boundingBox?.SetActive(false);
    }

    public void SelectWorkspace()
    {
        SceneManager.LoadScene("SelectWorkspace", LoadSceneMode.Single);
    }

    public void GotoSettings()
    {
        SceneManager.LoadScene("Settings", LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Return(string fromScene)
    {
        SceneManager.LoadScene(fromScene, LoadSceneMode.Single);
    }

    public void CreateProject()
    {
        var quad = GameObject.Find("HUD").transform.Find("Quad");
        var keyboard = GameObject.Find("HUD").transform.Find("Keyboard");

        quad.gameObject.SetActive(true);
        keyboard.gameObject.SetActive(true);
        keyboard.GetComponent<Keyboard>().InputTarget = quad.GetComponentInChildren<ParamTypein>().gameObject;
        quad.GetComponentInChildren<ParamTypein>().EnableInput = true;
    }

    public void CreateFile()
    {
        Transform root = GameObject.Find("HUD").transform;
        GameObject keyboard = root.Find("Keyboard").gameObject;
        keyboard.SetActive(true);

        GameObject singleLine = root.Find("SignleLineInput").gameObject;
        singleLine.SetActive(true);
        singleLine.GetComponent<SingleLineInput>().InputComplate = (inputContent) =>
        {
            //FileAndDictionary.Instance.CreateFile(singleLine.GetComponent<SingleLineInput>().GetContent());
            FileAndDirectory.Instance.CreateFile(inputContent);
            singleLine.SetActive(false);
            keyboard.SetActive(false);

            GetComponentInParent<ScanDictionary>().CreateFile();
        };
        keyboard.GetComponent<Keyboard>().InputTarget = singleLine;
    }

    public void CreateFolder()
    {

    }

    public void CreateProjectReality()
    {
        string projName = GameObject.Find("HUD/Quad/SingleLineInput").GetComponentInChildren<ParamTypein>().GetValue();
        string type = GameObject.Find("HUD/Quad/SingleLineSelect").GetComponentInChildren<ParamSelect>().GetValueName();

        if (!FileAndDirectory.Instance.CreateProject(projName))
        {
            return;
        }
        FileAndDirectory.Instance.ProjectName = projName;
        FileAndDirectory.Instance.SaveFile("config.taurus", string.Format("{{ \"{0}\" = \"{1}\"}}", "type", type));

        CancelCreate();
    }

    public void CancelCreate()
    {
        transform.parent.parent.Find("Keyboard").gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
    }


    public void Close()
    {
        SaveFile();
        GameObject.Find("Editor").GetComponent<EditorManager>().Remove(gameObject.transform.parent.gameObject);
    }

    public void SaveFile()
    {
        transform.parent.GetComponentInChildren<TypeIn>().SaveFile();
    }

    public void PageUpClick()
    {
        transform.parent.GetComponentInChildren<MyInputField>().PageUp();
    }

    public void PageDownClick()
    {
        transform.parent.GetComponentInChildren<MyInputField>().PageDown();
    }

    public void SaveParam()
    {
        GetComponentInParent<Inspector>().Save();
    }

    public void SaveBrainScript()
    {
        FileAndDirectory.Instance.SaveBrainScript(transform.parent.GetComponentInChildren<TMPro.TextMeshPro>().text);
    }

    public void Cancel()
    {
        GameObject.Find("ParamInspector").SetActive(false);
    }

    public void Delete()
    {
        GetComponentInParent<Inspector>().Delete();
    }

    private void Update()
    {
        if (isFocused && (Input.GetKeyUp(KeyCode.Return) || Input.GetMouseButtonDown(0)))
        {
            onClick.Invoke();
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

    public void OnPointerClicked()
    {
        Debug.Log("Clicked");
        onClick.Invoke();
    }

    protected virtual IEnumerator Await(AsyncOperation aop, System.Action complated)
    {
        while (!aop.isDone)
        {
            yield return null;
        }
        complated?.Invoke();
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
