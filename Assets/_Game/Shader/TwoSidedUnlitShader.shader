Shader "Unlit/TwoSidedUnlitShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_Stencil("Stencil Texture (RGB)", 2D) = "white" {}
	}
	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "False"
			"RenderType" = "Transparent"
		}

		Cull Off
		Lighting Off

		CGPROGRAM
		#pragma surface surf Lambert alpha
		//#pragma vertex vert
		//#pragma fragment frag
		// make fog work
		//#pragma multi_compile_fog
			
		#include "UnityCG.cginc"

		struct Input {
			float2 uv_MainTex;
		};

		half4 _Color;
		sampler2D _MainTex;
		sampler2D _Stencil;

		void surf(Input IN, inout SurfaceOutput o) {
			half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Emission = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
}
