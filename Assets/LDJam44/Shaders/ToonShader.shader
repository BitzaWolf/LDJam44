Shader "Bitzawolf/Toon"
{
	Properties
	{
		_MainColor("Main Color", Color) = (0.3, 0.3, 0.3, 1)
		_Texture("Texture", 2D) = "White" {}
		_LightRamp("Light Ramp", 2D) = "White" {}
	}

	SubShader
	{
		CGPROGRAM
			#pragma surface surf Custom fullforwardshadows
			#pragma target 3.0

			fixed4 _MainColor;
			sampler2D _Texture;
			sampler2D _LightRamp;

			struct Input
			{
				float2 uv_MainTex;
			};

			void surf(Input IN, inout SurfaceOutput o)
			{
				fixed4 c = tex2D(_Texture, IN.uv_MainTex) * _MainColor;
				o.Albedo = c.rgb;
				o.Alpha = 1;
			}

			half4 LightingCustom(SurfaceOutput s, half3 lightDir, half atten)
			{
				half NdotL = dot(s.Normal, lightDir);
				half val = (NdotL + 1) / 2;
				half4 ramp = tex2D(_LightRamp, float2(val, 0.5));
				half4 c;
				c.rgb = s.Albedo * _LightColor0.rgb * ramp;
				c.a = s.Alpha;
				return c;
			}
		ENDCG
	}
	Fallback "Diffuse"
}