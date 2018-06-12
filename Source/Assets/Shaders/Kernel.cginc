#include "UnityCG.cginc"

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