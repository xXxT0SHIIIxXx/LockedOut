Shader "Hidden/Pixelization"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Pixels ("Resolution", float) = 512
        _Pw ("Pixel Width", float) = 64
        _Ph ("Pixel Height", float) = 64
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _Pixels;
            float _Pw;
            float _Ph;
            float _Dx;
            float _Dy;

            fixed4 frag (v2f i) : SV_Target
            {
                _Dx = _Pw * (1/_Pixels);
                _Dy = _Ph * (1/_Pixels);
                float2 coord = float2(_Dx * floor(i.uv.x/ _Dx),_Dy * floor(i.uv.y/ _Dy));
                fixed4 col = tex2D(_MainTex, coord);
                return col;
            }
            ENDCG
        }
    }
}
