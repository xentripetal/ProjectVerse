Shader "Unlit/StarBlobs"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    	
	    _Pixels("Pixels", range(10,300)) = 200.0
	    _Rotation("Rotation",range(0.0, 6.28)) = 0.0
	    _Time_speed("Time Speed",range(-1.0, 1.0)) = 0.2
	        	
	    _Color1("Color", Color) = (1,1,1,1)
    	
	    _Size("Size",float) = 50.0
	    _OCTAVES("OCTAVES", range(0,20)) = 0
	    _Seed("Seed",range(1, 10)) = 1
    	_Circle_amount("Circle Amount",range(2, 30)) = 5
    	_Circle_Size("Circle Size",range(0.0, 1.0)) = 1.0
	    time("time",float) = 0.0
    	
    }
    SubShader
    {
        //Tags { "RenderType"="Opaque" }
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline" "IgnoreProjector" = "True"}
        LOD 100

        Pass
        {
			Tags { "LightMode"="UniversalForward"}

			CULL Off
			ZWrite Off // don't write to depth buffer 
         	Blend SrcAlpha OneMinusSrcAlpha // use alpha blending

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "../cginc/hlmod.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Pixels;
            float _Rotation;
			float _Time_speed;
            float _Size;
            int _OCTAVES;
            int _Seed;
			float time;
            float _Circle_amount;
            float _Circle_Size;           
    		fixed4 _Color1;

            
			struct Input
	        {
	            float2 uv_MainTex;
	        };
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
			
			float rand(float2 coord) {
				coord = mod(coord, float2(1.0,1.0)*round(_Size));
				return frac(sin(dot(coord.xy ,float2(12.9898,78.233))) * 15.5453 * _Seed);
			}

			float2 rotate(float2 coord, float angle){
				coord -= 0.5;
				//coord *= mat2(float2(cos(angle),-sin(angle)),float2(sin(angle),cos(angle)));            	
            	coord = mul(coord,float2x2(float2(cos(angle),-sin(angle)),float2(sin(angle),cos(angle))));
				return coord + 0.5;
			}
			
			float circle(float2 uv) {
				float invert = 1.0 / _Circle_amount;
				
				if (mod(uv.y, invert*2.0) < invert) {
					uv.x += invert*0.5;
				}
				float2 rand_co = floor(uv*_Circle_amount)/_Circle_amount;
				uv = mod(uv, invert)*_Circle_amount;
				
				float r = rand(rand_co);
				r = clamp(r, invert, 1.0 - invert);
				float circle = distance(uv, float2(r,r));
				return smoothstep(circle, circle+0.5, invert * _Circle_Size * rand(rand_co*1.5));
			}
			float noise(float2 coord){
				float2 i = floor(coord);
				float2 f = frac(coord);
				
				float a = rand(i);
				float b = rand(i + float2(1.0, 0.0));
				float c = rand(i + float2(0.0, 1.0));
				float d = rand(i + float2(1.0, 1.0));

				float2 cubic = f * f * (3.0 - 2.0 * f);

				return lerp(a, b, cubic.x) + (c - a) * cubic.y * (1.0 - cubic.x) + (d - b) * cubic.x * cubic.y;
			}

			float fbm(float2 coord){
				float value = 0.0;
				float scale = 0.5;

				for(int i = 0; i < _OCTAVES ; i++){
					value += noise(coord) * scale;
					coord *= 2.0;
					scale *= 0.5;
				}
				return value;
			}

			bool dither(float2 uv1, float2 uv2) {
				return mod(uv1.x+uv2.y,2.0/_Pixels) <= 1.0 / _Pixels;
			}

			float2 spherify(float2 uv) {
				float2 centered= uv *2.0-1.0;
				float z = sqrt(1.0 - dot(centered.xy, centered.xy));
				float2 sphere = centered/(z + 1.0);
				return sphere * 0.5+0.5;
			}
			fixed4 frag(v2f i) : COLOR {
				// pixelize uv
            	
				float2 pixelized = floor(i.uv*_Pixels)/_Pixels;				
				//uv.y = 1 - uv.y;
				// use dither val later to mix between colors
				bool dith = dither(i.uv, pixelized);
				
				float2 uv = rotate(pixelized, _Rotation);

				// angle from centered uv's
				float angle = atan2(uv.x - 0.5, uv.y - 0.5);
				float d = distance(pixelized, float2(0.5,0.5));
				
				
				float c = 0.0;
				for(int i = 0; i < 15; i++) {
					float r = rand(float2(float(i),float(i)));
					float2 circleUV = float2(d, angle);
					c += circle(circleUV*_Size -time * _Time_speed - (1.0/d) * 0.1 + r);
				}
				
				c *= 0.37 - d;
				c = step(0.07, c - d);
				
				return fixed4(float3(_Color1.rgb), c);
				}
            
            ENDCG
        }
    }
}
