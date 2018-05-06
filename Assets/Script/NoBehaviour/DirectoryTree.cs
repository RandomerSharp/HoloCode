using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DirectoryTree
{
    private List<GameObject> folderList;
    private List<GameObject> fileList;
    private bool extended;

    private Transform parentTrans;
    private string curAbsPath;
    public string relativePath;
    private List<DirectoryTree> children;

    public bool Extended
    {
        get
        {
            return extended;
        }

        set
        {
            extended = value;
        }
    }
    public int ItemCount
    {
        get
        {
            int r = 0;
            foreach (var item in folderList)
            {
                if (item.activeSelf) r++;
            }
            foreach (var item in fileList)
            {
                if (item.activeSelf) r++;
            }
            foreach (var item in children)
            {
                r += item.ItemCount;
            }
            return r;
        }
    }

    public DirectoryTree(string absPath, Transform parent)
    {
        curAbsPath = absPath;
        relativePath = absPath.Split('\\').LastOrDefault();
        parentTrans = parent;
        folderList = new List<GameObject>();
        fileList = new List<GameObject>();
        children = new List<DirectoryTree>();
        extended = false;
    }

    public void ScanFolder(GameObject folderPrefab, GameObject filePrefab)
    {
        extended = false;

        var folders = FileAndDictionary.Instance.GetFoldersInFolder(curAbsPath);
        var files = FileAndDictionary.Instance.GetFilesInFolder(curAbsPath);
        for (int i = 0; i < folders.Length; i++)
        {
            var l = folders[i].Remove(0, curAbsPath.Length).Substring(1);
            if (l.Contains(".meta")) continue;

            if ((from item in folderList
                 where item.name == l
                 select item).Count() > 0) continue;

            var o = GameObject.Instantiate(folderPrefab);
            o.transform.parent = parentTrans;
            o.name = l;
            o.GetComponentInChildren<TextMesh>().text = l;
            o.SetActive(false);

            DirectoryTree ch = new DirectoryTree(System.IO.Path.Combine(curAbsPath, l), parentTrans);
            ch.ScanFolder(folderPrefab, filePrefab);
            //ScanFolder(System.IO.Path.Combine(path, folders[i]), folderPrefab, filePrefab);
            children.Add(ch);

            folderList.Add(o);
        }

        for (int i = 0; i < files.Length; i++)
        {
            var l = files[i].Remove(0, curAbsPath.Length).Substring(1);
            if (l.Contains(".meta")) continue;

            if ((from item in fileList
                 where item.name == l
                 select item).Count() > 0) continue;

            var o = GameObject.Instantiate(filePrefab);
            o.name = l;
            o.GetComponentInChildren<TextMesh>().text = l;
            o.SetActive(false);

            fileList.Add(o);
        }

        folderList.Sort((GameObject a, GameObject b) => { return a.name.CompareTo(b.name); });
        fileList.Sort((GameObject a, GameObject b) => { return a.name.CompareTo(b.name); });
        children.Sort((DirectoryTree a, DirectoryTree b) => { return a.relativePath.CompareTo(b.relativePath); });
    }

    public void Extend()
    {
        if (extended == false)
        {
            Vector3 par = parentTrans.localPosition;
            par.y = 0;
            par.z = 0;
            par.x += 0.5f;

            for (int i = 0; i < folderList.Count; i++)
            {
                folderList[i].SetActive(true);

                folderList[i].transform.parent = parentTrans;
                folderList[i].transform.localPosition = Vector3.down * i + par;
                folderList[i].transform.localRotation = Quaternion.identity;
                folderList[i].transform.localScale = Vector3.one;
            }
            for (int i = 0; i < fileList.Count; i++)
            {
                fileList[i].SetActive(true);

                fileList[i].transform.parent = parentTrans;
                fileList[i].transform.localPosition = Vector3.down * (folderList.Count + i) + par;
                fileList[i].transform.localRotation = Quaternion.identity;
                fileList[i].transform.localScale = Vector3.one;
            }
            extended = true;

            for (int i = 0; i < parentTrans.childCount; i++)
            {

            }
        }
        else
        {
            Collapse();
        }
    }

    private void Collapse()
    {
        foreach (var item in children)
        {
            item.Collapse();
        }
        foreach (var item in folderList)
        {
            item.SetActive(false);
        }
        foreach (var item in fileList)
        {
            item.SetActive(false);
        }
        extended = false;
    }

    public DirectoryTree Find(string name)
    {
        foreach (var item in children)
        {
            if (item.relativePath == name)
            {
                return item;
            }
        }
        return null;
    }
}
