using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ゲームパッド(Xbox)振動用
/// </summary>
public class GamepadVibration : MonoBehaviour
{
    /// <summary>
    /// ゲームパッド
    /// </summary>
    Gamepad gamepad = null;

    /// <summary>
    /// タイム
    /// </summary>
    float timer = 0f;

    private void Awake()
    {
        // ゲームパッド取得
        gamepad = Gamepad.current;
        // 初期化
        gamepad.ResetHaptics();
    }

    private void Update()
    {
        if (timer <= 0f) return;

        timer -= Time.deltaTime;

        if (timer <= 0f) StopVibration();
    }

    /// <summary>
    /// 振動開始
    /// </summary>
    /// <param name="time">振動時間</param>
    /// <param name="leftStrength">左モーター回転％</param>
    /// <param name="rightStrength">右モーター回転％</param>
    public void StartVibration(float time, float leftStrength, float rightStrength)
    {
        StopVibration();
        timer = time;
        gamepad.SetMotorSpeeds(leftStrength, rightStrength);
    }

    /// <summary>
    /// 振動停止
    /// </summary>
    public void StopVibration()
    {
        timer = 0f;
        gamepad.ResetHaptics();
    }
}
