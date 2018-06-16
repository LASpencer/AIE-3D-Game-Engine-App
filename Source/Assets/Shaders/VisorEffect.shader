Shader "PostProcessing/VisorEffect"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#include "VisorEffect.cginc"

			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}
	}
}
