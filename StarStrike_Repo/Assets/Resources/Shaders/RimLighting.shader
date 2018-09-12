 Shader "StarSweeper/Rim Diffuse" {
    Properties {
      _MainTex ("Texture", 2D) = "white" {}
      _RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
      _TextureInfluence ("Texture Influence", Range(0.0,1.0)) = 0.0
      _RimPower ("Rim Power", Range(0.01,8.0)) = 3.0
      _RimStrength("Rim Strength", Range(0.0, 10.0)) = 1.0
    }
    SubShader {
      Tags { "RenderType" = "Opaque" }
      CGPROGRAM
      #pragma surface surf Lambert
      struct Input {
          float2 uv_MainTex;
          float3 viewDir;
      };
      sampler2D _MainTex;
      sampler2D _BumpMap;
      float4 _RimColor;
      float _RimPower;
      float _TextureInfluence;
      float _RimStrength;
      void surf (Input IN, inout SurfaceOutput o) {
          o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
          half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
          o.Emission = (_RimColor.rgb + (tex2D (_MainTex, IN.uv_MainTex).rgb * _TextureInfluence)) * pow (rim, _RimPower) * _RimStrength;
      }
      ENDCG
    } 
    Fallback "Diffuse"
  }