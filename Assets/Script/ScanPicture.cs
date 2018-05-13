using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanPicture : MonoBehaviour
{
    [SerializeField]
    private GameObject itemPrefab;

    private List<GameObject> m_list;
    private bool m_started = false;
    // Use this for initialization
    void Start()
    {
        m_started = true;
        if (m_list == null || m_list.Count == 0)
        {
            var pictures = FileAndDirectory.Instance.GetFilesInFolder(FileAndDirectory.Instance.PicturePath);
            for (int i = 0; i < pictures.Length; i++)
            {
                string fileName = System.IO.Path.GetFileName(pictures[i]);
                var obj = Instantiate(itemPrefab) as GameObject;
                obj.transform.parent = transform.Find("Pictures");
                obj.transform.localPosition = new Vector3(0, i * -0.075f, -0.05f);
                obj.transform.localRotation = Quaternion.identity;
                obj.transform.localScale = new Vector3(0.5f, 0.06f, 0.5f);

                obj.name = fileName;
                obj.GetComponentInChildren<TextMesh>().text = fileName;
            }
        }
    }

    private void OnEnable()
    {
        if (!m_started) return;
        if (m_list == null || m_list.Count == 0)
        {
            var pictures = FileAndDirectory.Instance.GetFilesInFolder(FileAndDirectory.Instance.PicturePath);
            for (int i = 0; i < pictures.Length; i++)
            {
                string fileName = System.IO.Path.GetFileName(pictures[i]);
                var obj = Instantiate(itemPrefab) as GameObject;
                obj.transform.parent = transform.Find("Pictures");
                obj.transform.localPosition = new Vector3(0, i * -0.075f, -0.05f);
                obj.transform.localRotation = Quaternion.identity;
                obj.transform.localScale = new Vector3(0.5f, 0.06f, 0.5f);

                obj.name = fileName;
                obj.GetComponentInChildren<TextMesh>().text = fileName;
            }
        }
    }
}
