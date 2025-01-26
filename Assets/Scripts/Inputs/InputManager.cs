using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;


public class InputManager : MonoBehaviour
{
    // 初期値をゲームパッドに設定
    private string _currentInputDevice = "Gamepad"; 

    private void OnEnable()
    {
        // デバイスの接続/切断のイベントに処理登録
        InputSystem.onDeviceChange += OnDeviceChange;

        // 入力アクションの変化を監視
        InputSystem.onActionChange += OnActionChange;

        // 現在の入力デバイスを確認
        CheckInitialInputDevice();

        InputSystem.onEvent += OnInputEvent;
    }

    private void OnDisable()
    {
        // デバイスの接続/切断のイベントの処理解除
        InputSystem.onDeviceChange -= OnDeviceChange;
        InputSystem.onActionChange -= OnActionChange;

        InputSystem.onEvent -= OnInputEvent;
    }

    // デバイスの変更があった場合の処理
    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        // ゲームパッドまたはキーボードが接続された場合
        if (device is Gamepad || device is Keyboard)
        {
            // 接続または再接続されたとき
            if (change == InputDeviceChange.Added || change == InputDeviceChange.Reconnected)
            {
                UpdateCurrentInputDevice();
            }
            // 切断されたとき
            else if (change == InputDeviceChange.Removed || change == InputDeviceChange.Disconnected)
            {
                UpdateCurrentInputDevice();
            }
        }
    }

    // 入力アクションの変更を監視
    private void OnActionChange(object obj, InputActionChange change)
    {
        if (change == InputActionChange.ActionPerformed || change == InputActionChange.ActionStarted)
        {
            if (obj is InputAction action && action.activeControl != null)
            {
                var device = action.activeControl.device;

                // アクションの発生したデバイスを切り替え
                if (device is Gamepad)
                {
                    SetCurrentInputDevice("Gamepad");
                }
                else if (device is Keyboard || device is Mouse)
                {
                    SetCurrentInputDevice("Keyboard/Mouse");
                }
            }
        }
    }

    private void OnInputEvent(InputEventPtr eventPtr, InputDevice device)
    {
        if (device is Gamepad)
        {
            SetCurrentInputDevice("Gamepad");
        }
        else if (device is Mouse || device is Keyboard)
        {
            SetCurrentInputDevice("Keyboard/Mouse");
        }
    }

    // 現在の入力デバイスを確認
    private void CheckInitialInputDevice()
    {
        foreach (var device in InputSystem.devices)
        {
            // ゲームパッドの場合
            if (device is Gamepad)
            {
                SetCurrentInputDevice("Gamepad");
                break;
            }
            // キーボード&マウスの場合
            else if (device is Keyboard || device is Mouse)
            {
                SetCurrentInputDevice("Keyboard/Mouse");
                break;
            }
        }
    }

    // 現在の入力デバイスを設定
    private void SetCurrentInputDevice(string deviceType)
    {
        if (_currentInputDevice != deviceType)
        {
            _currentInputDevice = deviceType;
            Debug.Log($"現在の入力デバイス: {_currentInputDevice}");
        }
    }

    // デバイスが変更された場合に現在の入力デバイスを更新
    private void UpdateCurrentInputDevice()
    {
        bool gamepadConnected = false;
        bool keyboardMouseConnected = false;

        // 接続されているデバイスをチェック
        foreach (var device in InputSystem.devices)
        {
            if (device is Gamepad) gamepadConnected = true;
            if (device is Keyboard || device is Mouse) keyboardMouseConnected = true;
        }

        if (gamepadConnected)
        {
            SetCurrentInputDevice("Gamepad");
        }
        else if (keyboardMouseConnected)
        {
            SetCurrentInputDevice("Keyboard/Mouse");
        }
    }

    // 現在の入力デバイスを取得
    public string GetCurrentInputDevice() { return _currentInputDevice; }
}