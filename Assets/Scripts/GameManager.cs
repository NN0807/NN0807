using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Common;

public class GameManager : MonoBehaviour
{
    // Singletonパターン: EffectManagerのインスタンスを1つだけ保持し、どこからでもアクセス可能にする
    public static GameManager Instance { get; private set; }

    /// <summary> ゲーム開始フラグ </summary>
    [ReadOnly]
    public bool _gameStartFLg  = false;

    /// <summary> ゲーム終了フラグ </summary>
    [ReadOnly]
    public bool _gameFinishFLg = false;

    /// <summary> ポーズフラグ </summary>
    [ReadOnly]
    public bool pauseFLg = false;

    /// <summary> 死亡したキャラクターをカウント </summary>
    [SerializeField]
    public int _deadCharacterCount = 0;

    /// <summary> "FINISH"画像 </summary>
    public Finish_UI _finish_UI;

    // ポストエフェクト
    [SerializeField] 
    public Volume _globalVolume;

    // 被写界深度
    private DepthOfField _depthOfField;

    // ゲーム終了してからのタイマー
    [SerializeField]
    public float _gameFinishTimer = 0.0f;

    // ランキング画像
    [SerializeField]
    public Image[] _rankingNumberImage = new Image[4];

    // EmissionHDR
    public Material _material;

    // 自身のゲーム順位
    [SerializeField]
    public int _myRanking;

    // コルーチンフラグ
    private bool _coroutineFlag = false;

    // イージング時間
    [SerializeField]
    private float _easingTime = 0.5f;

    // ランキング画像の拡縮値と色
    private Vector3  _startScale = new Vector3(1536.0f, 768.0f, 1.0f);
    private Vector3 _targetScale = new Vector3( 512.0f, 256.0f, 1.0f);
    private Color    _startColor = new Color  (   1.0f,   1.0f, 1.0f, 0.0f);
    private Color   _targetColor = new Color  (   1.0f,   1.0f, 1.0f, 1.0f);

    // 順位表示時に表示する自身のモデル
    [SerializeField]
    public GameObject _rankingCharacter;

    // "FIGHT"画像処理スクリプト
    [SerializeField]
    public Fight_UI _fight_UI;

