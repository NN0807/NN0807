using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverBattleModeEffect : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{

    // 演出用画像
    public Image effectImage;
    // 回転速度
    public float rotationSpeed = 100f;
    // 拡大縮小速度
    public float scaleSpeed = 5f;
    // 拡大縮小範囲
    public float scaleRange = 0.2f;

    // マウスが乗っているか
    private bool isHovered = false;
    // 初期スケール
    private Vector3 originalScale;
    // 現在の拡大縮小オフセット
    private float scaleOffset = 0f;

    // Start is called before the first frame update
    void Start()
    {
        if(effectImage != null)
        {
            // 初期スケールを記録
            originalScale = effectImage.transform.localScale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isHovered && effectImage != null)
        {
            // 回転
            effectImage.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

            // 拡大縮小
            scaleOffset = Mathf.Sin(Time.time * scaleSpeed) * scaleRange;
            effectImage.transform.localScale = originalScale + Vector3.one * scaleOffset;
        }
        else if(effectImage != null)
        {
            // アニメーションが無効な時はスケールをリセット

            effectImage.transform.localScale = originalScale;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }
}
