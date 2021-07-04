Shader "Custom/Fire Shader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _MainTex2 ("Sub Texture", 2D) = "white" {}
        _MainTex3("Sub Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        zwrite off
        blend SrcAlpha OneMinusSrcAlpha

        CGPROGRAM
        #pragma surface surf Lambert keepalpha
        //#pragma surface surf Standard alpha:fade

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _MainTex2;
        sampler2D _MainTex3;

        struct Input
        {
            float2 uv_MainTex;
            
            float2 uv_MainTex2;

            float2 uv_MainTex3;
        };


        UNITY_INSTANCING_BUFFER_START(Props)
        
        UNITY_INSTANCING_BUFFER_END(Props)

            /*void surf(Input IN, inout SurfaceOutputStandard o)*/
        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 d = tex2D(_MainTex2, float2(IN.uv_MainTex2.x, IN.uv_MainTex2.y - _Time.y));

            fixed4 e = tex2D(_MainTex3, float2(IN.uv_MainTex3.x, IN.uv_MainTex3.y) + d.r * -1.0f);

            fixed4 c = tex2D(_MainTex, IN.uv_MainTex + d.r);
            
            o.Emission = c.rgb;
            
            o.Alpha = e.a;
        }
        ENDCG
    }
    FallBack "Legacy Shaders/Transparent/VertexLit"


    /*FallBack "Legacy Shaders/Transparent/Cutout/VertexLit"*/

    /*FallBack "Legacy Shaders/Transparent/VertexLit"*/

    /*FallBack "Diffuse"*/
}
