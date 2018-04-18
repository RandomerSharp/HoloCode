using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
#if UNITY_WSA && NETFX_CORE 
using Windows.Storage;
#endif

public class ItemClick : MonoBehaviour, IInputClickHandler
{
    [SerializeField]
    protected UnityEvent onClick;

    private bool isFocused;

    public virtual void OnInputClicked(InputClickedEventData eventData)
    {
        onClick.Invoke();
    }

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
        Save();
        GameObject.Find("Editor").GetComponent<EditorManager>().Remove(gameObject.transform.parent.gameObject);
    }

    public void Save(string ty = "BS")
    {
        var dataPath = Application.dataPath;
#if UNITY_WSA && NETFX_CORE 
        dataPath = KnownFolders.DocumentsLibrary.Path;
#endif

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
    }

    public void SaveBrainScript()
    {
        FileAndDictionary.Instance.SaveBrainScript(transform.parent.GetComponentInChildren<TMPro.TextMeshPro>().text);
    }

    public void SaveParam()
    {

    }

    public void Cancel()
    {
        GameObject.Find("ParamInspector").SetActive(false);
    }

    public void Delete()
    {
    }

    private void Update()
    {
        if (isFocused && (Input.GetKeyUp(KeyCode.Return) || Input.GetMouseButtonDown(0)))
        {
            onClick.Invoke();
        }
    }

}
