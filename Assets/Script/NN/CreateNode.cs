using HoloToolkit.Unity.InputModule;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class CreateNode : MonoBehaviour, IInputClickHandler//IPointerHandler
{
    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (eventData.selectedObject == null || eventData.selectedObject.layer == LayerMask.GetMask("Environment"))
        {
            var inventory = GameObject.Find("HUD/Inventory");
            InteractionSourceInfo sourceKind;

            //InteractionInputSources.Instance.TryGetSourceKind(eventData.SourceId, out sourceKind);
            eventData.InputSource.TryGetSourceKind(eventData.SourceId, out sourceKind);
            Vector3 v;
            if (sourceKind == InteractionSourceInfo.Controller)
            {
                Ray r;
                /*if (InteractionInputSources.Instance.TryGetPointingRay(eventData.SourceId, out r))
                {
                    InteractionInputSources.Instance.TryGetGripPosition(eventData.SourceId, out v);
                    //Debug.Log(v);
                    inventory.GetComponent<InventoryManager>().CreateNode((r.origin - v).normalized * 12f);
                }*/
                if (eventData.InputSource.TryGetPointingRay(eventData.SourceId, out r))
                {
                    if (eventData.InputSource.TryGetGripPosition(eventData.SourceId, out v))
                    {
                        inventory.GetComponent<InventoryManager>().CreateNode((r.origin - v).normalized * 12f);
                    }
                    Debug.Log(v);
                }
            }
            else
            {
                inventory.GetComponent<InventoryManager>().CreateNode((GazeManager.Instance.GazeOrigin + GazeManager.Instance.GazeNormal).normalized * 12f);
            }
            eventData.Use();
            return;
        }

        /*public void OnPointerClicked(ClickEventData eventData)
        {
            if (eventData.selectedObject == null || eventData.selectedObject.layer == LayerMask.GetMask("Environment"))
            {
                var inventory = GameObject.Find("HUD/Inventory");
                InteractionSourceKind sourceKind;
                InteractionInputSources.Instance.TryGetSourceKind(eventData.SourceId, out sourceKind);
                Vector3 v;
                if (sourceKind == InteractionSourceKind.Controller)
                {
                    Ray r;
                    if (InteractionInputSources.Instance.TryGetPointingRay(eventData.SourceId, out r))
                    {
                        InteractionInputSources.Instance.TryGetGripPosition(eventData.SourceId, out v);
                        //Debug.Log(v);
                        inventory.GetComponent<InventoryManager>().CreateNode((r.origin - v).normalized * 12f);
                    }
                }
                else
                {
                    inventory.GetComponent<InventoryManager>().CreateNode((GazeManager.GazeOrigin + GazeManager.GazeDirection).normalized * 12f);
                }
                eventData.Use();
                return;
            }
        }

        public void OnPointerDown(ClickEventData eventData) { }

        public void OnPointerUp(ClickEventData eventData) { }*/
    }
}
