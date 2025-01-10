using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsList : MonoBehaviour
{
	// リスト構造体
	[System.Serializable]
	public struct PartsListStruct
	{
		[OverwriteLabel("モデルプレハブ")]
		public GameObject ModelPrefab;
		[OverwriteLabel("パーツ名スプライト")]
		public Sprite textNameSprite;
		[OverwriteLabel("パーツ名アイコン")]
		public Sprite IconSprite;
	}

	///<summary>体リスト</summary>
	[SerializeField]
	public PartsListStruct[] bodyList;

	///<summary>足リスト</summary>
	[SerializeField]
	public PartsListStruct[] legList;

	///<summary>パンチリスト</summary>
	[SerializeField]
	public PartsListStruct[] punchList;
}
