Shader "Custom/WavingSprite"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _WaveSpeed ("Wave Speed", float) = 1.0
        _WaveHeight ("Wave Height", float) = 0.1
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        LOD 200

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
            float _WaveSpeed;
            float _WaveHeight;

            v2f vert (appdata v)
            {
                v2f o;
                // »спользуем встроенную переменную _Time дл€ времени
                float wave = sin(v.vertex.x * 10 + _Time.y * _WaveSpeed) * _WaveHeight;
                v.vertex.y += wave;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}
