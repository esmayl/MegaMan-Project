// Shader created with Shader Forge v1.04 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.04;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:0,uamb:True,mssp:True,lmpd:False,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:True,dith:2,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:100,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:34516,y:32712,varname:node_1,prsc:2|normal-133-RGB,emission-127-OUT,custl-72-OUT;n:type:ShaderForge.SFN_Tex2d,id:2,x:33504,y:32694,ptovrint:False,ptlb:Diffuse Texture,ptin:_DiffuseTexture,varname:node_9924,prsc:2,tex:d21ef4243a92aa34aaaa77307534a3eb,ntxv:0,isnm:False|UVIN-8-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:8,x:33219,y:32610,varname:node_8,prsc:2,uv:0;n:type:ShaderForge.SFN_NormalVector,id:17,x:33173,y:33107,prsc:2,pt:True;n:type:ShaderForge.SFN_LightVector,id:18,x:33173,y:32914,varname:node_18,prsc:2;n:type:ShaderForge.SFN_Dot,id:19,x:33437,y:33045,varname:node_19,prsc:2,dt:1|A-18-OUT,B-17-OUT;n:type:ShaderForge.SFN_LightAttenuation,id:22,x:34018,y:33338,varname:node_22,prsc:2;n:type:ShaderForge.SFN_LightColor,id:23,x:33961,y:33181,varname:node_23,prsc:2;n:type:ShaderForge.SFN_HalfVector,id:24,x:33173,y:33350,varname:node_24,prsc:2;n:type:ShaderForge.SFN_Multiply,id:72,x:34219,y:33027,varname:node_72,prsc:2|A-6672-OUT,B-23-RGB,C-22-OUT;n:type:ShaderForge.SFN_Multiply,id:89,x:33683,y:32911,varname:node_89,prsc:2|A-2-RGB,B-19-OUT;n:type:ShaderForge.SFN_Tex2d,id:90,x:33557,y:33528,ptovrint:False,ptlb:Specular Texture,ptin:_SpecularTexture,varname:node_6456,prsc:2,tex:ae7498749b19e6e459381d2a8695c114,ntxv:2,isnm:False;n:type:ShaderForge.SFN_AmbientLight,id:117,x:33843,y:32613,varname:node_117,prsc:2;n:type:ShaderForge.SFN_Multiply,id:127,x:34041,y:32800,varname:node_127,prsc:2|A-117-RGB,B-2-RGB;n:type:ShaderForge.SFN_Tex2d,id:133,x:34073,y:32557,ptovrint:False,ptlb:node_133,ptin:_node_133,varname:node_7359,prsc:2,tex:7c71b00adc816e840a9bfc1abaee6382,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Slider,id:143,x:33005,y:33521,ptovrint:False,ptlb:Wetness,ptin:_Wetness,varname:node_3073,prsc:2,min:1,cur:1,max:11;n:type:ShaderForge.SFN_Exp,id:156,x:33373,y:33481,varname:node_156,prsc:2,et:1|IN-143-OUT;n:type:ShaderForge.SFN_Dot,id:852,x:33378,y:33248,varname:node_852,prsc:2,dt:1|A-17-OUT,B-24-OUT;n:type:ShaderForge.SFN_Power,id:869,x:33595,y:33246,varname:node_869,prsc:2|VAL-852-OUT,EXP-156-OUT;n:type:ShaderForge.SFN_Add,id:6672,x:33863,y:33003,varname:node_6672,prsc:2|A-89-OUT,B-2584-OUT;n:type:ShaderForge.SFN_Multiply,id:2584,x:33748,y:33308,varname:node_2584,prsc:2|A-869-OUT,B-90-RGB;proporder:2-90-133-143;pass:END;sub:END;*/

Shader "Shader Forge/ShaderForgeTest" {
    Properties {
        _DiffuseTexture ("Diffuse Texture", 2D) = "white" {}
        _SpecularTexture ("Specular Texture", 2D) = "black" {}
        _node_133 ("node_133", 2D) = "white" {}
        _Wetness ("Wetness", Range(1, 11)) = 1
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
            #include "Lighting.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform sampler2D _DiffuseTexture; uniform float4 _DiffuseTexture_ST;
            uniform sampler2D _SpecularTexture; uniform float4 _SpecularTexture_ST;
            uniform sampler2D _node_133; uniform float4 _node_133_ST;
            uniform float _Wetness;
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
                float4 _node_133_var = tex2D(_node_133,TRANSFORM_TEX(i.uv0, _node_133));
                float3 normalLocal = _node_133_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
////// Emissive:
                float4 _DiffuseTexture_var = tex2D(_DiffuseTexture,TRANSFORM_TEX(i.uv0, _DiffuseTexture));
                float3 emissive = (UNITY_LIGHTMODEL_AMBIENT.rgb*_DiffuseTexture_var.rgb);
                float4 _SpecularTexture_var = tex2D(_SpecularTexture,TRANSFORM_TEX(i.uv0, _SpecularTexture));
                float3 finalColor = emissive + (((_DiffuseTexture_var.rgb*max(0,dot(lightDirection,normalDirection)))+(pow(max(0,dot(normalDirection,halfDirection)),exp2(_Wetness))*_SpecularTexture_var.rgb))*_LightColor0.rgb*attenuation);
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
            #include "Lighting.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform sampler2D _DiffuseTexture; uniform float4 _DiffuseTexture_ST;
            uniform sampler2D _SpecularTexture; uniform float4 _SpecularTexture_ST;
            uniform sampler2D _node_133; uniform float4 _node_133_ST;
            uniform float _Wetness;
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
                float4 _node_133_var = tex2D(_node_133,TRANSFORM_TEX(i.uv0, _node_133));
                float3 normalLocal = _node_133_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float4 _DiffuseTexture_var = tex2D(_DiffuseTexture,TRANSFORM_TEX(i.uv0, _DiffuseTexture));
                float4 _SpecularTexture_var = tex2D(_SpecularTexture,TRANSFORM_TEX(i.uv0, _SpecularTexture));
                float3 finalColor = (((_DiffuseTexture_var.rgb*max(0,dot(lightDirection,normalDirection)))+(pow(max(0,dot(normalDirection,halfDirection)),exp2(_Wetness))*_SpecularTexture_var.rgb))*_LightColor0.rgb*attenuation);
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
