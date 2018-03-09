using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class PostEffectBase : MonoBehaviour
{
	public Shader shader = null;
	private Material myMaterial = null;
	protected float totalTime;

	public Material MyMaterial
	{
		get
		{
			if (myMaterial == null)
			{
				myMaterial = GenerateMaterial(shader);
			}

			return myMaterial;
		}
	}

	public float TotalTime
	{
		get
		{
			return totalTime;
		}
	}

	//根据shader创建用于屏幕特效的材质  
	protected Material GenerateMaterial(Shader shader)
	{
		if (shader == null)
		{
			return null;
		}
		//需要判断shader是否支持  
		if (shader.isSupported == false)
		{
			return null;
		}

		Material material = new Material(shader)
		{
			hideFlags = HideFlags.DontSave
		};
		if (material)
		{
			return material;
		}

		return null;
	}
}
