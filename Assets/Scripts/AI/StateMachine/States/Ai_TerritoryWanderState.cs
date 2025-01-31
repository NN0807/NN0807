using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ai_TerritoryWanderState : AIBaseState
{
	/// <summary>
	/// 目標地点
	/// </summary>
	private Vector3 targetposition = default;

	/// <summary>
	/// テリトリー半径
	/// </summary>
	private float territoryRadius = 1f;

	/// <summary>
	/// テリトリー中心位置
	/// </summary>
	private Vector2 territoryCenterPos = default;

	// ステートに入った時
	public override void Enter()
	{
		// キャラクターAI取得
		CharacterAI characterAI = stateMachine.characterAI;

		// 歩きアニメーション
		characterAI.characterManager.SetAnimations(Common.AnimationType.Walk);

		// 現在の位置を縄張りの中心に設定
		territoryCenterPos = new Vector2(characterAI.transform.GetChild(0).position.x, characterAI.transform.GetChild(0).position.z);

		// 目標地点設定
		SetRandomtarget();
	}

	// ステート更新処理
	public override void Update()
	{
		// エージェント取得
		var agent = stateMachine.characterAI.agent;

		// 徘徊処理
		// エージェントが目標地点に到着したかを確認
		if (agent.remainingDistance <= agent.stoppingDistance)
		{
			stateMachine.ChangeState(new AI_IdleState(0.1f));
		}

		// 何かに阻まれている状態の例外処理
		if (agent.velocity.sqrMagnitude < 0.01f &&
			agent.pathStatus == NavMeshPathStatus.PathComplete &&
			agent.remainingDistance < agent.stoppingDistance + 0.1f)
		{
			stateMachine.ChangeState(new AI_IdleState(0.1f));
		}
	}

	// ランダムな目標地点を設定
	private void SetRandomtarget()
	{
		// テリトリー内のランダムな方向と距離を計算
		Vector2 randomDirection = Random.insideUnitCircle.normalized;
		float randomDistance = Random.Range(0f, territoryRadius); 
		Vector2 randomPoint = randomDirection * randomDistance;

		// 目標地点をテリトリー内に設定
		targetposition = new Vector3(territoryCenterPos.x + randomPoint.x, 0.25f, territoryCenterPos.y + randomPoint.y);

		// NavMeshAgentに目標地点を設定
		stateMachine.characterAI.agent.SetDestination(new Vector3(targetposition.x, 0.25f, targetposition.z));
	}

	// ギズモ表示
	public override void DrawGizmos()
	{

	}

	// ステートから出る時
	public override void Exit()
	{
		territoryCenterPos = default;
		targetposition = default;
	}
}
