using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSandBag : MonoBehaviour
{

    public Animator bodyAnimator;
    public Animator legsAnimator;

    // Start is called before the first frame update
    void Start()
    {
        // 初期状態で待機モーション
        PlayIdleAnimation();
    }

    void PlayIdleAnimation()
    {
        bodyAnimator.SetTrigger("Idle");
        legsAnimator.SetTrigger("Idle");
    }

    void PlayerHitAnimation()
    {
        bodyAnimator.SetTrigger("Hit");
        legsAnimator.SetTrigger("Hit");
    }
}
