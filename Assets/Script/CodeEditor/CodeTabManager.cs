using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeTabManager : MonoBehaviour
{
    //private RenderTexture rt;
    private string path;
    private string fileName;
    private float width;
    private float angle;

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
    public float Width
    {
        get
        {
            return width;
        }
    }
    /*public float Angle
    {
        get
        {
            return angle;
        }
        set
        {
            angle = value;
            UpdatePosition();
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

    /*private void UpdatePosition()
    {
        //float r = transform.parent.GetComponent<EditorManager>().radius;
        float x = r * Mathf.Cos(angle);
        float z = r * Mathf.Sin(angle);
        float y = 0f;
        transform.localPosition = new Vector3(x, y, z);
    }*/
}
