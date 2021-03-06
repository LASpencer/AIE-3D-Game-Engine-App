﻿Shader "PostProcessing/BlendTextures"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BaseTex("", 2D) = "white" {}
		_Alpha("Alpha", Range(0,1)) = 1
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
			
			sampler2D	_MainTex;
			sampler2D	_BaseTex;
			fixed		_Alpha;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 mainCol = tex2D(_MainTex, i.uv);
				fixed4 baseCol = tex2D(_BaseTex, i.uv);
				baseCol.a *= _Alpha;
				return fixed4(lerp(mainCol.rgb, baseCol.rgb, baseCol.a), mainCol.a * baseCol.a);
			}
			ENDCG
		}
	}
}
