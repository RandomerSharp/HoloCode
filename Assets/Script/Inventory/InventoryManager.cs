using MixedRealityToolkit.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private List<GameObject> inventories;
    private int curSelected;
    [SerializeField]
    private GameObject selectedBoundingBox;

    [SerializeField]
    private GameObject line;

    private BaseNode node1;
    private BaseNode node2;

    [SerializeField]
    private GameObject inspector;

    private void Awake()
    {
        inventories = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            var obj = transform.GetChild(i);
            if (obj.name.Contains("One"))
            {
                inventories.Add(obj.gameObject);
            }
        }
        curSelected = 0;
        StartCoroutine(ClearNode());
    }

    public void NodeSelect(BaseNode node)
    {
        if (node == null) return;
        if (node1 == null || node1.gameObject.activeSelf == false)
        {
            node1 = node;
            return;
        }
        node2 = node;
        if (node1 == node2)
        {
            node1 = null;
            node2 = null;
            return;
        }

        if ((from item in FindObjectsOfType<LineUpdate>()
             where item != null && item.TryDestory(node1.transform, node2.transform)
             select item).Count() > 0)
        {
            node1 = null;
            node2 = null;
            return;
        }

        node1.SetNext(node2);
        node2.SetLast(node1);

        var obj = Instantiate(line);
        obj.GetComponent<LineUpdate>().Start = node1.transform;
        obj.GetComponent<LineUpdate>().End = node2.transform;

        node1 = null;
        node2 = null;
    }

    public void MoveToSelectObject(GameObject obj)
    {
        selectedBoundingBox.transform.parent = obj.transform;
        selectedBoundingBox.transform.localPosition = new Vector3(0, 0, -0.01f);
        selectedBoundingBox.transform.localRotation = Quaternion.identity;
        selectedBoundingBox.transform.localScale = new Vector3(3.6f, 3.6f, 1f);
        for (int i = 0; i < inventories.Count; i++)
        {
            if (inventories[i] == obj)
            {
                curSelected = i;
            }
        }
    }

    public void CreateNode(Vector3 pos)
    {
        string inventoryName = inventories[curSelected].GetComponent<InventorySelect>().InventoryName;
        GameObject newNode = Instantiate(Resources.Load(System.IO.Path.Combine("Prefab", "NN", inventoryName))) as GameObject;
        newNode.name = inventoryName;
        //Vector3 forward = (GameObject.Find("DefaultCursor").transform.position - CameraCache.Main.transform.position).normalized;
        newNode.transform.position = pos;
        newNode.transform.rotation = Quaternion.identity;
        newNode.transform.localScale = Vector3.one;
        newNode.layer = 0;
    }

    public void OpenInspector(BaseNode node)
    {
        inspector.SetActive(true);
        var inspector1 = inspector.GetComponent<Inspector>();
        inspector1.TargetObject = node.gameObject;
        inspector1.TargetName = node.gameObject.name;
        node.SetInspector(inspector, inspector1.paramInput, inspector1.paramSelect);
    }

    private void Update()
    {
        if (Input.mouseScrollDelta.y > 0.1f)
        {
            MoveToSelectObject(inventories[(curSelected + 1) % inventories.Count]);
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private IEnumerator ClearNode()
    {
        while (true)
        {
            node1 = null;
            node2 = null;
            yield return new WaitForSeconds(10f);
        }
    }
}
