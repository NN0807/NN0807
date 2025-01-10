using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFlg : MonoBehaviour
{
	/// <summary>
	/// アニメーションフラグ
	/// </summary>
	public bool animationFlg;

	/// <summary>
	/// パンチアニメーションのスロー再生開始＆終了フラグ
	/// </summary>
	public bool startSlowAnimationFlg;
	public bool endSlowAnimationFlg;

	public void EnbleFlg()
	{
		animationFlg = true;
	}

	public void DisableFlg()
	{
		animationFlg = false;
	}

	public void StartSlowAnimationFlg()
	{
		startSlowAnimationFlg = true;
	}
	public void EndSlowAnimationFlg()
	{
		endSlowAnimationFlg = true;
	}
}
