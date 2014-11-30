Shader "Custom/Shader01" {
	//_Name ( "Displayed Name", type ) = default value {options}
	//_Name：程序中引用的名字，和我们一般理解的变量名称是一样的。
	//Displayed Name：这个字符串将会出现在Unity材质的编辑面板上。
	//type：该属性的类型。Unity支持以下几种属性类型：
		//Color：表示一个单一的RGBA颜色值；
		//2D： 表示一张大小为2的次方的纹理贴图，可以使用基于模型UV坐标来进行采样；
		//Rect：表示一张纹理不是2的次方的纹理贴图；
		//Cube：表示一个可用于反射的3D立方体映射贴图，可以进行采样；
		//Range(min, max)：一个取值范围在min到max之间的浮点值；
		//Float： 一个可以为任意值的浮点值；
		//Vector：一个4维度的向量。 
	//default value：该属性的默认值。
		//Color：使用浮点值表示的(r, g, b, a)，例如(1,1,1,1)；
		//2D/Rect/Cube：对于贴图类型的属性，默认值可以是一个空字符串，或者"white", "black", "gray", "bump"这样的字符串；
		//Float/Range：在此范围内的值即可；
		//Vector：以(x,y,z,w)形式表示的4D向量；
	//{ options }：只和纹理类型的2D、Rect和Cube相关，它必须至少被指定为{ }。你可以使用空格分隔多个选项，有如下选择：
		//TexGen贴图生成模式：该纹理的自动纹理坐标生成模式。可以为ObjectLinear, EyeLinear, SphereMap, CubeReflect, CubeNormal。这些直接对应了OpenGL中的texgen modes。注意，如果你编写了一个顶点函数，那么可以忽略TexGen。
	Properties {
		_MainTexture ("MainTexture", 2D) = "white" {}
		_BumpMap ("Bumpmap", 2D) = "bump" {} 
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

		sampler2D _MainTexture;					//声明sampler2D类型的贴图_MainTex ( _MainTex必须和Properties中的变量名相同)
		sampler2D _BumpMap;

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
			half4 c = tex2D (_MainTexture, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));  
			//o.Alpha = c.a; //默认代码
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
