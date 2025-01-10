using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    // 入力処理
    [SerializeField]
    private HakopanControls _InputActions;



    private Vector2 lookInput;


    private Vector3 Offset;

    void Awake()
    {
        _InputActions = new HakopanControls();
        //Offset=new Vector3()
    }

    // ゲーム実行時にこのオブジェクトが存在していたら実行される
    void OnEnable()
    {
        // 入力実行中イベントを追加
        _InputActions.Player.Look.performed += OnLook;
        // 入力キャンセルイベントを追加
        _InputActions.Player.Look.canceled  += OnLook;
        // 有効化
        _InputActions.Enable();
    }

    // ゲーム実行中にこのオブジェクトが削除されたら実行される
    void OnDisable()
    {
        // 入力実行中イベントから削除
        _InputActions.Player.Look.performed -= OnLook;
        // 入力キャンセルイベントから削除
        _InputActions.Player.Look.canceled  -= OnLook;
        // 無効化
        _InputActions.Disable();
    }

    void OnLook(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            lookInput = context.ReadValue<Vector2>();
            Debug.Log("Looking with input: " + lookInput);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            lookInput = Vector2.zero;
            Debug.Log("Stopped looking");
        }
    }

    void Update()
    {
        // カメラの回転処理
        if (lookInput != Vector2.zero)
        {
            transform.Rotate(Vector3.up, lookInput.x * 20, Space.World);
            transform.Rotate(Vector3.right, -lookInput.y * 20, Space.Self);
        }
    }
}
