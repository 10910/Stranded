Shader "Unlit/FogShader"
{
    Properties
    {
        _FogColor("Fog Color", Color) = (0.8, 0.8, 0.8, 1)
        _FogDensity("Fog Density", Range(0, 5)) = 1
        _HeightFalloff("Height Falloff", Range(0.1, 10)) = 2
        _BoxHeight("Box Height", Float) = 2
        _MainTex("MainTex", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        GrabPass { "_GrabTexture" } // ץȡ��Ļ���ݣ���Ҫ������ȣ�

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _FogColor;
            float _FogDensity;
            float _HeightFalloff;
            float _BoxHeight;

            sampler2D _CameraDepthTexture;
            sampler2D _GrabTexture;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 localPos : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.localPos = v.vertex.xyz;
                o.screenPos = ComputeScreenPos(o.pos);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // ��ȡ��ǰƬԪ��Ӧ������ռ�λ��
                float2 uv = i.screenPos.xy / i.screenPos.w;
                float rawDepth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv);
                float4 clipPos = float4(uv * 2 - 1, rawDepth, 1);
                float4 projPos = float4(uv * 2 - 1, rawDepth, 1.0);
                float4 viewPos = mul(unity_CameraInvProjection, projPos);
                viewPos /= viewPos.w;

                float4 worldPos = mul(unity_CameraToWorld, viewPos);

                // ����������ת��Ϊ���屾������
                float3 localPos = mul(unity_WorldToObject, worldPos).xyz;

                // ���ڸ߶ȵ�����Խ����ԽŨ��
                float height = localPos.y + (_BoxHeight * 0.5);
                float hNorm = saturate(1.0 - pow(height / _BoxHeight, _HeightFalloff));

                float alpha = hNorm * _FogDensity * _FogColor.a;
                return fixed4(_FogColor.rgb, alpha);
            }
            ENDCG
        }
    }
}