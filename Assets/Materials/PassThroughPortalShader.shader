Shader "Unlit/PassThroughPortalShader"
{
    Properties
    {
        [IntRange] _StencilID ("Stencil ID", Range(0, 255)) = 0
    }
    SubShader
    {
        Tags 
        {
            "RenderType"="Opaque"
            "Queue" = "Geometry"
            "RenderPipeline" = "UniversalPipeline"
        }

        Pass
        {
            Blend Zero One
            ZWrite off

            Stencil
            {
                Ref [_StencilID]
                Comp Always
                Pass Replace
                Fail Keep
            }
        }
    }
}
