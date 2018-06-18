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
