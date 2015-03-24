Shader "Custom/BlinkWhileBeAttack" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color("Dissfuse Color",Color) = (1,1,1,1)
		_Cutoff("Alpha CutOff", Range(0,1)) = 0.5
		_BeAttack("BeAttack Color",Range(1,10)) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		Cull off
		
		CGPROGRAM
		#pragma surface surf Lambert alphatest:_Cutoff

		sampler2D _MainTex;
		fixed4 _Color;
		fixed _BeAttack;
		
		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 tex = tex2D (_MainTex, IN.uv_MainTex);
			fixed4 c = tex * _Color;
			
			o.Albedo = c.rgb * _BeAttack;
			o.Alpha = c.a;
			o.Emission = c.rgb;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
