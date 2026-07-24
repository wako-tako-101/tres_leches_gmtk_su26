Shader "Custom/SpriteGlitch"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _GlitchIntensity ("Glitch Intensity", Range(0, 0.1)) = 0.02
        _ChromaticAberration ("Chromatic Aberration", Range(0, 0.05)) = 0.01
        _GlitchSpeed ("Glitch Speed", Range(0, 20)) = 10
        _GlitchAmount ("Glitch Amount", Range(0, 1)) = 0.5
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;
            fixed4 _Color;

            float _GlitchIntensity;
            float _ChromaticAberration;
            float _GlitchSpeed;
            float _GlitchAmount;

            v2f vert(appdata_t IN)
            {
                v2f OUT;

                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;

                return OUT;
            }

            // Random number based on a value
            float random(float2 seed)
            {
                return frac(sin(dot(seed, float2(12.9898, 78.233))) * 43758.5453);
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                float2 uv = IN.texcoord;

                // Create horizontal glitch bands
                float band = floor(uv.y * 30.0);

                // Animate the random seed over time
                float timeSeed = floor(_Time.y * _GlitchSpeed);

                float noise = random(float2(band, timeSeed));

                // Only affect some bands
                float glitchMask = step(1.0 - _GlitchAmount, noise);

                // Generate horizontal displacement
                float displacement = random(float2(band, timeSeed + 10.0)) - 0.5;

                displacement *= _GlitchIntensity;
                displacement *= glitchMask;

                // Apply horizontal glitch
                uv.x += displacement;

                // Chromatic aberration
                float chroma = _ChromaticAberration * glitchMask;

                float red = tex2D(_MainTex, uv + float2(chroma, 0)).r;
                float green = tex2D(_MainTex, uv).g;
                float blue = tex2D(_MainTex, uv - float2(chroma, 0)).b;

                float alpha = tex2D(_MainTex, uv).a;

                fixed4 color = fixed4(red, green, blue, alpha);

                color *= IN.color;

                // Premultiply alpha
                color.rgb *= color.a;

                return color;
            }

            ENDCG
        }
    }
}