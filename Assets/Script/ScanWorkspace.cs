using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanWorkspace : MonoBehaviour
{
    private string workspacePath;
    [SerializeField] private GameObject folderObj;

    private void Awake()
    {
        var folders = FileAndDictionary.Instance.GetFoldersInFolder(FileAndDictionary.Instance.RootPath);
        for (int i = 0; i < folders.Length; i++)
        {
            string l = folders[i].Substring(FileAndDictionary.Instance.RootPath.Length + 1);
            var o = Instantiate(folderObj);
            o.transform.parent = transform;
            o.transform.localPosition = Vector3.down * i;
            o.transform.localRotation = Quaternion.identity;
            o.transform.localScale = Vector3.one;
            o.name = l;
            o.GetComponentInChildren<TextMesh>().text = l;
        }
    }
}
