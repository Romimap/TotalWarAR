Shader "Custom/UnitShader" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Frame ("Frame", Int) = 0
        _Orientation ("Orientation", Int) = 0
    }
    
    SubShader {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        ZWrite Off
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows alpha:fade
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_DEFINE_INSTANCED_PROP(fixed4, _Color)
        UNITY_DEFINE_INSTANCED_PROP(int, _Frame)
        UNITY_DEFINE_INSTANCED_PROP(int, _Orientation)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 uv = (IN.uv_MainTex + float2(UNITY_ACCESS_INSTANCED_PROP(Props, _Frame), UNITY_ACCESS_INSTANCED_PROP(Props, _Orientation))) / float2(3.0, 4.0);
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, uv) * UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = 0;
            o.Smoothness = 0;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
