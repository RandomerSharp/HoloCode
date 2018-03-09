using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRotate : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 10f)]
    private float rotateSpeed = 1f;

    public float RotateSpeed
    {
        get
        {
            return rotateSpeed;
        }
        set
        {
            rotateSpeed = value;
        }
    }

    void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed);
        //Debug.Log(HoloToolkit.Unity.CameraCache.Main.name);
    }
}
