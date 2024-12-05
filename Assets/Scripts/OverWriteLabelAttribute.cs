using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;
#endif

public class OverwriteLabelAttribute : PropertyAttribute
{
	public readonly GUIContent Label;
	public OverwriteLabelAttribute(string label)
	{
		Label = new GUIContent(label);
	}
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(OverwriteLabelAttribute))]
public class CustomLabelAttributeDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		var newLabel = attribute as OverwriteLabelAttribute;
		label = newLabel.Label;
		EditorGUI.PropertyField(position, property, label, true);
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return EditorGUI.GetPropertyHeight(property, true);
	}
}
#endif
