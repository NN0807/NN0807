using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Common;

public class CharacterOperation : MonoBehaviour
{
    // 入力処理
    [SerializeField]
    private HakopanControls _InputActions;

    // 入力方向数値
    [SerializeField]
    private float _HorizontalInput = 0.0f;
    [SerializeField]
    private float _VerticalInput   = 0.0f;

    // コントローラー振動フラグ
    [SerializeField]
    private bool _vibrationFlag = false;

    // 攻撃フラグ
    // ※攻撃ボタンを何度も押すのを防ぐ＆攻撃中に移動するのを防ぐ
    [SerializeField]
    public bool _attackFlag    = false;

    // ポーズマネージャー
    // ※ポーズ画面が閉じた瞬間に攻撃するのを防ぐ
    [SerializeField]
    public PauseManager _pauseManager;

    // ダッシュ中イベント
    public event Action ActivateDashEvent;
    // ダッシュ(解除)イベント
    public event Action DeactivateDashEvent;

    // Start is called before the first frame update
    void Start()
    {
        _InputActions = new HakopanControls();
        _InputActions.Enable();

        // PauseManagerを取得
        _pauseManager = FindObjectOfType<PauseManager>();
    }

    public void OperationUpdate(CharacterManager manager)
    {
        // フラグ更新
        _attackFlag = manager.GetCurrentAnimations() != "Attack" ? false : true;

        // 攻撃
        if (_InputActions.Player.Fire.triggered && manager.GetCurrentAnimations() != "Attack"&&
            _pauseManager != null && !_pauseManager.IsPaused)  
        {
            _attackFlag = true;
            manager.SetAnimations(AnimationType.Attack);
        }

        // 待機
        if (_InputActions.Player.Move.ReadValue<Vector2>().magnitude <= 0.0f && !_attackFlag) 
        {
            manager.SetAnimations(AnimationType.Idle);
        }

        // 移動
        if (_InputActions.Player.Move.ReadValue<Vector2>().magnitude > 0.0f && !_attackFlag) 
        {
            manager.SetAnimations(AnimationType.Walk);
        }

        // ダッシュ
        if (_InputActions.Player.Dash.ReadValue<float>() > 0)
        {
            // 押されている間の処理を発火
            ActivateDashEvent?.Invoke();
        }
        else
        {
            // 押されていない間の処理を発火
            DeactivateDashEvent?.Invoke();
        }

        // ポーズ
        if (_InputActions.Player.Pause.triggered)
        {
            
        }
    }

    // イベント登録をCharacterOperation内で行う
    public void RegisterColliderEvent(CharacterCollider collider)
    {
        // イベントに関数を登録
        collider.CollisionEFKStayEvent += GamePadStartVibration;
        collider.CollisionEFKExitEvent += GamePadEndVibration;
    }

    // ゲームパッドの振動開始
    public void GamePadStartVibration()
    {
        // デバイスがゲームパッド(コントローラー)の時だけ処理
        Gamepad gamepad = Gamepad.current;
        if (gamepad != null && !_vibrationFlag) 
        {
            gamepad.SetMotorSpeeds(1.0f, 1.0f);
            _vibrationFlag = true;
        }
    }

    // ゲームパッドの振動終了
    public void GamePadEndVibration()
    {
        // デバイスがゲームパッド(コントローラー)の時だけ処理
        Gamepad gamepad = Gamepad.current;
        if (gamepad != null && _vibrationFlag) 
        {
            gamepad.SetMotorSpeeds(0.0f, 0.0f);
            _vibrationFlag = false;
        }
    }

    // 十字キー又は左スティックの上下方向の入力値を取得
    public float GetHorizontalInput()
    {
        // 十字キー又は左スティックの入力値を取得
        var InputMoveAxis = _InputActions.Player.Move.ReadValue<Vector2>();
        _HorizontalInput = InputMoveAxis.x;
        return _HorizontalInput;
    }

    // 十字キー又は左スティックの左右方向の入力値を取得
    public float GetVerticalInput()
    {
        // 十字キー又は左スティックの入力値を取得
        var InputMoveAxis = _InputActions.Player.Move.ReadValue<Vector2>();
        _VerticalInput = InputMoveAxis.y;
        return _VerticalInput;
    }
}
