using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AIステートマシン
/// </summary>
public class AIStateMachine
{
	///<summary>
	/// キャラクターAI
	/// </summary>
	public CharacterAI characterAI;

	/// <summary>
	/// 現在のステート
	/// </summary>
	public AIBaseState currentState = null;

	/// <summary>
	/// コンストラクタ
	/// </summary>
	/// <param name="characterAI">ステートない処理でAIパラメーターを用いるので保持しておく</param>
	public AIStateMachine(CharacterAI characterAI)
	{
		this.characterAI = characterAI;
	}

	public void Update()
	{
		// ステートがあれば更新処理
		if (currentState != null)
			currentState.Update();	
	}

	public void Gizmos()
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
	public AIBaseState ChangeState(AIBaseState state) 
	{
		// 現在のステートの終了処理
		if(currentState != null)	currentState.Exit();

		// ステートの変更
		currentState = state;
		currentState.stateMachine = this;

		// ステートの開始処理
		currentState.Enter();

		return currentState;
	}
}
