Shader "Custom/RGBCube" {
	SubShader 
	{
		Pass {
		CGPROGRAM
		#pragma vertex vert		//顶点着色器入口函数声明
		#pragma fragment frag	//片段着色器入口函数声明
		//顶点输出结构体
		struct vertexOutput {
			//声明结构体的成员pos,类型为float类型的4元向量，语义为SV_POSITION,col同理;
			float4 pos : SV_POSITION;
			float4 col : TEXCOORD0;
		};
		//顶点着色器入口函数vert，与pragma第一条声明匹配，返回类型为刚刚定义的顶点输出结构体
		vertexOutput vert(float4 vertexPos : POSITION)
		
		{
			vertexOutput output; //这里不需要struct关键字
			//顶点着色器将数据写入输出结构体中。
			output.pos = mul(UNITY_MATRIX_MVP, vertexPos);
			//mul是顶点变换函数，UNITY_MATRIX_MVP是unity的内建矩阵既当前模型视图投影矩阵，vertexPos是这个函数的形参
			//此行代码的作用为将形参vertexPos（本例即Cube对象的顶点向量）按照unity的内建矩阵进行顶点变换
			output.col = vertexPos + float4(0.5, 0.5, 0.5, 0.0);
			//这行代码是实现RGB立方体的关键
			//vertexPos的值域为题干所提到的x,y,z三元组各自减去0.5构成的值域
			//但是这里接受的类型为float4，可见第四元应该是无意义的常数1
			//意思是vertexPos的值域为{-0.5,-0.5,-0.5,1}至{0.5,0.5,0.5,1}
			//而对这个值域进行+{0.5,0.5,0.5,0}的矢量相加才能得到RGB （A恒定为1）的所有颜色区间
			
			return output;
			//将输出结构体返回，进入下一个环节(简单理解为给片段着色器)
			//ps:更细致的环节有顶点变换-->顶点着色-->几何元的构建-->光栅化几何元
			//-->片段着色-->略
		}
		//片段着色器入口函数frag，与pragma第二条声明匹配，返回类型为float4语义为COLOR,
		//这里除了颜色没有其他的输出，所以没有输出结构体
		float4 frag(vertexOutput input) : COLOR 
		//此函数的形参类型为顶点着色器的输出结构体，没有语义
		//原因就在于片段着色器位于顶点着色器的下一个环节，参数按照这个顺序传递
		{
			//由于col属性已经在顶点着色器中计算，直接返回进入下一环节
			return input.col;
		}
		ENDCG
		}
	}
	//如果以上SubShader渲染失败则回滚采用Diffuse
	FallBack "Diffuse"
}

