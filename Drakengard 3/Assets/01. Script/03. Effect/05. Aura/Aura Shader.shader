Shader "Custom/Aura Shader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _NoiseTex("Nosie Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent"}
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard alpha:fade

        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _NoiseTex;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_NoiseTex;
        };

        fixed4 _Color;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 _NoiseTetxure = tex2D(_NoiseTex, float2(IN.uv_NoiseTex.x, IN.uv_NoiseTex.y - _Time.y * 0.5f));
            fixed4 _MainTetxure = tex2D (_MainTex, IN.uv_MainTex + _NoiseTetxure.r) * _Color;
            o.Emission = _MainTetxure.rgb;
            o.Alpha = _MainTetxure.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
