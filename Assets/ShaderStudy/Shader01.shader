Shader "Custom/Shader01" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		//标签Tags{}块 包括"Queue","RenderType","IgnorProjector"
		//Queue 渲染队列 可选值 Background,Geometry,AlphaTest,Overlay分别对应数字1000,2000,2450,3000,4000
		//RenderType 内置值 Opaque,Transparent,TransparentCutout,Background,Overlay
		//IgnorProjector 为true物体忽略Project的影响
		Tags { "RenderType"="Opaque" }
		
		LOD 200								//Shader使用限制 200
		
		CGPROGRAM							//注明一段CG程序的开始
		//声明我们要写的是一个表面着色器
		#pragma surface surf Lambert		

		sampler2D _MainTex;					//声明sampler2D类型的贴图_MainTex ( _MainTex必须和Properties中的变量名相同)

		//一个用于输入的结构体,命名必须为Input
		//定义一个特殊浮点类型(后缀2表示2个打包在一起的同类型) 的uv_MainTex(前缀uv_表示提取贴图uv值)
		//整体可以理解为在一个结构体里面定义一个双浮点型的一个有uv值的贴图变量用于输入到只有调用到它的surf中
		struct Input {						
			float2 uv_MainTex;
			 float2 uv_BumpMap;
		};

		//struct SurfaceOutput {			
		//	half3 Albedo;	//  像素颜色
		//	half3 Normal;	//  像素法线
		//	half3 Emission;	//  像素发散颜色 自发光,不受照明影响
		//	half3 Specular;	//	像素镜面高光
		//  half3 Gloss;	//	像素光强
		//  half3 Alpha;	//	像素透明
		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a; //默认代码
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
