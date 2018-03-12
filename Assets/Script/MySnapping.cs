using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MySnapping : MonoBehaviour
{
    private bool moving = false;

    public void MoveToLeftAndSnap()
    {
        if (moving) return;
        StartCoroutine(MoveAndSnap(Vector3.left));
    }

    private void Update()
    {
        Debug.Log(CameraCache.Main.transform.forward);
    }

    private IEnumerator MoveAndSnap(Vector3 left)
    {
        yield return null;
        moving = true;
        while (true)
        {
            Vector3 direction = transform.position - CameraCache.Main.transform.position;
            transform.RotateAround(CameraCache.Main.transform.position, Vector3.up, -0.5f);
            int itemCount = GetComponent<ScanDictionary>().ItemCount;
            Vector3 box = new Vector3(7, 0.7f, 0.5f);
            Vector3 pos1 = transform.position - box / 2;
            Vector3 pos2 = transform.position + box / 2;
            Vector3 pos3 = transform.position - box * (itemCount - 1) + Vector3.down * (itemCount - 1);
            Vector3 pos4 = transform.position + box * (itemCount - 1) + Vector3.down * (itemCount - 1);
            if (!(Physics.Raycast(pos1, direction, 35) || Physics.Raycast(pos2, direction, 35) ||
                Physics.Raycast(pos3, direction, 35) || Physics.Raycast(pos4, direction, 35))) break;
            Debug.DrawRay(pos1, direction.normalized * 35);
            yield return null;
        }
        moving = false;
    }

    /*public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, new Vector3(7, 0.7f, 0.5f));
    }*/
}
