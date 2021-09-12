Shader "Unlit/Heatmap"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float4 colors[5];
            float pointRanges[5];

            
            
            // every point has an x coordinate, a y coordinate and an intensity coordinate
            // here we allow for 32 points. these are populated by the script
            float _Hits[3 * 32];
            int _HitCount = 0;

            void init()
            {
                // we need to set up our color ranges which will range from transparent all the way
                // to red. Transparent - Green - Yellow - Orange - Red

                
                colors[0] = float4(0,0,0,0);
                colors[1] = float4(0,0.9,0.2,1);
                colors[2] = float4(.9,1,0.3,1);
                colors[3] = float4(0.9,0.7,0.1,1);
                colors[4] = float4(1,0,0,1);

                
                pointRanges[0] = 0;
                pointRanges[1] = 0.25;
                pointRanges[2] = 0.5;
                pointRanges[3] = 0.75;
                pointRanges[4] = 1.0;

                _HitCount = 1;
                _Hits[0] = 0;
                _Hits[1] = 0;
                _Hits[2] = 2;
            }


            float DistanceSquared(float2 a, float2 b)
            {
                float area_of_effect_size = 1.0f;

                // when effect size is = 1 d will effect 25% of the texture.
                // diameter of effected area.
                float d = pow(max(0.0, 1.0 - distance(a,b)/area_of_effect_size),2);

                return d;
                
            }

            float3 GetHeatForPixel(float weight)
            {
                if(weight <= pointRanges[0]) return colors[0];
                if(weight > pointRanges[0]) return colors[4];

                for(int i = 1; i < 5; i++)
                {
                    if(weight < pointRanges[i])
                    {
                        float distance_from_lower = weight - pointRanges[i-1];
                        float size_of_point_range = pointRanges[i] - pointRanges[i-1];
                        float point_ratio = distance_from_lower/size_of_point_range;
                        float3 color_range = colors[i] - colors[i-1];
                        float3 color_contribution = color_range * point_ratio;
                        float3 new_color = colors[i-1] + color_contribution;
                        return new_color;   
                    }
                }
                return colors[0];
            }
            
            // this gets called for every pixel in the shader and updates their color with
            // the return value
            fixed4 frag (v2f i) : SV_Target
            {
                init();
                fixed4 col = tex2D(_MainTex, i.uv);
                // uvs come in at 0-1 with 0.5,0.5 in the center.
                // the code below changes this so that they are between -2 and positive 2
                // with 0,0 being in the center. This gives us more range to play with.
                
                float2 uv = i.uv;
                uv = uv * 4.0 - float2(2.0,2.0); // this changes the uv coordinate range to 2 2

                // as the shader loops through each pixel we calculate the distance between the pixel
                // and our hits. The closer the pixel is to the hit the more red it becomes.
                float totalWeight = 0;

                for(float j = 0; j < _HitCount; j++)
                {
                    // every hit has 3 points x, y, and intensity
                    float2 work_point = float2(_Hits[j*3], _Hits[j*3+1]);
                    float pint_intensity = _Hits[j*3+2];

                    // after we get the weight we can get the colors using the point ranges.
                    // for example if totalWeight < 0.25 then we get our first color 
                    totalWeight += 0.5 * DistanceSquared(uv, work_point) * pint_intensity;
                }

                float3 heat = GetHeatForPixel(totalWeight);
                
                return col + float4(heat, 0.5);
            }
            ENDCG
        }
    }
}
