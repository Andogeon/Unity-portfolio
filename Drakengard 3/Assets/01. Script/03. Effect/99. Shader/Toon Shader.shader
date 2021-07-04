Shader "Unlit/Toon Shader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Diffuse Texture", 2D) = "white" {}
        _Bumpmap ("Normal Texture", 2D) = "bump" {}
        _StairNum("Stair Num", float) = 2
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "LightMode" = "ForwardBase"}
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            //struct appdata // 정점
            //{
            //    float4 vertex : POSITION;
            //    float2 uv : TEXCOORD0;
            //};

            /*struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };*/

            struct appdata // 정점
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float2 NormalUV : TEXCOORD1;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 NormalUV : TEXCOORD1;
                float4 vertex : SV_POSITION;
                /*float3 worldNormal : TEXCOORD1;*/
                float3 worldNormal : NORMAL;
            };

            sampler2D _MainTex;
            sampler2D _Bumpmap;
            float4 _MainTex_ST;

            float _StairNum;
            float4 _Color;

            /*v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }*/

            v2f vert(appdata v)
            {
                v2f o;
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                
                o.uv = v.uv;
                
                o.NormalUV = v.NormalUV;
                
                return o;
            }

            half4 frag(v2f i) : SV_Target // 픽셀 쉐이더 
            {
                half4 col;

                half4 _MainTexure = tex2D(_MainTex, i.uv);

                //half3 Normal = UnpackNormal(tex2D(_Bumpmap, i.NormalUV));
                
                half4 _ndotl = dot(i.worldNormal, normalize(_WorldSpaceLightPos0));

                half4 haifLambeif = _ndotl * 0.5f + 0.5f;

                half floorToon = floor(haifLambeif * _StairNum) * (1 / _StairNum);

                col = _MainTexure * floorToon * _Color;

                return col;
            }
            ENDCG
        }
    }
}


//fixed4 frag(v2f i) : SV_Target // 픽셀 쉐이더 
//{
//    // sample the texture
//    fixed4 col = tex2D(_MainTex, i.uv);
//// apply fog
//UNITY_APPLY_FOG(i.fogCoord, col);
//return col;
//}