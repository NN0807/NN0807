using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enumの日本語表示名を設定するカスタム属性クラス
public class EnumDisplayNamesAttribute : PropertyAttribute
{
    // Enum値とそれに対応した日本語名のペア
    public readonly Dictionary<System.Enum, string> _displayNames;

    // コンストラクタで初期化する
    public EnumDisplayNamesAttribute(System.Type enumType, params string[] displayNames)
    {
        // 初期化
        _displayNames = new Dictionary<System.Enum, string>();

        // Enum全取得
        var enumValues = System.Enum.GetValues(enumType); 

        // Enumの数だけ
        for (int index = 0; index < enumValues.Length; index++)
        {
            if (index < displayNames.Length)
            {
                // 日本語名を追加する
                _displayNames.Add((System.Enum)enumValues.GetValue(index), displayNames[index]);
            }
        }
    }
}
