using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Folder = System.IO.Directory;

public class ScanWorkspace : MonoBehaviour
{
    private string workspacePath;
    [SerializeField] private GameObject folderObj;
    [SerializeField] private GameObject fileObj;

    private void Awake()
    {
        var folders = Folder.GetDirectories(Application.dataPath);
        foreach (var f in folders)
        {
            if (f == "Workspace") goto LLL;
        }
        Folder.CreateDirectory(Application.dataPath + "/Workspace");
        LLL:
        workspacePath = Application.dataPath + "/Workspace";
        GengerateDictionaryTree(workspacePath);
    }

    private void GengerateDictionaryTree(string path)
    {
        var folders = Folder.GetDirectories(workspacePath);
        for (int i = 0; i < folders.Length; i++)
        {
            var l = folders[i].Remove(0, path.Length);
            l = l.Substring(1);
            //Debug.Log(i);
            var o = Instantiate(folderObj);
            o.transform.parent = transform;
            o.transform.localPosition = Vector3.down * i;
            o.transform.localRotation = Quaternion.identity;
            o.transform.localScale = Vector3.one;

            o.GetComponentInChildren<TextMesh>().text = l;
        }
    }
}
