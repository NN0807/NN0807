using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    // データアセット
    public CharacterParamAsset characterParamAsset;

    // 入力処理
    [SerializeField]
    private HakopanControls _InputActions;

    // 視点入力値
    private Vector2 _lookInput;

    // Playerから常に一定の間隔を保つためのオフセット値
    private Vector3    _offsetPosition = new Vector3     ( 0.0f,    4.6f, 4.0f);
    private Quaternion _offsetRotate   = Quaternion.Euler(55.0f, -180.0f, 0.0f);

    [SerializeField]
    public InputManager _inputManager; 

    void Awake()
    {
        // 初期設定
        _InputActions = new HakopanControls();
        // データ設定
        characterParamAsset = Resources.Load<CharacterParamAsset>("CharacterParamAsset");

        this.transform.position = _offsetPosition;
        this.transform.rotation = _offsetRotate;

        // 入力スクリプトを設定
        //TryGetComponent(out _inputManager);
    }

    // ゲーム実行時にこのオブジェクトが存在していたら実行される
    void OnEnable()
    {
        if (characterParamAsset == null)
        {
            Debug.LogError("CharacterParamAsset が見つかりません！");
        }

        if (_InputActions == null)
        {
            Debug.LogError("_InputActions が初期化されていません！");
        }

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
            _lookInput = context.ReadValue<Vector2>();
            //Debug.Log("Looking with input: " + _lookInput);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _lookInput = Vector2.zero;
            //Debug.Log("Stopped looking");
        }
    }

    void Update()
    {
        // マウス入力に対して感度を調整
        //float cameraSpeed = characterParamAsset.CameraSpeed;

        if (_inputManager.GetCurrentInputDevice() == "Keyboard") 
        {
            // マウス感度を下げる（例えば0.5f）
            //cameraSpeed *= 0.1f;
        }
        else
        {
            //cameraSpeed = characterParamAsset.CameraSpeed;
        }



        // カメラの回転処理
        if (_lookInput != Vector2.zero)
        {
            transform.Rotate(Vector3.up,     _lookInput.x * 1.0f/*cameraSpeed*/, Space.World);
            transform.Rotate(Vector3.right, -_lookInput.y * 1.0f/*cameraSpeed*/, Space.Self );
        }
    }
}
 