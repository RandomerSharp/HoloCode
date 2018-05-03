using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Folder = System.IO.Directory;
#if UNITY_WSA && NETFX_CORE 
using Windows.Storage;
#endif

public class ScanDictionary : MonoBehaviour
{
    [SerializeField] private GameObject folderObj;
    [SerializeField] private GameObject fileObj;

    private int folderCount = 0;
    private int fileCount = 0;
    private List<GameObject> folderList;
    private List<GameObject> fileList;

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
        var dataPath = FileAndDictionary.Instance.RootPath;
        string workspacePath = "Demo";
        folderList = new List<GameObject>();
        fileList = new List<GameObject>();
        FileAndDictionary.Instance.WorkspacePath = workspacePath;
        GengerateDictionaryTree(System.IO.Path.Combine(dataPath, "Workspace", workspacePath));
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
            o.name = l;

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
            o.name = l;
            o.transform.parent = transform;
            o.transform.localPosition = Vector3.down * (folderCount + fileCount);
            o.transform.localRotation = Quaternion.identity;
            o.transform.localScale = Vector3.one;
            o.GetComponentInChildren<TextMesh>().text = l;

            fileCount++;
            fileList.Add(o);

            if ((folderCount + fileCount) % 5 == 0) transform.position = transform.position + Vector3.up * 2;
        }
    }

    public void UpdateDictionaryTree()
    {
        string path = FileAndDictionary.Instance.WorkspacePath;

        var files = Folder.GetFiles(path);
        for (int i = 0; i < files.Length; i++)
        {
            var l = files[i].Remove(0, path.Length);
            l = l.Substring(1);
            if (l.Substring(l.Length - 5) == ".meta") continue;
            //Debug.Log(i);

            if ((from item in fileList
                 where item.name == l
                 select item).Count() > 0) continue;

            var o = Instantiate(fileObj);
            o.name = l;
            o.transform.parent = transform;
            o.transform.localPosition = Vector3.down * (folderCount + fileCount);
            o.transform.localRotation = Quaternion.identity;
            o.transform.localScale = Vector3.one;
            o.GetComponentInChildren<TextMesh>().text = l;

            fileCount++;
            fileList.Add(o);

            if ((folderCount + fileCount) % 5 == 0) transform.position = transform.position + Vector3.up * 2;
        }
    }

    public void OpenFolder()
    {

    }
}
