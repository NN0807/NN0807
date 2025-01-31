using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_DamageState : AIBaseState
{
	/// <summary>
	/// 落下又は吹っ飛ばされているかの判定閾値
	/// </summary>
	const float deathSpeed​​Threshold = 3f;

	/// <summary> 
	/// RigidbodyのisSleepingが使い物にならないので
	/// アニメーション終了で取得したいがそれも出来ないので
	/// タイマーで管理
	/// </summary>
	float timer = 0f;

	// ステートに入った時
	public override void Enter()
	{
		// タイマー初期化
		// 多分ダメージアニメーションは0.8秒くらい
		timer = 0.8f;

		// キャラクターAI取得
		CharacterAI characterAI = stateMachine.characterAI;

		// ダメージアニメーション
		characterAI.characterManager.SetAnimations(Common.AnimationType.Hit);
	}

	// ステート更新処理
	public override void Update()
	{
		if (!stateMachine.isUpdate) return;

		// キャラクターAI取得
		CharacterAI characterAI = stateMachine.characterAI;

		// タイマー処理
		timer -= Time.deltaTime;
		if(timer < 0f)
		{
			// 落下または、吹っ飛ばされて急上昇していたら
			if (Mathf.Abs(characterAI.characterManager._legRigidbody.velocity.y) > deathSpeed​​Threshold)
            {
				return;
            }

			// 攻撃的なら反撃
			if (characterAI.ConductLottery(characterAI.aIParam.Aggressiveness))
			{
				// しかしストレス値が溜まっているなら逃げる
				if (characterAI.ConductLottery(characterAI.mental))
				{
					// 逃走
					stateMachine.ChangeState(new AI_EscapeState());
				}
				else
				{
					// 反撃
					stateMachine.ChangeState(new AI_PursuitState());
				}
			}
			// そうでないなら逃げる
			else
			{
				// 逃走
				stateMachine.ChangeState(new AI_EscapeState());
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
		// キャラクターAI取得
		CharacterAI characterAI = stateMachine.characterAI;

		// 落下または、吹っ飛ばされて急上昇していたら
		if (Mathf.Abs(characterAI.characterManager._legRigidbody.velocity.y) > deathSpeed​​Threshold)
		{
			// なければこれ以上ステートマシンを処理しないように
			stateMachine.isUpdate = false;
			stateMachine.currentState = null;
		}
		// 落下も急上昇もしてなかったら
		else
		{
			// あればエージェントを再度有効化
			characterAI.agent.enabled = true;
		}
	}
}
