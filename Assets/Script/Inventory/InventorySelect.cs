using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySelect : MonoBehaviour, IInputClickHandler, IFocusable//, IPointerHandler
{
    [SerializeField]
    private string inventoryName;
    [SerializeField]
    private GameObject boundingBox;
    //private bool isFocused;

    public string InventoryName
    {
        get
        {
            return inventoryName;
        }

        set
        {
            inventoryName = value;
        }
    }

    private void Awake()
    {
        if (boundingBox == null) boundingBox = transform.Find("BoundingBox").gameObject;
    }

    /*public override void OnFocusEnter(FocusEventData eventData)
    {
        //isFocused = true;
        boundingBox?.SetActive(true);
    }

    public override void OnFocusExit(FocusEventData eventData)
    {
        //isFocused = false;
        boundingBox?.SetActive(false);
    }*/

    /*private void Create()
    {
        var newNode = Resources.Load(System.IO.Path.Combine("Prefab", "NN", inventoryName)) as GameObject;
        newNode.name = inventoryName;
        Vector3 forward = (GameObject.Find("DefaultCursor").transform.position - CameraCache.Main.transform.position).normalized;
        newNode.transform.position = forward;
        newNode.transform.rotation = Quaternion.identity;
        newNode.transform.localScale = Vector3.one;
    }*/

    public void Exchange(string newInventory)
    {
        GetComponent<SpriteRenderer>().material.mainTexture = Resources.Load(System.IO.Path.Combine("NN", "Texture", newInventory)) as Texture;
        inventoryName = newInventory;
    }

    /*public void OnPointerClicked(ClickEventData eventData)
    {
        transform.parent.GetComponent<InventoryManager>().MoveToSelectObject(gameObject);
    }

    public void OnPointerDown(ClickEventData eventData) { }

    public void OnPointerUp(ClickEventData eventData) { }*/

    public void OnInputClicked(InputClickedEventData eventData)
    {
        transform.parent.GetComponent<InventoryManager>().MoveToSelectObject(gameObject);
    }

    public void OnFocusEnter()
    {
        boundingBox?.SetActive(true);
    }

    public void OnFocusExit()
    {
        boundingBox?.SetActive(false);
    }
}
