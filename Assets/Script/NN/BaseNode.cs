using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BaseNode : MonoBehaviour
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

    public DisplayModeEnum DisplayMode
    {
        set { displayMode = value; }
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
    }

    public void CreateOne()
    {
        GameObject.Instantiate(gameObject, GameObject.Find("DefaultCursor").transform.position, Quaternion.identity);
    }

    public async void ShowDialog()
    {
        transform.Find("Quad").gameObject.SetActive(true);
        GetComponentInChildren<TextMesh>().text = gameObject.name;
        await Task.Delay(3000);
        transform.Find("Quad").gameObject.SetActive(false);
    }
}