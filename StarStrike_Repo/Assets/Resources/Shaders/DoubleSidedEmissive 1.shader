// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:True,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:5508,x:32867,y:32672,varname:node_5508,prsc:2|emission-7112-OUT,alpha-2239-OUT;n:type:ShaderForge.SFN_Depth,id:3821,x:31809,y:33246,varname:node_3821,prsc:2;n:type:ShaderForge.SFN_Color,id:9261,x:32151,y:32848,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_9261,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_ValueProperty,id:145,x:32151,y:33007,ptovrint:False,ptlb:Strength,ptin:_Strength,varname:node_145,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Subtract,id:8076,x:32109,y:33135,varname:node_8076,prsc:2|A-2424-OUT,B-561-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2424,x:31805,y:32974,ptovrint:False,ptlb:Range,ptin:_Range,varname:node_2424,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:5;n:type:ShaderForge.SFN_ValueProperty,id:1272,x:31809,y:33388,ptovrint:False,ptlb:Falloff,ptin:_Falloff,varname:node_1272,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_OneMinus,id:2239,x:32427,y:33073,varname:node_2239,prsc:2|IN-5227-OUT;n:type:ShaderForge.SFN_Clamp01,id:5227,x:32238,y:33135,varname:node_5227,prsc:2|IN-8076-OUT;n:type:ShaderForge.SFN_Power,id:561,x:31994,y:33255,varname:node_561,prsc:2|VAL-3821-OUT,EXP-1272-OUT;n:type:ShaderForge.SFN_Multiply,id:7112,x:32391,y:32798,varname:node_7112,prsc:2|A-9261-RGB,B-145-OUT;proporder:9261-145-2424-1272;pass:END;sub:END;*/

Shader "StarSweeper/Emmisive Twosided Depth" {
    Properties {
        _Color ("Color", Color) = (0.5,0.5,0.5,1)
        _Strength ("Strength", Float ) = 0
        _Range ("Range", Float ) = 5
        _Falloff ("Falloff", Float ) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 
            #pragma target 2.0
            uniform float4 _Color;
            uniform float _Strength;
            uniform float _Range;
            uniform float _Falloff;
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float4 projPos : TEXCOORD1;
                UNITY_FOG_COORDS(2)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
////// Lighting:
////// Emissive:
                float3 emissive = (_Color.rgb*_Strength);
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,(1.0 - saturate((_Range-pow(partZ,_Falloff)))));
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
