using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSkyboxRotate : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 10f)]
    private float skyboxRotateSpeed;
    private float skyboxAngular;

    private void Start()
    {
        skyboxAngular = 0f;
    }

    void Update()
    {
        skyboxAngular += Time.deltaTime * skyboxRotateSpeed;
        RenderSettings.skybox.SetFloat("_Rotation", skyboxAngular);
    }
}
