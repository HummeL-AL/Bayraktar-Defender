Shader "Joicy/Ground"
{
    Properties
    {
        [HideInInspector] _WorkflowMode("WorkflowMode", Float) = 1.0

        [MainColor] _BaseColor("Color", Color) = (1,1,1,1)
        [MainTexture] _BaseMap("Ground Texture", 2D) = "white" {}
        _LandscapeTexture ("Landscape Texture", 2D) = "black" {}
        _LandscapeHeight ("Landscape Height", Float) = 15

        _Smoothness("Smoothness", Range(0.0, 1.0)) = 0.5
        [Gamma] _Metallic("Metallic", Range(0.0, 1.0)) = 0.0

        [ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
        _SpecColor("Specular", Color) = (0.2, 0.2, 0.2)

        [HideInInspector] _SrcBlend("__src", Float) = 1.0
        [HideInInspector] _DstBlend("__dst", Float) = 0.0
        [HideInInspector] _ZWrite("__zw", Float) = 1.0
        [HideInInspector] _Cull("__cull", Float) = 2.0

        [HideInInspector] _QueueOffset("Queue offset", Float) = 0.0

    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Opaque"
            "UniversalMaterialType" = "Lit"
            "Queue"="Geometry"
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="UniversalLitSubTarget"
        }
        LOD 300

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
        
        sampler2D _LandscapeTexture;
        float4 _LandscapeTexture_ST = (1,1,0,0);
        float _LandscapeHeight = 0;

        struct VertexInput
        {
            float4 positionOS   : POSITION;
            float3 normalOS     : NORMAL;
            float4 tangentOS    : TANGENT;
            float2 texcoord_0     : TEXCOORD0;
            float2 texcoord_1   : TEXCOORD1;
            float2 staticLightmapUV   : TEXCOORD2;
            float2 dynamicLightmapUV  : TEXCOORD3;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };

        struct VertexOutput
        {
            float4 positionOS   : POSITION;
            float3 normalOS     : NORMAL;
            float4 tangentOS    : TANGENT;
            float2 uv_0 : TEXCOORD0;
            float2 uv_1 : TEXCOORD1;

            #if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
                float3 positionWS : TEXCOORD2;
            #endif
            float2 staticLightmapUV   : TEXCOORD3;
            float2 dynamicLightmapUV  : TEXCOORD4;
            
            #ifdef _ADDITIONAL_LIGHTS_VERTEX
                half4 fogFactorAndVertexLight : TEXCOORD5; // x: fogFactor, yzw: vertex light
            #else
                half  fogFactor : TEXCOORD5;
            #endif
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };

        struct GeometryOutput
        {
            float2 uv_0 : TEXCOORD0;
            float2 uv_1 : TEXCOORD1;
            
            #if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
                float3 positionWS : TEXCOORD2;
            #endif
                float3 normalWS : TEXCOORD3;
            #if defined(REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR)
                half4 tangentWS : TEXCOORD4;    // xyz: tangent, w: sign
            #endif

            #ifdef _ADDITIONAL_LIGHTS_VERTEX
                half4 fogFactorAndVertexLight : TEXCOORD5; // x: fogFactor, yzw: vertex light
            #else
                half  fogFactor : TEXCOORD5;
            #endif

            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
                float4 shadowCoord : TEXCOORD6;
            #endif

            DECLARE_LIGHTMAP_OR_SH(staticLightmapUV, vertexSH, 7);

            float4 positionCS : SV_POSITION;
            UNITY_VERTEX_INPUT_INSTANCE_ID
            UNITY_VERTEX_OUTPUT_STEREO
        };

        ENDHLSL

        Pass
        {
            Name "StandardLit"
            Tags{"LightMode" = "UniversalForward"}

            Blend[_SrcBlend][_DstBlend]
            ZWrite[_ZWrite]
            Cull[_Cull]

            HLSLPROGRAM
            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag

            // ------------------------------------------

            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #pragma shader_feature _NORMALMAP
            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _ALPHAPREMULTIPLY_ON
            #pragma shader_feature _METALLICSPECGLOSSMAP
            #pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature _OCCLUSIONMAP

            #pragma shader_feature _SPECULARHIGHLIGHTS_OFF
            #pragma shader_feature _GLOSSYREFLECTIONS_OFF
            #pragma shader_feature _SPECULAR_SETUP
            #pragma shader_feature _RECEIVE_SHADOWS_OFF



            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE

            

            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile_fog



            #pragma multi_compile_instancing
            
            // ------------------------------------------

            void InitializeInputData(GeometryOutput input, half3 normalTS, out InputData inputData)
                {
                inputData = (InputData)0;

                #if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
                    inputData.positionWS = input.positionWS;
                #endif

                half3 viewDirWS = GetWorldSpaceNormalizeViewDir(input.positionWS);
                    inputData.normalWS = input.normalWS;
                
                    inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
                    inputData.viewDirectionWS = viewDirWS;

                #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
                    inputData.shadowCoord = input.shadowCoord;
                #elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
                    inputData.shadowCoord = TransformWorldToShadowCoord(inputData.positionWS);
                #else
                    inputData.shadowCoord = float4(0, 0, 0, 0);
                #endif
                #ifdef _ADDITIONAL_LIGHTS_VERTEX
                    inputData.fogCoord = InitializeInputDataFog(float4(input.positionWS, 1.0), input.fogFactorAndVertexLight.x);
                    inputData.vertexLighting = input.fogFactorAndVertexLight.yzw;
                #else
                    inputData.fogCoord = InitializeInputDataFog(float4(input.positionWS, 1.0), input.fogFactor);
                #endif

                #if defined(DYNAMICLIGHTMAP_ON)
                    inputData.bakedGI = SAMPLE_GI(input.staticLightmapUV, input.dynamicLightmapUV, input.vertexSH, inputData.normalWS);
                #else
                    inputData.bakedGI = SAMPLE_GI(input.staticLightmapUV, input.vertexSH, inputData.normalWS);
                #endif
            
                inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(input.positionCS);
                inputData.shadowMask = SAMPLE_SHADOWMASK(input.staticLightmapUV);
            
                #if defined(DEBUG_DISPLAY)
                #if defined(DYNAMICLIGHTMAP_ON)
                    inputData.dynamicLightmapUV = input.dynamicLightmapUV;
                #endif
                #if defined(LIGHTMAP_ON)
                    inputData.staticLightmapUV = input.staticLightmapUV;
                #else
                    inputData.vertexSH = input.vertexSH;
                #endif
                #endif
            }

            VertexOutput vert(VertexInput input)
            {
                VertexOutput output = (VertexOutput)0;
                
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                output.positionOS = input.positionOS;
                output.normalOS = input.normalOS;
                output.tangentOS = input.tangentOS;
                output.staticLightmapUV = input.staticLightmapUV;
                output.dynamicLightmapUV = input.dynamicLightmapUV;

                output.uv_0 = TRANSFORM_TEX(input.texcoord_0, _BaseMap);
                output.uv_1 = TRANSFORM_TEX(input.texcoord_1, _LandscapeTexture);

                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);

                #if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
                    output.positionWS = vertexInput.positionWS;
                #endif
                
                half fogFactor = 0;
                #if !defined(_FOG_FRAGMENT)
                    fogFactor = ComputeFogFactor(vertexInput.positionCS.z);
                #endif

                OUTPUT_LIGHTMAP_UV(input.staticLightmapUV, unity_LightmapST, output.staticLightmapUV);
                #ifdef DYNAMICLIGHTMAP_ON
                    output.dynamicLightmapUV = input.dynamicLightmapUV.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
                #endif

                #ifdef _ADDITIONAL_LIGHTS_VERTEX
                    output.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
                #else
                    output.fogFactor = fogFactor;
                #endif
                
                #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
                    output.shadowCoord = GetShadowCoord(vertexInput);
                #endif

                output.positionWS += float4(0, _LandscapeHeight, 0, 0) * tex2Dlod(_LandscapeTexture, float4(output.uv_1, 0, 0)).x;

                return output;
            }

            float3 GetNormalFromTriangle(float3 a, float3 b, float3 c) {
                return normalize(cross(b - a, c - a));
            }

            GeometryOutput SetupVertex(VertexOutput input, float3 normalWS)
            {
                GeometryOutput output = (GeometryOutput)0;
                output.uv_0 = input.uv_0;
                output.uv_1 = input.uv_1;

                #if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
                    output.positionWS = input.positionWS;
                #endif
            
                #ifdef _ADDITIONAL_LIGHTS_VERTEX
                    houtput.fogFactorAndVertexLight = input.fogFactorAndVertexLight; // x: fogFactor, yzw: vertex light
                #else
                    output.fogFactor = input.fogFactor;
                #endif

                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);

                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);

                half3 vertexLight = VertexLighting(vertexInput.positionWS, normalWS);

                output.normalWS = normalWS;
                #if defined(REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR) || defined(REQUIRES_TANGENT_SPACE_VIEW_DIR_INTERPOLATOR)
                    real sign = input.tangentOS.w * GetOddNegativeScale();
                    half4 tangentWS = half4(normalInput.tangentWS.xyz, sign);
                #endif
                #if defined(REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR)
                    output.tangentWS = tangentWS;
                #endif
                
                #if defined(REQUIRES_TANGENT_SPACE_VIEW_DIR_INTERPOLATOR)
                    half3 viewDirWS = GetWorldSpaceNormalizeViewDir(vertexInput.positionWS);
                    half3 viewDirTS = GetViewDirectionTangentSpace(tangentWS, output.normalWS, viewDirWS);
                    output.viewDirTS = viewDirTS;
                #endif
                
                OUTPUT_SH(output.normalWS.xyz, output.vertexSH);

                output.positionCS = mul(UNITY_MATRIX_VP, float4(input.positionWS, 1));

                return output;
            }

            [maxvertexcount(3)]
            void geom(triangle VertexOutput inputs[3], inout TriangleStream<GeometryOutput> outputStream)
            {
                float3 triangleNormal = GetNormalFromTriangle(inputs[0].positionWS, inputs[1].positionWS, inputs[2].positionWS);

                outputStream.Append(SetupVertex(inputs[0], triangleNormal));
                outputStream.Append(SetupVertex(inputs[1], triangleNormal));
                outputStream.Append(SetupVertex(inputs[2], triangleNormal));
            }
                
            float4 frag(GeometryOutput input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                
                #if defined(_PARALLAXMAP)
                #if defined(REQUIRES_TANGENT_SPACE_VIEW_DIR_INTERPOLATOR)
                    half3 viewDirTS = input.viewDirTS;
                #else
                    half3 viewDirWS = GetWorldSpaceNormalizeViewDir(input.positionWS);
                    half3 viewDirTS = GetViewDirectionTangentSpace(input.tangentWS, input.normalWS, viewDirWS);
                #endif
                    ApplyPerPixelDisplacement(viewDirTS, input.uv_0);
                #endif
                
                    SurfaceData surfaceData;
                    InitializeStandardLitSurfaceData(input.uv_0, surfaceData);
                
                    InputData inputData;
                    InitializeInputData(input, surfaceData.normalTS, inputData);
                    SETUP_DEBUG_TEXTURE_DATA(inputData, input.uv_0, _BaseMap);

                    SurfaceData surfaceData2;
                    InitializeStandardLitSurfaceData(input.uv_1, surfaceData2);
                
                    InputData inputData2;
                    InitializeInputData(input, surfaceData.normalTS, inputData2);
                    SETUP_DEBUG_TEXTURE_DATA(inputData2, input.uv_1, _BaseMap);
                #ifdef _DBUFFER
                    ApplyDecalToSurfaceData(input.positionCS, surfaceData, inputData);
                    ApplyDecalToSurfaceData(input.positionCS, surfaceData2, inputData2);
                #endif

                half4 color = UniversalFragmentPBR(inputData, surfaceData);
                color.rgb = MixFog(color.rgb, inputData.fogCoord);
                color.a = OutputAlpha(color.a, _Surface);
                return color;
            }

            ENDHLSL
        }

        UsePass "Universal Render Pipeline/Lit/ShadowCaster"
        UsePass "Universal Render Pipeline/Lit/DepthOnly"
        UsePass "Universal Render Pipeline/Lit/Meta"
    }
    //CustomEditor "UnityEditor.Rendering.Universal.ShaderGUI.LitShader"
}
