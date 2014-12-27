Shader "Custom/Sphere" {
	SubShader {
		Pass{
			//Cull Off // 关掉裁剪模式 既关掉三角形剪裁 Unity中的ShaderLab的指令所以他不需要分号来结尾。
			Cull front // 外部剪裁，那么这个通道可以理解为是给篮球的内表面上色 
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc" //Unity内建的预定义输入结构体
			struct vertexOutput {
				float4 pos : SV_POSITION;
				//由顶点着色器输出mesh信息中的纹理坐标，这个坐标是以对象为坐标系的  
				float4 posInObjectCoords : TEXCOORD0;
				//float4 col : TEXCOORD0;
			};
			vertexOutput vert(appdata_full input)
			{
				vertexOutput output;
				output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
				//output.col = input.texcoord;  //纹理坐标系的第0个集合
				//output.col = float4(input.texcoord.x,0,0,1);
				//output.col = float4( ( input.normal + float3(1.0, 1.0, 1.0) ) / 2.0,1.0);
				//直接把texcoord传递给片段着色器
				output.posInObjectCoords = input.texcoord;   
				return output;
			}

			float4 frag(vertexOutput input) : COLOR
			{
				//当坐标的y值大于0.5的时候擦除片段
				if(input.posInObjectCoords.y > 0.5){
					discard;
				}
				//其余部分仍然按y值大小生成经度绿色球 
				return float4(0.0, input.posInObjectCoords.y , 0.0, 1.0);   
				//return input.col;
			}
			ENDCG
		}

		Pass{
		Cull front // 外部剪裁，那么这个通道可以理解为是给篮球的内表面上色
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"
		struct vertexOutput {
			float4 pos : SV_POSITION;
			//由顶点着色器输出mesh信息中的纹理坐标，这个坐标是以对象为坐标系的
			float4 posInObjectCoords : TEXCOORD0;
		};
		vertexOutput vert(appdata_full input)
		{
			vertexOutput output;
			output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
			//直接把texcoord传递给片段着色器
			output.posInObjectCoords = input.texcoord;
			return output;
		}
		float4 frag(vertexOutput input) : COLOR
		{
			//当坐标的y值大于0.5的时候擦除片段
			if (input.posInObjectCoords.y > 0.5)
			{
				discard; 
			}
			
			//其余部分仍然按y值大小生成经度绿色球
			return float4(0.0, input.posInObjectCoords.y , 0.0, 1.0); 
		}
		ENDCG
		} 
		
		Pass{
		Cull back //内部剪裁，那么这个通道可以理解为是给篮球的外表面上色
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"
		struct vertexOutput {
			float4 pos : SV_POSITION;
			//由顶点着色器输出mesh信息中的纹理坐标，这个坐标是以对象为坐标系的
			float4 posInObjectCoords : TEXCOORD0;
		};
		vertexOutput vert(appdata_full input)
		{
			vertexOutput output;
			output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
			//直接把texcoord传递给片段着色器
			output.posInObjectCoords = input.texcoord;
			return output;
		}
		float4 frag(vertexOutput input) : COLOR
		{
			//当坐标的y值大于0.5的时候擦除片段
			if (input.posInObjectCoords.y > 0.5)
			{
				discard; 
			}
			
			//其余部分仍然按y值大小生成经度红色球
			return float4(input.posInObjectCoords.y, 0.0 , 0.0, 1.0); 
		}
		ENDCG
		} 
	}
}
