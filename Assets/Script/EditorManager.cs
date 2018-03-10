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

    private List<GameObject> tabsList;

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

    private void Awake()
    {
        workspacePanel = GameObject.Find("DictionaryTree");
        workspacePanelActive = true;
        tabsList = new List<GameObject>();
    }

    void Update()
    {
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.Tab))
        {
            foreach (Transform g in workspacePanel.transform)
            {
                g.gameObject.SetActive(workspacePanelActive = !workspacePanelActive);
            }
        }
    }

    public void Create(string path, string fileName)
    {
        var opened = (from i in tabsList
                      where i.GetComponent<CodeTabManager>().FileName == fileName
                      select i).ToArray();
        if (opened.Length > 0)
        {
            return;
        }
        var c = Instantiate(codeTab);
        c.name = fileName;
        c.transform.parent = transform;
        c.transform.localPosition = Vector3.zero;
        c.transform.localRotation = Quaternion.identity;
        c.transform.localScale = Vector3.one;
        //c.transform.Find("Title").Find("Text1").GetComponent<TextMesh>().text = fileName;
        c.GetComponent<CodeTabManager>().FileName = fileName;
        c.GetComponent<CodeTabManager>().Path = path;

        using (StreamReader reader = new StreamReader(path + '\\' + fileName))
        {
            string code = reader.ReadToEnd();
            c.transform.Find("UI/InputField").GetComponent<UnityEngine.UI.InputField>().text = code;
            c.transform.Find("UI/InputField").GetComponent<UnityEngine.UI.InputField>().caretPosition = code.Length;
        }

        tabsList.Add(c);
    }
}
