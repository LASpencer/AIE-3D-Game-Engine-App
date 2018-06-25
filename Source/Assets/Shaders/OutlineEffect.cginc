#ifndef __OUTLINE_EFFECT_CGINC__
#define __OUTLINE_EFFECT_CGINC__
#include "UnityCG.cginc"
#include "Kernel.cginc"

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

sampler2D _MainTex;
float4 _MainTex_TexelSize;
sampler2D	_CameraDepthNormalsTexture;
float4	_CameraDepthNormalsTexture_TexelSize;
fixed4 _Colour;

v2f vert(appdata v)
{
	v2f o;
	o.vertex = UnityObjectToClipPos(v.vertex);
	o.uv = v.uv;
	return o;
}

// Sobel edge detection by colour
fixed4 detectEdges(v2f i) : SV_Target
{

	float3x3 xKernel = { 1,0,-1,
						2,0,-2,
						1,0,-1 };
	float3x3 yKernel = { 1,2,1,
						0,0,0,
						-1,-2,-1 };

fixed4 sobelX = filterKernel(xKernel, 1, _MainTex, _MainTex_TexelSize.xy, i.uv);
fixed4 sobelY = filterKernel(yKernel, 1, _MainTex, _MainTex_TexelSize.xy, i.uv);

fixed4 edge = sqrt(sobelX * sobelX + sobelY + sobelY);
fixed lum = (edge.r + edge.g + edge.b) / 3.0;
return _Colour * lum;
}

// Sobel edge detection by depth buffer
fixed4 detectDepthEdges(v2f i) : SV_Target
{
	float3x3 xKernel = {1,0,-1,
						2,0,-2,
						1,0,-1 };
	float3x3 yKernel = {1,2,1,
						0,0,0,
						-1,-2,-1 };


float sobelX = depthFilterKernel(xKernel, 1, _CameraDepthNormalsTexture, _CameraDepthNormalsTexture_TexelSize.xy, i.uv);
float sobelY = depthFilterKernel(yKernel, 1, _CameraDepthNormalsTexture, _CameraDepthNormalsTexture_TexelSize.xy, i.uv);

float edge = sqrt(sobelX * sobelX + sobelY + sobelY);
return _Colour * edge;
}

fixed4 linearBlur(v2f i) : SV_Target
{
	fixed4 colour = { 0,0,0,0 };
	float kernel[5] = {0.06136,0.24477,0.38774,0.24477, 0.06136 };
	float scale = 1;
	for (int j = 0; j < 5; ++j) {
	#if VERTICAL
		float2 sampleOffset = float2(0, 2 - j);
	#else
		float2 sampleOffset = float2(j - 2, 0);
	#endif
		colour += tex2D(_MainTex, i.uv + sampleOffset * _MainTex_TexelSize.xy) * kernel[j];
	}
	return colour * scale;
}

#endif