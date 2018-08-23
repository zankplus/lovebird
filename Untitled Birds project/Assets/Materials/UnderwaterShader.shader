Shader "Custom/GrabPassInvert"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
	}

    SubShader
    {
        // Draw ourselves after all opaque geometry
        Tags { 
			"Queue" = "Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off

        // Grab the screen behind the object into _BackgroundTexture
        GrabPass
        {
            "_BackgroundTexture"
        }

        // Render the object with the texture generated above, and invert the colors
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON

            #include "UnityCG.cginc"

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 uv : TEXCOORD0;
			};

            struct v2f
            {
				float2 uv : TEXCOORD0;
                float4 grabPos : TEXCOORD1;
                float4 pos : SV_POSITION;
            };

            v2f vert(appdata_t v) {
                v2f o;
                // use UnityObjectToClipPos from UnityCG.cginc to calculate 
                // the clip-space of the vertex
                o.pos = UnityObjectToClipPos(v.vertex);
                // use ComputeGrabScreenPos function from UnityCG.cginc
                // to get the correct texture coordinate
                
				o.uv = v.uv;

				#ifdef PIXELSNAP_ON
				o.pos = UnityPixelSnap (o.pos);
				#endif

				o.grabPos = ComputeGrabScreenPos(o.pos);

                return o;
            }

            sampler2D _BackgroundTexture;
			sampler2D _MainTex;

            half4 frag(v2f i) : SV_Target
            {
                half4 bgcolor = tex2Dproj(_BackgroundTexture, i.grabPos);

				fixed4 color = tex2D (_MainTex, i.uv);
				color.rgb *= color.a;
				
				if (color.a == 0)
					return bgcolor;
				else
				{
					if (bgcolor.b == 1)	// white --> blue
						return half4(.353, .431, .882, 1);
					else if (bgcolor.b > 0.8) // blue --> black
						return half4(0, 0, 0, 1);
					else if (bgcolor.b > 0)
						return half4(.353, .431, .882, 1);
					else
						return bgcolor;
				}
            }
            ENDCG
        }

    }
}