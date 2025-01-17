using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterAI : MonoBehaviour
{
	///<summary>データアセット</summary> 
	[SerializeField]
	[ReadOnly]
	private CharacterParamAsset characterParamAsset;

	///<summary>
	/// キャラクターマネージャー
	///</summary>
	[SerializeField]
	[ReadOnly]
	private CharacterManager characterManager;

	///<summary>
	/// サンダルオブジェクト
	/// </summary>
	[SerializeField]
	[ReadOnly]
	private GameObject sandalObj;

	/// <summary>
	/// ナビメッシュエージェント
	/// </summary>
	[SerializeField]
	[ReadOnly]
	public NavMeshAgent agent;
	  
	/// <summary>
	/// AIパラメーター(仮)
	/// </summary>
	private struct AIParam
	{
		private float Aggressiveness;	// 攻撃的か
		private float Intelligence;		// 賢さ
		private float MentalStrength;	// 気の強さ
	}

	private void Awake()
	{
		// 各コンポーネント取得
		TryGetComponent(out agent);
		TryGetComponent(out characterManager);

		// パラメーター取得してエージェントに設定
		characterParamAsset = Resources.Load<CharacterParamAsset>("CharacterParamAsset");
		agent.speed = characterParamAsset.MoveSpeed;            // 速度
		agent.angularSpeed = 1000f;								// 旋回速度
		agent.acceleration = characterParamAsset.Acceleration;  // 加速度
	}

	private void Start()
	{
		// サンダルオブジェクト取得
		sandalObj = transform.GetChild(0).gameObject;
	}

	void Update()
	{
		// アニメーション更新
		UpdateAnimation();
	}

	// アニメーション遷移更新処理
	void UpdateAnimation()
	{
		// 移動速度に応じてアニメーション
		if (agent.velocity.magnitude > 0f)
		{
			characterManager.SetAnimations(Common.AnimationType.Walk);
		}
		else
			characterManager.SetAnimations(Common.AnimationType.Idle);
	}
}
