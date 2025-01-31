using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_AttackState : AIBaseState
{
	bool once = false;

	// ステートに入った時
	public override void Enter()
	{
		// キャラクターAI取得
		CharacterAI characterAI = stateMachine.characterAI;

		// 攻撃アニメーション
		characterAI.characterManager.SetAnimations(Common.AnimationType.Attack);

		// 目標地点をリセット
		characterAI.agent.ResetPath();

		// 加速度を０に
		characterAI.agent.velocity = default;

		// メンタル加算
		characterAI.AddMental(0.25f);

		once = false;
	}

	// ステート更新処理
	public override void Update()
	{
		// キャラクターAI取得
		CharacterAI characterAI = stateMachine.characterAI;

		if(once == false)
        {
			once = !once;
			return;
        }

        // 攻撃が終了したら
        if (characterAI.characterManager.IsCurrentlyAttacking() == false)
        {
            // メンタルがたまっていたら逃げる
            if (characterAI.ConductLottery(characterAI.mental))
            {
                // 逃走ステートへ
                stateMachine.ChangeState(new AI_EscapeState());
            }
            // 攻撃性が高ければもう一度攻撃
            else if (characterAI.ConductLottery(characterAI.aIParam.Aggressiveness))
            {
                // もう一度攻撃
                stateMachine.ChangeState(new AI_PursuitState());
            }
            else
            {
                // 逃走ステートへ
                stateMachine.ChangeState(new AI_EscapeState());
            }
        }
    }

	// ギズモ
	public override void DrawGizmos()
	{

	}

	// ステートから出る時
	public override void Exit()
	{

	}
}
