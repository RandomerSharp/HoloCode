using MixedRealityToolkit.Common;
using UnityEngine;

public class SceneSkyboxRotate : Singleton<SceneSkyboxRotate>
{
    [System.Serializable]
    public struct SkyboxAndLight
    {
        public Material skybox;
        public float lightIntensity;
    }
    private enum Mode
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

    [SerializeField]
    private SkyboxAndLight[] skyboxes;
    [SerializeField]
    private GameObject directionLight;

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

    [ContextMenu("Change Skybox")]
    public void ChangeSky()
    {
        int i = Random.Range(0, skyboxes.Length);
        RenderSettings.skybox = skyboxes[i].skybox;
        directionLight.GetComponent<Light>().intensity = skyboxes[i].lightIntensity;
    }
}
