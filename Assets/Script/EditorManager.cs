using HoloToolkit.Unity;
using HoloToolkit.Unity.Collections;
using HoloToolkit.Unity.InputModule;
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
    public float radius;

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

    private void MoveAll()
    {
        var dt = GameObject.Find("DictionaryTree");
        dt.GetComponent<SphereBasedTagalong>().enabled = false;
        dt.GetComponent<MySnapping>().MoveToLeftAndSnap();
    }

    public void Create(string path, string fileName)
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
        c.GetComponent<CodeTabManager>().Path = path;

        using (Stream stream = new FileStream(path + '\\' + fileName, FileMode.Open))
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                string code = reader.ReadToEnd();
                c.GetComponentInChildren<TMPro.TextMeshPro>().text = code;
            }
        }
        GameObject.Find("DictionaryTree").GetComponent<SphereBasedTagalong>().enabled = false;

        tabsList.AddLast(c);
        GetComponent<ObjectCollection>().UpdateCollection();    
        //SelectAndInsert(c);
    }

    private void SelectAndInsert(GameObject newTab)
    {
        float minAngle = 180f;
        Vector3 cameraForward = CameraCache.Main.transform.forward;
        Vector3 planeNormal = Vector3.up;
        Vector3 camera2D = Vector3.ProjectOnPlane(cameraForward, planeNormal);
        Debug.DrawRay(transform.position, camera2D);
        GameObject nearest = null;
        if (GazeManager.Instance.HitObject != null)
        {

            foreach (var item in tabsList)
            {
                Vector3 ray = item.transform.position - center;
                Vector3 ray2D = Vector3.ProjectOnPlane(ray, planeNormal);
                float angle = Vector3.Angle(camera2D, ray2D);
                if (angle < minAngle)
                {
                    nearest = item;
                    minAngle = angle;
                }
            }
            Vector3 ray1 = nearest.transform.position - center;
            Vector3 ray2D1 = Vector3.ProjectOnPlane(ray1, planeNormal);
            Debug.Log(newTab.GetComponent<CodeTabManager>().Width);
            float transAngle = Mathf.Atan(newTab.GetComponent<CodeTabManager>().Width / (2f * radius));
            float deltaAngle = Vector3.Angle(ray2D1, camera2D);
            Debug.Log(deltaAngle);
            if (Vector3.SignedAngle(camera2D, ray2D1, planeNormal) <= 0)
            {
                float f = -1f;
                foreach (var item in tabsList)
                {
                    item.transform.RotateAround(center, planeNormal, f * (transAngle + f * deltaAngle) * Mathf.Rad2Deg);
                    if (item == nearest) f = 1f;
                }
            }
            else
            {
                float f = -1f;
                foreach (var item in tabsList)
                {
                    if (item == nearest) f = 1f;
                    item.transform.RotateAround(center, planeNormal, f * (transAngle - f * deltaAngle) * Mathf.Rad2Deg);
                }
            }
            tabsList.AddAfter(tabsList.Find(nearest), newTab);
        }
        else
        {
        }
    }

    private void AutoSort()
    {
    }

    public void CloseTab(GameObject tab)
    {
        tabsList.Remove(tab);

        float minAngle = 180f;
        Vector3 cameraForward = CameraCache.Main.transform.forward;
        Vector3 planeNormal = Vector3.up;
        Vector3 camera2D = cameraForward - Vector3.Dot(cameraForward, planeNormal) * planeNormal;
        Debug.DrawRay(transform.position, camera2D);
        GameObject nearest = null;

        foreach (var item in tabsList)
        {
            Vector3 ray = item.transform.position - center;
            Vector3 ray2D = ray - Vector3.Dot(ray, planeNormal) * planeNormal;
            float angle = Vector3.Angle(camera2D, ray2D);
            if (Mathf.Abs(angle) < minAngle)
            {
                nearest = item;
                minAngle = Mathf.Abs(angle);
            }
        }
        Vector3 ray1 = nearest.transform.position - center;
        Vector3 ray2D1 = ray1 - Vector3.Dot(ray1, planeNormal) * planeNormal;
        float transAngle = Mathf.Atan(tab.GetComponent<CodeTabManager>().Width / (2 * radius));
    }

    public void ResetCenter(Vector3 newCenter)
    {
        center = newCenter;
    }
}
