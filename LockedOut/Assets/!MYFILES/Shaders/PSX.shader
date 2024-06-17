Shader "Unlit/PSX"
{
    Properties
    {
        [MainTexture] _MainTex ("Albedo", 2D) = "black" {}
        [MainColor] _MainCol("Color",Vector) = (0,0,0,0)
        _PixelationFactor("Pixel",float) = 1
    }
    SubShader
    {
        Tags { "Queue" = "Transparent"
               "IgnoreProjector" = "True"
               "RenderType" = "Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };


            sampler2D _MainTex;
            float4 _MainCol;
            float4 _MainTex_ST;
            float _PixelationFactor;
            float _BackfaceCull;

            v2f vert (appdata v)
            {
                v2f o;

                // Transform the vertex position to clip space
                float4 clipPos = UnityObjectToClipPos(v.vertex);
                // Convert clip space to NDC (Normalized Device Coordinates)
                float2 ndcPos = clipPos.xy / clipPos.w;
                // Quantize NDC to create the vertex snapping effect
                ndcPos = floor(ndcPos * _PixelationFactor + 0.5) / _PixelationFactor;
                // Convert NDC back to clip space
                o.vertex = float4(ndcPos * clipPos.w, clipPos.z, clipPos.w);

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o, o.vertex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                
                if(col.x == 0)
                {
                    col = _MainCol;
                }
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                clip(col.a);
                return col;
            }
            ENDCG
        }
    }
}
