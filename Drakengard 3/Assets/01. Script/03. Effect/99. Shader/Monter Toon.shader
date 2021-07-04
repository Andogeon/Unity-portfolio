Shader "Custom/Monter Toon"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Diffuse Texture", 2D) = "white" {}
        _Bumpmap("Normal Texture", 2D) = "bump" {}
        _NoiseTex("Noise Texture", 2D) = "white" {}
        _OutColor("Out Color", Color) = (1,1,1,1)
        _ToonAtten("ToonAtten", float) = 2
        _Cut("Toon Cut", float) = 0
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        LOD 200
        CGPROGRAM
 
        #pragma surface surf _Toon alpha:fade
            //noambient
        #pragma target 3.0
 
        sampler2D _MainTex;
        sampler2D _Bumpmap;
        sampler2D _NoiseTex;
 
        struct Input
        {
            float2 uv_MainTex;
            float2 uv_Bumpmap;
            float2 uv_NoiseTex;
        };
 
        float _ToonAtten;
        float _Cut;
        float4 _Color;
        float4 _OutColor;
 
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
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            
            o.Albedo = c.rgb;
            
            o.NormalTexture = tex2D(_Bumpmap, IN.uv_Bumpmap);

            fixed4 _NoiseColor = tex2D(_NoiseTex, IN.uv_NoiseTex);

            float _Alpha, _OutLine;

            if (_NoiseColor.r >= _Cut)
                _Alpha = 1.0f;
            else
                _Alpha = 0.0f;

            if (_NoiseColor.r >= _Cut * 1.15f)
                _OutLine = 0.0f;
            else
                _OutLine = 1.0f;

            o.Emission = _OutLine * _OutColor.rgb;

            o.Alpha = _Alpha;
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
