// Shader created with Shader Forge v1.04 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.04;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:1,culm:0,dpts:2,wrdp:True,dith:2,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:493,x:32719,y:32712,varname:node_493,prsc:2|diff-1292-OUT,spec-2412-OUT,normal-5620-OUT;n:type:ShaderForge.SFN_Tex2d,id:1275,x:31817,y:32643,ptovrint:False,ptlb:Mask,ptin:_Mask,varname:node_1275,prsc:2,tex:40eeecf95be93f446b587cb43e1a5e7e,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:825,x:31829,y:32899,ptovrint:False,ptlb:Diffuse,ptin:_Diffuse,varname:node_825,prsc:2,tex:e8ee23dbbb3dcee42b35ea868d177374,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:5130,x:32124,y:32466,varname:node_5130,prsc:2|A-820-RGB,B-1275-RGB;n:type:ShaderForge.SFN_Tex2d,id:820,x:31785,y:32333,ptovrint:False,ptlb:Metal,ptin:_Metal,varname:node_820,prsc:2,tex:178ad93bf5f51a949a515daaa591cf09,ntxv:2,isnm:False;n:type:ShaderForge.SFN_Subtract,id:1969,x:32183,y:32696,varname:node_1969,prsc:2|A-825-RGB,B-1275-RGB;n:type:ShaderForge.SFN_Tex2d,id:8604,x:32262,y:32957,ptovrint:False,ptlb:Normal,ptin:_Normal,varname:node_8604,prsc:2,tex:3b8ea661c544be3418ed92c67d50abb5,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Add,id:1292,x:32479,y:32546,varname:node_1292,prsc:2|A-5130-OUT,B-1969-OUT;n:type:ShaderForge.SFN_Slider,id:2412,x:32377,y:32754,ptovrint:False,ptlb:Glosiness,ptin:_Glosiness,varname:node_2412,prsc:2,min:0,cur:0.6818175,max:1;n:type:ShaderForge.SFN_Tex2d,id:946,x:32262,y:33144,ptovrint:False,ptlb:Normal 2,ptin:_Normal2,varname:node_946,prsc:2,tex:48b2692e7a6a71e428925c72feb60eb3,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Subtract,id:3156,x:32466,y:32924,varname:node_3156,prsc:2|A-8604-RGB,B-1275-RGB;n:type:ShaderForge.SFN_Multiply,id:7922,x:32457,y:33108,varname:node_7922,prsc:2|A-1275-RGB,B-946-RGB;n:type:ShaderForge.SFN_Add,id:5620,x:32562,y:33015,varname:node_5620,prsc:2|A-3156-OUT,B-7922-OUT;proporder:1275-825-820-8604-2412-946;pass:END;sub:END;*/

Shader "Shader Forge/NewPlatformShadershader" {
    Properties {
        _Mask ("Mask", 2D) = "white" {}
        _Diffuse ("Diffuse", 2D) = "white" {}
        _Metal ("Metal", 2D) = "black" {}
        _Normal ("Normal", 2D) = "white" {}
        _Glosiness ("Glosiness", Range(0, 1)) = 0.6818175
        _Normal2 ("Normal 2", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _Mask; uniform float4 _Mask_ST;
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform sampler2D _Metal; uniform float4 _Metal_ST;
            uniform sampler2D _Normal; uniform float4 _Normal_ST;
            uniform float _Glosiness;
            uniform sampler2D _Normal2; uniform float4 _Normal2_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(_Object2World, float4(v.normal,0)).xyz;
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float4 _Normal_var = tex2D(_Normal,TRANSFORM_TEX(i.uv0, _Normal));
                float4 _Mask_var = tex2D(_Mask,TRANSFORM_TEX(i.uv0, _Mask));
                float4 _Normal2_var = tex2D(_Normal2,TRANSFORM_TEX(i.uv0, _Normal2));
                float3 normalLocal = ((_Normal_var.rgb-_Mask_var.rgb)+(_Mask_var.rgb*_Normal2_var.rgb));
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = 0.5;
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float3 specularColor = float3(_Glosiness,_Glosiness,_Glosiness);
                float3 directSpecular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0,dot(halfDirection,normalDirection)),specPow);
                float3 specular = directSpecular * specularColor;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 indirectDiffuse = float3(0,0,0);
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float4 _Metal_var = tex2D(_Metal,TRANSFORM_TEX(i.uv0, _Metal));
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float3 diffuse = (directDiffuse + indirectDiffuse) * ((_Metal_var.rgb*_Mask_var.rgb)+(_Diffuse_var.rgb-_Mask_var.rgb));
/// Final Color:
                float3 finalColor = diffuse + specular;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ForwardAdd"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            Fog { Color (0,0,0,0) }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _Mask; uniform float4 _Mask_ST;
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform sampler2D _Metal; uniform float4 _Metal_ST;
            uniform sampler2D _Normal; uniform float4 _Normal_ST;
            uniform float _Glosiness;
            uniform sampler2D _Normal2; uniform float4 _Normal2_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(_Object2World, float4(v.normal,0)).xyz;
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float4 _Normal_var = tex2D(_Normal,TRANSFORM_TEX(i.uv0, _Normal));
                float4 _Mask_var = tex2D(_Mask,TRANSFORM_TEX(i.uv0, _Mask));
                float4 _Normal2_var = tex2D(_Normal2,TRANSFORM_TEX(i.uv0, _Normal2));
                float3 normalLocal = ((_Normal_var.rgb-_Mask_var.rgb)+(_Mask_var.rgb*_Normal2_var.rgb));
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = 0.5;
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float3 specularColor = float3(_Glosiness,_Glosiness,_Glosiness);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow);
                float3 specular = directSpecular * specularColor;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float4 _Metal_var = tex2D(_Metal,TRANSFORM_TEX(i.uv0, _Metal));
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float3 diffuse = directDiffuse * ((_Metal_var.rgb*_Mask_var.rgb)+(_Diffuse_var.rgb-_Mask_var.rgb));
/// Final Color:
                float3 finalColor = diffuse + specular;
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
