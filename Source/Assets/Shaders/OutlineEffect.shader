Shader "PostProcessing/OutlineEffect"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Colour ("Colour", Color) = (1,1,1,1)
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
			//#include "OutlineEffect.cginc"
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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float4 _MainTex_TexelSize;

			fixed4 frag (v2f i) : SV_Target
			{
				//TODO extract to cginc, have multiple passes (blur first?)

				float3x3 xKernel = {1,0,-1,
									2,0,-2,
									1,0,-1};
				float3x3 yKernel = {1,2,1,
									0,0,0,
									-1,-2,-1};

				fixed4 sobelX = filterKernel(xKernel, 1, _MainTex, _MainTex_TexelSize.xy, i.uv);
				fixed4 sobelY = filterKernel(yKernel, 1, _MainTex, _MainTex_TexelSize.xy, i.uv);

				return sqrt(sobelX * sobelX + sobelY + sobelY);
				//TODO convert to greyscale and multiply by colour
			}
			ENDCG
		}
	}
}
