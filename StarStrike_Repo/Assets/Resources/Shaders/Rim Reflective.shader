
Shader "StarSweeper/Rim Reflective" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Emission ("Emission (RGB)", 2D) = "black"{}
	_EmissionStrength("Emission Strength", Range(0.0, 10.0)) = 0.0
    _ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)
    _Cube ("Reflection Cubemap", Cube) = "_Skybox" { TexGen CubeReflect }
    _RefInt ("Reflection Power",Range(0.5,7)) = 1
    _Blur ("reflection blur",Range(0,7)) = 1
    _RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
  	_TextureInfluence ("Texture Influence", Range(0.0,1.0)) = 0.0
  	_RimPower ("Rim Power", Range(0,8.0)) = 3.0
  	_RimStrength("Rim Strength", Range(0.0, 10.0)) = 1.0
}
SubShader {
    LOD 200
    Tags { "RenderType"="Opaque" }
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
#pragma target 3.0
sampler2D _MainTex;
sampler2D _Emission;
samplerCUBE _Cube;
 
fixed4 _Color;
fixed4 _ReflectColor;
fixed _Blur,_RefInt;
 
struct Input {
    float2 uv_MainTex;
    float3 worldRefl;
    float3 viewDir;
    half4 color : COLOR;
};
 	  float4 _RimColor;
      float _RimPower;
      float _TextureInfluence;
      float _RimStrength;
      float _EmissionStrength;

void surf (Input IN, inout SurfaceOutput o) {
    fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
    fixed4 c = tex * _Color * IN.color;
    o.Albedo = c.rgb;
    fixed4 reflcol = texCUBElod (_Cube, float4(IN.worldRefl,_Blur));
    reflcol = reflcol + (pow(reflcol,_RefInt)*_RefInt) ;
    half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
    o.Alpha = _Color.a;
    o.Emission = (reflcol.rgb * _ReflectColor.rgb) + ((_RimColor.rgb + (tex2D (_MainTex, IN.uv_MainTex).rgb * _TextureInfluence)) * pow (rim, _RimPower) * _RimStrength) + (tex2D(_Emission, IN.uv_MainTex) * _EmissionStrength);
}
ENDCG
}
   
FallBack "Diffuse"
}
 
 
 