using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_EscapeState : AIBaseState
{
    /// <summary>
    /// 逃げ先
    /// </summary>
    Vector3 escapPos = default;
    
	// ステートに入った時
	public override void Enter()
    {
        // キャラクターAI取得
        CharacterAI characterAI = stateMachine.characterAI;

        // 歩きアニメーション
        characterAI.characterManager.SetAnimations(Common.AnimationType.Walk);

        // 逃げ先算出
        escapPos = CalcEscapePosition();
        // 目標地点(逃げ先)を設定
        characterAI.agent.SetDestination(escapPos);

        // 走る
        characterAI.SetDashSpeed();

        // メンタル減算
        characterAI.SubMental(0.35f);
    }

    /// <summary>
    /// 逃げ先を算出
    /// </summary>
    private Vector3 CalcEscapePosition()
    {
        // キャラクターAI取得
        CharacterAI characterAI = stateMachine.characterAI;

        // 自分の位置
        Vector3 myPos = characterAI.transform.GetChild(0).position;
        // 相手から自分へのベクトル
        Vector3 myToEnemyVec = default;

        // 距離
        float distance = float.MaxValue;

        // 敵の数ループ
        foreach (GameObject enemy in characterAI.otherCharcterObjects)
        {
            // 敵の位置
            Vector3 enemyPos = enemy.transform.GetChild(0).position;

            // さらに近い敵が見つかったら
            if (distance > Vector3.Distance(myPos, enemyPos))
            {
                // 最短距離の敵から自分へのベクトルを保持
                myToEnemyVec = myPos - enemyPos;
                // 距離を保存
                distance = Vector3.Distance(myPos, enemyPos);
            }
        }

        // 逃げ先を一番近くの敵から 5f 離れた距離に
        return (myPos + Vector3.Normalize(myToEnemyVec) * 2f);
    }

    // ステート更新処理
    public override void Update()
    {
        // キャラクターAI取得
        CharacterAI characterAI = stateMachine.characterAI;

        // エージェント取得
        var agent = stateMachine.characterAI.agent;

        // エージェントが目標地点に到着したかを確認
        if (agent.remainingDistance <= agent.radius * 4f)
        {
            // メンタルがたまっていたら逃げる
            if (characterAI.ConductLottery(characterAI.mental))
            {
                // 逃走ステートへ
                stateMachine.ChangeState(new AI_EscapeState());
            }
            // 溜まっていなければ
            else
            {
                // 待機ステートへ
                stateMachine.ChangeState(new AI_IdleState(0.1f));
            }
        }
    }

    // ギズモ
    public override void DrawGizmos()
    {
        // 逃げ先表示
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(new Vector3(escapPos.x, 0.25f, escapPos.y), 1f);
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
