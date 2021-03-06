﻿using MixedRealityToolkit.UX.Lines;
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
        if (start == null || end == null)
        {
            Destroy(gameObject);
            return;
        }
        if (start.gameObject.activeSelf && end.gameObject.activeSelf)
        {
            line.Start = start.position;
            line.End = end.position;
        }
        else
        {
            Destroy(gameObject);
        }
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
