Shader "PostProcessing/FogEffect"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BaseTex("", 2D) = "" {}
		_FogColour("Fog Colour", Color) = (1,1,1,1)
		_Density("Fog Density", Range(0,100)) = 1

		_FilterPower("Bloom Prefilter Power", float) = 8
		_FilterCutoff("Bloom Prefilter Cutoff", Range(0,1)) = 0.8
	}

	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		// 0: pre-filter to only get bring areas
		Pass
		{
			CGPROGRAM
			#include "FogEffect.cginc"
			#pragma vertex vert
			#pragma fragment brightPrefilter
			ENDCG
		}

		// 1: Box Blur
		Pass
		{
			CGPROGRAM
			#include "FogEffect.cginc"
			#pragma vertex vert
			#pragma fragment boxBlur
			ENDCG
		}

		// 2: Horizontal blur
		Pass
		{
			CGPROGRAM
			#include "FogEffect.cginc"
			#pragma vertex vert
			#pragma fragment linearBlur
			ENDCG
		}

		// 3: Vertical blur
		Pass
		{
			CGPROGRAM
			#define VERTICAL 1
			#include "FogEffect.cginc"
			#pragma vertex vert
			#pragma fragment linearBlur
			ENDCG
		}

		//4: Fog effect
		Pass
		{
			CGPROGRAM
			#include "FogEffect.cginc"
			#pragma vertex vert
			#pragma fragment fogEffect
			ENDCG
		}
	}
}
