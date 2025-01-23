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
                // 衝突した場所（hitPoint）にヒットエフェクトを再生させる
                EffectManager.Instance.PlayEffect("NormalHitEffect", _hitPoint);

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
                // 衝突した場所（hitPoint）にヒットエフェクトを再生させる
                EffectManager.Instance.PlayEffect("CriticalHitEffect", _hitPoint);

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
        // 武器と衝突したら
        if (collision.gameObject.CompareTag("Weapon")) 
        {
            Debug.Log("攻撃を受けました");

            // イベント発火
            CollisionEFKStayEvent?.Invoke();

            // ヒットアニメーション再生
            _characterManager.SetAnimations(AnimationType.Hit);

            // 攻撃を与えたキャラのcharacterManagerを取得
            //_hitCharacterManager = this.gameObject.transform.parent.GetComponent<CharacterManager>();

            //// 衝突相手の"剛体"を取得
            //Rigidbody _rigidbody = this.gameObject.GetComponent<Rigidbody>();
            // ふっ飛ばさせる！！！
            //_hitCharacterManager.GetLegRigidBody().AddForce(-MoveForward * 50.0f, ForceMode.Impulse);

            // ヒットストップ演出
            _characterManager.HitStop();

            // 自身の衝撃判定をON
            _characterManager.SetImpulse();

            //_hitFlag = true;
        }
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

    // 攻撃を与えた時に呼ばれる関数(Trigger版)
    void OnTriggerEnter(Collider collision)
    {
        // Enemy(Player)と衝突したら
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player")) 
        {
            // 攻撃を与えたCharacterの"CharacterManager"を取得する
            _hitCharacterManager = collision.transform.parent.GetComponent<CharacterManager>();

            // 自身の体に攻撃が当たった場合に、コリジョン処理を無効化する
            if (_characterManager._characterNumber != _hitCharacterManager._characterNumber)
            {
                // 自分がAIかどうかを判定
                //CharacterAI characterAI = collision.transform.parent.GetComponent<CharacterAI>();
                //if (characterAI != null)
                //{
                //    // AIならダメージステートに遷移処理
                //    characterAI.stateMachine.ChangeState(new AI_DamageState());
                //}

                // 衝突相手の"剛体"を取得
                _rigidbody = collision.gameObject.GetComponent<Rigidbody>();

                // 衝突したオブジェクトのコライダーの表面上で、最寄りの接触点を取得
                _hitPoint = collision.ClosestPoint(transform.position);

                // 攻撃の発生位置から相手に向かってレイキャストを行う
                RaycastHit _hit;
                _attackDirection = (_rigidbody.position - _hitPoint).normalized;

                // レイキャスト
                if (Physics.Raycast(transform.position, _attackDirection, out _hit, Mathf.Infinity, _criticalHitLayer))
                {
                    // クリティカルポイントにヒットした場合
                    Debug.Log("弱点に衝突しました");

                    // クリティカル攻撃フラグをON
                    _criticalhitFlag = true;

                    // 攻撃を与えたキャラのcharacterManagerを取得
                    _hitCharacterManager = collision.gameObject.transform.parent.GetComponent<CharacterManager>();

                    // ヒットストップ演出
                    _characterManager.HitStop();
                }
                else
                {
                    // クリティカルポイントにヒットしなかった場合、通常の衝突処理
                    Debug.Log("攻撃が衝突しました");

                    // 通常攻撃フラグをON
                    _hitFlag = true;

                    // 攻撃を与えたキャラのcharacterManagerを取得
                    _hitCharacterManager = collision.gameObject.transform.parent.GetComponent<CharacterManager>();

                    // ヒットストップ演出
                    _characterManager.HitStop();
                }
            }
        }
    }
}
