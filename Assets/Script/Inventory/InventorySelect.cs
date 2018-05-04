using MixedRealityToolkit.Common;
using MixedRealityToolkit.InputModule.EventData;
using MixedRealityToolkit.InputModule.Focus;
using MixedRealityToolkit.InputModule.InputHandlers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySelect : FocusTarget, IPointerHandler
{
    [SerializeField]
    private string inventoryName;
    [SerializeField]
    private GameObject boundingBox;
    private bool isFocused;

    private void Awake()
    {
        if (boundingBox == null) boundingBox = transform.Find("BoundingBox").gameObject;
    }

    public override void OnFocusEnter(FocusEventData eventData)
    {
        isFocused = true;
        boundingBox?.SetActive(true);
    }

    public override void OnFocusExit(FocusEventData eventData)
    {
        isFocused = false;
        boundingBox?.SetActive(false);
    }

    private void Create()
    {
        var newNode = Resources.Load(System.IO.Path.Combine("Prefab", "NN", inventoryName)) as GameObject;
        newNode.name = inventoryName;
        Vector3 forward = (GameObject.Find("DefaultCursor").transform.position - CameraCache.Main.transform.position).normalized;
        newNode.transform.position = forward;
        newNode.transform.rotation = Quaternion.identity;
        newNode.transform.localScale = Vector3.one;
    }

    public void Exchange(string newInventory)
    {
        GetComponent<SpriteRenderer>().material.mainTexture = Resources.Load(System.IO.Path.Combine("NN", "Texture", newInventory)) as Texture;
        inventoryName = newInventory;
    }

    public void OnPointerClicked(ClickEventData eventData)
    {
        Create();
    }

    public void OnPointerDown(ClickEventData eventData) { }

    public void OnPointerUp(ClickEventData eventData) { }
}
