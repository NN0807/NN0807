using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void Initialize(CharacterManager manager)
    {
        Debug.Log("CharacterCollider 初期化");
        _animator = GetComponent<Animator>();
    }

    public void UpdatePart(CharacterManager manager)
    {
        Debug.Log("CharacterCollider 更新処理");
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

    // 攻撃アニメーションフラグ取得関数
    public bool GetAttackAnimationFlag() { return _attackAnimationFlag; }

    // 攻撃アニメーションフラグ取得設定関数
    public void AttackAnimationEvent()   { _attackAnimationFlag = !_attackAnimationFlag; }

    // アニメーションフラグ取得関数
    public bool GetAnimationFlag()       { return _animationFlag; }

    // アニメーションフラグ設定関数
    public void AnimationEvent()         { _animationFlag = !_animationFlag; }

    public void SetWalkAnimation()       { _animator.SetTrigger("Walk"); _animator.ResetTrigger("Idle"); _animator.ResetTrigger("Attack"); }

    public void SetIdleAnimation()       { _animator.SetTrigger("Idle"); _animator.ResetTrigger("Walk"); _animator.ResetTrigger("Attack"); }

    public void SetAttackAnimation()     { _animator.SetTrigger("Attack"); _animator.ResetTrigger("Walk"); _animator.ResetTrigger("Idle"); }

    public void SetHitAnimation()        { _animator.SetTrigger("Hit");    }
}
