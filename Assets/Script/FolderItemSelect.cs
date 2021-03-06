﻿using MixedRealityToolkit.InputModule.EventData;
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
    public int ChildCount
    {
        get
        {
            int r = transform.childCount;
            for (int i = 0; i < transform.childCount; i++)
            {
                r += transform.GetChild(i).GetComponent<FolderItemSelect>().ChildCount;
            }
            return r;
        }
    }

    public override void OnFocusEnter(FocusEventData eventData)
    {
        base.OnFocusEnter(eventData);

        GetComponentInChildren<CubeRotate>().RotateSpeed = 3f;
    }

    public override void OnFocusExit(FocusEventData eventData)
    {
        base.OnFocusExit(eventData);

        GetComponentInChildren<CubeRotate>().RotateSpeed = 0f;
    }

    public void OpenFolder()
    {
        GetComponentInParent<ScanDictionary>().OpenFolder(gameObject);
    }

    public void OpenFile()
    {
        //GameObject.Find("Editor").GetComponent<EditorManager>().Create(FileAndDictionary.Instance.FolderPath, name);
        string fileName = gameObject.name;
        Transform t = transform;
        while (t.parent.name != "DictionaryTree")
        {
            t = t.parent;
            Debug.Log(t.name);
            fileName = System.IO.Path.Combine(t.name, fileName);
        }
        GameObject.Find("Editor").GetComponent<EditorManager>().Create(fileName);
    }

    public void OpenProject()
    {
        string proj = gameObject.name;
        FileAndDirectory.Instance.ProjectName = proj;
        var trans = GameObject.Find("HUD").transform.Find("RotatingOrbs");
        trans.gameObject.SetActive(true);
        StartCoroutine(Await(SceneManager.LoadSceneAsync("Editor", LoadSceneMode.Single), () =>
        {
            GameObject.Find("HUD").transform.Find("RotatingOrbs")?.gameObject.gameObject.SetActive(false);
        }));
    }
}
