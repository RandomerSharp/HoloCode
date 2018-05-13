using HoloToolkit.Unity.InputModule;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ItemSelect : MonoBehaviour, IInputClickHandler, IFocusable//FocusTarget, IInputHandler//IPointerHandler//, 
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

    /*public override void OnFocusEnter(FocusEventData eventData)
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
    }*/

    public virtual void OnFocusEnter()
    {
        isFocused = true;
        boundingBox?.SetActive(true);
    }

    public virtual void OnFocusExit()
    {
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
        quad.GetComponentInChildren<ParamSelect>().SetType(typeof(ProjectConfig.ProjectTemplate), "Template", 0);
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

            GetComponentInParent<ScanDirectory>().CreateFile();
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
        ProjectConfig.Config a = new ProjectConfig.Config
        {
            type = (ProjectConfig.ProjectTemplate)GameObject.Find("HUD/Quad/SingleLineSelect").GetComponentInChildren<ParamSelect>().GetValue(),
            console = ProjectConfig.Console.Default
        };

        FileAndDirectory.Instance.SaveFile(FileAndDirectory.Instance.FullFilePath("config.taurus"), JsonUtility.ToJson(a, true));

        transform.parent.parent.Find("Keyboard").gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);

        if (a.type == ProjectConfig.ProjectTemplate.NN)
        {
            StartCoroutine(Await(SceneManager.LoadSceneAsync("NNBuild", LoadSceneMode.Single), () =>
            {
                GameObject.Find("HUD").transform.Find("RotatingOrbs")?.gameObject.gameObject.SetActive(false);
            }));
        }
        else if (a.console == ProjectConfig.Console.Image)
        {

        }
        else
        {
            StartCoroutine(Await(SceneManager.LoadSceneAsync("Editor", LoadSceneMode.Single), () =>
            {
                GameObject.Find("HUD").transform.Find("RotatingOrbs")?.gameObject.gameObject.SetActive(false);
            }));
        }
    }

    /*public void CancelCreate()
    {
        transform.parent.parent.Find("Keyboard").gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
    }*/


    public void Close()
    {
        SaveFile();
        GameObject.Find("Editor").GetComponent<EditorManager>().Remove(gameObject.transform.parent.gameObject);
    }

    public void Run()
    {
        GameObject.Find("Editor").GetComponent<EditorManager>().Run();
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

    /*public void SaveParam()
    {
        GetComponentInParent<Inspector>().Save();
    }*/

    public void SaveBrainScript()
    {
        FileAndDirectory.Instance.SaveBrainScript(transform.parent.GetComponentInChildren<TMPro.TextMeshPro>().text);
    }

    /*public void Cancel()
    {
        GameObject.Find("ParamInspector").SetActive(false);
    }*/

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
        float d = 0f;
        while (!aop.isDone)
        {
            yield return null;
            d += Time.deltaTime;
        }
        if (d < 2.9f)
        {
            yield return new WaitForSeconds(3f);
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

    //public void OnInputPressed(InputPressedEventData eventData) { }
    //public void OnInputPositionChanged(InputPositionEventData eventData) { }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        onClick.Invoke();
    }
}
