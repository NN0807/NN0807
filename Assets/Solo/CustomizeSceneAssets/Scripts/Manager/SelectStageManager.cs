using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectStageManager : MonoBehaviour
{
    ///<summary>
    /// 画像オブジェクト
    /// </summary>
    [SerializeField]
    [ReadOnly]
    public GameObject[] stageSpr = new GameObject[6];

    /// <summary>
    /// 目標位置
    /// </summary>
    [SerializeField]
    [ReadOnly]
    public Transform[] targetPos = new Transform[6];

    /// <summary>
    /// 移動完了フラグ
    /// </summary>
    [SerializeField]
    [ReadOnly]
    private bool isMoving = default;

    ///<summary>入力処理</summary>
    [SerializeField]
    private CustomizeSceneController inputActions;

    /// <summary>
    /// 選択ステージ番号
    /// </summary>
    [SerializeField]
    [ReadOnly]
    public int selectStageNum = default;

    private void Awake()
    {
        // 入力処理初期化
        inputActions = new CustomizeSceneController();
        inputActions.Enable();
    }

    private void Update()
    {
        // 遷移移動中は処理しない
        if (NetWorkCustomizeSceneManager.Instance != null)
        {
            if (NetWorkCustomizeSceneManager.Instance.isMoving) return;
        }
        if (CustomizeSceneManager.Instance != null)
        {
            if (CustomizeSceneManager.Instance.isMoving) return;
        }


        if (rouletteMoveFlg)
        {
            StartCoroutine(RouletteAnimation());
            rouletteFlg = true;
        }

        // ランダムルーレット処理
        if (rouletteFlg) return;

        // 入力制御
        if (isMoving) return;
        // 左に移動
        if (inputActions.UI.Move.ReadValue<Vector2>().x > 0.5f)
        {
            RightRotation();
            selectStageNum++;
            if (selectStageNum > stageSpr.Length - 1) selectStageNum = 0;
        }

        // 右に移動
        if (inputActions.UI.Move.ReadValue<Vector2>().x < -0.5f)
        {
            LefRotation();
            selectStageNum--;
            if (selectStageNum < 0) selectStageNum = stageSpr.Length - 1;
        }
    }

    // 右回転
    public void RightRotation()
    {
        // ステージ回転音
        AudioManager.instance.Play(SEPath.StageSelectRotate, 0.004f);

        for (int i = 0; i < stageSpr.Length; i++)
        {
            int targetPosNum = i - 1;
            if (targetPosNum < 0) targetPosNum = stageSpr.Length - 1;

            StartCoroutine(Move(stageSpr[i].transform, targetPos[targetPosNum].position, 0.8f, Easing.Ease.OutBack, true));
        }

        stageSpr = ShiftArray<GameObject>(stageSpr, true);
    }

    // 左回転
    public void LefRotation()
    {
        // ステージ回転音
        AudioManager.instance.Play(SEPath.StageSelectRotate, 0.004f);

        for (int i = 0; i < stageSpr.Length; i++)
        {
            int targetPosNum = i + 1;
            if (targetPosNum == targetPos.Length) targetPosNum = 0;

            StartCoroutine(Move(stageSpr[i].transform, targetPos[targetPosNum].position, 0.8f, Easing.Ease.OutBack, true));
        }

        stageSpr = ShiftArray<GameObject>(stageSpr, false);
    }

    /// <summary>
    /// 配列を1つずらす（左または右方向にシフト）
    /// </summary>
    /// <typeparam name="T">配列の型</typeparam>
    /// <param name="array">ずらす配列</param>
    /// <param name="isLeftRotation">左回転か右回転かを決めるフラグ</param>
    /// <returns>シフト後の新しい配列</returns>
    private T[] ShiftArray<T>(T[] array, bool isLeftRotation = true)
    {
        // 並び替えが無効な配列の場合そのまま返す
        if (array == null || array.Length <= 1) return array;

        // 新たな配列格納用
        T[] newArray = new T[array.Length];

        if (isLeftRotation)
        {
            // 左回転の場合
            // 最後の要素を最初に持ってくる
            newArray[array.Length - 1] = array[0];

            for (int i = 0; i < array.Length - 1; i++)
            {
                // 1つずつずらして格納
                newArray[i] = array[i + 1];
            }
        }
        else
        {
            // 右回転の場合
            // 最初の要素を最後に持ってくる
            newArray[0] = array[array.Length - 1];

            for (int i = 1; i < array.Length; i++)
            {
                // 1つずつずらして格納
                newArray[i] = array[i - 1];
            }
        }

        // 新しい配列を返す
        return newArray;
    }
    // 指定したTransformの座標を更新する
    // 引数 Transform, 目的値の座標、何秒で動かすか、イージングの種類、移動先が絶対座標かどうか
    public IEnumerator Move(Transform transform, Vector3 destinationPos, float seconds, Easing.Ease easing, bool absolute)
    {
        isMoving = true;

        // イージング関数の取得
        var Ease = Easing.GetEasingMethod(easing);

        // 現在点と移動先の設定
        Vector3 staPos = transform.position;
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
                transform.position = endPos;
                break;
            }
            Vector3 nextPos = staPos + Ease(e) * difPos;
            transform.position = nextPos;
        }

        isMoving = false;
    }


    /// <summary>
    /// ランダムルーレット更新制御フラグ
    /// </summary>
    [SerializeField]
    [OverwriteLabel("ルーレット更新制御フラグ")]
    [ReadOnly]
    private bool rouletteFlg = default;

    /// <summary>
    /// ルーレットの回転フラグ
    /// </summary>
    [SerializeField]
    [OverwriteLabel("ルーレット回転フラグ")]
    private bool rouletteMoveFlg = default;

    /// <summary>
    /// 経過時間
    /// </summary>
    [SerializeField]
    [OverwriteLabel("経過時間")]
    [ReadOnly]
    private float elapsedTime = 0.0f;

    [SerializeField]
    [OverwriteLabel("ルーレット合計時間に対する経過時間の割合")]
    [ReadOnly]
    private float rate = 0f;

    [SerializeField]
    [OverwriteLabel("停止するステージ番号")]
    private int targetStageNum = 0;

    [SerializeField]
    [OverwriteLabel("必要な回転数")]
    [ReadOnly]
    private int totalSpins = 0;

    /// <summary>
    /// ランダムルーレットのアニメーションを実行する
    /// </summary>
    private IEnumerator RouletteAnimation()
    {
        // 実行フラグをリセット
        rouletteMoveFlg = false; 

        // ランダムで次のステージ番号を選択
        int randomStageNum = Random.Range(0, stageSpr.Length);

        // 必要な回転数を算出
        totalSpins = (targetStageNum - selectStageNum + stageSpr.Length) % stageSpr.Length;
        totalSpins = 41 - (stageSpr.Length - totalSpins);

        // 現在の回転数
        int currentSpins = 0;

        // 経過時間初期化
        elapsedTime = 0.0f;

        // ルーレット処理
        while (elapsedTime < 10f)
        {
            // 経過時間を更新
            elapsedTime += Time.deltaTime;

            // 補完割合算出
            rate = elapsedTime / 10f;

            // 加速と減速をイージングで制御（加速：前半、減速：後半）
            float easingRate = rate < 0.5f
                ? Mathf.Lerp(0, 1, Mathf.Pow(rate * 2, 1))            // 加速部分
                : Mathf.Lerp(1, 0, Mathf.Pow((rate - 0.5f) * 2, 1));  // 減速部分

            // 現在の速度を計算
            float speedFactor = Mathf.Lerp(0.5f, 0.08f, easingRate);
            // 処理を止める分を経過時間として数える
            elapsedTime += speedFactor;

            // 現在のステージを右回転
            RightRotation(); 
            // 選択ステージ番号設定
            selectStageNum = (selectStageNum + 1) % stageSpr.Length;

            // 次の回転まで待機
            yield return new WaitForSeconds(speedFactor);

            // 終了条件
            // 指定した番号で止めるため
            if (currentSpins >= totalSpins)
            {
                break;
            }
            // 現在の回転数増加
            currentSpins++;
        }

        // ランダムルーレット終了後の処理
        Debug.Log($"ランダムルーレット終了: 選択されたステージ番号 {selectStageNum}");

        // 自動で決定する
        CustomizeSceneManager.Instance.SelectComplete();
    }
}
