Shader "Hidden/Shader/SunDeform"
{
    HLSLINCLUDE

    #pragma target 4.5
    #pragma only_renderers d3d11 ps4 xboxone vulkan metal switch

    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/PostProcessing/Shaders/FXAA.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/PostProcessing/Shaders/RTUpscale.hlsl"

    struct Attributes
    {
        uint vertexID : SV_VertexID;
        sampler2D _CameraDepthTexture;
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
    float _Intensity;
    float _Distance;
    float _Pow;
    float _DeformPow;
    float4x4 unity_ViewToWorldMatrix;
    float4x4 unity_InverseProjectionMatrix;
    TEXTURE2D_X(_InputTexture);
    
    
    float2 unity_gradientNoise_dir(float2 p)
    {
        p = p % 289;
        float x = (34 * p.x + 1) * p.x % 289 + p.y;
        x = (34 * x + 1) * x % 289;
        x = frac(x / 41) * 2 - 1;
        return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
    }
    
    float unity_gradientNoise(float2 p)
    {
        float2 ip = floor(p);
        float2 fp = frac(p);
        float d00 = dot(unity_gradientNoise_dir(ip), fp);
        float d01 = dot(unity_gradientNoise_dir(ip + float2(0, 1)), fp - float2(0, 1));
        float d10 = dot(unity_gradientNoise_dir(ip + float2(1, 0)), fp - float2(1, 0));
        float d11 = dot(unity_gradientNoise_dir(ip + float2(1, 1)), fp - float2(1, 1));
        fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
        return lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x);
    }
    
    void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
    {
        Out = unity_gradientNoise(UV * Scale) + 0.5;
    }
    float3 GetWorldFromViewPosition (Varyings i) {
        // get depth value
        float z = LOAD_TEXTURE2D_X(_CameraDepthTexture, i.texcoord * _ScreenSize.xy).r;
        
        // get view-space position
        float4 result = mul(unity_InverseProjectionMatrix, float4(2*(i.texcoord)-1.0, z, 1.0));
        float3 viewPos = result.xyz / result.w;
        
        // get world-space position
        float3 worldPos = mul(unity_ViewToWorldMatrix, float4(viewPos, 1.0));
        return worldPos;
    }

    
    float4 CustomPostProcess(Varyings input) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
        uint2 positionSS = input.texcoord * _ScreenSize.xy;
        float depth = LOAD_TEXTURE2D_X(_CameraDepthTexture, positionSS).r;
        
        float3 lerpDepth = (1-step(depth,0)) * min(1,(pow(depth,_Pow))*_Distance) + (step(depth,0));
        
        float time = -_Time.y;
        float cosTime = _CosTime.w*0.2;
        float gradientvalue;
        Unity_GradientNoise_float(input.texcoord + float2(cosTime,time)*0.05,100,gradientvalue);
        float value =1-min(1,max(0,(GetWorldFromViewPosition(input).y )/3));
        uint2 newpos = (input.texcoord) * _ScreenSize.xy  + float2(gradientvalue,gradientvalue) * _DeformPow * (1-lerpDepth) * _Intensity * value;
        
        float3 deform = LOAD_TEXTURE2D_X(_InputTexture, newpos).xyz;
        
        float3 outColor = LOAD_TEXTURE2D_X(_InputTexture, positionSS).xyz;
        
        return float4(deform,1); //float4(outColor,1); // float4(outColor, 1);
    }
    
    
    ENDHLSL

    SubShader
    {
        Pass
        {
            Name "SunDeform"

            ZWrite Off
            ZTest Always
            Blend Off
            Cull Off

            HLSLPROGRAM
                #pragma fragment CustomPostProcess
                #pragma vertex Vert
            ENDHLSL
        }
    }
    Fallback Off
}
