using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSlot : MonoBehaviour
{
	// 定数
	public static class Constants
	{
		///<summary>表示されるアイテム</summary>
		public const int maxShowItemCount = 7;
		///<summary>アイテム位置の高さ</summary>
		public static readonly float[] hight = { 384f * 1.6f, 256f * 1.6f, 128f * 1.6f, 0f, -128f * 1.6f, -256f * 1.6f, -384f * 1.6f };
	}

	///<summary>ラープ用の速度値</summary>
	private float velocity = 0f;

	///<summary>補完移動フラグ</summary>
	public bool Upflg = default;
	public bool Downflg = default;

	///<summary>生成する画像オブジェクトリスト</summary>
	[SerializeField]
	public List<GameObject> spawnSpriteObjectList;

	///<summary>実際に生成されているオブジェクト</summary>
	[SerializeField]
	public GameObject[] SpriteObjects = new GameObject[Constants.maxShowItemCount];

	///<summary>選択されている番号</summary>
	[SerializeField]
	public int selectNum = 0;

	private void Awake()
	{
		// 表示物の生成
		SpawnSprite();

		// 選択されている番号を保存
		selectNum = SpriteObjects[3].GetComponent<Item>().GetNumber();
	}

	/// <summary>画像生成</summary>
	private void SpawnSprite()
	{
		// 見せる数だけ生成
		for(int i = 0; i < Constants.maxShowItemCount; i++)
		{
			// 生成
			Vector3 pos = new Vector3(0f, Constants.hight[i], 0f);
			SpriteObjects[i] = Instantiate(spawnSpriteObjectList[i], transform, false);
			SpriteObjects[i].transform.localPosition = pos;
		}
	}

	// アイテム選択
	private void SelectItem(bool isUp)
	{
		// 移動が終わっているか
		bool allReached = true;

		// ループの開始位置
		int start = isUp ? 0 : 1;
		// ループの終了位置
		int end = isUp ? SpriteObjects.Length : SpriteObjects.Length;
		// 足し引き
		int increment = isUp ? 1 : -1;

		// 破棄
		Destroy(isUp ? SpriteObjects[end - 1] : SpriteObjects[0]);

		for(int i = start; i < end; i++)
		{
			if (isUp)
				if (i == 6) break;

			// 位置取得
			Vector3 pos = SpriteObjects[i].transform.localPosition;

			// 目標位置
			float targetY = Constants.hight[i + increment];

			// 目標位置への距離が0.1fを上回っていたら
			if (Mathf.Abs(pos.y - targetY) >= 0.1f)
			{
				// 補完移動
				pos.y = Mathf.SmoothDamp(pos.y, targetY, ref velocity, 0.05f);
				// まだ移動完了していない
				allReached = false;
			}

			// 位置代入
			SpriteObjects[i].transform.localPosition = pos;

			// 移動完了時処理
			if (allReached)
			{
				// リスト更新
				UpdateList(isUp);
				// 新しいアイテムを生成
				GenerateNewItem(isUp);
				// 目標位置にスナップ
				SnapToTargetPositions();

				// 選択されている番号を保存
				selectNum = SpriteObjects[3].GetComponent<Item>().GetNumber();

				// フラグ
				if (isUp) Upflg = false; 
				else Downflg = false;				

				// ログ
				Debug.Log("全てのオブジェクトが移動完了しました。");
				break;
			}
		}
	}

	// スナップ
	private void SnapToTargetPositions()
	{
		// 目標位置にスナップ補完する
		for (int k = 0; k < SpriteObjects.Length - 1; k++)
		{
			Vector3 snapPos = default;
			snapPos.y = Constants.hight[k];
			SpriteObjects[k].transform.localPosition = snapPos;
		}
	}

	// 新しいアイテム生成
	private void GenerateNewItem(bool isUp)
	{
		// 生成するアイテム番号
		int index = isUp ? 0 : SpriteObjects.Length - 1;
		// 生成アイテムの１つ前のアイテムを確認する
		int num = isUp
			? SpriteObjects[1].GetComponent<Item>().GetNumber()
			: SpriteObjects[SpriteObjects.Length - 2].GetComponent<Item>().GetNumber();

		int newItemIndex = isUp
			? (num == 0 ? spawnSpriteObjectList.Count - 1 : num - 1)
			: (num == spawnSpriteObjectList.Count - 1 ? 0 : num + 1);

		Vector3 pos = new Vector3(0, Constants.hight[index], 0);
		SpriteObjects[index] = Instantiate(spawnSpriteObjectList[newItemIndex], transform, false);
		SpriteObjects[index].transform.localPosition = pos;
	}

	// リスト更新
	private void UpdateList(bool isUp)
	{
		if (isUp)
		{
			for (int j = SpriteObjects.Length - 1; j > 0; j--)
			{
				SpriteObjects[j] = SpriteObjects[j - 1];
			}
		}
		else
		{
			for (int j = 0; j < SpriteObjects.Length - 1; j++)
			{
				SpriteObjects[j] = SpriteObjects[j + 1];
			}
		}
	}

	///<summary>更新処理</summary>
	public void SlotItemUpdate()
	{
		if (Upflg)
		{
			SelectItem(true);
		}
		else if (Downflg)
		{
			SelectItem(false);
		}
		else
		{
			if (Input.GetKeyDown(KeyCode.W)) Upflg = true;
			if (Input.GetKeyDown(KeyCode.S)) Downflg = true;
		}
	}
}
