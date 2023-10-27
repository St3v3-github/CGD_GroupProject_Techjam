Shader"Unlit/BloodShader"
{

    Properties
    { 
        _BaseColor("Base Colour", Color) = (1,1,1,1)
    }


    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalRenderPipeline" }
/*
        Pass
        {
            HLSLPROGRAM 
            #pragma vertex vert
            #pragma fragment frag

        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"            

        struct Attributes
        {

            float4 positionOS : POSITION;
        };

        struct Varyings
        {

            float4 positionHCS : SV_POSITION;
        };
*/

  /*      
        Varyings vert(Attributes IN)
        {

            Varyings OUT;

            OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);

            return OUT;
        }

          
        half4 frag() : SV_Target
        {
            return _BaseColor;
        }
            ENDHLSL
        }
    */
        Pass
        {
            Name "ColorBlitPass"

            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            #pragma vertex Vert
            #pragma fragment frag

            TEXTURE2D_X(_CameraOpaqueTexture);
            SAMPLER(sampler_CameraOpaqueTexture);

            float _Intensity;

            CBUFFER_START(UnityPerMaterial)
            half4 _BaseColor;
            CBUFFER_END

            half4 frag (Varyings input) : SV_Target
            {
    /*
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                float4 color = SAMPLE_TEXTURE2D_X(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, input.texcoord);
                return color * float4(_Intensity, 0, 0, 0.5);
*/
                return _BaseColor;
}
            ENDHLSL
        }
    }

}
