Shader "Custom/BlackCellShaderURP"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineWidth ("Outline Width", Range(0, 1)) = 0.1
        _Threshold ("Threshold", Range(0, 1)) = 0.5
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 5.0 target, to get nicer looking lighting
        #pragma target 5.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        fixed4 _OutlineColor;
        float _OutlineWidth;
        float _Threshold;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
    {
        // Albedo comes from a texture
        fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
        o.Albedo = c.rgb;

        // Add outline
        fixed4 outline = tex2D(_MainTex, IN.uv_MainTex) - _Threshold;
        outline = outline * _OutlineColor * _OutlineWidth;
        o.Albedo += outline;
    }
ENDCG
}
}
