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

	CGINCLUDE

	struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}


	// TODO if needed write as template function?
	//TODO move out to some cginc file?
	//fixed4 filterKernel(float3x3 kernel, float scale, sampler2D tex, float2 texelSize, float2 uv)
	//{
	//	fixed4 colour = fixed4(0,0,0,0);
    //
	//	for(int i = 0; i < 3; ++i){
	//		for(int j = 0; j < 3; ++j){
	//			float2 sampledTexel = float2(i - 1, 1 - j) * texelSize;
	//			colour += tex2D(tex, uv + sampledTexel) * kernel[i][j];
	//		}
	//	}
	//	return colour * scale;
	//}
	ENDCG

	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		// 0: pre-filter to only get bring areas
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D	_MainTex;
			fixed		_FilterPower;
			fixed		_FilterCutoff;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed lum = Luminance(col.xyz);
				lum = pow(lum, _FilterPower);	// use values close to 1


				return col * step(_FilterCutoff, lum);
			}

			ENDCG
		}

		// 1: Box Blur
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "Kernel.cginc"

			sampler2D	_MainTex;
			float4		_MainTex_TexelSize;
			fixed		_Density;

			fixed4 frag (v2f i) : SV_Target
			{
				float3x3 kernel = {	1,1,1,
									1,1,1,
									1,1,1};
				return filterKernel(kernel, 1.0/9.0, _MainTex, _MainTex_TexelSize.xy, i.uv);
				//fixed foo = filterKernel(kernel, 1.0/9.0, _MainTex, _MainTex_TexelSize.xy, i.uv).a;
				//return fixed4(foo,foo,foo,foo);
				// TODO bluriness based on fog density? Maybe it determines blur size?
			}

			ENDCG
		}

		// 2: Horizontal blur
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D	_MainTex;
			float4		_MainTex_TexelSize;
			fixed		_Density;

			//TODO extract out to cginc file, use definitions to change between vertical and horizontal
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 colour = {0,0,0,0};
				float kernel[5] = {1,1,1,1,1};
				float scale = 0.2;
				for(int j = 0; j < 5; ++j){
					colour += tex2D(_MainTex, i.uv + float2(j - 2, 0) * _MainTex_TexelSize.xy) * kernel[j];
				}
				return colour * scale;
			}

			ENDCG
		}

		// 3: Vertical blur
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D	_MainTex;
			float4		_MainTex_TexelSize;
			fixed		_Density;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 colour = {0,0,0,0};
				float kernel[5] = {1,1,1,1,1};
				float scale = 0.2;
				for(int j = 0; j < 5; ++j){
					colour += tex2D(_MainTex, i.uv + float2(0, 2 - j) * _MainTex_TexelSize.xy) * kernel[j];
				}
				return colour * scale;
			}

			ENDCG
		}

		//4: Fog effect
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			
			sampler2D	_MainTex;
			sampler2D	_BaseTex;
			sampler2D	_CameraDepthNormalsTexture;
			fixed4		_FogColour;
			fixed		_Density;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 mainCol = tex2D(_MainTex, i.uv);
				fixed4 baseCol = tex2D(_BaseTex, i.uv);
				//fixed4 col = fixed4(lerp(mainCol.rgb, baseCol.rgb, baseCol.a), mainCol.a * baseCol.a);
				fixed4 col = fixed4(mainCol.rgb + baseCol.rgb, mainCol.a);

				// TODO figure out parameters for fog
				float4 depthNormal = tex2D(_CameraDepthNormalsTexture, i.uv);
				float depth;
				float3 normal;
				DecodeDepthNormal(depthNormal, depth, normal);

				//float fogFactor = depth * _Density;
				float fogFactor = 1 - (exp(-depth * _Density));

				col.xyz = lerp(col.xyz, _FogColour.xyz, saturate(fogFactor));

				return col;
				// return col + tex2D(_BaseTex, i.uv); //(looks interesting, like a will o wisp thing?)
				//return depthNormal;
				//return float4(depth, depth, depth, 1);
			}
			ENDCG
		}

	}
}
