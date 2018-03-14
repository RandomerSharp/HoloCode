using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class OutlinePostProcess : PostEffectBase
{
    private Camera mainCamera = null;
    private Camera additionalCamera = null;
    private RenderTexture renderTexture = null;

    public Shader outlineShader = null;
    //采样率
    public float samplerScale = 1;
    public int downSample = 1;
    public int iteration = 2;

    void Awake()
    {
        InitAdditionalCam();

        additionalCamera.SetReplacementShader(outlineShader, string.Empty);
    }

    private void InitAdditionalCam()
    {
        mainCamera = GetComponent<Camera>();
        if (mainCamera == null)
        {
            return;
        }
        Transform addCamTransform = transform.Find("additionalCamera");
        if (addCamTransform != null)
        {
            DestroyImmediate(addCamTransform.gameObject);
        }
        GameObject additionalCamObj = new GameObject("additionalCamera");
        additionalCamera = additionalCamObj.AddComponent<Camera>();

        SetAdditionalCam();
    }

    private void SetAdditionalCam()
    {
        if (additionalCamera)
        {
            additionalCamera.transform.parent = mainCamera.transform;
            additionalCamera.transform.localPosition = Vector3.zero;
            additionalCamera.transform.localRotation = Quaternion.identity;
            additionalCamera.transform.localScale = Vector3.one;
            additionalCamera.farClipPlane = mainCamera.farClipPlane;
            additionalCamera.nearClipPlane = mainCamera.nearClipPlane;
            additionalCamera.fieldOfView = mainCamera.fieldOfView;
            additionalCamera.backgroundColor = Color.clear;
            additionalCamera.clearFlags = CameraClearFlags.Color;
            additionalCamera.cullingMask = 1 << LayerMask.NameToLayer("Highlight");
            additionalCamera.depth = -999;

            if (renderTexture == null)
            {
                renderTexture = RenderTexture.GetTemporary(additionalCamera.pixelWidth >> downSample, additionalCamera.pixelHeight >> downSample, 0);
            }
            additionalCamera.targetTexture = renderTexture;
        }
    }

    void OnEnable()
    {
        SetAdditionalCam();
        if (additionalCamera != null)
        {
            additionalCamera.enabled = true;
        }
    }

    void OnDisable()
    {
        if (additionalCamera != null)
        {
            additionalCamera.enabled = false;
        }
    }

    void OnDestroy()
    {
        if (renderTexture)
        {
            RenderTexture.ReleaseTemporary(renderTexture);
        }
        DestroyImmediate(additionalCamera.gameObject);
    }

    //unity提供的在渲染之前的接口，在这一步渲染描边到RT
    void OnPreRender()
    {
        //使用OutlinePrepass进行渲染，得到RT
        if (additionalCamera != null && additionalCamera.enabled)
        {
            if (renderTexture != null && (renderTexture.width != Screen.width >> downSample || renderTexture.height != Screen.height >> downSample))
            {
                RenderTexture.ReleaseTemporary(renderTexture);
                renderTexture = RenderTexture.GetTemporary(Screen.width >> downSample, Screen.height >> downSample, 0);
            }
            additionalCamera.targetTexture = renderTexture;
        }
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (MyMaterial && renderTexture)
        {
            RenderTexture temp1 = RenderTexture.GetTemporary(source.width >> downSample, source.height >> downSample, 0);
            RenderTexture temp2 = RenderTexture.GetTemporary(source.width >> downSample, source.height >> downSample, 0);

            //高斯模糊，两次模糊，横向纵向，使用pass0进行高斯模糊
            MyMaterial.SetVector("_offsets", new Vector4(0, samplerScale, 0, 0));
            Graphics.Blit(renderTexture, temp1, MyMaterial, 0);
            MyMaterial.SetVector("_offsets", new Vector4(samplerScale, 0, 0, 0));
            Graphics.Blit(temp1, temp2, MyMaterial, 0);

            //如果有叠加再进行迭代模糊处理
            for (int i = 0; i < iteration; i++)
            {
                MyMaterial.SetVector("_offsets", new Vector4(0, samplerScale, 0, 0));
                Graphics.Blit(temp2, temp1, MyMaterial, 0);
                MyMaterial.SetVector("_offsets", new Vector4(samplerScale, 0, 0, 0));
                Graphics.Blit(temp1, temp2, MyMaterial, 0);
            }

            //用模糊图和原始图计算出轮廓图
            //MyMaterial.SetTexture("_BlurTex", temp2);
            //Graphics.Blit(renderTexture, temp1, MyMaterial, 1);

            //轮廓图和场景图叠加
            MyMaterial.SetTexture("_BlurTex", temp1);
            Graphics.Blit(source, destination, MyMaterial, 2);

            RenderTexture.ReleaseTemporary(temp1);
            RenderTexture.ReleaseTemporary(temp2);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}
