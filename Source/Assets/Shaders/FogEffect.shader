Shader "PostProcessing/FogEffect"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_FogColour("Fog Colour", Color) = (1,1,1,1)
		_Density("Fog Density", Range(0,100)) = 1
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

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
			
			sampler2D	_MainTex;
			sampler2D	_CameraDepthNormalsTexture;
			fixed4		_FogColour;
			fixed		_DepthMultiplier;
			fixed		_Density;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				// TODO figure out parameters for fog
				float4 depthNormal = tex2D(_CameraDepthNormalsTexture, i.uv);
				float depth;
				float3 normal;
				DecodeDepthNormal(depthNormal, depth, normal);

				//float fogFactor = depth * _Density;
				float fogFactor = 1 - (exp(-depth * _Density));

				col.xyz = lerp(col.xyz, _FogColour.xyz, saturate(fogFactor));

				return col;
				//return depthNormal;
				//return float4(depth, depth, depth, 1);
			}
			ENDCG
		}
	}
}
