﻿Shader "Hidden/NGUIDualTextureBlend 2"
{
	Properties
	{
		_MainTex ("Main Texture", 2D) = "white" {}
		_SecondTex ("Second Texture", 2D) = "white" {}
		_BlendFactor ("Blend Factor", range(0, 1)) = 0.5
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

			sampler2D _MainTex;
			sampler2D _SecondTex;
			float4 _MainTex_ST;
			float _BlendFactor;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
                fixed4 noise = tex2D(_SecondTex, i.uv);
                fixed2 uv = noise.xy;
                uv = lerp(i.uv, uv, _BlendFactor);
		        fixed4 col = tex2D(_MainTex, uv);
				return col;
			}
			ENDCG
		}
	}
}
