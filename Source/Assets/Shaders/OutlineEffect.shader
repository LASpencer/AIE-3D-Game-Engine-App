Shader "PostProcessing/OutlineEffect"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Colour("Colour", Color) = (1,1,1,1)
	}
		SubShader
		{
				// No culling or depth
				Cull Off ZWrite Off ZTest Always
		
			//0: Horizontal blur
			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment linearBlur

				#include "OutlineEffect.cginc"
				ENDCG
			}

			//1: Vertical blur
			Pass
			{
				CGPROGRAM
				#define VERTICAL 1
				#pragma vertex vert
				#pragma fragment linearBlur
				#include "OutlineEffect.cginc"
				ENDCG
			}

			// 2: Detect edges
			Pass
			{
				
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment detectEdges

				#include "OutlineEffect.cginc"
				ENDCG
			}

			// 3: Detect edges by depth
			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment detectDepthEdges

				#include "OutlineEffect.cginc"
				ENDCG
			}
	}
}
