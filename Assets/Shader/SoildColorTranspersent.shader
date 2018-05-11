Shader "Custom/SoildColorTransparent"
{
	Properties
	{
		_MainColor("Color", Color) = (1, 1, 1, 1)
		_Factor("Factor", Range(0, 1)) = 0.9
	}
	SubShader
	{
		// No culling or depth
		Cull Back
		ZWrite On
		ZTest Always
		Blend SrcAlpha One//MinusSrcAlpha
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent-100" }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};

			fixed4 _MainColor;
			float _Factor;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = _MainColor;
				col.a = _Factor;
				return col;
			}
			ENDCG
		}
	}
}
