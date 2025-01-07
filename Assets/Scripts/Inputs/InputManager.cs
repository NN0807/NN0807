using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private string currentInputDevice = "Gamepad"; // 初期値をゲームパッドに設定

    private void OnEnable()
    {
        // デバイスの接続/切断のイベントに処理登録
        InputSystem.onDeviceChange += OnDeviceChange;
        InputSystem.onActionChange += OnActionChange;

        // 現在の入力デバイスを確認
        CheckInitialInputDevice();
    }

    private void OnDisable()
    {
        // デバイスの接続/切断のイベントの処理解除
        InputSystem.onDeviceChange -= OnDeviceChange;
        InputSystem.onActionChange -= OnActionChange;
    }

    // デバイスの変更があった場合の処理
    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (device is Gamepad || device is Keyboard)
        {
            Debug.Log($"デバイスの変更検出: {device.displayName}, 種類: {device.GetType().Name}");
        }
    }

    // 入力アクションの変更を監視
    private void OnActionChange(object obj, InputActionChange change)
    {
        if (change == InputActionChange.ActionPerformed)
        {
            if (obj is InputAction action && action.activeControl != null)
            {
                var device = action.activeControl.device;

                if (device is Gamepad)
                {
                    SetCurrentInputDevice("Gamepad");
                }
                else if (device is Keyboard)
                {
                    SetCurrentInputDevice("Keyboard");
                }
            }
        }
    }

    // 初期の入力デバイスを確認
    private void CheckInitialInputDevice()
    {
        foreach (var device in InputSystem.devices)
        {
            if (device is Gamepad)
            {
                SetCurrentInputDevice("Gamepad");
                break;
            }
        }
    }

    // 現在の入力デバイスを設定
    private void SetCurrentInputDevice(string deviceType)
    {
        if (currentInputDevice != deviceType)
        {
            currentInputDevice = deviceType;
            Debug.Log($"現在の入力デバイス: {currentInputDevice}");
        }
    }
}
