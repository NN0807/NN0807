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
               _stateInfo.IsName("Hit")    ? "Idle"   :
               _stateInfo.IsName("Attack") ? "Attack" : "None";
    }

    public bool GetAnimationFlag()   { return _animationFlag; }

    public void AnimationEvent()     { _animationFlag = !_animationFlag; }

    public void SetWalkAnimation()   { _animator.SetTrigger("Walk"); _animator.ResetTrigger("Idle"); }

    public void SetIdleAnimation()   { _animator.SetTrigger("Idle"); _animator.ResetTrigger("Walk"); }

    public void SetAttackAnimation() { _animator.SetTrigger("Attack"); }

    public void SetHitAnimation()    { _animator.SetTrigger("Hit");    }
}
