Shader "Hidden/Shader/Depth_local_illumination"
{
    SubShader
    {
        Pass
        {
            Name "Depth_local_illumination"

            ZWrite Off
            ZTest Always
            Blend Off
            Cull Off

            HLSLPROGRAM
            #pragma fragment CustomPostProcess
            #pragma vertex Vert

            #pragma target 4.5
            #pragma only_renderers d3d11 playstation xboxone vulkan metal switch

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/NormalBuffer.hlsl"


             float GetDepth(int2 SSRegular)
             {
                return LOAD_TEXTURE2D_X(_CameraDepthTexture, SSRegular).x;
             }


            struct Attributes
            {
                uint vertexID : SV_VertexID;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 texcoord   : TEXCOORD0;
 
       
                UNITY_VERTEX_OUTPUT_STEREO
            };


            Varyings Vert(Attributes input)
            {
                Varyings output;

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
                
                output.positionCS = GetFullScreenTriangleVertexPosition(input.vertexID);
                output.texcoord = GetFullScreenTriangleTexCoord(input.vertexID);
                
  
                return output;
            }




            // List of properties to control your post process effect
            float4 _Params;
            #define _fov _Params.w
            TEXTURE2D_X(_InputTexture);
            TEXTURE2D(_DepthTexture);

            float4 CustomPostProcess(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                

                float2 positionSS = input.texcoord * _ScreenSize.xy; //individueller pixel (als wert von 0-1) * resolution = xy position in pixel

                float3 inColor = LOAD_TEXTURE2D_X(_InputTexture, positionSS).xyz;


                // get camera resolution
                float width = float(_ScreenSize.x);
                float height = float(_ScreenSize.y);

                float fov_x = _fov * (width / height);

                // Read and decode Z-Buffer
                float depth = GetDepth(positionSS);
                float linearEyeDepth = Linear01Depth(depth, _ZBufferParams);
                float z = linearEyeDepth;
                z = z * (z < 1);
                z = _ProjectionParams.z * z;

                /*
                // calculate scene x/y coordinate
                float pi = 3.14159;
                float a_y = tan((_fov * (pi / 180.0f)) * 0.5f);
                float a_x = tan((fov_x * (pi / 180.0f)) * 0.5f);
                float x = a_x * ((float(positionSS.x) - (width/2)) / (width/2));
                float y = a_y * ((float(positionSS.y) - (height/2)) / (height/2));

                // calculate distance from camera to scene pixel; the factor 1.42 is to prevent that the distance value gets greater than 1
                float distance;
                distance = _ProjectionParams.z * (z * sqrt(1 + pow(x, 2) + pow(y, 2)));
                */

                //float delta_t = unity_DeltaTime.x; // frame to frame time
                float time = _Time.x;

                return float4(inColor.x, z, inColor.z, 1.0);
                //return float4(inColor.x, distance, delta_t, 1.0);
            }

            ENDHLSL
        }
    }

        Fallback Off
}