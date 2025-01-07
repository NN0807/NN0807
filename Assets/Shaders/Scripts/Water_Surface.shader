Shader "Unlit/Water_Surface"
{

    Properties
    {
        _MainTex  ("Texture",               2D) = "white"   {}
        _Cube     ("Cube",                CUBE) = ""        {} //キューブマップを使用するにはCUBEに設定する


        _BumpMap  ("Normal Map",            2D) = "bump"    {} // 法線マップ
        _BumpScale("Normal Scale", Range(0, 3)) = 0.5

        _ScrollSpeedX("UV_Scroll_SpeedX",Range(0,10)) = 0.5    // 波のスクロール値
        _ScrollSpeedY("UV_Scroll_SpeedY",Range(0,10)) = 0.5
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

            // フォグ用のバリアントを生成
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "UnityStandardUtils.cginc"  // UnpackNormalWithScale関数を使用するために追加

            struct appdata
            {
                float4 vertex  : POSITION;
                float3 normal  : Normal;
                float2 uv      : TEXCOORD0;
                float4 tangent : TANGENT;
            };

            struct v2f
            {

                // フォグ用のTEXCOORDのindexを指定
                UNITY_FOG_COORDS(1)

                float4 vertex   : SV_POSITION;
                float2 uv       : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float3 normal   : TEXCOORD2;
                float3 viewDir  : TEXCOORD3;
                float4 tangent  : TEXCOORD4;
                float3 binormal : TEXCOORD5;
            };

            sampler2D _MainTex;
            sampler2D _BumpMap;
            float     _BumpScale;
            float     _ScrollSpeedX;
            float     _ScrollSpeedY;
            float4 _MainTex_ST;

            UNITY_DECLARE_TEXCUBE(_Cube); //キューブマップとして使用することを宣言

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex   = UnityObjectToClipPos(v.vertex);         //頂点をMVP行列変換
                o.uv       = TRANSFORM_TEX(v.uv, _MainTex);          //テクスチャスケールとオフセットを加味
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz; //頂点座標をワールド座標系に変換
                o.normal   = UnityObjectToWorldNormal(v.normal);     //法線をワールド座標系に変換


                o.tangent = mul(unity_ObjectToWorld, v.tangent.xyz);
                o.binormal = normalize(cross(v.normal.xyz, v.tangent.xyz) * v.tangent.w * unity_WorldTransformParams.w);
                o.binormal = mul(unity_ObjectToWorld, o.binormal);

                return o;
            }

            // 最終出力処理
            fixed4 frag(v2f i) : SV_Target
            {
                // UVスクロール処理
                float2 scroll = float2(_ScrollSpeedX, _ScrollSpeedY) * _Time;
                //float2 scroll = float2(0.0f,0.0f);

                // ノーマルマップから法線情報を取得する
                float3 localNormal = UnpackNormalWithScale(tex2D(_BumpMap, i.uv + scroll), _BumpScale);

                // タンジェントスペースの法線をワールドスペースに変換する
                i.normal = i.tangent * localNormal.x + i.binormal * localNormal.y + i.normal * localNormal.z;

                //視線ベクトルを計算
                float3 viewDir     = normalize(_WorldSpaceCameraPos - i.worldPos);

                //反射ベクトルを計算
                float3 refDir      = reflect(-viewDir, i.normal);

                //キューブマップからサンプリング
                fixed4 col         = UNITY_SAMPLE_TEXCUBE(_Cube, refDir);

                return col;
            }
            ENDCG
        }
    }
}
