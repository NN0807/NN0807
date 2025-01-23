using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    // データアセット
    public CharacterParamAsset characterParamAsset;

    // プレイヤーオブジェクト
    public Transform _playerTransform;

    // キャンバスオブジェクト
    public GameObject _canvas;

    // スタミナゲージ画像
    [SerializeField]
    private Image StaminaGauge     = default;

    // スタミナゲージ背景画像
    [SerializeField]
    private Image StaminaGaugeBack = default;

    // 現在のスタミナ
    private float _currentStamina;

    //[SerializeField]
    private Quaternion Rotate = Quaternion.Euler(-38.0f, 0.0f, 0.0f);

    // EmissionHDR
    public Material _material;

    // 基本の強度
    private float _baseIntensity = 1.4f;

    // 点滅の速さ
    [SerializeField]
    public float  _blinkSpeed    = 20.0f; 

    // 通常色を保存する変数
    private Color _originalColor; 

    // ダッシュが可能か不可能かの判定フラグ
    private bool _canDashFlag = true;

    // Start is called before the first frame update
    void Start()
    {
        // データアセット設定
        characterParamAsset = Resources.Load<CharacterParamAsset>("CharacterParamAsset");

        // スタミナ設定
        _currentStamina = characterParamAsset.MaxStamina;

        _material.SetColor("_Color", Color.white * 3.0f);

        // マテリアルの初期カラーを保存
        _originalColor = _material.GetColor("_Color");
    }

    // Update is called once per frame
    void Update()
    {
        // Canvasの回転を常にリセット
        _canvas.transform.rotation = Quaternion.identity;
        // スタミナゲージを反映する
        StaminaGauge.fillAmount = _currentStamina / characterParamAsset.MaxStamina;

        // ダッシュする事が可能か不可能か判定する
        if      ( _currentStamina <= 0.0f) _canDashFlag = false;
        // ダッシュが不可能になり、スタミナが50%回復したら再度可能に
        else if (!_canDashFlag && _currentStamina >= characterParamAsset.MaxStamina * 0.5f)
                  _canDashFlag = true;

        // キャンバス表示非表示
        if (_currentStamina >= characterParamAsset.MaxStamina) _canvas.SetActive(false);
        else                                                   _canvas.SetActive(true);

        // 3D空間を移動するオブジェクトに追従させる為、
        // スタミナゲージの向きを固定する
        StaminaGaugeBack.rectTransform.rotation = Rotate;

        // スタミナゲージの色更新関数を呼ぶ
        UpdateEmissionColor();
    }

    // スタミナゲージの色更新関数
    public void UpdateEmissionColor()
    {
        // スタミナが40%以下なら
        if (_currentStamina <= characterParamAsset.MaxStamina * 0.4f)
        {  
            Debug.Log("スタミナが40%以下です！");

            // 時間に基づいて強度を変動させる
            float intensity = 0.0f;
            // 消費中は点滅させ、回復中は点滅させない
            if (_canDashFlag) intensity = _baseIntensity + Mathf.Sin(Time.time * _blinkSpeed) * 0.5f; // 0.5fは変動の幅
            else              intensity = _baseIntensity;
            // 赤にする
            _material.SetColor("_Color", Color.red * intensity);
        }
        else
        {
            // 通常色に戻す
            _material.SetColor("_Color", _originalColor);
        }
    }

    // ダッシュ中の処理関数
    private void ActivateDash()
    {
        // スタミナを減少させる
        _currentStamina = Mathf.Max(0f, _currentStamina - Time.deltaTime);
    }

    // ダッシュ解除処理関数
    private void DeactivateDash()
    {
        // スタミナを回復させる(1.25倍の速度で回復させる)
        _currentStamina = Mathf.Min(_currentStamina + Time.deltaTime * 1.25f, characterParamAsset.MaxStamina);
    }

    // イベント登録をCharacterUI内で行う
    public void RegisterOperationEvent(CharacterOperation operation)
    {
        if (!operation) return;
        // イベントに関数を登録
        operation.ActivateDashEvent   += ActivateDash;
        operation.DeactivateDashEvent += DeactivateDash;
    }


    // ダッシュが可能か不可能かの判定フラグ取得関数
    public bool GetCanDashFlag() { return _canDashFlag; }
}
