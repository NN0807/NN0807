using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using Common;

public class CharacterOperation : MonoBehaviour
{
    // 入力処理
    [SerializeField]
    private HakopanControls _InputActions;

    [SerializeField]
    private float _HorizontalInput = 0.0f;
    [SerializeField]
    private float _VerticalInput   = 0.0f;

    // コントローラー振動フラグ
    [SerializeField]
    private bool VibrationFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        _InputActions = new HakopanControls();
        _InputActions.Enable();
    }

    public void OperationUpdate(CharacterManager manager)
    {
        // 攻撃
        if (_InputActions.Player.Fire.triggered)
        {
            manager.SetAnimations(AnimationType.Attack);
        }

        // 移動
        if (_InputActions.Player.Move.ReadValue<Vector2>().magnitude <= 0.0f)
        {
            manager.SetAnimations(AnimationType.Idle);
        }

        // 移動
        if (_InputActions.Player.Move.ReadValue<Vector2>().magnitude > 0.0f)
        {
            manager.SetAnimations(AnimationType.Walk);
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
        if (gamepad != null && !VibrationFlag) 
        {
            gamepad.SetMotorSpeeds(1.0f, 1.0f);
            VibrationFlag = true;
        }
    }

    // ゲームパッドの振動終了
    public void GamePadEndVibration()
    {
        // デバイスがゲームパッド(コントローラー)の時だけ処理
        Gamepad gamepad = Gamepad.current;
        if (gamepad != null && VibrationFlag) 
        {
            gamepad.SetMotorSpeeds(0.0f, 0.0f);
            VibrationFlag = false;
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
