using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
	///<summary>番号</summary>
	[SerializeField]
	private int number = 3;

	///<summary>番号設定</summary>
	public void SetNumber(int num)
	{
		number = num;
	}

	///<summary>番号取得</summary>
	public int GetNumber()
	{
		return number;
	}
}
