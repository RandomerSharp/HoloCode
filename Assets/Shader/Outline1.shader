// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/OutLine1"
{
    Properties
    {
        _MainTex("Main Tex", 2D) = ""{}
        _Factor("Factor", Range(0, 0.1)) = 0.01
        _OutLineColor("Outline Color", Color) = (0, 0, 0, 1)
    }
 
    SubShader 
    {
        Pass
        {
            Cull Front //剔除前面
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
 
            struct v2f
            {
                float4 vertex :POSITION;
            };
 
            float _Factor;
            half4 _OutLineColor;
 
            v2f vert(appdata_full v)
            {
                v2f o;
                //v.vertex.xyz += v.normal * _Factor;
                //o.vertex = mul(UNITY_MATRIX_MVP,v.vertex);
 
                //变换到视坐标空间下，再对顶点沿法线方向进行扩展
                float4 view_vertex = mul(UNITY_MATRIX_MV, v.vertex);
                float3 view_normal = mul(UNITY_MATRIX_IT_MV, v.normal);
                view_vertex.xyz += normalize(view_normal) * _Factor; //记得normalize
                o.vertex = mul(UNITY_MATRIX_P, view_vertex);
                return o;
            }
 
            half4 frag(v2f IN):COLOR
            {
                return _OutLineColor;
            }
            ENDCG
        }
 
        Pass
        {
            Cull Back //剔除后面
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
 
            struct v2f
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
            };
 
            sampler2D _MainTex;
 
            v2f vert(appdata_full v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }
 
            half4 frag(v2f IN) :COLOR
            {
                half4 c = tex2D(_MainTex, IN.uv);
                return c;
            }
            ENDCG
        }
    } 
    FallBack "Diffuse"
}