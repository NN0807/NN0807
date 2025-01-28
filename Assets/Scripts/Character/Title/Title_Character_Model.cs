using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class Title_Character_Model : MonoBehaviour
{
    // 全脚部パーツ
    [SerializeField]
    public GameObject[] LegModels    = new GameObject[CharacterConst.CONST_MODEL_NUM];

    // 全体部パーツ
    [SerializeField]
    public GameObject[] BodyModels   = new GameObject[CharacterConst.CONST_MODEL_NUM];

    // 各パーツオブジェクト変数
    private GameObject Leg    = default;
    private GameObject Body   = default;

    // 各ジョイントの検索結果を保存しておく変数
    private GameObject LegJoint  = default;
    private GameObject BodyJoint = default;


    // 開始地点と終了地点の位置と拡縮値
    private Vector3   _startPos = new Vector3(0.0f, -0.45f, -4.0f);
    private Vector3[] _endPos;
    private Vector3 _startScale = new Vector3(0.0f,   0.0f,  0.0f);
    private Vector3 _endScale   = new Vector3(1.8f,   1.8f,  1.8f);

    // イージング時間
    [SerializeField]
    private float _easingTime = 1.5f;

    // モデル生成フラグ
    [SerializeField]
    private bool _generateFlag = false;

    void Awake()
    {
        // 配列のサイズを指定して初期化
        _endPos = new Vector3[4];

        // 値を設定
        _endPos[0] = new Vector3( 1.160f, -0.711f, -0.960f);
        _endPos[1] = new Vector3(-1.190f, -0.574f, -1.294f);
        _endPos[2] = new Vector3( 1.510f,  0.590f, -2.210f);
        _endPos[3] = new Vector3(-1.250f,  0.540f, -2.110f);
    }


    // 各部位を生成し、初期化
    public void GenerateAndRegisterParts(Title_Character_Manager manager, int number)
    {
        var _bodyNumber = Random.Range(0, 8); // 0以上8未満の整数を取得
        var _legNumber  = Random.Range(0, 8); // 0以上8未満の整数を取得

        // 脚部、体部を 生成 & 登録
        Leg  = Instantiate(LegModels[_legNumber],   this.transform);
        Body = Instantiate(BodyModels[_bodyNumber], this.transform);
        manager.RegisterPart(Leg);
        manager.RegisterPart(Body);

        // ↓イージング演出

        // 開始地点を設定
        this.transform.position = _startPos;
        this.transform.localScale = _startScale;

        // 引数のEnumを変えるだけで、イージング関数の差し替えができる
        // EaseOutQuadで、絶対座標で_endPosの位置に1秒かけて移動させる
        StartCoroutine(
            Move(this.transform, _endPos[number], _easingTime, Easing.Ease.OutBack, true)
        );

        StartCoroutine(
            Scale(this.transform, _endScale, _easingTime, Easing.Ease.OutExpo)
        );

        _generateFlag = true;
    }

    // Update is called once per frame
    void Update()
    {
        // モデルパーツ接続
        if (_generateFlag) ModelConnection();
    }

    void ModelConnection()
    {
        // 体と足
        {
            // 子ノードをタグで検索  　　　検索済か？　　　　　　　　　検索                      保存データ
            GameObject LegTargetNode  = LegJoint  == null ? FindChildWithTag(Leg,  "LegJoint") : LegJoint;
            GameObject BodyTargetNode = BodyJoint == null ? FindChildWithTag(Body, "LegJoint") : BodyJoint;

            if (LegTargetNode != null && BodyTargetNode != null)
            {
                // 何度も検索を行うと処理負荷につながる為、保存しておく。
                LegJoint  = LegTargetNode;
                BodyJoint = BodyTargetNode;

                // 接続ボーンのワールド座標を取得
                Vector3 worldPosition  = LegJoint.transform.position;
                Vector3 worldPosition2 = BodyJoint.transform.position;

                // 2つの接続点のベクトルを算出する
                Vector3 Vec = new Vector3(
                    worldPosition.x - worldPosition2.x,
                    worldPosition.y - worldPosition2.y,
                    worldPosition.z - worldPosition2.z
                );

                // 接続
                Body.transform.position = Body.transform.position + Vec;

                // 回転
                Body.transform.rotation = Leg.transform.rotation;
            }
            else
            {
                Debug.LogError("指定したタグの子ノードが見つかりませんでした");
            }
        }
    }

    // 子ノードをタグで再帰的に検索
    GameObject FindChildWithTag(GameObject parent, string tag)
    {
        // 子オブジェクトを探索
        foreach (Transform child in parent.transform)
        {
            if (child.CompareTag(tag)) return child.gameObject;

            // 再帰的に探索
            GameObject result = FindChildWithTag(child.gameObject, tag);
            if (result != null) return result;
        }

        return null;
    }

    // 指定したTransformの座標を更新する
    // 引数 Transform, 目的値の座標、何秒で動かすか、イージングの種類、移動先が絶対座標かどうか
    public IEnumerator Move(Transform transform, Vector3 destinationPos, float seconds, Easing.Ease easing, bool absolute)
    {
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
    }

    public IEnumerator Scale(Transform transform, Vector3 destinationScale, float seconds, Easing.Ease easing)
    {
        // イージング関数の取得
        var Ease = Easing.GetEasingMethod(easing);

        // 現在点と移動先の設定
        Vector3 staScale = transform.localScale;
        Vector3 endScale = destinationScale;
        // 初期地点と目標地点の差
        Vector3 difScale = endScale - staScale;

        // N秒かけて移動させる
        float e = 0;
        while (true)
        {
            yield return null;
            e += Time.deltaTime / seconds;
            if (e >= 1.0f)
            {
                transform.localScale = endScale;
                break;
            }
            Vector3 nextPos = staScale + Ease(e) * difScale;
            transform.localScale = nextPos;
        }
    }

    // モデルの位置取得関数
    public Vector3 GetModelPosition() { return Leg.transform.position; }
}
