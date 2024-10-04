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
        // �ǂݎ���p�ɂ���
        GUI.enabled = false;  

        // �v���p�e�B�̕`��
        EditorGUI.PropertyField(position, property, label, true);

        // GUI�����L���ɖ߂�
        GUI.enabled = true;  
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property.propertyType == SerializedPropertyType.Generic)
        {
            // �\���̂�N���X�̍������v�Z
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