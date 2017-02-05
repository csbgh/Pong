Shader "Minimal/Diffuse"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
		
		_RimColor ("Rim Color", Color) = (0.5,0.5,0.5,1.0)
        _RimPower ("Rim Power", Range(0.5,8.0)) = 3.0
		
		_SpecularColor ("Specular Color", Color) = (0.5,0.5,0.5,1.0)
		_SpecularSmoothness ("Specular Smoothness", Range(0.0, 1.0)) = 0.5
		_SpecularGloss ("Specular Gloss", Range(0.0, 4.0)) = 0.5
		
		_EmissiveStrength ("Emissive Strength", Range(0.0,2.0)) = 1.0
	}
	
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		
			#pragma surface surf BlinnPhong
			#pragma target 3.0
			#pragma multi_compile RIM_LIGHTING_ON RIM_LIGHTING_OFF
			#pragma multi_compile SPECULAR_ON SPECULAR_OFF
			#pragma multi_compile EMISSIVE_ON EMISSIVE_OFF

			fixed4 _Color;
	
			// Rim Lighting
			fixed4 _RimColor;
			float _RimPower;
			
			// Specular
			fixed4 _SpecularColor;
			half _SpecularSmoothness;
			float _SpecularGloss;
			
			// Emissive
			float _EmissiveStrength;

			struct Input
			{
				float2 uv_MainTex;
				fixed4 color : COLOR;
				#if RIM_LIGHTING_ON
					float3 viewDir;
				#endif
			};
			sampler2D _MainTex;

			void surf (Input IN, inout SurfaceOutput o)
			{
				fixed4 base_color = IN.color *_Color;
				o.Albedo = base_color.rgb;
				
				#if EMISSIVE_ON
					// modulate emissive power by uv.x coordinate
					float emissiveVal = _EmissiveStrength * clamp(IN.uv_MainTex.x, 0.0f, 2.0f);
				#endif

				#if RIM_LIGHTING_ON
					half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
				#endif
				
				#if RIM_LIGHTING_ON && EMISSIVE_ON
					o.Emission = (_RimColor.rgb * pow(rim, _RimPower)) + (emissiveVal * o.Albedo);
				#elif RIM_LIGHTING_ON
					o.Emission = _RimColor.rgb * pow(rim, _RimPower);
				#elif EMISSIVE_ON
					o.Emission = emissiveVal * o.Albedo;
				#endif
				
				o.Alpha = 1.0f;
				
				#if SPECULAR_ON
					_SpecColor = _SpecularColor * IN.uv_MainTex.y; // modulate specular value by uv.y coordinate
					o.Alpha = 1.0f;
					o.Specular = _SpecularSmoothness;
					o.Gloss = _SpecularGloss;
				#endif
			}
		
		ENDCG
	}
	CustomEditor "MinimalDiffuseInspector"

	
}
