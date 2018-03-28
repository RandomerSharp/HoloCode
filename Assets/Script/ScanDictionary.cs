using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Folder = System.IO.Directory;
#if UNITY_WSA && NETFX_CORE 
using Windows.Storage;
#endif

public class ScanDictionary : MonoBehaviour
{
    private string workspacePath;
    [SerializeField] private GameObject folderObj;
    [SerializeField] private GameObject fileObj;

    private int folderCount = 0;
    private int fileCount = 0;
    private List<GameObject> folderList;
    private List<GameObject> fileList;

    public string WorkspacePath
    {
        get
        {
            return workspacePath;
        }
    }
    public int ItemCount
    {
        get
        {
            return folderCount + fileCount;
        }
    }

    private void Awake()
    {
        //workspacePath = GameObject.Find("DataCache").GetComponent<DataCache>().Find("WorkspaceName");
        var dataPath = Application.dataPath;
#if UNITY_WSA && NETFX_CORE 
        dataPath = KnownFolders.DocumentsLibrary.Path;
#endif
        workspacePath = "Demo";
        folderList = new List<GameObject>();
        fileList = new List<GameObject>();
        GengerateDictionaryTree(dataPath + "/Workspace/" + workspacePath);
    }

    private void GengerateDictionaryTree(string path)
    {
        var folders = Folder.GetDirectories(path);
        for (int i = 0; i < folders.Length; i++)
        {
            var l = folders[i].Remove(0, path.Length);
            l = l.Substring(1);
            if (l.Substring(l.Length - 5) == ".meta") continue;
            //Debug.Log(i);
            var o = Instantiate(folderObj);
            o.transform.parent = transform;
            o.transform.localPosition = Vector3.down * folderCount;
            o.transform.localRotation = Quaternion.identity;
            o.transform.localScale = Vector3.one;
            o.GetComponentInChildren<TextMesh>().text = l;

            folderCount++;
            folderList.Add(o);
        }
        var files = Folder.GetFiles(path);
        for (int i = 0; i < files.Length; i++)
        {
            var l = files[i].Remove(0, path.Length);
            l = l.Substring(1);
            if (l.Substring(l.Length - 5) == ".meta") continue;
            //Debug.Log(i);
            var o = Instantiate(fileObj);
            o.transform.parent = transform;
            o.transform.localPosition = Vector3.down * (folderCount + fileCount);
            o.transform.localRotation = Quaternion.identity;
            o.transform.localScale = Vector3.one;
            o.GetComponentInChildren<TextMesh>().text = l;

            fileCount++;
            fileList.Add(o);
        }
    }

    public void OpenFolder()
    {

    }
}
