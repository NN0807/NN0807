using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayerController : MonoBehaviour
{
    public Animator bodyAnimator;
    public Animator legsAnimator;
    public Animator punchAnimator;

    [SerializeField] private GameObject punchModel;
    [SerializeField] private Transform punchBone;


    // パンチが出現している時間
    public float punchDuration = 0.2f;

    private Collider punchCollider;

    public float moveSpeed = 5f;
    public float dashSpeed = 10f;
    public float dashDuration = 0.2f;

    private bool isDashing = false;

    private float totalMoveTime = 0f;

    public bool HasMoved { get; private set; } = false;
    public bool HasDashed { get; private set; } = false;
    public bool HasPunched { get; private set; } = false;



    // アニメーションイベントフラグ
    private bool _animationFlag;

    // 攻撃アニメーションイベントフラグ
    private bool _attackAnimationFlag;

    // 攻撃アニメーションフラグ取得関数
    public bool GetAttackAnimationFlag() { return _attackAnimationFlag; }

    // 攻撃アニメーションフラグ取得設定関数
    public void AttackAnimationTrueEvent() { _attackAnimationFlag = true; }
    public void AttackAnimationFalseEvent() { _attackAnimationFlag = false; }

    // アニメーションフラグ取得関数
    public bool GetAnimationFlag() { return _animationFlag; }

    // アニメーションフラグ設定関数
    public void AnimationEvent() { _animationFlag = !_animationFlag; }



    private void Start()
    {

        punchModel.SetActive(false);
        punchCollider = punchBone.GetComponent<Collider>();
        if (punchCollider != null)
        {
            // 当たり判定も無効化s
            punchCollider.enabled = false;
        }
    }



    private void Update()
    {
        if (TutorialManager.Instance.NowEraser)
        {
            if (TutorialManager.Instance.CanMove)
            {
                HandleMovement();
            }
            if (TutorialManager.Instance.CanDash)
            {
                HandleDash();
            }
            if (TutorialManager.Instance.CanPunch)
            {
                HandlePunch();
            }
        }


    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // 正規化して一定の速度を維持
        Vector3 moveDirection = new Vector3(moveX, 0, moveY).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            // フラグを更新
            HasMoved = true;
            totalMoveTime += Time.deltaTime;

            // 旋回処理
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

            bodyAnimator.SetTrigger("Walk");
            legsAnimator.SetTrigger("Walk");

            TutorialManager.Instance.RegisterPlayerAction("Walk");
        }
    }

    void HandleDash()
    {
        if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) &&
            Input.GetMouseButtonDown(1))
        {
            StartCoroutine(DashCoroutine());
        }
    }

    IEnumerator DashCoroutine()
    {
        isDashing = true;
        HasDashed = true;

        float originalSpeed = moveSpeed;
        moveSpeed = dashSpeed;

        TutorialManager.Instance.RegisterPlayerAction("Dash");

        yield return new WaitForSeconds(dashDuration);

        moveSpeed = originalSpeed;
        isDashing = false;
    }

    void HandlePunch()
    {
        if(Input.GetMouseButtonDown(0))
        {
            HasPunched = true;
            bodyAnimator.SetTrigger("Attack");
            legsAnimator.SetTrigger("Attack");

            punchModel.SetActive(true);
            punchAnimator.SetTrigger("Attack");

            // 当たり判定をonにする
            punchCollider.enabled = true;

            StartCoroutine(HidePunchAfterDelay(0.2f));

            TutorialManager.Instance.RegisterPlayerAction("Attack");
        }
    }

    private IEnumerator HidePunchAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // 当たり判定off
        punchCollider.enabled = false;

        //punchModel.SetActive(false);
    }

    public void ResetActions()
    {
        HasMoved = false;
        HasDashed = false;
        HasPunched = false;
    }

    public void ResetPlayerState()
    {
        transform.position = new Vector3(0, 0, 2);
        transform.rotation = Quaternion.Euler(0, 180, 0);
        ResetActions();
        gameObject.SetActive(true);
    }
}
