Shader "Personal/CameraShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
		// Tags { "RenderType" = "Opaque" "RenderPipeline" = "LWRP" "IgnoreProjector" = "True"}
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

            sampler2D _MainTex;

			uniform float u_vignette_size;
			uniform float u_vignette_smoothness;
			uniform float u_vignette_edge_round;

			float vignette(half2 uv, float size, float smoothness, float edgeRounding)
			{
				uv -= .5;
				uv *= size;
				
				float amount = sqrt(pow(abs(uv.x), edgeRounding) + pow(abs(uv.y), edgeRounding));
				amount = 1. - amount;
				return smoothstep(0, smoothness, amount);
			}

            fixed4 frag (v2f i) : SV_Target
            {
				//const vec3 vignetteColor = vec3(1.0, 0.0, 0.0);
                fixed4 col = tex2D(_MainTex, i.uv);
				float vig = vignette(i.uv, u_vignette_size, u_vignette_smoothness, u_vignette_edge_round);
				col.rgb = lerp(half4(0.5, 0, 0, 0.5), col.rgb, vig);
				return col; //* vignette(i.uv, u_vignette_size, u_vignette_smoothness, u_vignette_edge_round);
            }
            ENDCG
        }
    }
}


//half get_vignette_factor(half2 uv)
//{
//	half2 d = (uv - 0.5) * _Vignette.x;
//	return pow(saturate(1.0 - dot(d, d)), _Vignette.y);
//}
//
//half4 vignette_simple(half4 color, half2 uv)
// {
//	half v = get_vignette_factor(uv);
//	color.rgb = lerp(_VignetteColor.rgb, color.rgb, lerp(1.0, v, _VignetteColor.a));
//	return color;
//  }
//
//half4 frag_simple(v2f_img i) : SV_Target
// {
//	half4 color = tex2D(_MainTex, i.uv);
//	//color = chromaticAberration(color, i.uv);
//	color = vignette_simple(color, i.uv);
//	return color;
//}