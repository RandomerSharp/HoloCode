using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
#if UNITY_WSA && NETFX_CORE 
using Windows.Storage;
#endif

public class FolderItemSelect : ItemSelect
{
    public override void OnFocusEnter()
    {
        base.OnFocusEnter();

        GetComponentInChildren<CubeRotate>().RotateSpeed = 3f;
    }

    public override void OnFocusExit()
    {
        base.OnFocusExit();

        GetComponentInChildren<CubeRotate>().RotateSpeed = 0f;
    }

    public override void OnInputClicked(InputClickedEventData eventData)
    {
        base.OnInputClicked(eventData);
    }

    public void OpenFolder()
    {
    }

    public void OpenFile()
    {
        var dataPath = Application.dataPath;
#if UNITY_WSA && NETFX_CORE 
        dataPath = KnownFolders.DocumentsLibrary.Path;
#endif
        GameObject.Find("Editor").GetComponent<EditorManager>().Create(dataPath + "/Workspace/" + GameObject.Find("DictionaryTree").GetComponent<ScanDictionary>().WorkspacePath, GetComponentInChildren<TextMesh>().text);
    }

    public void OpenWorkspace()
    {
        string workspaceName = GetComponentInChildren<TextMesh>().text;
        GameObject.Find("DataCache").GetComponent<DataCache>().Add("WorkspaceName", workspaceName);
        SceneManager.LoadScene("Editor");
    }
}
