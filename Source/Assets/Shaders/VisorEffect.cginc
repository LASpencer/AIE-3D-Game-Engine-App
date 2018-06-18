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

sampler2D _MainTex;
float _LensPower;

float2 FishEye(float2 uv) {
	//TODO make more parameterizable
	//TODO elliptical fisheye?
	float2 mid = { 0.5,0.5 };
	float distanceFromFocus = distance(mid, uv);
	float2 direction = normalize(uv - mid);
	float distortedRadius = pow(distanceFromFocus, _LensPower);

	if (_LensPower < 1) {
		float scaling = 0.5 * pow(0.5, -_LensPower);
		distortedRadius *= scaling;
	}

	return mid + direction * distortedRadius;
}

v2f vert(appdata v)
{
	v2f o;
	o.vertex = UnityObjectToClipPos(v.vertex);
	o.uv = v.uv;
	return o;
}

fixed4 frag(v2f i) : SV_Target
{
	fixed4 col = tex2D(_MainTex, FishEye(i.uv));
	return col;
}



#endif