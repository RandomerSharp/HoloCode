using MRDL.Design;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineUpdate : MonoBehaviour
{
    private Transform start;
    private Transform end;

    private Line line;

    public Transform Start
    {
        get
        {
            return start;
        }

        set
        {
            start = value;
        }
    }
    public Transform End
    {
        get
        {
            return end;
        }

        set
        {
            end = value;
        }
    }

    private void Awake()
    {
        line = GetComponent<Line>();
    }

    void Update()
    {
        line.Start = start.position;
        line.End = end.position;
    }

    public bool TryDestory(Transform a, Transform b)
    {
        if (a == start && b == end)
        {
            Destroy(gameObject);
            a.GetComponent<BaseNode>().RemoveNext(b.GetComponent<BaseNode>());
            b.GetComponent<BaseNode>().RemoveLast(a.GetComponent<BaseNode>());
            return true;
        }
        if (a == end && b == start)
        {
            Destroy(gameObject);
            b.GetComponent<BaseNode>().RemoveNext(a.GetComponent<BaseNode>());
            a.GetComponent<BaseNode>().RemoveLast(b.GetComponent<BaseNode>());
            return true;
        }
        return false;
    }
}
