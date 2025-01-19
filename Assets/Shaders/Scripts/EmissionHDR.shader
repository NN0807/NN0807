Shader "UI/EmissionHDR"
// 　　↑ これがMaterialのShaderを選ぶ際の名前になる
{
    // プロパティの宣言
    Properties
    {
        // テクスチャ(デフォは白色)
        [PerRendererData] _MainTex ("Sprite Texture",   2D) = "white" {}
        // エミッションカラー。デフォルトは白色で、HDRカラーとして扱われる
        [HDR]             _Color   ("EmissionColor", Color) = (1,1,1,1)
    }

    SubShader
    {
        // シェーダのタグ設定。UIのレンダリングに関連する情報を指定
        Tags
        {
            "Queue"             = "Transparent"    // レンダリングキューを透明に設定
            "IgnoreProjector"   = "True"           // プロジェクタの影響を無視
            "RenderType"        = "Transparent"    // 透過オブジェクトとしてレンダリング
            "PreviewType"       = "Plane"          // プレビューモードでは平面として表示
            "CanUseSpriteAtlas" = "True"           // スプライトアトラスを使用可能
        }

        // シェーダの描画設定
        Cull Off                                   // 裏面を描画しない（双方向描画）
        Lighting Off                               // 照明を無効化
        ZWrite Off                                 // 深度書き込み無効
        ZTest [unity_GUIZTestMode]                 // 深度テスト設定（UnityのGUI用）
        Blend SrcAlpha OneMinusSrcAlpha            // アルファブレンド設定

        Pass
        {
            // デフォのパス名
            Name "Default"

        CGPROGRAM
            #pragma vertex vert                                // 頂点シェーダーを指定
            #pragma fragment frag                              // フラグメントシェーダーを指定
            #pragma target 2.0                                 // シェーダターゲットのバージョンを指定

            // Unityの共通シェーダライブラリをインクルード
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            // コンパイル時に使用する定義
            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT   // UIのクリッピング矩形をサポート
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP   // アルファクリップをサポート

            // 入力構造体（頂点データ）
            struct appdata_t
            {
                float4 vertex   : POSITION;         // 頂点位置
                float4 color    : COLOR;            // 頂点カラー
                float2 texcoord : TEXCOORD0;        // テクスチャ座標
                UNITY_VERTEX_INPUT_INSTANCE_ID      // インスタンスID（インスタンシング用）
            };

            // 出力構造体（ピクセルシェーダーに渡すデータ）
            struct v2f
            {
                float4 vertex        : SV_POSITION; // 画面座標系での頂点位置
                fixed4 color         : COLOR;       // ピクセルカラー
                float2 texcoord      : TEXCOORD0;   // テクスチャ座標
                float4 worldPosition : TEXCOORD1;   // ワールド座標
                UNITY_VERTEX_OUTPUT_STEREO          // ステレオレンダリング用のデータ
            };

            // シェーダで使用するサンプラーとパラメータ
            sampler2D _MainTex;                     // メインテクスチャ
            fixed4 _Color;                          // エミッションカラー
            fixed4 _TextureSampleAdd;               // テクスチャのサンプル加算
            float4 _ClipRect;                       // UIのクリッピング矩形
            float4 _MainTex_ST;                     // テクスチャのST変換パラメータ（※???あまり理解してない）

            // 頂点シェーダー
            v2f vert(appdata_t v)
            {
                // 出力用構造体を宣言
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);                           // インスタンスIDの設定
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);           // ステレオ出力の初期化
                OUT.worldPosition = v.vertex;                         // ワールド空間での頂点位置
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition); // クリッピング空間に変換

                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);   // テクスチャ座標の変換

                OUT.color = v.color * _Color;                         // 頂点カラーとエミッションカラーを掛け算

                return OUT;
            }

            // フラグメントシェーダー(ピクセルシェーダー)
            fixed4 frag(v2f IN) : SV_Target
            {
                // メインテクスチャの色にサンプル加算を加え、カラーを掛け算
                half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;

                // UIのクリッピング処理（矩形内に含まれているか確認）
                #ifdef UNITY_UI_CLIP_RECT

                // アルファをクリッピング結果で掛け算
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect); 

                #endif

                // アルファクリップ（アルファ値がしきい値以下の場合、ピクセルを破棄）
                #ifdef UNITY_UI_ALPHACLIP

                clip (color.a - 0.001);  // アルファが0.001未満ならクリップ（破棄）

                #endif

                // 最終的な色を返す
                // この辺りのShader変数宣言、その他Shaderパラメーター設定、
                // 頂点シェーダーからピクセルシェーダーまでの処理は
                // 描画エンジンⅠⅡでやったのと同じ
                return color;  
            }
        ENDCG
        }
    }
}