    private void Awake()
    {
        // Singletonインスタンスがまだ存在しない場合、現在のインスタンスを設定
        if (Instance == null)
        {
            // インスタンスを設定
            Instance = this;
        }
        else
        {
            // すでにインスタンスが存在する場合、現在のオブジェクトを削除
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 初期設定
        _deadCharacterCount = 4;

        // ポストエフェクトから被写界深度を取得
        _globalVolume.profile.TryGet(out _depthOfField);

        // 被写界深度はなければ
        if (_depthOfField == null)
            Debug.LogError("被写界深度が見つかりません！！");

        // 一旦被写界深度を非表示
        _depthOfField.active = false;

        // ランキング画像は一旦非表示
        for (int Index = 0; Index < 4; Index++)
        {
            _rankingNumberImage[Index].gameObject.SetActive(false);

            // サイズと色の初期設定
            _rankingNumberImage[Index].rectTransform.sizeDelta = _startScale;
            _rankingNumberImage[Index].color                   = _startColor;
        }

        // 自身のゲーム順位を1位にしておく
        _myRanking = 1;

        // ランキング画像マテリアルを初期化
        _material.SetColor("_Color", Color.white);

        // 自身のモデルは一旦非表示
        _rankingCharacter.SetActive(false);

        // フラグ初期化
        _gameStartFLg  = false;
        _gameFinishFLg = false;
        _coroutineFlag = false;
    }

    // 更新処理
    private void Update()
    {
        // ゲーム開始！！！
        if (_fight_UI.GetCoroutineFlag2()) _gameStartFLg = true;

        // 死亡したキャラクターが3体を越えたら
        if (_deadCharacterCount <= 1)
        {
            // ゲーム終了フラグをON
            _gameFinishFLg = true;

            // "FINISH"画像演出開始
            _finish_UI.SetPlayEffect();

            // ゲーム終了からタイマーを起動
            _gameFinishTimer += Time.deltaTime;
        }

        // "FINISH"画像演出の2段階目が始まり & ゲーム終了から2秒後
        if (_finish_UI.GetCoroutineFlag2() && _gameFinishTimer >= 2.0f)
        {
            // 被写界深度"ON"
            _depthOfField.active = true;
        }

        // ゲーム終了から3秒後
        if (_gameFinishTimer > 3.0f)
        {
            // イージング
            if (!_coroutineFlag)
            {
                // リザルトBGMを再生
                AudioManager.instance.StopAllBGM();
                AudioManager.instance.Play(BGMPath.ResultBGM, AudioManager.ALL_VOLUME_VALUE, 0f, 1f, true);

                StartCoroutine(
                Scale(_rankingNumberImage[_myRanking - 1], _targetScale, _easingTime, Easing.Ease.OutQuad));

                // 透明値のイージングだけ気持ち短め
                StartCoroutine(
                Alpha(_rankingNumberImage[_myRanking - 1], _targetColor, _easingTime / 2.0f, Easing.Ease.OutQuad));

                _coroutineFlag = true;
            }

            // 自身のモデルを表示
            _rankingCharacter.SetActive(true);

            // ランキング画像を表示
            _rankingNumberImage[_myRanking - 1].gameObject.SetActive(true);

            // 少し光らせる
            _material.SetColor("_Color", Color.white * 1.5f);
        }


        // ゲームが終了していたら
        // ゲーム終了処理
        if (_gameFinishFLg) { GameFinish(); }
    }

    /// <summary> ゲーム終了処理 </summary>
    void GameFinish()
    {
        // シーン遷移

    }

    // 自身のゲーム順位を設定する関数
    public void SetMyRanking(int myRank) { _myRanking = myRank; }

    // ゲーム開始フラグを取得する関数
    public bool GetGameStartFLg()        { return _gameStartFLg; }

    // ゲーム終了フラグを取得する関数
    public bool GetGameFinishFLg()       { return _gameFinishFLg;     }

    // 死亡キャラクターカウント変数取得関数
    public int GetDeadCharacterCount()   { return _deadCharacterCount; }

    // 死亡キャラクターをカウント
    public void AddDeadCharacterCount()  { _deadCharacterCount -= 1; }

    // ゲーム終了してからのタイマーを取得する関数
    public float GetGameFinishTimer()    { return _gameFinishTimer; }

    public IEnumerator Scale(Image transform, Vector3 destinationScale, float seconds, Easing.Ease easing)
    {
        // イージング関数の取得
        var Ease = Easing.GetEasingMethod(easing);

        // 現在点と移動先の設定s
        Vector3 staScale = transform.rectTransform.sizeDelta;
        Vector3 endScale = destinationScale;
        // 初期地点と目標地点の差
        Vector3 difScale = endScale - staScale;

        // N秒かけて移動させる
        float e = 0;
        while (true)
        {
            yield return null;
            e += Time.deltaTime / seconds;
            if (e >= 1.0f)
            {
                transform.rectTransform.sizeDelta = endScale;
                break;
            }
            Vector3 nextPos = staScale + Ease(e) * difScale;
            transform.rectTransform.sizeDelta = nextPos;
        }
    }

    public IEnumerator Alpha(Image image, Color destinationAlpha, float seconds, Easing.Ease easing)
    {
        // イージング関数の取得
        var Ease = Easing.GetEasingMethod(easing);

        // 現在点と移動先の設定s
        Color staAlpha = image.color;
        Color endAlpha = destinationAlpha;
        // 初期地点と目標地点の差
        Color difAlpha = endAlpha - staAlpha;

        // N秒かけて移動させる
        float e = 0;
        while (true)
        {
            yield return null;
            e += Time.deltaTime / seconds;
            if (e >= 1.0f)
            {
                image.color = endAlpha;
                break;
            }
            Color nextPos = staAlpha + Ease(e) * difAlpha;
            image.color = nextPos;
        }
    }
}
