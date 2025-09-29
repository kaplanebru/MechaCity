Shader "Custom/Perforated"
{
    Properties
    {

        _AlbedoColor ("Albedo Color", Color) = (1, 1, 1, 1) // later
        _MainTex ("Texture", 2D) = "white" {}
        _Metallic ("Metallic", Range(0, 1)) = 0 
        _Smoothness("Smoothness", Range(0, 1)) = 0
        _MaterialID ("Material ID", Float) = 0
    }

    SubShader
    {

        Tags
        {
            "Queue" = "Geometry"
        }

        Stencil
        {
            Ref [_MaterialID]
            Comp notequal
            Pass keep
        }

        //Cull Back

        CGPROGRAM
        #pragma surface surf Standard//Lambert

        struct Input
        {
            float2 uv_MainTex;
        };

        sampler2D _MainTex;
        fixed4 _AlbedoColor;
        float _Metallic;
        float _Smoothness;

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            //o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * _AlbedoColor.rgb;
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _AlbedoColor;

            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Smoothness; //0.5; 
        }
        ENDCG
    }

    Fallback "Diffuse"
}