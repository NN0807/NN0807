using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetWorkCustomizeSceneManager : MonoBehaviour
{
    /// <summary>
    /// キャラクターカスタマイズ中か
    /// </summary>
    public bool isCharacterCustomize = default;
    /// <summary>
    /// ステージ選択中か
    /// </summary>
    public bool isStgaeSelect = default;

    ///<summary>入力処理</summary>
    [SerializeField]
    private CustomizeSceneController inputActions;

    ///<summary>
    /// キャラクター
    /// </summary>
    [SerializeField]
    private GameObject customizeCharacter = default;

    ///<summary>
    /// スロット
    /// </summary>
    [SerializeField]
    private GameObject Slot = default;

    /// <summary>
    /// ステージ選択
    /// </summary>
    [SerializeField]
    private GameObject selectStageManager = default;

    [SerializeField]
    private GameObject selectStageArrowUI = default;

    ///<summary>
    /// パーツネームUI
    /// </summary>
    [SerializeField]
    private GameObject partsNameUI = default;

    /// <summary>
    /// 移動フラグ
    /// スロットとキャラクターが動いている時はtrue,そうでなければfalse
    /// </summary>
    [HideInInspector]
    public bool isMoving = default;

    // 静的インスタンス
    [HideInInspector]
    public static NetWorkCustomizeSceneManager Instance { get; private set; }

    private void Awake()
    {
        // 既にインスタンスが存在する場合は破棄する
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // このインスタンスを設定
        Instance = this;

        // 入力処理初期化
        inputActions = new CustomizeSceneController();
        inputActions.Enable();
    }

    private void Update()
    {
        // 決定・戻る処理
        if (inputActions.UI.Decision.triggered)
        {
            SelectComplete();
        }
        if (inputActions.UI.Back.triggered)
        {
            BackScene();
        }
    }

    /// <summary>
    /// ステージセレクトに遷移
    /// </summary>
    public void ChangeStageSelect()
    {
        // キャラクターの移動先
        Vector3 characterTargetPos = customizeCharacter.transform.position + (-Camera.main.transform.right * 2f);

        // スロットの移動先
        Vector3 slotTargetPos = Slot.transform.position;
        slotTargetPos.x -= 1200f;

        // パーツ名UI非表示
        partsNameUI.SetActive(false);

        // ステージセレクトアクティブ化
        selectStageManager.SetActive(true);

        // スロットとキャラクターをイージングで画面から外れる
        StartCoroutine(Move(Slot.transform, slotTargetPos, 1f, Easing.Ease.InSine, false));
        StartCoroutine(Move(customizeCharacter.transform, characterTargetPos, 1f, Easing.Ease.InSine, true));

        // ステージ選択をイージングで画面内へ
        StartCoroutine(Move(selectStageManager.transform, new Vector3(0, 0, 60), 3f, Easing.Ease.OutBack, true));

        StartCoroutine(WaitForMoveComplete());
    }

    ///<summary>戻る</summary>
    public void BackScene()
    {
        // 移動中は処理しない
        if (isMoving) return;

        // デバッグ
        Debug.Log("ひとつ前に戻る");

        // カスタマイズから戻るので前のシーンへ
        if (isCharacterCustomize)
        {
            // 前のシーンへ
        }
        // ステージから戻るのでカスタマイズへ
        else if (isStgaeSelect)
        {
            // スロットとキャラクターをアクティブ化
            Slot.SetActive(true);
            customizeCharacter.SetActive(true);

            // ステージ選択用矢印UIを非アクティブ化
            selectStageArrowUI.SetActive(false);

            // ステージ選択をイージングで画面外に
            StartCoroutine(Move(selectStageManager.transform, new Vector3(35, 0, 0), 1f, Easing.Ease.InSine, false));

            // カスタマイズキャラクターとスロットを画面内へ
            StartCoroutine(Move(Slot.transform, new Vector3(-570f, 70f, 0f), 3f, Easing.Ease.OutBack, true));
            StartCoroutine(Move(customizeCharacter.transform, new Vector3(0.4f, 0.74f, -9.2f), 3f, Easing.Ease.OutBack, true));

            StartCoroutine(WaitForMoveComplete());
        }
    }

    ///<summary>決定</summary> 
    public void SelectComplete()
    {
        // 移動中は処理しない
        if (isMoving) return;

        // デバッグ表示
        Debug.Log("決定");

        // カスタマイズ決定時処理
        if (isCharacterCustomize)
        {
            // 選択されたパーツ文字列をデータに保存
            Slot.GetComponent<NetWorkSlotManager>().SavePartsData();

            // ステージ選択画面に遷移
            ChangeStageSelect();
        }
        // ステージ決定時処理
        else if (isStgaeSelect)
        {
            // 選択ステージ点滅
            StartCoroutine(SpriteFlash(selectStageManager.GetComponent<SelectStageManager>().stageSpr[0].GetComponent<SpriteRenderer>(), 3, 10f));

            // ステージデータ保存
            PlayerPrefs.SetInt("stageIngex", selectStageManager.GetComponent<SelectStageManager>().selectStageNum);

            // シーン遷移
            Debug.Log("シーン遷移");
        }
    }

    /// <summary>
    ///  移動完了を待つ
    /// </summary>
    public IEnumerator WaitForMoveComplete()
    {
        while (isMoving)
        {
            yield return null;
        }

        if (isStgaeSelect)
        {
            // フラグ制御
            isCharacterCustomize = true;
            isStgaeSelect = false;
        }
        else if (isCharacterCustomize)
        {
            // ステージ選択用の矢印をアクティブ化
            selectStageArrowUI.SetActive(true);

            // スロットとキャラクターを非アクティブ化
            Slot.SetActive(false);
            customizeCharacter.SetActive(false);

            isCharacterCustomize = false;
            isStgaeSelect = true;
        }
    }

    // 指定したTransformの座標を更新する
    // 引数 Transform, 目的値の座標、何秒で動かすか、イージングの種類、移動先が絶対座標かどうか
    public IEnumerator Move(Transform transform, Vector3 destinationPos, float seconds, Easing.Ease easing, bool absolute)
    {
        isMoving = true;

        // イージング関数の取得
        var Ease = Easing.GetEasingMethod(easing);

        // 現在点と移動先の設定
        Vector3 staPos = transform.localPosition;
        Vector3 endPos = absolute ? destinationPos : staPos + destinationPos;
        // 初期地点と目標地点の差
        Vector3 difPos = endPos - staPos;

        // N秒かけて移動させる
        float e = 0;
        while (true)
        {
            isMoving = true;
            yield return null;
            e += Time.deltaTime / seconds;
            if (e >= 1.0f)
            {
                transform.localPosition = endPos;
                break;
            }
            Vector3 nextPos = staPos + Ease(e) * difPos;
            transform.localPosition = nextPos;
        }

        isMoving = false;
    }

    /// <summary>
    /// 点滅処理
    /// </summary>
    /// <param name="sprite">点滅させる画像</param>
    /// <param name="flashCount">点滅回数</param>
    /// <param name="flashSpeed">点滅スピード(高いほど早い)</param>
    /// <returns></returns>
    public IEnumerator SpriteFlash(SpriteRenderer spriteRenderer, int flashCount, float flashSpeed)
    {
        if (spriteRenderer == null)
        {
            yield break; // SpriteRenderer が null の場合は処理を終了
        }

        Color originalColor = spriteRenderer.color; // 元の色を保持
        float alpha;

        for (int i = 0; i < flashCount; i++)
        {
            // フェードアウト
            for (alpha = 1f; alpha >= 0f; alpha -= Time.deltaTime * flashSpeed)
            {
                spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;
            }

            // フェードイン
            for (alpha = 0f; alpha <= 1f; alpha += Time.deltaTime * flashSpeed)
            {
                spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;
            }
        }

        // 最後に元の色に戻す
        spriteRenderer.color = originalColor;
    }
}
