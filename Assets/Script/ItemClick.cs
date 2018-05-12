using MixedRealityToolkit.InputModule.EventData;
using MixedRealityToolkit.InputModule.InputHandlers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ItemClick : MonoBehaviour, IPointerHandler
{
    [SerializeField]
    protected UnityEvent onClick;

    private bool isFocused = false;

    /*public virtual void OnInputClicked(InputClickedEventData eventData)
    {
        onClick.Invoke();
        //MixedRealityToolkit.InputModule.EventData.ClickEventData
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
        //var as1 = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        //while (!as1.isDone) ;
        Application.Quit();
    }

    public void Return(string fromScene)
    {
        //var as1 = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        //while (!as1.isDone) ;
        SceneManager.LoadScene(fromScene, LoadSceneMode.Single);
    }

    public void GenerateBS()
    {
        Debug.Log("Generate");
        FindObjectsOfType<MenuWheelSelector>()[0].Generate();
    }

    public void Close()
    {
        SaveFile();
        GameObject.Find("Editor").GetComponent<EditorManager>().Remove(gameObject.transform.parent.gameObject);
    }

    /*public void Save(string ty = "BS")
    {
        var dataPath = Application.dataPath;

        var ctm = transform.parent.gameObject.GetComponent<CodeTabManager>();
        Debug.Log(ctm == null);
        string path;
        if (ty != "BS") path = ctm.Path;
        else path = dataPath + "/Workspace/BrainScript";
        string fileName = ctm.FileName;
        if (fileName == null || fileName.Length == 0) fileName = System.DateTime.Now.Ticks.ToString() + ".bs";
        using (Stream stream = new FileStream(path + '\\' + fileName, FileMode.OpenOrCreate, FileAccess.Write))
        {
            using (StreamWriter sw = new StreamWriter(stream))
            {
                sw.Write(transform.parent.GetComponentInChildren<TMPro.TextMeshPro>().text);
            }
        }
    }*/

    public void SaveFile()
    {
        transform.parent.GetComponentInChildren<TypeIn>().SaveFile();
    }

    public void SaveBrainScript()
    {
        FileAndDirectory.Instance.SaveBrainScript(transform.parent.GetComponentInChildren<TMPro.TextMeshPro>().text);
    }

    public void SaveParam()
    {
        GetComponentInParent<Inspector>().Save();
    }

    public void Cancel()
    {
        GameObject.Find("ParamInspector").SetActive(false);
    }

    public void Delete()
    {
        GetComponentInParent<Inspector>().Delete();
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

    public void NextParam()
    {
        GetComponentInParent<ParamSelect>().Next();
    }

    public void LastParam()
    {
        GetComponentInParent<ParamSelect>().Last();
    }

    public void OnPointerUp(ClickEventData eventData) { }

    public void OnPointerDown(ClickEventData eventData) { }

    public void OnPointerClicked(ClickEventData eventData)
    {
        onClick.Invoke();
        eventData.Use();
    }
}
