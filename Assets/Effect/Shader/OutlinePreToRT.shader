// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/OutlinePreToRT"
{
	//Properties
    //{
        //_Factor("Factor", Range(0, 0.1)) = 0.01
        //_OutLineColor("Outline Color", Color) = (1.0, 0, 0, 1.0)
    //}

	SubShader
	{
		Pass
		{	
			CGPROGRAM
			#include "UnityCG.cginc"
			fixed4 _OutLineColor;
			
			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 color : COLOR;
    			float3 normal : NORMAL;
			};
			
			v2f vert(appdata_full v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				o.normal = v.normal;
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target
			{
				//return _OutLineColor;
				//return fixed4(1.0, 0, 0, i.color.a);
				//return i.tangent;
				return fixed4(i.normal, i.color.a);
				//return i.color;
			}

			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}
	}

}
