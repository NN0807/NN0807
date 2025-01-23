using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverMouseEffect : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    // 元のスケールを保持
    private Vector3 originalScale;
    // 目標スケール
    private Vector3 targetScale;
    // 拡大倍率
    public float scaleMultiplier = 1.1f;
    // アニメーションの速度
    public float animationSpeed = 10f;

    // マウスが乗ってるか
    private bool isHovered = false;

    // Start is called before the first frame update
    private void Start()
    {
        //初期スケールを保存
        originalScale = transform.localScale;
        // 初期値設定
        targetScale = originalScale;
    }

    private void Update()
    {
        if(isHovered)
        {
            // 現在のスケールを目標スケールに滑らかに補間
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * animationSpeed);
        }
        else
        {
            transform.localScale = originalScale;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        // スケール拡大
        targetScale = originalScale * scaleMultiplier;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        // 元のスケールに戻す
        targetScale = originalScale;
    }
}
