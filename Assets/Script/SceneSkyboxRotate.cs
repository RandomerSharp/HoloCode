using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSkyboxRotate : MonoBehaviour
{
    enum Mode
    {
        Auto,
        Maumal
    }
    [SerializeField] Mode mode;

    [SerializeField]
    [Range(0f, 10f)]
    private float skyboxRotateSpeed;
    [SerializeField]
    private float skyboxAngular = 0f;

    private void Start()
    {
        GameObject.Find("FloorQuad(Clone)").layer = 11;
    }

    void Update()
    {
        if (mode == Mode.Auto)
        {
            skyboxAngular += Time.deltaTime * skyboxRotateSpeed;
        }
        RenderSettings.skybox.SetFloat("_Rotation", skyboxAngular);
    }
}
