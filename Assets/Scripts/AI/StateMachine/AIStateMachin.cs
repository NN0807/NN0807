using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AIステートマシン
/// </summary>
public class AIStateMachin : MonoBehaviour
{
	///<summary>
	/// ステート番号
	/// </summary>
	[SerializeField]
	public enum StateNum
	{
		Idle = 0,				// 待機
		TerritoryWander,		// 縄張りで徘徊
		MoveTerritoryWander,    // 縄張り変更徘徊
		Pursuit,                // 追跡
		DiversionaryAttack,		// 牽制攻撃
		Attack,                 // 攻撃
		Escape					// 逃げる
	}

	///<summary>
	/// ステートリスト
	/// </summary>
	[SerializeField]
	private List<AIBaseState> stateList = default;

	/// <summary>
	/// 現在のステート
	/// </summary>
	[SerializeField]
	[ReadOnly]
	private AIBaseState currentState = null;

	private void Awake()
	{
		// ステートマシン登録
		foreach(AIBaseState state in stateList)
		{
			state.stateMachine = this;
		}
	}

	private void Start()
	{
		// 初期ステート設定
		ChangeState((int)StateNum.Idle);
	}

	private void Update()
	{
		// ステートがあれば更新処理
		if (currentState != null)
			currentState.Update();	
	}

	private void OnDrawGizmos()
	{
		// ステートがあればギズモ表示
		if (currentState != null)
		{
			currentState.DrawGizmos();
		}
	}

	/// <summary>
	/// ステート変更
	/// </summary>
	/// <param name="StateNum">ステート番号</param>
	public void ChangeState(int stateNum) 
	{
		// 現在のステートの終了処理
		if(currentState != null)	currentState.Exit();

		// ステートの変更
		currentState = stateList[stateNum];

		// ステートの開始処理
		currentState.Enter();
	}
}
