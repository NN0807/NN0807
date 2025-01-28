using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialIntro : MonoBehaviour
{

    // UIのRectTransform
    public RectTransform introUI;
    // 画面外右の開始位置
    public Vector3 startPosition;
    // 画面中央の停止位置
    public Vector3 centerPosition;
    // 画面外左の終了位置
    public Vector3 endPosition;
    // 移動にかかる時間
    public float moveDuration = 1.5f;
    // 中央で停止する時間
    public float stopDuration = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        // 初期位置を設定
        introUI.anchoredPosition = startPosition;
        // 演出を開始
        StartCoroutine(PlayTutorialIntro());
    }

    IEnumerator PlayTutorialIntro()
    {
        // 1.画面外右から中央へ移動
        yield return StartCoroutine(MoveUI(introUI, startPosition, centerPosition, moveDuration));

        // 2.中央で2秒停止
        yield return new WaitForSeconds(stopDuration);

        // 3.中央から画面外左へ移動
        yield return StartCoroutine(MoveUI(introUI, centerPosition, endPosition, moveDuration));
    }

    IEnumerator MoveUI(RectTransform ui,Vector3 from,Vector3 to,float duration)
    {
        float elapsedTime = 0f;

        while(elapsedTime < duration)
        {
            ui.anchoredPosition = Vector3.Lerp(from, to, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        ui.anchoredPosition = to;
    }
}
