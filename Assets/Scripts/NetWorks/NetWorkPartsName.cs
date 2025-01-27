using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetWorkPartsName : MonoBehaviour
{
	///<summary>表示するテキストイメージ</summary>
	[SerializeField]
	[OverwriteLabel("表示するテキストイメージ")]
	private Image textImage = null;

	///<summary>スロットマネージャー</summary>
	[SerializeField]
	[OverwriteLabel("スロットマネージャー")]
	private NetWorkSlotManager slotManager = null;

	/// <summary>
	/// アイコン画像オブジェクト
	/// </summary>
	[SerializeField]
	[OverwriteLabel("アイコン画像オブジェクト")]
	private Image iconImage = null;

	private void Awake()
	{
		TryGetComponent(out textImage);
	}

	private void Update()
	{
		// パーツ名更新処理
		UpdatePartsName();

		// パーツアイコン更新処理
		UpdatePartsIcon();
	}

	/// <summary>
	/// パーツ名更新処理
	/// </summary>
	private void UpdatePartsName()
	{
		// パーツごとの表示名更新
		// 体
		if (slotManager.selectSlotNum      == (int)NetWorkSlotManager.reelType.body)
		{
			textImage.sprite = slotManager.partsList.GetComponent<PartsList>().bodyList[slotManager.reelID[slotManager.selectSlotNum]].textNameSprite;
		}
		// 足
		else if (slotManager.selectSlotNum == (int)NetWorkSlotManager.reelType.leg)
		{
			textImage.sprite = slotManager.partsList.GetComponent<PartsList>().legList[slotManager.reelID[slotManager.selectSlotNum]].textNameSprite;
		}
		// パンチ
		else if (slotManager.selectSlotNum == (int)NetWorkSlotManager.reelType.punch)
		{
			textImage.sprite = slotManager.partsList.GetComponent<PartsList>().punchList[slotManager.reelID[slotManager.selectSlotNum]].textNameSprite;
		}

		// スプライトをデフォルトサイズに
		textImage.SetNativeSize();
	}

	/// <summary>
	/// パーツアイコン更新処理
	/// </summary>
	private void UpdatePartsIcon()
	{
		// アイコン追従
		FollowIcon();

		// パーツごとの表示アイコン更新
		// 体
		if (slotManager.selectSlotNum      == (int)NetWorkSlotManager.reelType.body)
		{
			iconImage.sprite = slotManager.partsList.GetComponent<PartsList>().bodyList[slotManager.reelID[slotManager.selectSlotNum]].IconSprite;
		}
		// 足
		else if (slotManager.selectSlotNum == (int)NetWorkSlotManager.reelType.leg)
		{
			iconImage.sprite = slotManager.partsList.GetComponent<PartsList>().legList[slotManager.reelID[slotManager.selectSlotNum]].IconSprite;
		}
		// パンチ
		else if (slotManager.selectSlotNum == (int)NetWorkSlotManager.reelType.punch)
		{
			iconImage.sprite = slotManager.partsList.GetComponent<PartsList>().punchList[slotManager.reelID[slotManager.selectSlotNum]].IconSprite;
		}

		// スプライトをデフォルトサイズに
		iconImage.SetNativeSize();
	}

	/// <summary>
	/// アイコン追従
	/// </summary>
	private void FollowIcon()
	{
		// テキスト画像の位置（ローカル座標）
		Vector2 textImagePos = textImage.transform.localPosition;

		// テキスト画像のサイズ（ピクセル単位のサイズからスケールを考慮）
		Vector2 textImageSize = new Vector2(
			textImage.rectTransform.rect.width,
			textImage.rectTransform.rect.height
			);

		// テキスト画像の左端にアイコンを配置する座標
		Vector2 iconPos = new Vector2(
			textImagePos.x - textImageSize.x * 0.5f,    // 左端
			textImagePos.y                              // 同じY座標
		);

		// 補正
		iconPos.x -= iconImage.rectTransform.rect.width * 0.75f;

		// アイコン画像の位置を更新
		iconImage.rectTransform.localPosition = new Vector3(iconPos.x, iconPos.y, 0f);
	}
}
