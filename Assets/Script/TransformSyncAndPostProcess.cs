using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformSyncAndPostProcess : MonoBehaviour
{
    [SerializeField]
    private Transform targerTransform;
    [SerializeField]
    private Shader outlinePre;

    private void Awake()
    {
        if (targerTransform == null) targerTransform = CameraCache.Main.transform;
        if (outlinePre != null) GetComponent<Camera>().SetReplacementShader(outlinePre, string.Empty);
    }

    void Update()
    {
        transform.position = targerTransform.position;
        transform.rotation = targerTransform.rotation;
        transform.localScale = targerTransform.localScale;
    }
}
