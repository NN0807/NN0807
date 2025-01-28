using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayerController : MonoBehaviour
{
    public Animator bodyAnimator;
    public Animator legsAnimator;
    public Animator punchAnimator;

    public float moveSpeed = 5f;
    public float dashSpeed = 10f;
    public float dashDuration = 0.2f;

    private bool isDashing = false;

    private float totalMoveTime = 0f;

    public bool HasMoved { get; private set; } = false;
    public bool HasDashed { get; private set; } = false;
    public bool HasPunched { get; private set; } = false;

    private void Update()
    {
        if(TutorialManager.Instance.CanMove)
        {
            HandleMovement();
            //HandleDash();
            //HandlePunch();
        }
        if(TutorialManager.Instance.CanDash)
        {
            HandleDash();
        }
        if(TutorialManager.Instance.CanPunch)
        {
            HandlePunch();
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
        else
        {
            bodyAnimator.ResetTrigger("Walk");
            legsAnimator.ResetTrigger("Walk");
        }
    }

    void HandleDash()
    {
        if((Input.GetAxis("Horizontal")!=0 || Input.GetAxis("Vertical") != 0) && 
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
            //bodyAnimator.SetTrigger("Attack");
            //legsAnimator.SetTrigger("Attack");
            //punchAnimator.SetTrigger("Attack");
        }
    }

    public void ResetActions()
    {
        HasMoved = false;
        HasDashed = false;
        HasPunched = false;
    }

}
