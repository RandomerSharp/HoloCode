using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_WSA && NETFX_CORE 
using Windows.Storage;
#endif

using Folder = System.IO.Directory;

public class ScanWorkspace : MonoBehaviour
{
    private string workspacePath;
    [SerializeField] private GameObject folderObj;
    [SerializeField] private GameObject fileObj;

    private void Awake()
    {
        var dataPath = Application.dataPath;
#if UNITY_WSA && NETFX_CORE 
        dataPath = KnownFolders.DocumentsLibrary.Path;
#endif
        var folders = Folder.GetDirectories(dataPath);
        foreach (var f in folders)
        {
            if (f == "Workspace") goto LLL;
        }
        Folder.CreateDirectory(dataPath + "/Workspace");
        LLL:
        workspacePath = dataPath + "/Workspace";
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
