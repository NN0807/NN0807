using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai_TerritoryWanderState : AIBaseState
{
	/// <summary>
	/// 目標地点
	/// </summary>
	private Vector3 targetposition = default;

	/// <summary>
	/// テリトリー半径
	/// </summary>
	private float territoryRadius = 0.4f;

	/// <summary>
	/// テリトリー中心
	/// </summary>
	private Vector2 territoryCenterPos = default;

	// ステートに入った時
	public override void Enter()
	{
		// 現在の位置を縄張りの中心に設定
		territoryCenterPos = new Vector2(stateMachine.transform.position.x, stateMachine.transform.position.z);

		// 目標地点設定
		SetRandomtarget();
	}

	// ステート更新処理
	public override void Update()
	{
		var agent = stateMachine.GetComponent<CharacterAI>().agent;

		// 徘徊処理
		// エージェントが目標地点に到着したかを確認
		if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
		{
			if (!agent.hasPath || agent.velocity.sqrMagnitude <= 0f) // 停止しているか確認
			{
				stateMachine.ChangeState((int)AIStateMachin.StateNum.Idle);
			}
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
		stateMachine.GetComponent<CharacterAI>().agent.SetDestination(new Vector3(targetposition.x, 0.25f, targetposition.y));
	}

	public override void DrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(territoryCenterPos, territoryRadius);
	}

	// ステートから出る時
	public override void Exit()
	{

	}
}
