using MixedRealityToolkit.Common;
using MixedRealityToolkit.InputModule.EventData;
using MixedRealityToolkit.InputModule.Gaze;
using MixedRealityToolkit.InputModule.InputHandlers;
using MixedRealityToolkit.InputModule.InputSources;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class CreateNode : MonoBehaviour, IPointerHandler
{
    public void OnPointerClicked(ClickEventData eventData)
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

    public void OnPointerUp(ClickEventData eventData) { }
}
