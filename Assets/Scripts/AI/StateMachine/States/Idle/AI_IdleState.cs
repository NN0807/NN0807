using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_IdleState : AIBaseState
{
	/// <summary>
	/// 待機時間
	/// </summary>
	public float time { get; set; } = 0f;

	// ステートに入った時
	public override void Enter()
	{
		time = 1f;
	}

	// ステート更新処理
	public override void Update()
	{
		// タイマー処理
		time -= Time.deltaTime;

		if (time <= 0f)
		{
			stateMachine.ChangeState((int)AIStateMachin.StateNum.TerritoryWander);
		}
	}

	public override void DrawGizmos()
	{

	}

	// ステートから出る時
	public override void Exit()
	{
		// タイマー初期化
		time = 0f;
	}
}
