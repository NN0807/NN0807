using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Common;

public class CharacterCollider : MonoBehaviour, ICharacterPart
{
    // データアセット
    public CharacterParamAsset characterParamAsset;

    // マネージャー
    private CharacterManager _characterManager;

    // CharacterMoveから前方方向を取得し、保存する変数
    private Vector3 MoveForward;

    // 衝突中(Effect)イベント
    public event Action CollisionEFKStayEvent;
    // 衝突終(Effect)イベント
    public event Action CollisionEFKExitEvent;
    // 衝突中(攻撃)  イベント
    public event Action<Vector3, float> CollisionAttackEnterEvent;

    public void Initialize(CharacterManager manager)
    {
        Debug.Log("CharacterCollider 初期化");
        // マネージャー登録
        _characterManager = manager;
        // データアセット設定
        characterParamAsset = Resources.Load<CharacterParamAsset>("CharacterParamAsset");
        // 変数初期化
        MoveForward = Vector3.zero;
    }

    public void UpdatePart(CharacterManager manager)
    {
        Debug.Log("CharacterCollider 更新処理");

        // 自身の前方方向を取得
        MoveForward = manager.GetMoveForward();
    }

    // 当たった時に呼ばれる関数
    void OnCollisionEnter(Collision collision)
    {
        // 武器と衝突したら
        if (collision.gameObject.CompareTag("Weapon")) 
        {
            Debug.Log("攻撃を受けました");

            // イベント発火
            CollisionEFKStayEvent?.Invoke();

            CollisionAttackEnterEvent?.Invoke(-MoveForward.normalized, characterParamAsset.Attack);

            // ヒットアニメーション再生
            _characterManager.SetAnimations(AnimationType.Hit);
        }
    }

    // 当たっている間に呼ばれる関数
    void OnCollisionStay(Collision collision)
    {
        // "火炎"に衝突したら
        if (collision.gameObject.CompareTag("Effect"))
        {
            Debug.Log("Effectタグのオブジェクトに衝突しました");

            // イベント発火
            CollisionEFKStayEvent?.Invoke();

            // ヒットアニメーション再生
            _characterManager.SetAnimations(AnimationType.Hit);
        }
    }

    // 離れたら呼ばれる関数
    void OnCollisionExit(Collision collision)
    {
        // "火炎"or武器との衝突から離れたら
        if (collision.gameObject.CompareTag("Effect") ||
            collision.gameObject.CompareTag("Weapon") ||
            collision.gameObject.CompareTag("Enemy")) 
        {
            Debug.Log("火炎or武器タグのオブジェクトから離れました");

            // イベント発火
            CollisionEFKExitEvent?.Invoke();
        }
    }

    // 当たった時に呼ばれる関数(Trigger版)
    void OnTriggerEnter(Collider collision)
    {
        // Enemy(Player)と衝突したら
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player")) 
        {
            Debug.Log("攻撃が衝突しました");
            // イベント発火
            // 攻撃中は、、、、
            CollisionAttackEnterEvent?.Invoke(-MoveForward,100.0f);

            // 衝突したオブジェクトのコライダーの表面上で、最寄りの接触点を取得
            Vector3 _hitPoint = collision.ClosestPoint(transform.position);

            // 衝突した場所（hitPoint）にヒットエフェクトを再生させる
            EffectManager.Instance.PlayEffect("NormalHitEffect", _hitPoint);

            _characterManager.HitStop();
        }

        // クリティカルポイントと衝突したら
        if (collision.gameObject.CompareTag("CriticalPoint"))
        {
            Debug.Log("弱点に衝突しました");
            // イベント発火
            CollisionAttackEnterEvent?.Invoke(MoveForward, characterParamAsset.Attack * 100.0f);
        }
    }
}
