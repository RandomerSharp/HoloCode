using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeTabManager : MonoBehaviour
{
    //private RenderTexture rt;
    private string path;
    private string fileName;
    //private float width;

    public string FileName
    {
        get
        {
            return fileName;
        }

        set
        {
            fileName = value;
        }
    }
    public string Path
    {
        get
        {
            return path;
        }

        set
        {
            path = value;
        }
    }
    /*public float Width
    {
        get
        {
            return width;
        }
    }*/

    void Awake()
    {
        StartCoroutine(SetTitle());
    }

    private IEnumerator SetTitle()
    {
        yield return null;
        transform.Find("Title/Text1").GetComponent<TextMesh>().text = fileName;
    }
}
