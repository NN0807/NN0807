using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_DamageState : AIBaseState
{
	// ステートに入った時
	public override void Enter()
	{
		// エージェントの物理挙動を有効化
		stateMachine.characterAI.agent.GetComponent<Rigidbody>().isKinematic = false;

		// エージェントの経路探索を無効化
		stateMachine.characterAI.agent.enabled = false;
	}

	// ステート更新処理
	public override void Update()
	{
		// 衝突後にRigidbodyが停止していたら
		if (stateMachine.characterAI.agent.GetComponent<Rigidbody>().IsSleeping())
        {
			stateMachine.ChangeState(new AI_IdleState(1f));
        }
	}

	// ギズモ表示
	public override void DrawGizmos()
	{

	}

	// ステートから出る時
	public override void Exit()
	{
		// エージェントの物理挙動を無効化
		stateMachine.characterAI.agent.GetComponent<Rigidbody>().isKinematic = true;

		// エージェントの経路探索を有効化
		stateMachine.characterAI.agent.enabled = true;
	}
}
