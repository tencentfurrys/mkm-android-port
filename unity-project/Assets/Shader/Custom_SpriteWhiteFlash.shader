Shader "Custom/SpriteWhiteFlash" {
	Properties {
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		_FlashAmount ("_FlashAmount", Range(0, 1)) = 0
		_SpriteFade ("SpriteFade", Range(0, 1)) = 1
		Pixel_Fade ("Pixel_Fade", Range(0, 1)) = 1
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
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
			float _FlashAmount;
			float _SpriteFade;
			float Pixel_Fade;
			v2f vert(appdata_t IN) {
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap(OUT.vertex);
				#endif
				return OUT;
			}
			fixed4 frag(v2f IN) : SV_Target {
				fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
				c.rgb = lerp(c.rgb, fixed3(1,1,1), saturate(_FlashAmount));
				c.a *= _SpriteFade * Pixel_Fade;
				return c;
			}
			ENDCG
		}
	}
	Fallback "Sprites/Default"
}
