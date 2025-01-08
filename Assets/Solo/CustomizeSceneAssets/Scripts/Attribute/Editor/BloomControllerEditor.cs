using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BloomController))]
public class BloomControllerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		BloomController instance = target as BloomController;

		switch(instance.bloomType)
		{
			case BloomController.BloomType.Normal:
				instance.bloomIntensity = EditorGUILayout.FloatField("強度", instance.bloomIntensity);
			break;

			case BloomController.BloomType.Flash:
				instance.baseParam.speed = EditorGUILayout.FloatField("点滅速度", instance.baseParam.speed);
				instance.baseParam.factorMin = EditorGUILayout.FloatField("最小強度", instance.baseParam.factorMin);
				instance.baseParam.factorMax = EditorGUILayout.FloatField("最大強度", instance.baseParam.factorMax);
			break;

			case BloomController.BloomType.Trigger:
				instance.triggerParam.trigger = EditorGUILayout.Toggle("トリガー", instance.triggerParam.trigger);
				instance.triggerParam.speed = EditorGUILayout.FloatField("点灯速度", instance.triggerParam.speed);
				instance.triggerParam.factorMin = EditorGUILayout.FloatField("最小強度", instance.triggerParam.factorMin);
				instance.triggerParam.factorMax = EditorGUILayout.FloatField("最大強度", instance.triggerParam.factorMax);
			break;
		}

		if(GUILayout.Button("All Status Clear"))
        {
			instance.bloomIntensity = 0f;

			instance.baseParam.speed = 0f;
			instance.baseParam.factorMin = 0f;
			instance.baseParam.factorMax = 1f;

			instance.triggerParam.speed = 0f;
			instance.triggerParam.factorMin = 0f;
			instance.triggerParam.factorMax = 1f;
		}	
	}		
}
