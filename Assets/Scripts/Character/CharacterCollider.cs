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

    // クリティカルポイントのレイヤー
    public LayerMask _criticalHitLayer;  

    // 攻撃を与えたCharacterManagerを取得し、保存しておく変数
    public CharacterManager _hitCharacterManager;

    // 攻撃が通常攻撃だった場合のフラグ
    private bool         _hitFlag = false;
    // 攻撃がクリティカル攻撃だった場合のフラグ
    private bool _criticalhitFlag = false;

    // 衝突したオブジェクトのコライダーの表面上で、最寄りの接触点を取得し、保存しておく変数
    private Vector3 _hitPoint;

    // 衝突相手の"剛体"を取得し、保存しておく変数
    private Rigidbody _rigidbody;

    // 攻撃の発生位置から相手に向かってのベクトル変数
    private Vector3 _attackDirection;

    // 複数回当たり判定関数に侵入するのを防ぐ
    public bool _hasWeaponEntered = false;
    public bool _hasCharacterEntered = false;

    public void Initialize(CharacterManager manager)
    {
        Debug.Log("CharacterCollider 初期化");
        // マネージャー登録
        _characterManager = manager;
        // データアセット設定
        characterParamAsset = Resources.Load<CharacterParamAsset>("CharacterParamAsset");
        // 変数初期化
        MoveForward = Vector3.zero;
        // レイヤー設定
        _criticalHitLayer = 1 << LayerMask.NameToLayer("CriticalPoint");
    }

    public void UpdatePart(CharacterManager manager)
    {
        Debug.Log("CharacterCollider 更新処理");

        // 自身の前方方向を取得
        MoveForward = manager.GetMoveForward();

        // 攻撃が当たった場合の吹き飛ばし処理を行う
        if (!_characterManager.GetHitStopFlag())
        {
            // 通常攻撃
            if (_hitFlag)
            {
                // 攻撃が当たった剛体があれば
                if (_rigidbody != null)
                {
                    // "脚部を"ふっ飛ばさせる！！！
                    _hitCharacterManager.GetLegRigidBody().AddForce(_attackDirection * characterParamAsset.Attack, ForceMode.Impulse);
                }

                // フラグをOFF
                _hitFlag = false;
            }

            // クリティカル攻撃
            if (_criticalhitFlag)
            {
                // 攻撃が当たった剛体があれば
                if (_rigidbody != null)
                {
                    // "脚部を"ふっ飛ばさせる！！！
                    _hitCharacterManager.GetLegRigidBody().AddForce(_attackDirection * characterParamAsset.Attack * 100.0f, ForceMode.Impulse);
                }

                // フラグをOFF
                _criticalhitFlag = false;
            }
        }
    }

    // 攻撃を受けた時に呼ばれる関数
    void OnCollisionEnter(Collision collision)
    {
        
    }

    // ステージギミック用、当たっている間に呼ばれる関数
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

    // 相手の攻撃を受けて、離れたら呼ばれる関数
    void OnCollisionExit(Collision collision)
    {
        // "火炎"との衝突から離れたら
        if (collision.gameObject.CompareTag("Effect")) 
        {
            Debug.Log("火炎oタグのオブジェクトから離れました");

            // イベント発火
            CollisionEFKExitEvent?.Invoke();

            // アニメーションを強制的に歩きアニメーションに遷移させる
            _characterManager.AnimationChange("Walk");
        }

        // 武器との衝突から離れたら
        if (collision.gameObject.CompareTag("Weapon"))
        {
            Debug.Log("武器タグのオブジェクトから離れました");

            // イベント発火
            CollisionEFKExitEvent?.Invoke();
        }
    }

    void OnTriggerExit(Collider other)
    {  
        // 武器と衝突し、離れたら
        if (other.gameObject.CompareTag("Weapon") && _hasWeaponEntered)
        {
            // 武器同士の衝突
            if (transform.gameObject.CompareTag("Weapon")) return;

            // 攻撃を与えたCharacterの"CharacterManager"を取得する
            if (_characterManager._characterNumber != other.transform.root.gameObject.GetComponent<CharacterManager>()._characterNumber)
            {
                // 相手が攻撃中なら
                if (other.transform.root.gameObject.GetComponent<CharacterManager>().IsCurrentlyAttacking())
                {
                    Debug.Log($"衝突し、離れた武器オブジェクト: {other.gameObject.name}");

                    // Triggerから出たらリセット
                    _hasWeaponEntered = false;

                    // イベント発火
                    CollisionEFKExitEvent?.Invoke();
                }  
            }
        }

        // 武器が相手から離れたら
        if ((other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Player")) &&
        _hasCharacterEntered)
        {
            // 自分が攻撃中なら
            if (this.transform.root.gameObject.GetComponent<CharacterManager>().IsCurrentlyAttacking()) 
            {
                Debug.Log($"衝突し、離れた体部パーツ: {other.gameObject.name}");

                // Triggerから出たらリセット
                _hasCharacterEntered = false;

                // イベント発火
                CollisionEFKExitEvent?.Invoke();
            } 
        }
    }

    // 攻撃を与えた時に呼ばれる関数(Trigger版)
    void OnTriggerEnter(Collider collision)
    {
        // 武器と衝突したら
        if (collision.gameObject.CompareTag("Weapon") && !_hasWeaponEntered)
        {
            // 武器同士の衝突
            if (transform.gameObject.CompareTag("Weapon")) return;

            // 攻撃を与えたCharacterの"CharacterManager"を取得する
            _hitCharacterManager = collision.transform.root.gameObject.GetComponent<CharacterManager>();

            // 自身の体に攻撃が当たった場合に、コリジョン処理を無効化する
            if (_characterManager._characterNumber != _hitCharacterManager._characterNumber)
            {
                // 相手が攻撃中なら
                if (collision.transform.root.gameObject.GetComponent<CharacterManager>().IsCurrentlyAttacking()) 
                {
                    // すでに処理済みなら無視
                    if (_hasWeaponEntered) return;
                    _hasWeaponEntered = true;

                    Debug.Log($"衝突した武器オブジェクト: {collision.gameObject.name}");

                    Debug.Log("攻撃を受けました");

                    // イベント発火
                    CollisionEFKStayEvent?.Invoke();

                    // ヒットアニメーション再生
                    _characterManager.SetAnimations(AnimationType.Hit);

                    // ヒットストップ演出
                    _characterManager.HitStop();
                }  
            }
        }

        // Enemy(Player)と衝突したら
        if ((collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player")))
        {
            // 攻撃を与えたCharacterの"CharacterManager"を取得する
            _hitCharacterManager = collision.transform.root.gameObject.GetComponent<CharacterManager>();

            // 自身の子オブジェクト同士の衝突を回避
            if (_characterManager._characterNumber != _hitCharacterManager._characterNumber)
            {
                // 自分が攻撃中なら
                if (this.transform.root.gameObject.GetComponent<CharacterManager>().IsCurrentlyAttacking())
                {
                    // すでに処理済みなら無視
                    if (_hasCharacterEntered) return;
                    _hasCharacterEntered = true;

                    // 衝突相手の"剛体"を取得
                    _rigidbody = collision.GetComponent<Rigidbody>();

                    // 衝突方向を算出する
                    _attackDirection = (collision.transform.position - _characterManager.GetLegParts().transform.position).normalized;

                    // 衝突したオブジェクトのコライダーの表面上で、最寄りの接触点を取得
                    _hitPoint = collision.ClosestPoint(transform.position);

                    // 攻撃の発生位置から相手に向かってレイキャストを行う
                    RaycastHit _hit;

                    // レイキャスト
                    if (Physics.Raycast(transform.position, _attackDirection, out _hit, Mathf.Infinity, _criticalHitLayer) &&
                        !_hitFlag)
                    {
                        // クリティカルポイントにヒットした場合
                        Debug.Log("弱点に衝突しました");

                        // 衝突した場所（hitPoint）にクリティカルヒットエフェクトを再生させる
                        EffectManager.Instance.PlayEffect("CriticalHitEffect", _hitPoint);

                        // 攻撃を与えた相手の"CharacterManager"の速度計算を停止
                        _hitCharacterManager.SetImpulse();

                        // 攻撃が当たった剛体があれば
                        if (_rigidbody != null)
                        {
                            Debug.Log($"弱点に衝突したオブジェクト: {collision.gameObject.name}");
                        }

                        // ヒットストップ演出
                        _characterManager.HitStop();

                        // クリティカル攻撃フラグをON
                        _criticalhitFlag = true;
                    }
                    else
                    {
                        // クリティカルポイントにヒットしなかった場合、通常の衝突処理
                        Debug.Log("攻撃が衝突しました");

                        // 衝突した場所（hitPoint）にヒットエフェクトを再生させる
                        EffectManager.Instance.PlayEffect("NormalHitEffect", _hitPoint);

                        // 攻撃を与えた相手の"CharacterManager"の速度計算を停止
                        _hitCharacterManager.SetImpulse();

                        // 攻撃が当たった剛体があれば
                        if (_rigidbody != null)
                        {
                            Debug.Log($"衝突したオブジェクト: {collision.gameObject.name}");
                        }

                        // 自身のヒットストップ演出
                        _characterManager.HitStop();

                        // 通常攻撃フラグをON
                        _hitFlag = true;
                    }
                }
            }
        } 
    }
}
