using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEditor;

// "EnumDisplayNamesAttribute"カスタムプロパティドローアクラス
[CustomPropertyDrawer(typeof(EnumDisplayNamesAttribute))]
public class EnumDisplayNamesDrawer : PropertyDrawer
{
    // Inspectorに描画されるときに呼び出される関数
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 属性のインスタンスを取得し
        // それに対応したDisplayNames変数を取得
        EnumDisplayNamesAttribute _namesAttribute = (EnumDisplayNamesAttribute)attribute;
        Dictionary<System.Enum, string> _displayNames = _namesAttribute._displayNames;

        // プロパティがEnum型の場合
        if (property.propertyType == SerializedPropertyType.Enum)
        {
            // 現在のEnum値を取得
            System.Enum _currentEnumValue = (System.Enum)System.Enum.ToObject(fieldInfo.FieldType, property.enumValueIndex);

            // 表示するオプションの配列を作成
            string[] _displayedOptions = new string[_displayNames.Count];

            // 現在のEnum値のインデックス
            int _currentIndex = 0;

            // ループカウント
            int Index = 0; 

            foreach (var pair in _displayNames) 
            {
                // 表示名を配列に追加
                _displayedOptions[Index] = pair.Value;

                // 現在のEnum値に一致する場合インデックス番号を記録する
                if (pair.Key.Equals(_currentEnumValue)) _currentIndex = Index;
                // ループカウンターを増加
                Index++; 
            }

            // Popupメニューを描画し、選択されたインデックスを取得
            int _selectedIndex = EditorGUI.Popup(position, label.text, _currentIndex, _displayedOptions);

            // Keysをリストに変換し、選択されたEnum値を取得
            List<System.Enum> keys = new List<System.Enum>(_displayNames.Keys);
            property.enumValueIndex = System.Array.IndexOf(System.Enum.GetValues(fieldInfo.FieldType), keys[_selectedIndex]);
        }
        else
        {
            // Enum型でない場合は通常のプロパティ描画
            EditorGUI.PropertyField(position, property, label);
        }
    }
}