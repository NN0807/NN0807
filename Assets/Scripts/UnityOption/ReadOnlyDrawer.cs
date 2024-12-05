using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 読み取り専用にする
        GUI.enabled = false;  

        // プロパティの描画
        EditorGUI.PropertyField(position, property, label, true);

        // GUI操作を有効に戻す
        GUI.enabled = true;  
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property.propertyType == SerializedPropertyType.Generic)
        {
            // 構造体やクラスの高さを計算
            float totalHeight = EditorGUIUtility.singleLineHeight;
            SerializedProperty iterator = property.Copy();
            SerializedProperty endProperty = property.GetEndProperty();

            while (iterator.NextVisible(true) && !SerializedProperty.EqualContents(iterator, endProperty))
            {
                totalHeight += EditorGUI.GetPropertyHeight(iterator, true) + 2;
            }

            return totalHeight;
        }
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}
#endif