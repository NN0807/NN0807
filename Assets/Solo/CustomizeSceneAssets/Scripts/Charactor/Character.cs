using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	///<summary>足の付け根リグ名</summary>
	public const string legRigName = "leg_All";

	///<summary>箱の底の中心リグ名</summary>
	public const string bodyDownCenterRigName = "body_ALL/body_2";

	///<summary>仮の足の付け根の位置</summary>
	private Vector3 TempgroinPos = new Vector3(0f, 0.132f, -0.05f);

	///<summary>仮のパンチ生成位置</summary>
	private Vector3 TempPunchPos = new Vector3(0f, 0.26f, 0f);

	// 各アニメーション
	[StartFolding("アニメーション")]
	[OverwriteLabel("体アニメーション")]
	[SerializeField]
	public Animator bodyAnime;		// 体

	[OverwriteLabel("足アニメーション")]
	[SerializeField]
	public Animator legAnime;		// 足

	[EndFolding]
	[OverwriteLabel("パンチアニメーション")]
	[SerializeField]
	public Animator punchAnime;		// パンチ
	
	///<summary>同体(箱)プレハブ</summary>
	[StartFolding("プレハブ")]
	[OverwriteLabel("体プレハブ")]
	[SerializeField]
	public GameObject bodyPrefab = default;

	/// <summary>箱</summary>
	private GameObject body = default;

	///<summary>足(靴)プレハブ</summary>
	[OverwriteLabel("足プレハブ")]
	[SerializeField]
	public GameObject legPrefab = default;

	///<summary>足</summary>
	private GameObject leg = default;

	///<summary>パンチモデル</summary>
	[EndFolding]
	[OverwriteLabel("パンチプレハブ")]
	[SerializeField]
	public GameObject punchPrefab = default;

	///<summary>パンチ</summary>
	private GameObject punch = default;

	///<summary>デバッグ用</summary>
	///<summary>モデル変更</summary>
	[StartFolding("デバッグ")]
	[OverwriteLabel("モデル変更")]
	[Button("ChangeModel")]
	public bool modelChange = default;

	///<summary>攻撃アニメーション</summary>
	[EndFolding]
	[OverwriteLabel("攻撃アニメーション再生")]
	[Button("StartAttackAnimation")]
	public bool attack = default;

	///<summary>追従用オフセット</summary>
	public Vector3 legOffset = default;

	private void Start()
	{
		ChangeModel();
	}

	private void Update()
	{
		// 体に足を追従させる
		LegFollowBody();

		// パンチモデルをアニメーションに合わせてスケーリング
		PunchScaleByAnim();
	}

	///<summary>攻撃アニメーション再生</summary>
	public void StartAttackAnimation()
	{
		if (bodyAnime != null)
		{
			bodyAnime.SetTrigger("Attack");
		}

		if (legAnime != null)
		{
			legAnime.SetTrigger("Attack");
		}

		if (punchAnime != null)
		{
			punchAnime.SetTrigger("Attack");
		}
	}

	///<summary>モデル変更</summary>
	public void ChangeModel()
	{
		// 現在の各アニメーションフレーム数を取得
		float bodyAnimeNormalizedTime = 0f;
		float legAnimeNormalizedTime = 0f;

		if (bodyAnime != null) bodyAnimeNormalizedTime = bodyAnime.GetCurrentAnimatorStateInfo(0).normalizedTime;
		if (legAnime != null) legAnimeNormalizedTime = legAnime.GetCurrentAnimatorStateInfo(0).normalizedTime;

		// Parts全削除
		DestroyAllParts();

		// 足のプレハブが設定せれている場合のみ生成
		if (legPrefab != null)
		{
			// 足生成
			leg = Instantiate(legPrefab, transform).transform.GetChild(0).gameObject;
			legAnime = leg.GetComponent<Animator>();

			// アニメーションを引き継いで再生
			AnimatorStateInfo legStateInfo = legAnime.GetCurrentAnimatorStateInfo(0);
			legAnime.Play(legStateInfo.fullPathHash, 0, legAnimeNormalizedTime % 1f);
			legAnime.Update(0);
		}

		// 足の付けね位置
		Vector3 groinPos = default;
		// 足が生成されいれば
		if (leg != null)
		{
			// 足の付けね位置取得
			groinPos = legPrefab.transform.GetChild(0).Find(legRigName).localPosition;
		}
		// 足が生成されいなければ
		else
		{
			// 仮に付け根位置を代入
			groinPos = TempgroinPos;
		}

		// 体のプレハブがあれば
		if (bodyPrefab != null)
		{
			// 箱生成
			// 箱の底の中心の高さ
			float downCenterPosY = bodyPrefab.transform.GetChild(0).Find(bodyDownCenterRigName).localPosition.y;
			// 高さを足す
			Vector3 spawnPos = new Vector3(groinPos.x, groinPos.y + Mathf.Abs(downCenterPosY), groinPos.z);
			// 取得した位置に生成
			body = Instantiate(bodyPrefab, spawnPos + transform.position, transform.rotation, transform).transform.GetChild(0).gameObject;
			bodyAnime = body.GetComponent<Animator>();

			// アニメーションを引き継いで再生
			AnimatorStateInfo bodyStateInfo = bodyAnime.GetCurrentAnimatorStateInfo(0);
			bodyAnime.Play(bodyStateInfo.fullPathHash, 0, bodyAnimeNormalizedTime % 1f);
			bodyAnime.Update(0);
		}

		// 足と体が生成されていれば
		if (leg != null && body != null)
		{
			// 攻撃アニメーションの際の補正位置
			legOffset = leg.transform.position - body.transform.position;
		}

		// パンチプレハブがあれば
		if (punchPrefab != null)
		{
			if (body != null)
			{
				// パンチ生成
				// パンチの中心から付け根までの差を求める
				punch = Instantiate(punchPrefab, body.transform.position, body.transform.rotation, transform);
				// スケールを０に
				punch.transform.localScale = new Vector3(0f, 0f, 0f);
				punchAnime = punch.transform.GetChild(0).GetComponent<Animator>();
			}
			else
			{
				// パンチ生成
				// パンチの中心から付け根までの差を求める
				punch = Instantiate(punchPrefab, transform.position + TempPunchPos, transform.rotation, transform);
				// スケールを０に
				punch.transform.localScale = new Vector3(0f, 0f, 0f);
				punchAnime = punch.transform.GetChild(0).GetComponent<Animator>();
			}
		}
	}

	///<summary>パーツ全削除</summary>
	void DestroyAllParts()
	{
		// 子供がいなければ処理しない
		if(gameObject.transform.childCount == 0)
		{ return; }

		// 子供オブジェクトの数を取得し、逆順に削除
		for(int i = gameObject.transform.childCount - 1; i >= 0; i--)
		{
			Destroy(gameObject.transform.GetChild(i).gameObject);
		}

		// 各変数初期化
		body = default;
		bodyAnime = default;
		leg = default;
		legAnime = default;
		punch = default;
		punchAnime = default;
	}

	///<summary>体に足を追従させる</summary>
	void LegFollowBody()
	{
		if (body != null && leg != null)
		{
			leg.transform.position = body.transform.Find(bodyDownCenterRigName).position + (legOffset - legOffset * 0.5f);
		}
	}

	///<summary>パンチモデルをアニメーションに合わせてスケーリング</summary>
	void PunchScaleByAnim()
	{
		// パンチモデルがある場合のみ処理
		if (punch != null)
		{
			// アニメーションフラグ取得
			AnimationFlg animeFlg = punch.transform.GetChild(0).GetComponent<AnimationFlg>();
			// アニメーションフラグが立っていたら
			if (animeFlg.animationFlg)
			{
				// ラープでスケールを徐々に大きく
				punch.transform.localScale = Vector3.Lerp(punch.transform.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 15f);
			}
			else
			{
				// ラープでスケールを徐々に小さく
				if (punch.transform.localScale.x > 0.01f)
				{
					punch.transform.localScale = Vector3.Lerp(punch.transform.localScale, new Vector3(0f, 0f, 0f), Time.deltaTime * 15f);
				}
				else
				{
					punch.transform.localScale = new Vector3(0f, 0f, 0f);
				}
			}
		}
	}
}
