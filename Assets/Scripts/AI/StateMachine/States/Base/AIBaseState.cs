using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>基本ステート(純粋仮想クラス)</summary>
public　abstract class AIBaseState
{
	// ステートマシン
	[SerializeField]
	[HideInInspector]
	public AIStateMachine stateMachine = null;

	// ステートに入った時
	public abstract void Enter();

	// ステート更新処理
	public abstract void Update();

	// ギズモ
	public abstract void DrawGizmos();

	// ステートから出る時
	public abstract void Exit();
}
