using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterAI : MonoBehaviour
{
	///<summary>自分以外の他キャラクターオブジェクト</summary> 
	[SerializeField]
	[ReadOnly]
	[OverwriteLabel("自分以外のキャラ")]
	public List<GameObject> otherCharcterObjects = null;

	///<summary>データアセット</summary> 
	[SerializeField]
	[ReadOnly]
	[OverwriteLabel("キャラクターパラメーター")]
	private CharacterParamAsset characterParamAsset = null;

	///<summary>
	/// キャラクターマネージャー
	///</summary>
	[SerializeField]
	[ReadOnly]
	[OverwriteLabel("キャラクターマネージャー")]
	public CharacterManager characterManager = null;

	/// <summary>
	/// ナビメッシュエージェント
	/// </summary>
	[SerializeField]
	[ReadOnly]
	[OverwriteLabel("エージェント")]
	public NavMeshAgent agent = null;

	///<summary>
	/// ステートマシン
	/// </summary>
	[SerializeField]
	[ReadOnly]
	[OverwriteLabel("ステートマシン")]
	public AIStateMachine stateMachine = null;

	///<summary>
	/// 現在のステート
	/// </summary>
	[SerializeField]
	[ReadOnly]	
	[OverwriteLabel("現在のステート")]
	private string currentStateName = default;

	/// <summary>
	/// AIパラメーター(仮)
	/// 全て０～１で設定
	/// </summary>
	[System.Serializable]
	public struct AIParam
	{
		[OverwriteLabel("攻撃的か")]
		public float Aggressiveness;    // 攻撃的か
		[OverwriteLabel("賢さ")]
		public float Intelligence;      // 賢さ
		[OverwriteLabel("気の強さ")]
		public float MentalStrength;    // 気の強さ
	}
	[SerializeField]
	[OverwriteLabel("AIパラメーター")]
	public AIParam aIParam;

	/// <summary>
	/// メンタル(0.0f～1.0fで管理)
	/// これが「追跡」「攻撃」等をするごとに溜まっていく
	/// 溜まれば溜まるほど逃げることが多くなる
	/// 逃げれば少し減る
	/// </summary>
	[SerializeField]
	[ReadOnly]
	[OverwriteLabel("メンタル")]
	public float mental = 0f;

	private void Start()
	{
		// 初期化処理
		StartCoroutine(waitForChildAndInitialize());
	}

	/// <summary>
	/// 初期化処理
	/// 子供オブジェクトが生成されるまで待機する
	/// </summary>
	private System.Collections.IEnumerator waitForChildAndInitialize()
	{
		// 子供オブジェクトが生成されていなければ次のフレームまで待機
		while (transform.childCount == 0)
		{

			yield return null;
		}
		// キャラクターマネージャー保持
		TryGetComponent(out characterManager);

		// ステートマシン生成
		stateMachine = new AIStateMachine(this);
		stateMachine.ChangeState(new AI_IdleState(0.1f));

		// パラメーター取得してエージェントに設定
		characterParamAsset = Resources.Load<CharacterParamAsset>("CharacterParamAsset");

		// 足にエージェントコンポーネントを付ける
		agent = transform.GetChild(0).gameObject.AddComponent<NavMeshAgent>();
		// 各種設定
		agent.radius = 0.2f;                                    // 半径
		agent.height = 0.5f;                                    // 高さ
		agent.speed = characterParamAsset.MoveSpeed;            // 速度
		agent.angularSpeed = 1000f;                             // 旋回速度
		agent.acceleration = characterParamAsset.Acceleration;  // 加速度

		// AIの性格値をランダムで決定
		aIParam.Aggressiveness = Random.Range(0.5f, 0.9f);	// 攻撃的か(反撃や追撃)
		aIParam.Intelligence = Random.Range(0.2f, 0.9f);	// 賢いか(ギミック回避率)
		aIParam.MentalStrength = Random.Range(0.3f, 0.9f);	// 気が強いか(メンタル値の上昇率)

		// 自分以外のキャラクターオブジェクト取得
		CharacterManager[] characters = FindObjectsOfType<CharacterManager>();
		foreach (CharacterManager charcter in characters)
		{
			// 自分自身だったらスキップ
			if (charcter.gameObject == this.gameObject) continue;

			// 登録
			otherCharcterObjects.Add(charcter.gameObject);
		}
	}
	void Update()
	{
		if (stateMachine != null)
		{
			// ステートマシン更新処理
			stateMachine.Update();

			// デバッグ用
			currentStateName = stateMachine.currentState.ToString();
		}
	}

	/// <summary>
	/// 抽選を行う
	/// </summary>
	/// <param name="rate">当たる確率、０～１の範囲で渡す</param>
	/// <returns>当たり：true、はずれ：false</returns>
	public bool ConductLottery(float rate)
    {
		// 入力値が範囲外の場合、例外をスローする
		if (rate < 0f || rate > 1f)
		{
			Debug.LogError("確率は０～１の範囲で指定してください");
		}

		// ランダム値を生成し、確率と比較
		return UnityEngine.Random.value <= rate;
	}

	/// <summary>
	/// メンタル加算
	/// </summary>
	/// <param name="addValue">加算値</param>
	public void AddMental(float addValue)
    {
		// もうすでにメンタルが上限なら処理しない
		if (mental >= 1f) return;

		// 加算
		mental += addValue * (1f - aIParam.MentalStrength);

		// 上限制御
		if (mental > 1f) mental = 1f;
    }

	/// <summary>
	/// メンタル減算
	/// </summary>
	/// <param name="sumValue">減算値</param>
	public void SubMental(float sumValue)
    {
		// すでに最小値なら処理しない
		if (mental <= 0f) return;

		// 減算
		mental -= sumValue;

		// 最小値制御
		if (mental < 0f) mental = 0f;
    }

	/// <summary>
	/// 走る速度設定
	/// </summary>
	public void SetDashSpeed()
    {
		agent.speed = characterParamAsset.MaxDashSpeed;
    }

	/// <summary>
	/// 通常速度に戻す
	/// </summary>
	public void ResetSpeed()
    {
		agent.speed = characterParamAsset.MoveSpeed;
    }

	private void OnDrawGizmos()
	{
		if (stateMachine != null)
			stateMachine.Gizmos();
	}}
