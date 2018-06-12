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


sampler2D	_MainTex;
sampler2D	_BaseTex;
sampler2D	_CameraDepthNormalsTexture;
float4		_MainTex_TexelSize;
fixed4		_FogColour;
fixed		_Density;
fixed		_FilterPower;
fixed		_FilterCutoff;



v2f vert (appdata v)
{
	v2f o;
	o.vertex = UnityObjectToClipPos(v.vertex);
	o.uv = v.uv;
	return o;
}

fixed4 brightPrefilter (v2f i) : SV_Target
{
	fixed4 col = tex2D(_MainTex, i.uv);
	fixed lum = Luminance(col.xyz);
	lum = pow(lum, _FilterPower);	// use values close to 1


	return col * step(_FilterCutoff, lum);
}

fixed4 boxBlur (v2f i) : SV_Target
{
float3x3 kernel = {	1,1,1,
					1,1,1,
					1,1,1};
return filterKernel(kernel, 1.0/9.0, _MainTex, _MainTex_TexelSize.xy, i.uv);
//fixed foo = filterKernel(kernel, 1.0/9.0, _MainTex, _MainTex_TexelSize.xy, i.uv).a;
//return fixed4(foo,foo,foo,foo);
}

fixed4 linearBlur(v2f i) : SV_Target
{
	fixed4 colour = {0,0,0,0};
	float kernel[5] = {1,1,1,1,1};
	float scale = 0.2;
	for(int j = 0; j < 5; ++j){
	#if VERTICAL
		float2 sampleOffset = float2(0, 2 - j);
	#else
		float2 sampleOffset = float2(j - 2, 0);
	#endif
		colour += tex2D(_MainTex, i.uv + sampleOffset * _MainTex_TexelSize.xy) * kernel[j];
	}
	return colour * scale;
}

fixed4 fogEffect(v2f i) : SV_Target
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