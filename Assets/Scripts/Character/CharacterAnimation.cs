using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;


[RequireComponent(typeof(Animator))]
public class CharacterAnimation : MonoBehaviour,ICharacterPart
{
    // アニメーター
    [SerializeField]
    private Animator _animator;

    // アニメーションイベントフラグ
    private bool _animationFlag;

    // 攻撃アニメーションイベントフラグ
    private bool _attackAnimationFlag;

    // 自身のパーツタイプをインスペクター側で設定
    [EnumDisplayNamesAttribute(typeof(PartsType), "脚部", "体部", "武器")]
    public PartsType _partsType;

    // ダッシュ中のアニメーション速度変数
    [SerializeField]
    public float _activateDashAnimationSpeed    = 0.0f;
    // ダッシュ以外のアニメーション速度変数
    private float _deactivatedashAnimationSpeed = 1.0f;

    /// <summary> ヒットストップ時間(秒) </summary>
    public float _hitStopTime  = 0.3f;
    public float _hitStopTimer = 0.0f;

    // ヒットストップフラグ
    private bool _hitStopFlag  = false;

    // デフォルトのアニメーション速度を保存しておく変数
    private float _defaultAnimationSpeed = 0.0f;

    public void Initialize(CharacterManager manager)
    {
        Debug.Log("CharacterAnimation 初期化");
        _animator = GetComponent<Animator>();

        // デフォのアニメーション速度を保存しておく
        _deactivatedashAnimationSpeed = _animator.speed;
    }

    public void UpdatePart(CharacterManager manager)
    {
        Debug.Log("CharacterAnimation 更新処理");

        // ダッシュ中のアニメーション速度を更新する
        // 脚部パーツのみ、その他パーツは若干、、
        _activateDashAnimationSpeed = _partsType == PartsType.Leg ? 4.0f : 2.0f;

        // ヒットストップフラグがONになれば
        if(_hitStopFlag)
        {
            // アニメーション速度を保存しておく
            _defaultAnimationSpeed = _animator.speed;
            // モーションを止める
            _animator.speed = 0.0f;

            // タイマー起動
            _hitStopTimer += Time.deltaTime;
            // タイマーがヒットストップ時間を越えたら
            if (_hitStopTimer > _hitStopTime) 
            {
                // アニメーション速度を元に戻す
                _animator.speed = _defaultAnimationSpeed;
                // ヒットストップフラグをOFF
                _hitStopFlag = false;
                // タイマーリセット
                _hitStopTimer = 0.0f;
            }
        }
    }

    public string GetCurrentAnimation() 
    {
        // 現在のアニメーションステートを取得
        AnimatorStateInfo _stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        // ステート名を取得して返す
        return _stateInfo.IsName("Idle")   ? "Idle"   :
               _stateInfo.IsName("Walk")   ? "Walk"   :
               _stateInfo.IsName("Hit")    ? "Hit"    :
               _stateInfo.IsName("Attack") ? "Attack" : "None";
    }

    // イベント登録をCharacterAnimation内で行う
    public void RegisterOperationEvent(CharacterOperation operation)
    {
        if (!operation) return;
        // イベントに関数を登録
        operation.ActivateDashEvent   += ActivateDash;
        operation.DeactivateDashEvent += DeactivateDash;
    }

    // アニメーション強制遷移関数
    public void AnimationChange(string animationName)
    {
        // 一旦全アニメーションをリセット
        _animator.ResetTrigger("Idle"); 
        _animator.ResetTrigger("Walk");
        _animator.ResetTrigger("Attack"); 
        _animator.ResetTrigger("Hit");
        // 遷移したいアニメーションを宣言
        _animator.SetTrigger(animationName);
    }

    // ヒットストップ関数
    public void HitStop()                  { _hitStopFlag = true; }

    // ダッシュイベント
    private void ActivateDash()            { _animator.speed = _activateDashAnimationSpeed; }

    // ダッシュ解除イベント                                                                                                                
    private void DeactivateDash()          { _animator.speed = _hitStopFlag ? 0.0f : _deactivatedashAnimationSpeed; }

    // 攻撃アニメーションフラグ取得関数                                                                                                    
    public bool GetAttackAnimationFlag()   { return _attackAnimationFlag; }

    // 攻撃アニメーションフラグ取得設定関数                                                                                                
    public void AttackAnimationEvent()     { _attackAnimationFlag = !_attackAnimationFlag; }

    // アニメーションフラグ取得関数                                                                                                        
    public bool GetAnimationFlag()         { return _animationFlag; }

    // アニメーションフラグ設定関数
    public void AnimationEvent()           { _animationFlag = !_animationFlag; }
                                           
    public void SetWalkAnimation()         { _animator.SetTrigger("Walk"); _animator.ResetTrigger("Idle"); }
                                           
    public void SetIdleAnimation()         { _animator.SetTrigger("Idle"); _animator.ResetTrigger("Walk"); }
                                           
    public void SetAttackAnimation()       { _animator.SetTrigger("Attack");}
                                           
    public void SetHitAnimation()          { _animator.SetTrigger("Hit"); _animator.ResetTrigger("Idle"); _animator.ResetTrigger("Walk"); }

    public void SetHitEarlyExitAnimation() { _animator.SetTrigger("HitEarlyExit"); }
}
