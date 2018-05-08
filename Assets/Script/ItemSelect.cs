using MixedRealityToolkit.InputModule.EventData;
using MixedRealityToolkit.InputModule.Focus;
using MixedRealityToolkit.InputModule.InputHandlers;
using System.Collections;
using System.Collections.Generic;
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
        foreach (var child in GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = LayerMask.NameToLayer("Highlight");
            //Debug.Log(child.name);
        }
        isFocused = true;
        boundingBox?.SetActive(true);
        //if (onFocusEnter != null) onFocusEnter.Invoke();
    }

    public override void OnFocusExit(FocusEventData eventData)
    {
        isFocused = false;
        foreach (var child in GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = LayerMask.NameToLayer("Default");
        }
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
            FileAndDictionary.Instance.CreateFile(inputContent);
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
        string projName = GameObject.Find("HUD/Quad/SingleInput").GetComponentInChildren<TextMesh>().text;
        string type = GameObject.Find("HUD/Quad/SingleSelect").GetComponentInChildren<TextMesh>().text;

        if (!FileAndDictionary.Instance.CreateProject(projName))
        {
            return;
        }
        FileAndDictionary.Instance.ProjectName = projName;
        FileAndDictionary.Instance.SaveFile(".taurus", string.Format("{{ \"{0}\" = {1}}}", "type", type));
    }

    public void CancelCreate()
    {
        transform.parent.parent.Find("Keyboard").gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
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
}
