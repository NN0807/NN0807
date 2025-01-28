using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class Idle_Character_Model : MonoBehaviour
{
    // 全脚部パーツ
    [SerializeField]
    public GameObject[] LegModels  = new GameObject[CharacterConst.CONST_MODEL_NUM];

    // 全体部パーツ
    [SerializeField]
    public GameObject[] BodyModels = new GameObject[CharacterConst.CONST_MODEL_NUM];

    // 各パーツオブジェクト変数
    private GameObject Leg  = default;
    private GameObject Body = default;

    // 各ジョイントの検索結果を保存しておく変数
    private GameObject LegJoint  = default;
    private GameObject BodyJoint = default;

    // モデル生成フラグ
    [SerializeField]
    private bool _generateFlag = false;


    // 各部位を生成し、初期化
    public void GenerateAndRegisterParts(Idle_Character_Manager manager, int number)
    {
        // カスタマイズシーンで選択した各パーツ番号を読み込む
        // キー名「body,leg,punch」の値をロードする。データが存在しない場合「0」を返す
        // ※セーブ処理　SlotManager.cs 284行目
        // キー名の後で指定しているのは、データが存在しなかった場合のデフォルト値
        var _bodyNumber   = PlayerPrefs.GetInt("body",  0);
        var _legNumber    = PlayerPrefs.GetInt("leg",   0);
        var _weaponNumber = PlayerPrefs.GetInt("punch", 0);

        // 脚部、体部を 生成 & 登録
        Leg  = Instantiate(LegModels[_legNumber],   this.transform);
        Body = Instantiate(BodyModels[_bodyNumber], this.transform);
        manager.RegisterPart(Leg);
        manager.RegisterPart(Body);

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
            GameObject LegTargetNode = LegJoint == null ? FindChildWithTag(Leg, "LegJoint") : LegJoint;
            GameObject BodyTargetNode = BodyJoint == null ? FindChildWithTag(Body, "LegJoint") : BodyJoint;

            if (LegTargetNode != null && BodyTargetNode != null)
            {
                // 何度も検索を行うと処理負荷につながる為、保存しておく。
                LegJoint = LegTargetNode;
                BodyJoint = BodyTargetNode;

                // 接続ボーンのワールド座標を取得
                Vector3 worldPosition = LegJoint.transform.position;
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

    // モデルの位置取得関数
    public Vector3 GetModelPosition() { return Leg.transform.position; }
}
