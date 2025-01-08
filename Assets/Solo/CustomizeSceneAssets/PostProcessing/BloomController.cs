using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloomController : MonoBehaviour
{
	///<summary>ブルームスクリプト</summary>
	[SerializeField]
	[ReadOnly]
	private ImageGlow ImageGlow;

	///<summary>初期エミッションカラー</summary>
	[SerializeField]
	[ReadOnly]
	private Color emission = default;

	///<summary>強度</summary>
	[SerializeField]
	[ReadOnly]
	private float factor = 0f;

	///<summary>ブルームの光らせ方種別</summary>
	public enum BloomType
	{
		Normal = 0,	// 無し
		Flash,		// 点滅
		Trigger,	// １度だけ
	}
	[SerializeField]
	public BloomType bloomType = default;

	// 通常時のパラメーター
	[SerializeField]
	[HideInInspector]
	public float bloomIntensity = 0.0f;

	// 点滅時のパラメーター
	[System.Serializable]
	[HideInInspector]
	public class BaseParam
	{
		// 点滅速度
		[HideInInspector]
		public float speed = 1.0f;
		// 点滅の最少
		[HideInInspector]
		public float factorMin = 0f;
		// 点滅の最大
		[HideInInspector]
		public float factorMax = 0.5f;
	}
	[SerializeField]
	[HideInInspector]
	public BaseParam baseParam = new BaseParam();

	// １回だけ点灯時のパラメーター
	[System.Serializable]
	[HideInInspector]
	public class TriggerParam : BaseParam
	{
		[HideInInspector]
		public bool trigger = false;

		[HideInInspector]
		public bool flip = false;
	}
	[SerializeField]
	[HideInInspector]
	public TriggerParam triggerParam = new TriggerParam();


	private void Awake()
	{
		// ぼかしているスクリプト取得
		TryGetComponent(out ImageGlow);
		// 設定されている初期カラー取得(この色を基準に光らせる)
		emission = ImageGlow.EmissionColor;
	}

	void Update()
	{
		// 処理分岐
		switch(bloomType)
		{
			// 通常
			case BloomType.Normal:
				Normal();
			break;
			// 点滅
			case BloomType.Flash:
				Flash();
			break;
			case BloomType.Trigger:
				Trigger();
			break;
		}

		// ブルームの明度を反映
		ImageGlow.EmissionColor = new Color(emission.r * factor, emission.g * factor, emission.b * factor);
	}

	// 通常処理
	private void Normal()
	{
		factor = bloomIntensity;
	}

	// 点滅処理
	private void Flash()
	{
		// 点滅速度が０を下回らないように制御
		if (baseParam.speed < 0f) baseParam.speed = 0f;
			
		// 正弦波を計算 (-1から1の範囲)
		float sinValue = Mathf.Sin((Time.time * baseParam.speed / 3f) * Mathf.PI * 2.0f);

		// -1から1を0から1の範囲に変換
		float normalizedSin = (sinValue + 1.0f) / 2.0f;

		// 最小値から最大値に変換
		float value = Mathf.Lerp(baseParam.factorMin, baseParam.factorMax, normalizedSin);

		factor = value;
	}

	// １回だけ点灯
	private void Trigger()
	{
		// フラグが立っていないなら処理しない
		if(triggerParam.trigger == false) return;

		// 反転制御
		if(triggerParam.flip == false)
		{
			factor += Time.deltaTime * triggerParam.speed;
		}
		else if(triggerParam.flip)
		{
			factor -= Time.deltaTime * triggerParam.speed;
		}

		// 最大値に達した
		if (factor > triggerParam.factorMax - 0.01f) triggerParam.flip = true;

		// value の値で判定
		if (factor < (triggerParam.factorMin + 0.01f) && triggerParam.flip)
		{
			// 最小値に到達した -> 1往復完了
			triggerParam.trigger = false;
			triggerParam.flip = false;
			factor = 0f;
		}
	}

	// パラメーター全初期化

}
