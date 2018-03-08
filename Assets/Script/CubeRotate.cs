using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRotate : MonoBehaviour
{
    [SerializeField]
    [Range(1f, 10f)]
    private float rotateSpeed = 1f;

    void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed);
        //Debug.Log(HoloToolkit.Unity.CameraCache.Main.name);
    }
}
