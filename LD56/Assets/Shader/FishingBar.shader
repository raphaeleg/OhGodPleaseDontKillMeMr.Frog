Shader "Unlit/FishingBar"
{
    Properties
    {
        _MinBound ("Min Boundary", Range(0.0, 1.0)) = 0.3
        _MaxBound ("Max Boundary", Range(0.0, 1.0)) = 0.8
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float _MinBound;
            float _MaxBound;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                if (i.uv.x > _MinBound && i.uv.x < _MaxBound) {
                    return float4(1.0, 1.0, 1.0, 1.0);
                }
                return float4(0.0, 0.0, 0.0, 1.0);
            }
            ENDCG
        }
    }
}
