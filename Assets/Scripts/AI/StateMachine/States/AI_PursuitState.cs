using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_PursuitState : AIBaseState
{
	/// <summary>
	/// 諦めるまでの時間
	/// </summary>
	private float giveUpTimer = 5f;

	/// <summary>
	/// 追跡するターゲット
	/// </summary>
	Transform targetTransform = null;

	// ステートに入った時
	public override void Enter()
	{
		// キャラクターAI取得
		CharacterAI characterAI = stateMachine.characterAI;

		// 歩きアニメーション
		characterAI.characterManager.SetAnimations(Common.AnimationType.Walk);

		// キャラクターオブジェクトを取得
		targetTransform = characterAI.otherCharcterObjects[Random.Range(0, characterAI.otherCharcterObjects.Count)].transform.GetChild(0).transform;
		// NavMeshAgentに目標地点を設定
		characterAI.agent.SetDestination(targetTransform.position);

		// 走る
		characterAI.SetDashSpeed();

		// メンタル加算
		characterAI.AddMental(0.1f);

		// タイマー初期化
		giveUpTimer = 5f;
	}

	// ステート更新処理
	public override void Update()
	{
		// キャラクターAI取得
		CharacterAI characterAI = stateMachine.characterAI;

		// もし追跡中のキャラが死亡していたら
		if(targetTransform == null)
        {
			// 待機ステートへ
			stateMachine.ChangeState(new AI_IdleState(0.1f));
			return;
        }

		// NavMeshAgentに目標地点を設定
		characterAI.agent.SetDestination(targetTransform.position);

		// エージェント取得
		var agent = characterAI.agent;

		// 目標までの方向を計算
		Vector3 directionToTarget = (targetTransform.position - characterAI.transform.GetChild(0).position).normalized;

		// キャラクターの前方向
		Vector3 forward = characterAI.transform.GetChild(0).forward;

		// キャラクターの前方向と目標方向の角度を計算
		float angle = Vector3.Angle(forward, directionToTarget);

		// 条件: 目標がキャラクターの前方かつ、エージェントの距離が一定以内
		if (angle <= 30f && agent.remainingDistance <= agent.radius * 4f)
		{
			// メンタルがたまっていたら逃げる
			if (characterAI.ConductLottery(characterAI.mental))
			{
				// 逃走ステートへ
				stateMachine.ChangeState(new AI_EscapeState());
			}
			else 
				// 攻撃
				stateMachine.ChangeState(new AI_AttackState());
		}

		// ギブアップ処理
		// いつまでも追いかけていると変なので
		giveUpTimer -= Time.deltaTime;
		if(giveUpTimer < 0f)
        {
			// メンタルがたまっていたら逃げる
			if (characterAI.ConductLottery(characterAI.mental))
			{
				// 逃走ステートへ
				stateMachine.ChangeState(new AI_EscapeState());
			}
            else
            {
				// 徘徊ステートへ
				stateMachine.ChangeState(new Ai_TerritoryWanderState());
			}
		}
	}

	// ギズモ
	public override void DrawGizmos() 
	{
		// 追跡先表示
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(targetTransform.position, 1f);
	}

	// ステートから出る時
	public override void Exit() 
	{
		// キャラクターAI取得
		CharacterAI characterAI = stateMachine.characterAI;

		// 移動スピードリセット
		characterAI.ResetSpeed();
	}
}
