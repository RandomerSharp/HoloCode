using MixedRealityToolkit.InputModule.Utilities;
using MixedRealityToolkit.UX.Lines;
using UnityEngine;

public class ControllerLine : AttachToController
{
    protected override void OnAttachToController()
    {
        base.OnAttachToController();
        GetComponent<Line>().End = new Vector3(0, 0, 10);
        transform.localScale = Vector3.one;
    }

    protected override void OnDetachFromController()
    {
        base.OnDetachFromController();
        transform.localScale = Vector3.zero;
    }
}
