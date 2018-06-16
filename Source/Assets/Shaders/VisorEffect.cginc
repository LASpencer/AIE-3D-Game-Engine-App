#ifndef __VISOR_EFFECT_CGINC__
#define __VISOR_EFFECT_CGINC__

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

float2 FishEye(float2 uv) {
	//TODO make more parameterizable
	//TODO elliptical fisheye?
	float2 mid = { 0.5,0.5 };
	float distanceFromFocus = distance(mid, uv);
	float bias = distanceFromFocus - cos(distanceFromFocus * 2) * 0.02; //hack
	float2 direction = normalize(uv - mid);

	return mid + direction * bias;
}

v2f vert(appdata v)
{
	v2f o;
	o.vertex = UnityObjectToClipPos(v.vertex);
	o.uv = v.uv;
	return o;
}

sampler2D _MainTex;

fixed4 frag(v2f i) : SV_Target
{
	fixed4 col = tex2D(_MainTex, FishEye(i.uv));
	return col;
}



#endif