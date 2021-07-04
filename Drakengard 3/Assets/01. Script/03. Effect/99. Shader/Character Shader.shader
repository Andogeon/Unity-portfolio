Shader "Custom/Character Shader"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _GlossTex("Gloss Texutre", 2D) = "white" {}
        _BumpMap("Normal Texture", 2D) = "bump "{}
        _MaskTex("Mask Texture", 2D) = "white" {}
        _SpecPow("Specular Power", Range(10, 200)) = 100
        _Color("BloodColor", Color) = (1,1,1,1)
        _Cut("Blood", float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf _CustomLighting

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _GlossTex;
        sampler2D _BumpMap;
        sampler2D _MaskTex;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_GlossTex;
            float2 uv_BumpMap;
            float2 uv_MaskTex;
        };
        
        float _SpecPow;
        float4 _Color;
        float _Cut;

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            fixed4 d = tex2D(_GlossTex, IN.uv_GlossTex);
            fixed4 f = tex2D(_MaskTex, IN.uv_MaskTex);

            float3 _BloodTexture = c.rgb * (1 - f.r * _Color);

            o.Albedo = lerp(c.rgb, _BloodTexture, _Cut);
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
            o.Gloss = d.r;
            o.Alpha = c.a;
        }

        float4 Lighting_CustomLighting(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten)
        {
            float4 Final;

            float Diffuse = saturate(dot(s.Normal, lightDir));

            float3 DiffuseColor = Diffuse * s.Albedo * atten;
            
            
            
            float3 H = normalize(lightDir + viewDir);

            float Spec = saturate(dot(s.Normal, H));

            Spec = pow(Spec, _SpecPow);

            float3 SpecColor = Spec * float3(1, 1, 1) * s.Gloss;
            
           
            Final.rgb = DiffuseColor + SpecColor;

            Final.a = s.Alpha;

            return Final;
        }

        ENDCG
    }
    FallBack "Diffuse"
}
