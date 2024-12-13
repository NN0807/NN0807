using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartsName : MonoBehaviour
{
    ///<summary>表示するテキストイメージ</summary>
    [SerializeField]
    [OverwriteLabel("表示するテキストイメージ")]
    private Image textImage = null;

    ///<summary>スロットマネージャー</summary>
    [SerializeField]
    [OverwriteLabel("スロットマネージャー")]
    private SlotManager slotManager = null;

    private void Awake()
    {
        TryGetComponent(out textImage);
    }

    private void Update()
    {
		// パーツごとの表示名更新
		// 体
		if (slotManager.selectSlotNum == (int)SlotManager.reelType.body)
		{
			textImage.sprite = slotManager.partsList.GetComponent<PartsList>().bodyList[slotManager.reelID[slotManager.selectSlotNum]].textNameSprite;
		}
		// 足
		else if (slotManager.selectSlotNum == (int)SlotManager.reelType.leg)
		{
			textImage.sprite = slotManager.partsList.GetComponent<PartsList>().legList[slotManager.reelID[slotManager.selectSlotNum]].textNameSprite;
		}
		// パンチ
		else if (slotManager.selectSlotNum == (int)SlotManager.reelType.punch)
		{
			textImage.sprite = slotManager.partsList.GetComponent<PartsList>().punchList[slotManager.reelID[slotManager.selectSlotNum]].textNameSprite;
		}

		// スプライトをデフォルトサイズに
		textImage.SetNativeSize();
	}
}
