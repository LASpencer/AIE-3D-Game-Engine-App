#include "UnityCG.cginc"
// Upgrade NOTE: excluded shader from DX11 because it uses wrong array syntax (type[size] name)
#pragma exclude_renderers d3d11

#ifndef __KERNEL_CGINC__
#define __KERNEL_CGINC__

fixed4 filterKernel(float3x3 kernel, float scale, sampler2D tex, float2 texelSize, float2 uv)
{
	fixed4 colour = fixed4(0,0,0,0);

	for(int i = 0; i < 3; ++i){
		for(int j = 0; j < 3; ++j){
			float2 sampledTexel = float2(i - 1, 1 - j) * texelSize;
			colour += tex2D(tex, uv + sampledTexel) * kernel[i][j];
		}
	}
	return colour * scale;
}

float depthFilterKernel(float3x3 kernel, float scale, sampler2D tex, float2 texelSize, float2 uv) {

	float value = 0;

	for (int i = 0; i < 3; ++i) {
		for (int j = 0; j < 3; ++j) {
			float2 sampledTexel = float2(i - 1, 1 - j) * texelSize;

			float4 depthNormal = tex2D(tex, uv + sampledTexel);
			float depth;
			float3 normal;
			DecodeDepthNormal(depthNormal, depth, normal);

			value += depth * kernel[i][j];
		}
	}
	return value * scale;
}
#endif