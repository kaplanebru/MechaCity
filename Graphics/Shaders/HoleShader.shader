Shader "Custom/HoleShader"
{
     Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BumpMap ("Normal Map", 2D) = "bump" {}
        _Metallic ("Metallic", Range(0, 1)) = 0.0
        _Gloss ("Smoothness", Range(0, 1)) = 0.5
        _Color ("Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _BumpMap;
            float4 _BumpMap_ST;
            float _Metallic;
            float _Gloss;
            float4 _Color;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Calculate the distance from the center
                float2 center = float2(0.5, 0.5);
                float dist = distance(i.uv, center);

                // Set the radius within which you want the hole to be invisible
                float holeRadius = 0.2; // Adjust this value as needed

                if (dist < holeRadius)
                {
                    discard;
                }

                // Sample the normal map
                half3 normal = UnpackNormal(tex2D(_BumpMap, i.uv));

                // Calculate lighting
                half3 lightDir = normalize(float3(0, 0, 1)); // Adjust light direction as needed
                half3 viewDir = normalize(_WorldSpaceCameraPos.xyz - mul(unity_ObjectToWorld, i.vertex).xyz);

                half3 halfDir = normalize(lightDir + viewDir);
                half NdotL = max(0.0, dot(normal, lightDir));
                half NdotH = max(0.0, dot(normal, halfDir));
                half3 fresnel = _Metallic + (1.0 - _Metallic) * pow(1.0 - NdotH, 5.0);

                half3 specular = fresnel * _Gloss;

                fixed4 col = tex2D(_MainTex, i.uv);
                col.rgb *= NdotL + specular;

                return col * _Color;
            }
            ENDCG
        }
    }
}