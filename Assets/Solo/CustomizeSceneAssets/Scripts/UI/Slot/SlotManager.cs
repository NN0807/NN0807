using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlotManager : MonoBehaviour
{
	///<summary>入力処理</summary>
	[SerializeField]
	private CustomizeSceneController inputActions;

	///<summary>データ連携用のテキスト</summary>
	private static readonly string[,] partsDataText =
	{
		// 体テキスト
		{
			"TCBody_Animation",
			"WRBody_Animation",
			"DBBody_Animation",
			"TBBody_Animation",
			"SBBody_Animation",
			"OBBody_Animation",
			"SHBody_Animation",
			"DBody_Animation"
		},

		// 足テキスト
		{
			"TCFoot_Animation",
			"WRFoot_Animation",
			"DBFoot_Animation",
			"TBFoot_Animation",
			"SBFoot_Animation",
			"OBFoot_Animation",
			"SHFoot_Animation",
			"DFoot_Animation"
		},

		// パンチテキスト
		{
			"TCPunch_Animation",
			"WRPunch_Animation",
			"DBPunch_Animation",
			"TBPunch_Animation",
			"SBPunch_Animation",
			"OBPunch_Animation",
			"SHPunch_Animation",
			"DPunch_Animation"
		}

	};

	///<summary>選択スロット</summary>
	[SerializeField]
	private SelectSlot selectSlot = default;

	/// <summary>
	/// 選択されているリール番号
	/// 左から０・１・２です。
	/// </summary>
	[SerializeField]
	public int selectSlotNum = 0;

	///<summary>古い選択番号</summary>
	[SerializeField]
	private int oldSelectNumber = 0;

	///<summary>選択枠</summary>
	[SerializeField]
	private GameObject selectFream = default;

	///<summary>スロット回転加速度</summary>
	float velocity = 0f;

	///<summary>リール</summary>
	///<summary>リール種類</summary>
	public　enum reelType
	{
		body,
		leg,
		punch
	};
	///<summary>各リールの選択されているID</summary>
	[SerializeField]
	public int[] reelID = new int[3];

	///<summary>パーツリスト</summary>
	[SerializeField]
	public GameObject partsList;

	///<summary>キャラクタースクリプト</summary> 
	[SerializeField]
	public CustomizeCharacter character;

	///<summary>モデル更新フラグ</summary>>
	[SerializeField]
	public bool UpdateModelFlg = false;

	private void Awake()
	{
		CustomizeSceneManager.Instance.isCharacterCustomize = true;

		// 入力処理初期化
		inputActions = new CustomizeSceneController();
		inputActions.Enable();

		// 選択されているスロットを取得
		selectSlot = transform.Find("Slots").GetChild(selectSlotNum).GetComponent<SelectSlot>();
		oldSelectNumber = selectSlotNum;

		// 各リールIDの初期化
		for (int i = 0; i < 3; i++) reelID[i] = 3;

		// 各パーツモデルの更新
		character.bodyPrefab = partsList.GetComponent<PartsList>().bodyList[reelID[selectSlotNum]].ModelPrefab;
		character.legPrefab = partsList.GetComponent<PartsList>().legList[reelID[selectSlotNum]].ModelPrefab;
		character.punchPrefab = partsList.GetComponent<PartsList>().punchList[reelID[selectSlotNum]].ModelPrefab;
	}

	private void Update()
	{
		// 選択遷移中は処理しない
		if (CustomizeSceneManager.Instance.isMoving) return;

		// 上下移動中ではなく
		if (!selectSlot.Upflg && !selectSlot.Downflg)
		{
			// 入力があったら
			if (inputActions.UI.Move.ReadValue<Vector2>().y > 0.5f)
			{
				// リール回転音
				AudioManager.instance.Play(SEPath.SlotRotation, AudioManager.ALL_VOLUME_VALUE);

				// 上へ移動
				selectSlot.Upflg = true;
				// 矢印を光らせる
				selectFream.transform.Find("UpArrow").GetComponent<BloomController>().triggerParam.trigger = true;
			}
			if (inputActions.UI.Move.ReadValue<Vector2>().y < -0.5f)
			{
				// リール回転音
				AudioManager.instance.Play(SEPath.SlotRotation, AudioManager.ALL_VOLUME_VALUE);

				// 下へ移動
				selectSlot.Downflg = true;
				// 矢印を光らせる
				selectFream.transform.Find("DownArrow").GetComponent<BloomController>().triggerParam.trigger = true;
			}
		}

		// 選択されているスロットの更新処理
		selectSlot.SlotItemUpdate();

		// 各リールの選択ID取得
		if (!(selectSlot.Downflg || selectSlot.Upflg))
		{
			// 入力がない場合のみモデル更新するように
			if (!(Mathf.Abs(inputActions.UI.Move.ReadValue<Vector2>().y) > 0.5f))
			{
				reelID[selectSlotNum] = selectSlot.selectNum;

				// 各パーツモデルの更新
				if (UpdateModelFlg)
				{
					UpdatePartsModel();
					// フラグ初期化
					UpdateModelFlg = false;
				}
			}
		}
		else
		{
			UpdateModelFlg = true;
		}

		// 移動中の処理
		if (oldSelectNumber != selectSlotNum)
		{
			// 選択Frameの移動
			MoveSelectFream();
		}
		// 移動が完了している場合
		else
		{
			// 選択
			Select();
		}
	}

	///<summary>選択フレームの移動</summary> 
	private void MoveSelectFream()
	{
		// 移動先の位置
		float targetX = transform.Find("Slots").GetChild(selectSlotNum).localPosition.x + transform.Find("Slots").localPosition.x;
		// 位置代入用
		Vector3 pos = selectFream.transform.localPosition;

		if (Mathf.Abs(pos.x - targetX) < 0.1f) // 誤差範囲を0.1fに
		{
			pos.x = targetX;

			oldSelectNumber = selectSlotNum;
		}
		else
		{
			pos.x = Mathf.SmoothDamp(pos.x, targetX, ref velocity, 0.05f); // 第4引数で滑らかさを調整
			pos.z = 0f;
			selectFream.transform.localPosition = pos;
		}
	}

	///<summary>選択</summary> 
	private void Select()
	{
		// 現在移動中なら受け付けない
		if (selectSlot.Upflg) return; 
		if (selectSlot.Downflg) return;

		// 右に移動
		if (inputActions.UI.Move.ReadValue<Vector2>().x > 0.5f)
		{
			SelectRightSlot();
		}

		// 左に移動
		if(inputActions.UI.Move.ReadValue<Vector2>().x < -0.5f)
		{
			SelectLeftSlot();
		}
	}

	///<summary>左側のスロットを選択</summary> 
	private void SelectLeftSlot()
	{
		// 一番左のリール選択中なら処理しない
		if (selectSlotNum == 0) return;

		// スロット横移動音
		AudioManager.instance.Play(SEPath.SlotSideMove, AudioManager.ALL_VOLUME_VALUE);

		selectSlotNum--;
		selectSlot = transform.Find("Slots").GetChild(selectSlotNum).GetComponent<SelectSlot>();
	}

	///<summary>右側のスロット選択</summary> 
	private void SelectRightSlot()
	{
		// 一番右のリール選択中なら処理しない
		if (selectSlotNum == 2) return;

		// スロット横移動音
		AudioManager.instance.Play(SEPath.SlotSideMove, AudioManager.ALL_VOLUME_VALUE);

		selectSlotNum++;
		selectSlot = transform.Find("Slots").GetChild(selectSlotNum).GetComponent<SelectSlot>();
	}

	///<summary>各パーツモデルの更新</summary> 
	private void UpdatePartsModel()
	{
		//各パーツの更新
		// 体
		if (selectSlotNum == (int)reelType.body)
		{
			character.bodyPrefab = partsList.GetComponent<PartsList>().bodyList[reelID[selectSlotNum]].ModelPrefab;
		}
		// 足
		else if (selectSlotNum == (int)reelType.leg)
		{
			character.legPrefab = partsList.GetComponent<PartsList>().legList[reelID[selectSlotNum]].ModelPrefab;
		}
		// パンチ
		else if(selectSlotNum == (int)reelType.punch)
		{
			character.punchPrefab = partsList.GetComponent<PartsList>().punchList[reelID[selectSlotNum]].ModelPrefab;
		}

		// モデル変更
		character.ChangeModel();

		// パンチモデルを変更した場合は攻撃アニメーション再生
		if(selectSlotNum == (int)reelType.punch)
		{
			character.StartAttackAnimation();
		}
	}

	///<summary>選択されたパーツ文字列をデータに保存</summary>
	public void SavePartsData()
	{
		// staticを使ったデータ受け渡し
		//GameData.bodySelectPartsName = partsDataText[selectSlotNum,reelID[(int)reelType.body]];
		//GameData.legSelectPartsName = partsDataText[selectSlotNum, reelID[(int)reelType.leg]];
		//GameData.punchSelectPartsName = partsDataText[selectSlotNum, reelID[(int)reelType.punch]];

		// PlayerPrefsを使ったデータ受け渡し
		// キーと値をセット
		PlayerPrefs.SetInt("body", reelID[(int)reelType.body]);
		PlayerPrefs.SetInt("leg", reelID[(int)reelType.leg]);
		PlayerPrefs.SetInt("punch", reelID[(int)reelType.punch]);

		// 保存
		PlayerPrefs.Save();
	}
}
