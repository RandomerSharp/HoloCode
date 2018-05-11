using MixedRealityToolkit.InputModule.EventData;
using MixedRealityToolkit.InputModule.InputHandlers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNode : MonoBehaviour, IPointerHandler
{
    public void OnPointerClicked(ClickEventData eventData)
    {
        if (eventData.selectedObject == null || eventData.selectedObject.layer == LayerMask.GetMask("Environment"))
        {
            Debug.Log("Create New Node");
            var inventory = GameObject.Find("HUD/Inventory");
            inventory.GetComponent<InventoryManager>().CreateNode();
            return;
        }
        Debug.Log("Clicked");
        //eventData.Use();
    }

    public void OnPointerDown(ClickEventData eventData)
    {
    }

    public void OnPointerUp(ClickEventData eventData)
    {
    }
}
