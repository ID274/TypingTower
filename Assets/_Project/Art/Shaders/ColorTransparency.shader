Shader "Custom/ColorTransparency"
{
    Properties
    {
        _Color ("Base Color", Color) = (1, 1, 1, 1) // Default white with full alpha
        _Transparency ("Transparency", Range(0, 1)) = 1 // Default fully opaque
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha // Enable alpha blending
        Cull Off                       // Render both sides
        ZWrite Off                     // Disable depth writing for transparency

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

            sampler2D _MainTex;
            float4 _Color;
            float _Transparency;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Base color and transparency adjustment
                fixed4 col = _Color;
                col.a *= _Transparency; // Apply transparency factor
                return col;
            }
            ENDCG
        }
    }
    FallBack "Transparent/Diffuse"
}


