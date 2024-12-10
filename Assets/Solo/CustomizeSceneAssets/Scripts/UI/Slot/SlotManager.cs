using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
	///<summary>選択スロット</summary>
	[SerializeField]
	private SelectSlot selectSlot = default;

	/// <summary>
	/// 選択されている番号
	/// 左から１・２・３です。
	/// </summary>
	[SerializeField]
	private int selectSlotNum = 0;

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
	public Character character;

	///<summary>モデル更新フラグ</summary>>
	[SerializeField]
	public bool UpdateModelFlg = false;

	private void Awake()
	{
		selectSlot = transform.Find("Slots").GetChild(selectSlotNum).GetComponent<SelectSlot>();
		oldSelectNumber = selectSlotNum;

		// 各リールIDの初期化
		for (int i = 0; i < 3; i++)
			reelID[i] = 3;

		// 各パーツモデルの更新
		character.bodyPrefab = partsList.GetComponent<PartsList>().bodyList[reelID[selectSlotNum]];
		character.legPrefab = partsList.GetComponent<PartsList>().legList[reelID[selectSlotNum]];
		character.punchPrefab = partsList.GetComponent<PartsList>().punchList[reelID[selectSlotNum]];
	}

	private void Update()
	{
		// 選択されているスロットの更新処理
		selectSlot.SlotItemUpdate();

		// 各リールの選択ID取得
		if(!(selectSlot.Downflg || selectSlot.Upflg))
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

	// 選択フレームの移動
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

	// 選択
	private void Select()
	{
		// 現在移動中なら受け付けない
		if (selectSlot.Upflg) return;
		if (selectSlot.Downflg) return;

		// 右に移動
		if (Input.GetKeyDown(KeyCode.D))
		{
			SelectRightSlot();
		}

		// 左に移動
		if(Input.GetKeyDown(KeyCode.A))
		{
			SelectLeftSlot();
		}
	}

	// 左側のスロットを選択
	private void SelectLeftSlot()
	{
		// 一番左のリール選択中なら処理しない
		if (selectSlotNum == 0) return;

		selectSlotNum--;
		selectSlot = transform.Find("Slots").GetChild(selectSlotNum).GetComponent<SelectSlot>();
	}

	// 右側のスロット選択
	private void SelectRightSlot()
	{
		// 一番右のリール選択中なら処理しない
		if (selectSlotNum == 2) return;

		selectSlotNum++;
		selectSlot = transform.Find("Slots").GetChild(selectSlotNum).GetComponent<SelectSlot>();
	}

	// 各パーツモデルの更新
	private void UpdatePartsModel()
	{
		//各パーツの更新
		// 体
		if (selectSlotNum == (int)reelType.body)
		{
			character.bodyPrefab = partsList.GetComponent<PartsList>().bodyList[reelID[selectSlotNum]];
		}
		// 足
		else if (selectSlotNum == (int)reelType.leg)
		{
			character.legPrefab = partsList.GetComponent<PartsList>().legList[reelID[selectSlotNum]];
		}
		// パンチ
		else if(selectSlotNum == (int)reelType.punch)
		{
			character.punchPrefab = partsList.GetComponent<PartsList>().punchList[reelID[selectSlotNum]];
		}

		// モデル変更
		character.ChangeModel();

		// パンチモデルを変更した場合は攻撃アニメーション再生
		if(selectSlotNum == (int)reelType.punch)
        {
			character.StartAttackAnimation();
        }
	}
}
