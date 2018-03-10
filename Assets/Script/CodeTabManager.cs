using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeTabManager : MonoBehaviour
{
    private string fileName;
    private string path;
    private RenderTexture rt;

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


    void Start()
    {
        transform.Find("Title/Text1").GetComponent<TextMesh>().text = fileName;
        rt = RenderTexture.GetTemporary(1440, 1440);
        GetComponentInChildren<Camera>().targetTexture = rt;
        transform.Find("Input").GetComponent<MeshRenderer>().material.mainTexture = rt;
    }

    private void OnDestroy()
    {
        RenderTexture.ReleaseTemporary(rt);
    }
}
