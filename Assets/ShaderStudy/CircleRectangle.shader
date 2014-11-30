Shader "Custom/CircleRectangle" {
//圆角矩形shader
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_RoundRadius("Radius",float) = 0.1 //半径
	}
	SubShader {
		Pass{
			CGPROGRAM
// Upgrade NOTE: excluded shader from OpenGL ES 2.0 because it does not contain a surface program or both vertex and fragment programs.
#pragma exclude_renderers gles
			#pragma fragment frag  
			#include "UnityCG.cginc" //Unity内建的预定义输入结构体
			 struct FragInput{  
                float2 texcoord:TEXCOORD0;  
            };  

			sampler2D _MainTex;
			float _RoundRadius;  

			float4 frag(FragInput input) : COLOR
			{
				float4 c = tex2D(_MainTex,input.texcoord);//将图片信息按坐标转换成颜色  
				//x,y两个变元，区间均为[0,1]  
				float x = input.texcoord.x;  
				float y = input.texcoord.y;
				//（x-a）^2+（y-b）^2=r^2。 
				if( x < _RoundRadius && y < _RoundRadius){
					if( pow( (x - _RoundRadius), 2 ) + pow(y - _RoundRadius,2) > pow(_RoundRadius,2))
					{
						discard;
					}
				}

				if( x < _RoundRadius && y > 1 - _RoundRadius){
					if( pow( (x - _RoundRadius), 2 ) + pow(y -(1 - _RoundRadius),2) > pow(_RoundRadius,2))
					{
						discard;
					}
				}
				
				if( x > 1 - _RoundRadius && y < _RoundRadius){
					if( pow( (x - (1 - _RoundRadius)), 2 ) + pow(y -_RoundRadius,2) > pow(_RoundRadius,2))
					{
						discard;
					}
				}

				if( x > 1 - _RoundRadius && y > 1 - _RoundRadius){
					if( pow( (x - (1 - _RoundRadius)), 2 ) + pow(y -(1 - _RoundRadius),2) > pow(_RoundRadius,2))
					{
						discard;
					}
				}												
				return c;
			}
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
