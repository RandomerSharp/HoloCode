using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inspector : MonoBehaviour
{
    private List<Transform> list;
    private GameObject targetObject;
    private string targetName; // 正在被设置的节点名称
    public GameObject paramInput;
    public GameObject paramSelect;
    public Action OnSave;

    public string TargetName
    {
        get
        {
            return targetName;
        }

        set
        {
            targetName = value;
        }
    }

    public GameObject TargetObject
    {
        get
        {
            return targetObject;
        }

        set
        {
            targetObject = value;
        }
    }

    private void Awake()
    {
        list = new List<Transform>();
    }

    private void OnEnable()
    {
        if (list == null) list = new List<Transform>();
        else list.Clear();
    }

    public void Add(Transform newTrans)
    {
        newTrans.parent = transform.Find("Quad/Collection");
        newTrans.localPosition = new Vector3(0, 0.325f - list.Count * 0.1f, 0);
        newTrans.localRotation = Quaternion.identity;
        newTrans.localScale = new Vector3(0.9f, 0.1f, 1);
        list.Add(newTrans);
    }

    public void Save()
    {
        OnSave?.Invoke();
        gameObject.SetActive(false);
    }

    public void Delete()
    {
        Destroy(targetObject);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        foreach (var item in list)
        {
            item.parent = null;
            Destroy(item.gameObject);
        }
        list.Clear();
        OnSave = null;
    }
}
