using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_IdleState : AIBaseState
{
	/// <summary>
	/// コンストラクタ
	/// </summary>
	/// <param name="idleTIme">待機時間</param>
	public AI_IdleState(float idleTIme)
    {
		// 待機時間設定
		time = idleTIme;
    }

	/// <summary>
	/// 待機時間
	/// </summary>
	public float time { get; set; } = 0f;

	// ステートに入った時
	public override void Enter()
	{
		if (!stateMachine.isUpdate) return;

		// 待機アニメーション
		stateMachine.characterAI.characterManager.SetAnimations(Common.AnimationType.Idle);
	}

	// ステート更新処理
	public override void Update()
	{
		if (!stateMachine.isUpdate) return;

		// キャラクターAI取得
		CharacterAI characterAI = stateMachine.characterAI;

		// タイマー処理
		time -= Time.deltaTime;

		// 待機終了処理
		if (time <= 0f)
		{
			// 攻撃的かで分岐
			if (characterAI.ConductLottery(characterAI.aIParam.Aggressiveness))
			{
				// 敵キャラがステージ上に存在するなら
				if (characterAI.otherCharcterObjects.Count > 0)
				{
					// 追跡ステートへ
					stateMachine.ChangeState(new AI_PursuitState());
				}
				else
                {
					// 徘徊ステートへ
					stateMachine.ChangeState(new Ai_TerritoryWanderState());
				}
			}
			else
            {
				// 徘徊ステートへ
				stateMachine.ChangeState(new Ai_TerritoryWanderState());
            }
		}
	}

	// ギズモ表示
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
