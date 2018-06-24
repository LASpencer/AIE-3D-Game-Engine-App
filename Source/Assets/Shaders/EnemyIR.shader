Shader "Unlit/EnemyIR"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Colour("Colour", Color) = (1,0,0,1)
		_RimColour("Rim Colour", Color) = (0.8,0.5,0.2,1)
		_FuzzColour("Fuzz Colour", Color) = (0.5,0.8,0.5,1)
		_RimPower("Rim Power", Range(0.5, 30.0)) = 10
		_RowHeight("Row Height", Range(1,5)) = 1
		_RowCutoff("Row Cutoff", Range(0,1)) = 0.5
		_FuzzHeight("Fuzz Height", Range(1,200)) = 10
		_FuzzSpeed("Fuzz Speed", Range(0,5)) = 1
		_FuzzPower("Fuzz Power", Range(0.5, 10)) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Colour;
			fixed4 _RimColour;
			fixed4 _FuzzColour;
			float _RimPower;
			float _RowHeight;
			float _RowCutoff;
			float _FuzzHeight;
			float _FuzzSpeed;
			float _FuzzPower;
			
			v2f vert (appdata_base v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord.xy, _MainTex);
				//UNITY_TRANSFER_FOG(o,o.vertex);
				o.normal = UnityObjectToWorldNormal(v.normal);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				//Maybe semitransparent and scanlines?

				// Colour based on angle between normal and camera
				float3 forward = mul((float3x3)unity_CameraToWorld, float3(1,1,0));
				fixed4 mainCol = _Colour;
				float rim = 1.0 - saturate(dot(normalize(forward), i.normal));
				fixed4 rimCol = fixed4(_RimColour.rgb, _RimColour.a * pow(rim, _RimPower));
				fixed4 col = fixed4(lerp(rimCol.rgb, mainCol.rgb, rimCol.a), mainCol.a * rimCol.a);

				float time = _Time.y;
				float4 screenPos = ComputeScreenPos(i.vertex);

				// Clip gaps between scanlines based on screen space
				clip(frac(screenPos.y / _RowHeight) - _RowCutoff);

				// Add moving fuzz colour in screen space
				float fuzzAmount = frac( (screenPos.y / _FuzzHeight) + time * _FuzzSpeed);
				return lerp(col, _FuzzColour ,pow(fuzzAmount, _FuzzPower));
			}
			ENDCG
		}
	}
}
