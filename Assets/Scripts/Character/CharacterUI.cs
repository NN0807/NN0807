using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    // データアセット
    public CharacterParamAsset characterParamAsset;

    // スタミナゲージ画像
    [SerializeField]
    private Image StaminaGauge = default;

    // 現在のスタミナ
    private float _currentStamina;


    // Start is called before the first frame update
    void Start()
    {
        // データアセット設定
        characterParamAsset = Resources.Load<CharacterParamAsset>("CharacterParamAsset");

        // スタミナ設定
        _currentStamina = characterParamAsset.MaxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        // スタミナゲージを反映する
        StaminaGauge.fillAmount = _currentStamina / characterParamAsset.MaxStamina;

        // 3D空間を移動するオブジェクトに追従させる為、
        // スタミナゲージの向きを固定する
        StaminaGauge.rectTransform.rotation = Quaternion.Euler(-38.0f, 0.0f, 0.0f);
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
        // スタミナを回復させる
        _currentStamina = Mathf.Min(_currentStamina + Time.deltaTime * 2, characterParamAsset.MaxStamina);
    }

    // イベント登録をCharacterUI内で行う
    public void RegisterOperationEvent(CharacterOperation operation)
    {
        // イベントに関数を登録
        operation.ActivateDashEvent += ActivateDash;
        operation.DeactivateDashEvent += DeactivateDash;
    }
}
