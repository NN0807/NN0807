using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

// 全てのオブジェクトに適応
// デフォルトでこのエディタを適応
[CustomEditor(typeof(MonoBehaviour), true, isFallback = true)]
// 複数オブジェクトに適応
[CanEditMultipleObjects]
public class FoldingEditor : Editor
{
    // 折り畳み状態を保持するディクショナリ
    private Dictionary<string, bool> foldoutStates = new Dictionary<string, bool>();
    // 初期化状態
    private bool initialized;

    // エディタが有効化された時
    private void OnEnable()
    {
        // 初期化状態を false
        initialized = false;
    }

    // インスペクタに表示するGUI
    public override void OnInspectorGUI()
    {
        // オブジェクトを更新
        serializedObject.Update();
        // セットアップ処理
        Setup();

        // 折り畳み描画
        DrawFoldingProperties();

        // 変更をSerializedObjectに適用
        serializedObject.ApplyModifiedProperties();
    }

    // セットアップ処理
    void Setup()
    {
        // もうすでに初期化されていればセットアップしない
        if (initialized) { return; }

        // セットアップ完了
        initialized = true;
    }

    // 折り畳みプロパティ描画
    void DrawFoldingProperties()
    {
        // 自身のクラスからフィールド情報を取得
        List<FieldInfo> objectFields;
        int length = EditorTypes.Get(target, out objectFields);

        // 現在折り畳みグループ内かどうか
        bool insideFoldGroup = true;

        // 現在の折り畳みグループ名
        string currentFoldGroupName = "";

        // 折り畳みをしたか
        bool isFolding = false;

        // フィールド分ループ
        for (int i = 0; i < length; i++)
        {
            // フィールド情報を取得
            FieldInfo field = objectFields[i];

            // StartFoldingAttribute を持つかチェック
            StartFoldingAttribute startFold = field.GetCustomAttribute<StartFoldingAttribute>();
            if (startFold != null)
            {
                // 折り畳みをした
                isFolding = true;

                // フォールドアウト状態を取得（無ければ追加）
                if (!foldoutStates.ContainsKey(startFold.foldName))
                {
                    // デフォルトは閉じる
                    foldoutStates[startFold.foldName] = false;
                }

                // フォールドアウトグループを表示
                foldoutStates[startFold.foldName] = EditorGUILayout.Foldout(foldoutStates[startFold.foldName], startFold.foldName, true);
                // 現在の折り畳みグループ名を設定
                currentFoldGroupName = startFold.foldName;
                // 折り畳みグループ内に入ったことを記録
                insideFoldGroup = true;
            }

            // 折り畳みグループ内ならば描画
            if (!insideFoldGroup || (foldoutStates.ContainsKey(currentFoldGroupName) && foldoutStates[currentFoldGroupName]))
            {
                // EndFoldingAttribute を持つかチェック
                EndFoldingAttribute endFold = field.GetCustomAttribute<EndFoldingAttribute>();
                if (endFold != null)
                {
                    // 折り畳みグループを終了
                    insideFoldGroup = false;
                    currentFoldGroupName = "";
                }

                // シリアライズされたプロパティを取得
                SerializedProperty property = serializedObject.FindProperty(field.Name);
                if (property != null)
                {
                    EditorGUI.indentLevel++;
                    // プロパティを描画
                    EditorGUILayout.PropertyField(property, true);
                    EditorGUI.indentLevel--;
                }
            }
        }

        // 一度も折り畳み処理をしていないなら
        // デフォルトインスペクタを表示
        if(!isFolding) { DrawDefaultInspector(); }
    }
}

// タイプ情報を管理
static class EditorTypes
{
    // フィールド情報取得
    public static int Get(UnityEngine.Object target, out List<FieldInfo> objectFields)
    {
        // フィールド情報
        Dictionary<int, List<FieldInfo>> fields = new Dictionary<int, List<FieldInfo>>();
        // 型を取得
        Type t = target.GetType();
        // ハッシュコード取得
        int hash = t.GetHashCode();
        // ターゲットオブジェクトの型からすべてのフィールド情報を取得し、型ツリーの順序に基づいて並べ替え
        IList<Type> typeTree = t.GetTypeTree();
        objectFields = target.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .OrderByDescending(x => typeTree.IndexOf(x.DeclaringType))
                .ToList();

        // 取得したフィールド情報の数を返す
        return objectFields.Count;
    }

    // 継承しているクラスの型をリストで返す
    public static IList<Type> GetTypeTree(this Type t)
    {
        // 継承型リスト
        var types = new List<Type>();
        // 派生クラスである限りループ
        while (t.BaseType != null)
        {
            // リストに追加
            types.Add(t);
            // 基底クラスに変更
            t = t.BaseType;
        }

        // リストを返す
        return types;
    }
}