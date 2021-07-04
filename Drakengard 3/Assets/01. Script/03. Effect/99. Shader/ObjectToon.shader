Shader "Custom/ObjectToon"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Diffuse Texture", 2D) = "white" {}
        _Bumpmap("Normal Texture", 2D) = "bump" {}
        _ToonAtten("ToonAtten", float) = 2
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM

        #pragma surface surf _Toon
            //noambient
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _Bumpmap;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_Bumpmap;
        };

        float _ToonAtten;
        float4 _Color;

        struct SurfaceOutputCustom
        {
            fixed3 Albedo;
            fixed3 Normal;
            fixed3 Emission;
            half Specular;
            fixed Gloss;
            fixed Alpha;
            fixed4 NormalTexture;
        };

        void surf(Input IN, inout SurfaceOutputCustom o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.NormalTexture = tex2D(_Bumpmap, IN.uv_Bumpmap);
            o.Alpha = c.a;
        }

        float4 Lighting_Toon(SurfaceOutputCustom s, float3 lightDir, float atten)
        {
            float _ndotl = dot(s.Normal, lightDir);

            _ndotl = _ndotl * 0.5f + 0.5f;

            float haifLambeif = floor(_ndotl * _ToonAtten) * (1 / _ToonAtten);

            float4 Final;

            Final.rgb = s.Albedo * haifLambeif;

            s.Normal = UnpackNormal(s.NormalTexture);

            Final.a = s.Alpha;

            return Final;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
