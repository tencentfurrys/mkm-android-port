Shader "2DShaderCollection/Outline" {
	Properties {
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		_Outline_Size_1 ("_Outline_Size_1", Range(1, 16)) = 1
		_Outline_Color_1 ("_Outline_Color_1", Color) = (1,1,1,1)
		_Outline_HDR_1 ("_Outline_HDR_1", Range(0, 2)) = 1
		_SpriteFade ("SpriteFade", Range(0, 1)) = 1
		Pixel_Fade ("Pixel_Fade", Range(0, 1)) = 1
		_FlashAmount ("_FlashAmount", Range(0, 1)) = 0
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
			#include "UnityCG.cginc"
			struct appdata_t { float4 vertex : POSITION; float2 texcoord : TEXCOORD0; float4 color : COLOR; };
			struct v2f { float4 vertex : SV_POSITION; float2 texcoord : TEXCOORD0; float4 color : COLOR; };
			sampler2D _MainTex;
			float4 _MainTex_TexelSize;
			fixed4 _Color;
			float _Outline_Size_1;
			fixed4 _Outline_Color_1;
			float _Outline_HDR_1;
			float _SpriteFade;
			float Pixel_Fade;
			float _FlashAmount;
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
				fixed4 c = tex2D(_MainTex, IN.texcoord) * IN.color;
				float2 px = _MainTex_TexelSize.xy * _Outline_Size_1;
				float a = c.a;
				float outlineA = 0;
				outlineA += tex2D(_MainTex, IN.texcoord + float2(px.x, 0)).a;
				outlineA += tex2D(_MainTex, IN.texcoord - float2(px.x, 0)).a;
				outlineA += tex2D(_MainTex, IN.texcoord + float2(0, px.y)).a;
				outlineA += tex2D(_MainTex, IN.texcoord - float2(0, px.y)).a;
				outlineA += tex2D(_MainTex, IN.texcoord + px).a;
				outlineA += tex2D(_MainTex, IN.texcoord - px).a;
				outlineA += tex2D(_MainTex, IN.texcoord + float2(px.x, -px.y)).a;
				outlineA += tex2D(_MainTex, IN.texcoord - float2(px.x, -px.y)).a;
				outlineA = saturate(outlineA);
				fixed3 oc = _Outline_Color_1.rgb * _Outline_HDR_1;
				c.rgb = lerp(oc, c.rgb, a);
				c.a = saturate(max(a, outlineA * (1 - a)));
				c.rgb = lerp(c.rgb, fixed3(1,1,1), _FlashAmount);
				c.a *= _SpriteFade * Pixel_Fade;
				return c;
			}
			ENDCG
		}
	}
	Fallback "Sprites/Default"
}
