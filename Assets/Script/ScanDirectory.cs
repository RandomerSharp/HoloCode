using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_WSA && NETFX_CORE 
using Windows.Storage;
#endif

public class ScanDirectory : MonoBehaviour
{
    [SerializeField] private GameObject folderObj;
    [SerializeField] private GameObject fileObj;

    private int folderCount = 0;
    private int fileCount = 0;
    //private List<GameObject> folderList;
    //private List<GameObject> fileList;
    private DirectoryTree dt;

    public int ItemCount
    {
        get
        {
            return folderCount + fileCount;
        }
    }

    private void Start()
    {
        //workspacePath = GameObject.Find("DataCache").GetComponent<DataCache>().Find("WorkspaceName");
        //var dataPath = FileAndDirectory.Instance.WorkspacePath;
        //string workspacePath = "Demo";
        //folderList = new List<GameObject>();
        //fileList = new List<GameObject>();
        FileAndDirectory.Instance.ProjectName = "Flower";
        //UpdateDictionaryTree(FileAndDictionary.Instance.FolderPath, transform);
        dt = new DirectoryTree(FileAndDirectory.Instance.FolderPath, transform);
        dt.ScanFolder(folderObj, fileObj);
        dt.Extend();
    }

    public void CreateFile()
    {
        //UpdateDictionaryTree(FileAndDictionary.Instance.FolderPath, transform);
        dt.ScanFolder(folderObj, fileObj);
    }

    public void OpenFolder(GameObject obj)
    {
        List<string> path = new List<string>
        {
            obj.name
        };
        Transform t = obj.transform;
        t = t.parent;
        while (t != transform)
        {
            path.Add(t.name);
            t = t.parent;
        }
        DirectoryTree dtt = dt;
        for (int i = path.Count - 1; i >= 0; i--)
        {
            dtt = dtt.Find(path[i]);
        }
        dtt.Extend();
    }
}
