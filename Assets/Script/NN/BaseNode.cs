using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.Controllers;

public class BaseNode : MonoBehaviour, IInputClickHandler, IHoldHandler
{
    public enum DisplayModeEnum
    {
        InMenu,
        InHand,
        Hidden
    }

    private DisplayModeEnum displayMode;
    private List<BaseNode> next;
    private List<BaseNode> last;
    private bool isDraging;

    private IInputSource inputSource;

    private MenuWheelSelector nodeManager;

    public DisplayModeEnum DisplayMode
    {
        set { displayMode = value; }
    }
    public List<BaseNode> Next
    {
        get
        {
            return next;
        }
    }
    public List<BaseNode> Last
    {
        get
        {
            return last;
        }
    }

    public BaseNode LastNode<T>()
    {
        if (last.Count == 0) return null;
        foreach (var item in last)
        {
            if (item is T)
            {
                return item;
            }
        }
        return null;
    }

    public BaseNode NextNode<T>()
    {
        if (next.Count == 0) return null;
        foreach (var item in next)
        {
            if (item is T)
            {
                return item;
            }
        }
        return null;
    }

    public void SetNext(BaseNode node)
    {
        next.Add(node);
    }

    public void SetLast(BaseNode node)
    {
        last.Add(node);
    }

    private void Awake()
    {
        next = new List<BaseNode>();
        last = new List<BaseNode>();

        nodeManager = FindObjectsOfType<MenuWheelSelector>()[0];
    }

    public void CreateOne()
    {
        var obj = GameObject.Instantiate(gameObject, GameObject.Find("DefaultCursor").transform.position, Quaternion.identity);
        obj.layer = 0;
    }

    public async void ShowDialog()
    {
        transform.Find("Quad").gameObject.SetActive(true);
        GetComponentInChildren<TextMesh>().text = gameObject.name;
        await Task.Delay(3000);
        transform.Find("Quad").gameObject.SetActive(false);
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        Debug.Log("Click " + gameObject.name);
        nodeManager.NodeSelect(gameObject.GetComponent<BaseNode>());
    }

    private void Update()
    {
        if (isDraging && inputSource != null)
        {
            IPointingSource ips = null;
            if (!FocusManager.Instance.TryGetSinglePointer(out ips)) return;
            var detail = FocusManager.Instance.GetFocusDetails(ips);
            Debug.Log(detail.Point);
        }
    }

    public void OnHoldStarted(HoldEventData eventData)
    {
        inputSource = eventData.InputSource;
        isDraging = true;
    }

    public void OnHoldCompleted(HoldEventData eventData)
    {
        inputSource = null;
        isDraging = false;
    }

    public void OnHoldCanceled(HoldEventData eventData)
    {
        inputSource = null;
        isDraging = false;
    }
}