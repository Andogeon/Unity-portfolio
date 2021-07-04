Shader "Custom/Sword Knight Shader"
{
	Properties
	{
		_MainTex("Diffuse Texture", 2D) = "white" {}
		_NoiseTex("Noise Texture", 2D) = "white" {}
		_Bumpmap("Normal Texture", 2D) = "bump" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		[HDR]_Color("Color", Color) = (1,1,1,1)
		_Cut("Alpha Cut", Range(0, 1)) = 0
	}
	//SubShader
	//{
	//	Tags { "RenderType" = "Opaque" }

	//	// 1st Pass

	//	CGPROGRAM
	//	#pragma surface surf Standard
	//	#pragma target 3.0

	//	sampler2D _MainTex;
	//	sampler2D _NoiseTex;
	//	sampler2D _Bumpmap;

	//	struct Input
	//	{
	//		float2 uv_MainTex;
	//		float2 uv_NoiseTex;
	//		float2 uv_Bumpmap;
	//	};

	//	half _Glossiness;
	//	half _Metallic;
	//	float _Cut;
	//	fixed4 _Color;

	//	void surf(Input IN, inout SurfaceOutputStandard o)
	//	{
	//		fixed4 Diffuse = tex2D(_MainTex, IN.uv_MainTex);

	//		fixed4 Noise = tex2D(_NoiseTex, IN.uv_NoiseTex);

	//		o.Albedo = Diffuse.rgb;

	//		o.Metallic = _Metallic;

	//		o.Smoothness = _Glossiness;

	//		o.Normal = UnpackNormal(tex2D(_Bumpmap, IN.uv_Bumpmap));

	//		o.Alpha = Diffuse.a;
	//	}


	//	ENDCG
	//}

	// 2nd Pass
	SubShader
	{
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }

		CGPROGRAM
		/*#pragma surface surf Standard alpha:fade*/
		#pragma surface surf Standard noshadow alpha:blend 
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _NoiseTex;
		sampler2D _Bumpmap;

		struct Input
		{
			float2 uv_MainTex;
			float2 uv_NoiseTex;
			float2 uv_Bumpmap;
		};

		half _Glossiness;
		half _Metallic;
		float _Cut;
		fixed4 _Color;

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 Diffuse = tex2D(_MainTex, IN.uv_MainTex);

			fixed4 Noise = tex2D(_NoiseTex, IN.uv_NoiseTex);

			o.Albedo = Diffuse.rgb;

			o.Metallic = _Metallic;

			o.Smoothness = _Glossiness;

			o.Normal = UnpackNormal(tex2D(_Bumpmap, IN.uv_Bumpmap));

			float Alpha = 1.0f;

			if (Noise.r >= _Cut)
				Alpha = 1.0f;
			else
				Alpha = 0.0f;

			float OutLine = 0.0f;

			if (Noise.r >= _Cut * 1.15f)
				OutLine = 0.0f;
			else
				OutLine = 1.0f;

			o.Emission = OutLine * _Color.rgb;

			o.Alpha = Alpha;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
