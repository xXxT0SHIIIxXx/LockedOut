Shader "Unlit/PSX"
{
    Properties
    {
        [MainTexture] _MainTex ("Albedo", 2D) = "black" {}
        [MainColor] _MainCol("Color",Vector) = (0,0,0,0)
        _Levels("Levels",int) = 10
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
                float4 screenPosition : TEXCOORD1;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };


            sampler2D _MainTex;
            float4 _MainCol;
            float4 _MainTex_ST;

            int _Levels;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
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

                float greyscale = max(col.r, max(col.g, col.b));
                float lower     = floor(greyscale * _Levels) / _Levels;
                float lowerDiff = abs(greyscale - lower);

                float upper     = ceil(greyscale * _Levels) / _Levels;
                float upperDiff = abs(upper - greyscale);

                float level      = lowerDiff <= upperDiff ? lower : upper;
                float adjustment = level / greyscale;

                col.rgb = col.rgb * adjustment;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                clip(col.a);
                return col;
            }
            ENDCG
        }
    }
}
