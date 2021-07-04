Shader "Custom/PlayerToon"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _LerpColor("Color", Color) = (1,1,1,1)
        _MainTex ("Diffuse Texture", 2D) = "white" {}
        _LerpTex("Mix Texture", 2D) = "white" {}
        _Bumpmap("Normal Texture", 2D) = "bump" {}
        _ToonAtten("ToonAtten", float) = 2
        _Blood("Blood", float) = 0
        _BloodAlpha("BloodAlpha", float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        cull front
        CGPROGRAM
        #pragma surface surf _Nolight vertex:vert noshadow noambient
        #pragma target 3.0

        struct Input
        {
            float4 color:COLOR;
        };

        void vert(inout appdata_full v)
        {
            v.vertex.xyz = v.vertex.xyz + v.normal.xyz * 0.01;
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
        }

        float4 Lighting_Nolight(SurfaceOutput s, float3 lightDir, float atten)
        {
            return float4(0.0f, 0.0f, 0.0f, 1.0f);
        }
        ENDCG
        
        cull back
        CGPROGRAM
        
        #pragma surface surf _Toon
            //noambient
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _LerpTex;
        sampler2D _Bumpmap;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_LerpTex;
            float2 uv_Bumpmap;
        };

        float _ToonAtten;
        float4 _Color;
        float4 _LerpColor;
        float _Blood;
        float _BloodAlpha;

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

        void surf (Input IN, inout SurfaceOutputCustom o)
        {
            fixed4 d = tex2D(_LerpTex, IN.uv_LerpTex) * _LerpColor;
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            float3 f;

            if (d.r < _BloodAlpha) // 특정 알파값이 존재하는 것들..
                d.rgb = float3(1.0f, 1.0f, 1.0f);

            f = c.rgb * d.rgb;

            o.Albedo = lerp(c.rgb, f.rgb, _Blood);
            o.NormalTexture = tex2D(_Bumpmap, IN.uv_Bumpmap);
            o.Alpha = c.a;
        }

        float4 Lighting_Toon(SurfaceOutputCustom s, float3 lightDir, float atten)
        {
            float _ndotl = dot(s.Normal, lightDir);

            _ndotl = _ndotl * 0.5f + 0.5f;

            float haifLambeif = floor(_ndotl * _ToonAtten)* (1 / _ToonAtten);

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
