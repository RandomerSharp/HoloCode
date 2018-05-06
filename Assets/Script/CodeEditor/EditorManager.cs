using MixedRealityToolkit.Common;
using MixedRealityToolkit.UX.Collections;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class EditorManager : MonoBehaviour
{
    private GameObject workspacePanel;
    private bool workspacePanelActive;
    [SerializeField]
    private GameObject codeTab;
    private Vector3 center;
    //public float radius;

    private LinkedList<GameObject> tabsList;

    private string fileName;

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
    public Vector3 Center
    {
        get
        {
            return center;
        }

        set
        {
            center = value;
        }
    }

    private void Awake()
    {
        workspacePanel = GameObject.Find("DictionaryTree");
        workspacePanelActive = true;
        tabsList = new LinkedList<GameObject>();
        tabsList.AddFirst(GameObject.Find("DictionaryTree"));
    }

    void Update()
    {
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.Tab))
        {
        }
    }

    public void Create(string fileName)
    {
        var opened = (from i in tabsList
                      where i.GetComponent<CodeTabManager>() != null && i.GetComponent<CodeTabManager>().FileName == fileName
                      select i).ToArray();
        if (opened.Length > 0)
        {
            return;
        }
        var c = Instantiate(codeTab);
        c.name = fileName;
        c.transform.parent = transform;
        c.transform.localPosition = Vector3.down;
        c.transform.localRotation = Quaternion.identity;
        c.transform.localScale = Vector3.one;
        //c.transform.Find("Title").Find("Text1").GetComponent<TextMesh>().text = fileName;
        c.GetComponent<CodeTabManager>().FileName = fileName;
        c.GetComponent<CodeTabManager>().Path = FileAndDictionary.Instance.FolderPath;//path;

        //using (Stream stream = new FileStream(path + '\\' + fileName, FileMode.Open))
        //{
        //using (StreamReader reader = new StreamReader(stream))
        //{
        string code = FileAndDictionary.Instance.OpenFile(fileName);//reader.ReadToEnd();
        //c.GetComponentInChildren<TMPro.TextMeshPro>().text = code;
        c.GetComponentInChildren<MyInputField>().SetText(code);
        //}
        //}
        //GameObject.Find("DictionaryTree").GetComponent<SphereBasedTagalong>().enabled = false;
        transform.position = new Vector3(CameraCache.Main.transform.position.x, transform.position.y, CameraCache.Main.transform.position.z);

        tabsList.AddLast(c);
        GetComponent<ObjectCollection>().UpdateCollection();
        //SelectAndInsert(c);
    }

    public void Remove(GameObject obj)
    {
        obj.transform.parent = null;
        tabsList.Remove(obj);
        Destroy(obj);
        GetComponent<ObjectCollection>().UpdateCollection();
    }


    private void OnDestroy()
    {
        foreach (var item in tabsList)
        {
            DestroyImmediate(item);
        }
        tabsList.Clear();
        //DestroyImmediate(gameObject);
    }
}
