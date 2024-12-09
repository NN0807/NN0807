using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterAI : MonoBehaviour
{
	/// <summary>
	/// ナビメッシュエージェント
	/// </summary>
	[SerializeField]
	[ReadOnly]
	private NavMeshAgent agent;

	/// <summary>
	/// ゴール位置
	/// </summary>
	[SerializeField]
	private GameObject goalPos;

	/// <summary>
	/// AIパラメーター(仮)
	/// </summary>
	private struct AIParam
    {
		private float Aggressiveness;	// 攻撃的か
		private float Intelligence;		// 賢さ
		private float MentalStrength;	// 気の強さ
	}

	// Start is called before the first frame update
	private void Awake()
	{
		// エージェント取得
		TryGetComponent(out agent);
	}

	// Update is called once per frame
	void Update()
	{
		// ゴール位置を設定
		agent.SetDestination(goalPos.transform.position);
	}
}
