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
    }
}
