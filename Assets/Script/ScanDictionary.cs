using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_WSA && NETFX_CORE 
using Windows.Storage;
#endif

public class ScanDictionary : MonoBehaviour
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

    private void Awake()
    {
        //workspacePath = GameObject.Find("DataCache").GetComponent<DataCache>().Find("WorkspaceName");
        var dataPath = FileAndDictionary.Instance.RootPath;
        string workspacePath = "Demo";
        //folderList = new List<GameObject>();
        //fileList = new List<GameObject>();
        FileAndDictionary.Instance.WorkspacePath = workspacePath;
        //UpdateDictionaryTree(FileAndDictionary.Instance.FolderPath, transform);
        dt = new DirectoryTree(FileAndDictionary.Instance.FolderPath, transform);
        dt.ScanFolder(folderObj, fileObj);
        dt.Extend();
    }
    /*
    private void GengerateDictionaryTree()
    {
        string path = FileAndDictionary.Instance.FolderPath;
        var folders = FileAndDictionary.Instance.GetFoldersInFolder(path);
        for (int i = 0; i < folders.Length; i++)
        {
            var l = folders[i].Remove(0, path.Length);
            l = l.Substring(1);
            if (l.Substring(l.Length - 5) == ".meta") continue;
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
        var files = FileAndDictionary.Instance.GetFilesInFolder(path);
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

    public void UpdateDictionaryTree(string path, Transform parent)
    {
        Debug.Log(path);

        var folders = FileAndDictionary.Instance.GetFoldersInFolder(path);
        var files = FileAndDictionary.Instance.GetFilesInFolder(path);

        for (int i = 0; i < folders.Length; i++)
        {
            var l = folders[i].Remove(0, path.Length).Substring(1);
            if (l.Contains(".meta")) continue;

            if ((from item in folderList
                 where item.name == l
                 select item).Count() > 0) continue;

            var o = Instantiate(folderObj);
            o.transform.parent = transform;
            //o.transform.localPosition = Vector3.down * folderCount;
            o.transform.localRotation = Quaternion.identity;
            o.transform.localScale = Vector3.one;
            o.GetComponentInChildren<TextMesh>().text = l;
            o.name = l;

            UpdateDictionaryTree(System.IO.Path.Combine(path, l), o.transform);

            folderCount++;
            folderList.Add(o);
        }

        for (int i = 0; i < files.Length; i++)
        {
            var l = files[i].Remove(0, path.Length).Substring(1);
            if (l.Contains(".meta")) continue;

            if ((from item in fileList
                 where item.name == l
                 select item).Count() > 0) continue;

            var o = Instantiate(fileObj);
            o.name = l;
            o.transform.parent = transform;
            //o.transform.localPosition = Vector3.down * (folderCount + fileCount);
            o.transform.localRotation = Quaternion.identity;
            o.transform.localScale = Vector3.one;
            o.GetComponentInChildren<TextMesh>().text = l;

            fileCount++;
            fileList.Add(o);

            //if ((folderCount + fileCount) % 5 == 0) transform.position = transform.position + Vector3.up * 2;
        }

        folderList.Sort((GameObject a, GameObject b) => { return a.name.CompareTo(b.name); });
        fileList.Sort((GameObject a, GameObject b) => { return a.name.CompareTo(b.name); });

        Vector3 par = parent.transform.localPosition;
        par.y = 0;
        par.z = 0;
        par.x += 0.5f;
        for (int i = 0; i < folderList.Count; i++)
        {
            folderList[i].transform.localPosition = Vector3.down * i + par;
        }
        for (int i = 0; i < fileList.Count; i++)
        {
            fileList[i].transform.localPosition = Vector3.down * (folderCount + i) + par;
        }
    }
    */

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
            Debug.Log(path[i]);
            dtt = dtt.Find(path[i]);
        }
        dtt.Extend();
    }
}
