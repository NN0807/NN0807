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

    /// <summary> ゲーム終了フラグ </summary>
    [ReadOnly]
    public bool gameFinishFLg = false;

    /// <summary> ポーズフラグ </summary>
    [ReadOnly]
    public bool pauseFLg = false;

    /// <summary> キャラクターズ </summary>
    [SerializeField]
    public CharacterManager[] characterManagers = new CharacterManager[CharacterConst.CHARACTER_NUM];

    /// <summary> 死亡したキャラクターをカウント </summary>
    [SerializeField]
    public int _deadCharacterCount = 0;

    /// <summary> "FINISH"画像 </summary>
    public Finish_UI _finish_UI;


    [SerializeField] Volume globalVolume;
    [SerializeField] GameObject menuCanvas;
    private DepthOfField depthOfField;


    public float _gameFinishTimer = 0.0f;

    public Material material;
    public float alphaLevel = 1.0f; // 変更したいアルファレベルの値

    // ランキング画像
    [SerializeField]
    public Image[] _rankingNumberImage = new Image[4];

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
        _deadCharacterCount = 0;

        globalVolume.profile.TryGet(out depthOfField);

        if (depthOfField == null)
            Debug.LogError("被写界深度が見つかりません！！");
        depthOfField.active = false;
        alphaLevel = 0.0f;


        // ランキング画像は一旦非表示
        for (int Index = 0; Index < 4; Index++)
        {
            _rankingNumberImage[Index].gameObject.SetActive(false);
        }
    }

    // 更新処理
    private void Update()
    {
        // 死亡したキャラクターが3体を越えたら
        if (_deadCharacterCount >= 1)
        {
            // ゲーム終了フラグをON
            gameFinishFLg = true;
            _finish_UI.SetPlayEffect();

            _gameFinishTimer += Time.deltaTime;
        }

        if (_finish_UI.GetCoroutineFlag2() && _gameFinishTimer >= 2.0f)
        {
            // 被写界深度"ON"
            depthOfField.active = true;
        }

        if (_gameFinishTimer > 3.0f)
        {
            // ランキング画像は一旦非表示
            for (int Index = 0; Index < 4; Index++)
            {
                _rankingNumberImage[Index].gameObject.SetActive(true);
            }


        }

        if (material != null)
        {
            // シェーダーの _AlphaLevel プロパティを変更
            material.SetFloat("_AlphaLevel", alphaLevel);
        }

        // ゲームが終了していたら
        // ゲーム終了処理
        if (gameFinishFLg) { GameFinish(); }

    }

    /// <summary> ゲーム終了処理 </summary>
    void GameFinish()
    {
        // シーン遷移
        // 現在はゲームを落とす
        //Application.Quit();
    }

    // ゲーム終了フラグを取得する関数
    public bool GetGameFinishFLg() { return gameFinishFLg; }

    // 死亡キャラクターをカウント
    public void AddDeadCharacterCount() { _deadCharacterCount += 1; }
}
