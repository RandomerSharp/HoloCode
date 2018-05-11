using MixedRealityToolkit.InputModule.EventData;
using MixedRealityToolkit.InputModule.Focus;
using MixedRealityToolkit.InputModule.InputHandlers;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ItemSelect : FocusTarget, IPointerHandler//, IInputClickHandler, IFocusable
{
    [SerializeField]
    protected UnityEvent onClick;
    [SerializeField]
    private GameObject boundingBox;

    private bool isFocused;

    public override void OnFocusEnter(FocusEventData eventData)
    {
        base.OnFocusEnter(eventData);
        /*foreach (var child in GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = LayerMask.NameToLayer("Highlight");
            //Debug.Log(child.name);
        }*/
        isFocused = true;
        boundingBox?.SetActive(true);
        //if (onFocusEnter != null) onFocusEnter.Invoke();
    }

    public override void OnFocusExit(FocusEventData eventData)
    {
        base.OnFocusExit(eventData);
        isFocused = false;
        /*foreach (var child in GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = LayerMask.NameToLayer("Default");
        }*/
        boundingBox?.SetActive(false);
        //if (onFocusExit != null) onFocusExit.Invoke();
    }

    /*public virtual void OnInputClicked(InputClickedEventData eventData)
    {
        onClick.Invoke();
    }*/

    public void SelectWorkspace()
    {
        //var as1 = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        //while (!as1.isDone) ;
        SceneManager.LoadScene("SelectWorkspace", LoadSceneMode.Single);
    }

    public void GotoSettings()
    {
        //var as1 = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        //while (!as1.isDone) ;
        SceneManager.LoadScene("Settings", LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Return(string fromScene)
    {
        //var as1 = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        //while (!as1.isDone) ;
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

    private void Update()
    {
        if (isFocused && (Input.GetKeyUp(KeyCode.Return) || Input.GetMouseButtonDown(0)))
        {
            onClick.Invoke();
        }
    }

    public void OnPointerUp(ClickEventData eventData) { }

    public void OnPointerDown(ClickEventData eventData) { }

    public void OnPointerClicked(ClickEventData eventData)
    {
        Debug.Log("Clicked");
        onClick.Invoke();
        eventData.Use();
    }

    protected virtual IEnumerator Await(AsyncOperation aop, System.Action complated)
    {
        while (!aop.isDone)
        {
            yield return null;
        }
        complated?.Invoke();
    }
}
