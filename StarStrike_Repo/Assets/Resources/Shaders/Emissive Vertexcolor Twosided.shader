Shader "StarSweeper/Emmisive Vertexcolor Twosided" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Power ("Power", Range(0.5,8.0)) = 3.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		CULL off
		CGPROGRAM
		 #pragma surface surf Lambert
		struct Input {
          float2 uv_MainTex;
          float3 viewDir;
          half4 color : COLOR;
      };

      uniform float4 _Color;
      float _Power;
      void surf (Input IN, inout SurfaceOutput o) {
          o.Emission = IN.color.rgb * _Power;
      }
		
		ENDCG
	}
	FallBack "Diffuse"
}
