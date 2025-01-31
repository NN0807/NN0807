using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    // インスタンス
    public static TutorialManager Instance { get; private set; }

    public GameObject sandbagModel;
    public Transform sandbagSpawnPoint;
    private GameObject activeSandbag;

    // プレイヤーの行動許可
    public bool CanMove { get; private set; }
    public bool CanDash { get; private set; }
    public bool CanPunch { get; private set; }

    public bool NowEraser { get; private set; }


    // イントロUI
    public RectTransform introUI;
    // 黒板UI
    public RectTransform blackBoardUI;
    // 説明文UI
    public RectTransform[] guideTextUIs;


    // 黒板消しUI
    public RectTransform eraserUI;

    // 黒板消しUIの開始位置
    public Vector3 eraserStartPosition;
    // 黒板消しUIの終了位置
    public Vector3 eraserEndPosition;
    // 黒板消しUIの移動時間
    public float eraserMoveDuration = 1.0f;
    // マスク用のUI
    public RectTransform eraserMask;


    // イントロUI開始位置
    public Vector3 introStartPosition;
    // イントロUI停止位置
    public Vector3 introCenterPosition;
    // イントロUI終了位置
    public Vector3 introEndPosition;
    // イントロ移動時間
    public float introMoveDuration = 1.0f;
    // イントロ停止時間
    public float introPauseDuration = 2.0f;

    // 説明文UIの開始位置
    public Vector3 guideCenterPosition;
    // 説明文UIの終了位置
    public Vector3 guideEndPosition;

    // 背景UIが表示された後の説明文UIの遅延
    public float guideDisplayDelay = 1.0f;
    // 説明文UIを画面中心に留めておく時間
    public float guideCenterPauseDuration = 1.0f;
    // 説明文UIの移動時間
    public float guideMoveDuration = 1.0f;

    // 現在の説明文UIの番号
    private int currentGuideIndex = 0;

    // プレイヤーの状態検出
    private bool hasPlayerMoved = false;
    private bool hasPlayerDashed = false;
    private bool hasPlayerPunched = false;

    [SerializeField]
    public float totalMoveTime = 0.0f;
    private float requiredMoveTime = 2.0f;

    public TutorialPlayerController tutorialPlayer;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        CanMove = false;
        CanDash = false;
        CanPunch = false;

        StartCoroutine(TutorialSequence());
    }

    public void OnDashComplete()
    {
        StartCoroutine(DashCompletionSequence());

        //ActivateSandbag();

        //ResetPlayerPosition();

        //CanMove = false;
        //CanDash = false;
        //CanPunch = true;
    }

    private IEnumerator DashCompletionSequence()
    {
        // ダッシュのガイドUIを黒板消しで消す
        yield return StartCoroutine(EraseBlackBoard());

        // 画面暗転処理
        CanvasGroup fadeCanvas = GameObject.Find("FadeCanvas").GetComponent<CanvasGroup>();
        yield return StartCoroutine(FadeScreen(fadeCanvas, 0f, 1f, 0.3f));

        // 0.5秒待機
        yield return new WaitForSeconds(0.5f);

        // プレイヤーの位置をリセット
        ResetPlayerPosition();

        // サンドバッグ配置
        ActivateSandbag();

        // 画面を元に戻す
        yield return StartCoroutine(FadeScreen(fadeCanvas, 1f, 0f, 0.3f));

        // パンチ解禁
        CanMove = false;
        CanDash = false;
        CanPunch = true;
    }

    private void ActivateSandbag()
    {
        if(activeSandbag != null)
        {
            Destroy(activeSandbag);
        }

        if(sandbagModel != null && sandbagSpawnPoint != null)
        {
            activeSandbag = Instantiate(sandbagModel, sandbagSpawnPoint.position, Quaternion.identity);
            activeSandbag.SetActive(true);
        }
    }

    private void ResetPlayerPosition()
    {
        if (tutorialPlayer != null)
        {
            tutorialPlayer.transform.position = new Vector3(0, 0, 2);
            tutorialPlayer.transform.rotation = Quaternion.Euler(0, 180, 0);
            tutorialPlayer.ResetActions();

            tutorialPlayer.gameObject.SetActive(true);
        }

    }

    IEnumerator TutorialSequence()
    {
        // イントロ処理
        yield return StartCoroutine(IntroSequence());

        // 背景UIを表示
        blackBoardUI.anchoredPosition = guideCenterPosition;
        blackBoardUI.gameObject.SetActive(true);

        // 画面中心で1秒停止
        yield return new WaitForSeconds(guideCenterPauseDuration);

        yield return StartCoroutine(MoveUI(blackBoardUI, guideCenterPosition, guideEndPosition, guideMoveDuration));

        // 画面中心で1秒停止
        yield return new WaitForSeconds(guideCenterPauseDuration);

        while(currentGuideIndex < guideTextUIs.Length)
        {
            yield return StartCoroutine(DisplayGuideUI(currentGuideIndex));

            NowEraser = true;

            if (currentGuideIndex == 0) CanMove = true;
            if (currentGuideIndex == 1) CanDash = true;
            if (currentGuideIndex == 2) CanPunch = true;

            yield return StartCoroutine(WaitForPlayerAction(currentGuideIndex));

            //CanMove = false;
            //CanDash = false;
            //CanPunch = false;

            yield return StartCoroutine(EraseBlackBoard());

            NowEraser = false;

            yield return new WaitForSeconds(guideCenterPauseDuration);

            currentGuideIndex++;
        }
    }

    IEnumerator IntroSequence()
    {
        // イントロUIを右から中心に移動
        introUI.anchoredPosition = introStartPosition;
        yield return StartCoroutine(MoveUI(introUI, introStartPosition, introCenterPosition, introMoveDuration));

        // 中心で2秒停止
        yield return new WaitForSeconds(introPauseDuration);

        // イントロUIを中心から左に移動
        yield return StartCoroutine(MoveUI(introUI, introCenterPosition, introEndPosition, introMoveDuration));
    }

    IEnumerator WaitForPlayerAction(int guideIndex)
    {
        ResetPlayerActions();
        totalMoveTime = 0.0f;

        while(true)
        {
            if (guideIndex == 0)
            {
                if (hasPlayerMoved)
                {
                    totalMoveTime += Time.deltaTime;
                }
                if (totalMoveTime >= requiredMoveTime)
                {
                    NowEraser = false;
                    break;
                }

            }
            if (guideIndex == 1)
            {
                if (hasPlayerDashed)
                {
                    totalMoveTime += Time.deltaTime;
                }
                if (totalMoveTime >= requiredMoveTime)
                {
                    NowEraser = false;
                    OnDashComplete();
                    break;
                }

            }
            if (guideIndex == 2 && hasPlayerPunched) break;

            yield return null;
        }
    }

    void ResetPlayerActions()
    {
        hasPlayerMoved = false;
        hasPlayerDashed = false;
        hasPlayerPunched = false;
    }

    private IEnumerator DisplayGuideUI(int index)
    {
        RectTransform guideUI = guideTextUIs[index];
        guideUI.gameObject.SetActive(true);
        guideUI.anchoredPosition = guideCenterPosition;
        yield return new WaitForSeconds(guideDisplayDelay);
    }

    public void RegisterPlayerAction(string actionType)
    {
        switch(actionType)
        {
            case "Walk":
                hasPlayerMoved = true;
                break;
            case "Dash":
                hasPlayerDashed = true;
                break;
            case "Attack":
                hasPlayerPunched = true;
                break;
        }
    }

    IEnumerator EraseBlackBoard()
    {
        // 黒板消しUIを表示し左から右に移動
        eraserUI.anchoredPosition = eraserStartPosition;
        // マスクも初期位置に置く
        eraserMask.anchoredPosition = eraserUI.anchoredPosition;
        eraserUI.gameObject.SetActive(true);
        eraserMask.gameObject.SetActive(true);

        float elapsedTime = 0f;
        RectTransform currentGuideUI = guideTextUIs[currentGuideIndex];
        CanvasGroup guideCanvasGroup = currentGuideUI.GetComponent<CanvasGroup>();

        // テキストの長さに合わせて消す範囲を設定
        float textWidth = guideTextUIs[currentGuideIndex].rect.width;
        float maxEraseDistance = textWidth;

        while (elapsedTime < eraserMoveDuration)
        {
            // 黒板消しとマスクの位置を更新
            float t = elapsedTime / eraserMoveDuration;
            Vector3 newPosition = Vector3.Lerp(eraserStartPosition, eraserEndPosition, t);
            eraserUI.anchoredPosition = newPosition;
            eraserMask.anchoredPosition = newPosition;

            float eraseDistance = Mathf.Lerp(0f, maxEraseDistance, t);
            guideCanvasGroup.alpha = Mathf.Lerp(1f, 0f, eraseDistance / maxEraseDistance);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 最終位置を設定
        eraserUI.anchoredPosition = eraserEndPosition;
        eraserMask.anchoredPosition = eraserEndPosition;

        // ガイドUIを非表示にして黒板消しUIを非アクティブ
        currentGuideUI.gameObject.SetActive(false);
        eraserUI.gameObject.SetActive(false);
        eraserMask.gameObject.SetActive(false);
    }

    // 画面のフェード処理
    private IEnumerator FadeScreen(CanvasGroup canvasGroup,float startAlpha,float endAlpha,float duration)
    {
        float elapsedTime = 0f;
        while(elapsedTime < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = endAlpha;
    }

    IEnumerator MoveUI(RectTransform ui, Vector3 from, Vector3 to, float duration, bool useLocalPosition = false)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            //ui.anchoredPosition = Vector3.Lerp(from, to, elapsedTime / duration);
            //elapsedTime += Time.deltaTime;
            //yield return null;
            Vector3 newPosition = Vector3.Lerp(from, to, elapsedTime / duration);

            if (useLocalPosition)
            {
                ui.localPosition = newPosition;
            }
            else
            {
                ui.anchoredPosition = newPosition;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (useLocalPosition)
        {
            ui.localPosition = to;
        }
        else
        {
            ui.anchoredPosition = to;
        }
        
    }

    IEnumerator MoveUIBoth(RectTransform ui1,RectTransform ui2,Vector3 from ,Vector3 to,float duration)
    {
        float elapsedTime = 0f;
        while(elapsedTime < duration)
        {
            Vector3 newPosition = Vector3.Lerp(from, to, elapsedTime / duration);
            ui1.anchoredPosition = newPosition;
            ui2.anchoredPosition = newPosition;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        ui1.anchoredPosition = to;
        ui2.anchoredPosition = to;
    }

}
