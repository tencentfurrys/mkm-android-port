Shader "2DShaderCollection/TurnMetal" {
	Properties {
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_TurnMetal_Speed_1 ("_TurnMetal_Speed_1", Range(-8, 8)) = 1
		_SpriteFade ("SpriteFade", Range(0, 1)) = 1
		[HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
		[HideInInspector] _Stencil ("Stencil ID", Float) = 0
		[HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
		[HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
		[HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255
		[HideInInspector] _ColorMask ("Color Mask", Float) = 15
	
		Pixel_Fade ("Pixel_Fade", Range(0, 1)) = 1
		_FlashAmount ("_FlashAmount", Range(0, 1)) = 0
		_Brightness_Fade_1 ("_Brightness_Fade_1", Range(0, 1)) = 0.5
		Disolve_Value ("Disolve_Value", Range(0, 1)) = 0
		_Fire_Displacement_Value ("_Fire_Displacement_Value", Float) = 0
		_Emboss_Intensity_1 ("_Emboss_Intensity_1", Float) = 0

	}

	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }
		Cull Off Lighting Off ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"
			struct appdata_t { float4 vertex : POSITION; float2 texcoord : TEXCOORD0; float4 color : COLOR; };
			struct v2f { float4 vertex : SV_POSITION; float2 texcoord : TEXCOORD0; float4 color : COLOR; };
			sampler2D _MainTex;
			sampler2D _AlphaTex;
			fixed4 SampleSpriteTexture(float2 uv) {
				fixed4 c = tex2D(_MainTex, uv);
#if ETC1_EXTERNAL_ALPHA
				c.a = tex2D(_AlphaTex, uv).r;
#endif
				return c;
			}
			fixed4 _Color;
			float _SpriteFade;
			float Pixel_Fade;
			float _FlashAmount;
			float _Brightness_Fade_1;
			float Disolve_Value;
			float _Fire_Displacement_Value;
			float _TurnMetal_Speed_1;
			float _Emboss_Intensity_1;
			v2f vert(appdata_t IN) {
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap(OUT.vertex);
				#endif
				OUT.color = IN.color;
				return OUT;
			}
			fixed4 frag(v2f IN) : SV_Target {
				fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
				c.rgb *= (0.5 + _Brightness_Fade_1);
				c.rgb = lerp(c.rgb, fixed3(1,1,1), saturate(_FlashAmount));
				float fade = _SpriteFade * Pixel_Fade;
				fade *= saturate(1.0 - Disolve_Value);
				c.a *= saturate(fade);
				return c;
			}
			ENDCG
		}
	}
	Fallback "Sprites/Default"
}
