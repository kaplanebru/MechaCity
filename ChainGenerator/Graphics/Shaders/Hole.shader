Shader "Custom/Hole"
{
    Properties
    {
        _MaterialID ("Material ID", Float) = 0
    }

    SubShader
    {

        Tags
        {
            "Queue" = "Geometry-1"
        }

        ColorMask 0

        ZWrite off

        Stencil
        {
            Ref [_MaterialID]
            Comp always
            Pass replace
        }

        CGPROGRAM
        #pragma surface surf Lambert

        struct Input
        {
            float2 uv_MainTex; //added later
        };


        void surf(Input IN, inout SurfaceOutput o)
        {
            o.Albedo = fixed4(1, 1, 1, 1);
        }
        ENDCG
    }

    Fallback "Diffuse"
}