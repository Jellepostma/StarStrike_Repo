Shader "StarSweeper/Emmisive Twosided" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Power ("Power", Range(0.5,8.0)) = 3.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		CULL off
		Pass {
            ZWrite On
            ColorMask 0
 
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
 
            float4 vert(float4 vertex : POSITION) : SV_POSITION { return UnityObjectToClipPos(vertex); }
            fixed4 frag() : SV_Target { return 0; }
            ENDCG
        }
		CGPROGRAM
		 #pragma surface surf Lambert alpha:fade
		struct Input {
          float2 uv_MainTex;
          float2 uv_BumpMap;
          float3 viewDir;
      };

      uniform float4 _Color;
      float _Power;
      void surf (Input IN, inout SurfaceOutput o) {
        	o.Albedo = _Color.rgb;
          o.Emission = _Color.rgb * _Power;
          o.Alpha = _Color.a;
      }
		
		ENDCG
	}
	FallBack "Diffuse"
}
