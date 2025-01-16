using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common;

public class CharacterModel : MonoBehaviour
{
    // 全脚部パーツ
    [SerializeField]
    public GameObject[] LegModels    = new GameObject[CharacterConst.CONST_MODEL_NUM];

    // 全体部パーツ
    [SerializeField]
    public GameObject[] BodyModels   = new GameObject[CharacterConst.CONST_MODEL_NUM];

    // 全武器パーツ
    [SerializeField]
    public GameObject[] WeaponModels = new GameObject[CharacterConst.CONST_MODEL_NUM];

    // 各パーツオブジェクト変数
    private GameObject Leg    = default;
    private GameObject Body   = default;
    private GameObject Weapon = default;

    // 各ジョイントの検索結果を保存しておく変数
    private GameObject LegJoint    = default;
    private GameObject BodyJoint1  = default;
    private GameObject BodyJoint2  = default;
    private GameObject WeaponJoint = default;

    // GameObjectを引数に取り、生成イベントを処理するためのデリゲート
    public delegate void ObjectCreatedHandler(GameObject createdObject);
    // GameObject生成時に通知するイベント
    public static event ObjectCreatedHandler OnObjectCreated;

    // 各部位を生成し、初期化
    public void GenerateAndRegisterParts(CharacterManager manager, int characterNumber)
    {
        // カスタマイズシーンで選択した各パーツ番号を読み込む
        // キー名「body,leg,punch」の値をロードする。データが存在しない場合「0」を返す
        // ※セーブ処理　SlotManager.cs 284行目
        // キー名の後で指定しているのは、データが存在しなかった場合のデフォルト値
        var _bodyNumber   = PlayerPrefs.GetInt("body",  0);
        var _legNumber    = PlayerPrefs.GetInt("leg",   0);
        var _weaponNumber = PlayerPrefs.GetInt("punch", 0);


        // 脚部、体部、武器を 生成 & 登録
        GenerateTransform _mt = TransformInfo._generateTransforms[characterNumber];
        Leg    = Instantiate(LegModels[0],   this.transform);
        Body   = Instantiate(BodyModels[0],  this.transform);
        Weapon = Instantiate(WeaponModels[0],this.transform);
        Leg.   transform.localScale = _mt.Scale;
        Body.  transform.localScale = _mt.Scale;
        // 武器は体部に格納しておく為、拡縮値を"0"にしておく
        Weapon.transform.localScale = Initialize.Vector3;
        manager.RegisterPart(Leg);
        manager.RegisterPart(Body);
        manager.RegisterPart(Weapon);

        // イベントで通知
        OnObjectCreated?.Invoke(Leg);
    }

    public void ModelUpdate(CharacterManager manager, int _characterNumber)
    {
        // モデルパーツ接続
        ModelConnection();
        // 武器の拡縮値更新
        WeaponScaleUpdate(manager, _characterNumber);
    }

    void ModelConnection()
    {
        // 体と足
        {
            // 子ノードをタグで検索  　　　検索済か？　　　　　　　　　検索                      保存データ
            GameObject LegTargetNode  = LegJoint   == null ? FindChildWithTag(Leg, "LegJoint") : LegJoint;
            GameObject BodyTargetNode = BodyJoint1 == null ? FindChildWithTag(Body,"LegJoint") : BodyJoint1;

            if (LegTargetNode != null && BodyTargetNode != null)
            {
                // 何度も検索を行うと処理負荷につながる為、保存しておく。
                LegJoint   = LegTargetNode;
                BodyJoint1 = BodyTargetNode;

                // 接続ボーンのワールド座標を取得
                Vector3 worldPosition  = LegJoint.transform.position;
                Vector3 worldPosition2 = BodyJoint1.transform.position;

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

        // 体と武器
        {
            // 子ノードをタグで検索  　　　検索済か？　　　　　　　　　検索                               保存データ
            GameObject BodyTargetNode   = BodyJoint2  == null ? FindChildWithTag(Body,  "WeaponJoint") : BodyJoint2;
            GameObject WeaponTargetNode = WeaponJoint == null ? FindChildWithTag(Weapon,"WeaponJoint") : WeaponJoint;

            if (WeaponTargetNode != null && BodyTargetNode != null)
            {
                // 何度も検索を行うと処理負荷につながる為、保存しておく。
                BodyJoint2  = BodyTargetNode;
                WeaponJoint = WeaponTargetNode;

                // 接続ボーンのワールド座標を取得
                Vector3 worldPosition  = BodyJoint2.transform.position;
                Vector3 worldPosition2 = WeaponJoint.transform.position;

                // 2つの接続点のベクトルを算出する
                Vector3 Vec = new Vector3(
                    worldPosition.x - worldPosition2.x,
                    worldPosition.y - worldPosition2.y,
                    worldPosition.z - worldPosition2.z
                );

                // 接続
                Weapon.transform.position = Weapon.transform.position + Vec;

                // 回転
                Weapon.transform.rotation = Leg.transform.rotation;
            }
            else
            {
                Debug.LogError("指定したタグの子ノードが見つかりませんでした");
            }
        }
    }

    // 武器の拡縮値更新処理
    void WeaponScaleUpdate(CharacterManager manager, int _characterNumber)
    {
        // アニメーションフラグが立っていたら
        if (manager.GetAnimationEvent())
        {
            GenerateTransform _mt = TransformInfo._generateTransforms[_characterNumber];
            // ラープでスケールを徐々に大きく
            Weapon.transform.localScale = Vector3.Lerp(Weapon.transform.localScale, _mt.Scale, Time.deltaTime * 15f);
        }
        else
        {
            // ラープでスケールを徐々に小さく
            if (Weapon.transform.localScale.x > 0.01f)
            {
                Weapon.transform.localScale = Vector3.Lerp(Weapon.transform.localScale, new Vector3(0f, 0f, 0f), Time.deltaTime * 15f);
            }
            else
            {
                Weapon.transform.localScale = new Vector3(0f, 0f, 0f);
            }
        }
    }

    // ギズモ
    void OnDrawGizmos()
    {
        if (LegJoint != null)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawSphere(LegJoint.transform.position, 0.02f);
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
            if (result != null)return result;
        }

        return null;
    }

    // モデルの位置取得関数
    public Vector3 GetModelPosition() { return Leg.transform.position; }
}
