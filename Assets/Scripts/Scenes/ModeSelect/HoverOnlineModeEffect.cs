using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverOnlineModeEffect : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{

    // 演出用画像

    public Image[] effectImage;
    // 拡大倍率
    public float scaleMaltiplier = 1.1f;
    // 上下移動速度
    public float moveSpeed = 4f;
    // 上下移動範囲
    public float moveRange = 10f;

    // マウスが乗っているか
    private bool isHovered = false;
    // 各UI画像初期位置
    private Vector3[] originalPositions;
    // 初期スケール
    private Vector3 originalScale;

    // Start is called before the first frame update
    void Start()
    {
        // 初期位置と初期スケールを記録
        originalPositions = new Vector3[effectImage.Length];
        for (int i = 0; i < effectImage.Length; i++)
        {
            originalPositions[i] = effectImage[i].transform.localPosition;
        }
        if (effectImage.Length > 0)
        {
            originalScale = effectImage[0].transform.localScale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isHovered)
        {
            for(int i = 0; i < effectImage.Length; i++)
            {
                // 拡大
                effectImage[i].transform.localScale = Vector3.Lerp(
                    effectImage[i].transform.localScale,
                    originalScale * scaleMaltiplier,
                    Time.deltaTime * 10f);

                // 上下移動
                Vector3 pos = originalPositions[i];
                pos.y += Mathf.Sin(Time.time * moveSpeed) * moveRange;
                effectImage[i].transform.localPosition = pos;
            }
        }
        else
        {
            for(int i = 0; i < effectImage.Length; i++)
            {
                // スケールと位置をリセット
                effectImage[i].transform.localScale = originalScale;
                effectImage[i].transform.localPosition = originalPositions[i];
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // アニメーションを有効化
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // アニメーションを無効化
        isHovered = false;
    }
}
