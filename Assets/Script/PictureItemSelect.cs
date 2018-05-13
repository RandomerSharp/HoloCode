using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureItemSelect : ItemSelect
{
    public void OpenPictureFile()
    {
        StartCoroutine(GetTexture());
    }

    private IEnumerator GetTexture()
    {
        string filePath = System.IO.Path.Combine(FileAndDirectory.Instance.PicturePath, name);
        WWW www = new WWW("file://" + filePath);
        yield return www;
        if (www != null && string.IsNullOrEmpty(www.error))
        {
            transform.parent.parent.Find("Texture").GetComponent<MeshRenderer>().material.mainTexture = www.texture;
        }
    }
}
