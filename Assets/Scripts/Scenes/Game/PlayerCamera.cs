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

    // プレイヤーオブジェクト
    public Transform _playerTransform;  

    public float rotationSpeed = 10.0f;   // 回転速度
    public float distance = 5.0f;       // プレイヤーからの距離
    public float verticalRotationLimit = 80.0f; // 垂直回転の制限角度
    private Vector3 _offset;            // カメラとプレイヤーの相対位置
    private float _currentAngleX = 15.0f; // 初期の垂直回転角度（斜め下）
    private float _currentAngleY = -180.0f;  // 初期の水平回転角度（正面）

    void Awake()
    {
        // 初期設定
        _InputActions = new HakopanControls();
        // データ設定
        characterParamAsset = Resources.Load<CharacterParamAsset>("CharacterParamAsset");

        // カメラの初期オフセット
        _offset = new Vector3(0.0f, 2.0f, -distance); 
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

        // OnObjectCreatedイベントにSetCameraTargetメソッドを登録
        // オブジェクト生成時にカメラのターゲットを設定するため
        // イベントを"static"として宣言されているため、
        // クラスのインスタンスを作成せずに直接アクセス出来る
        CharacterModel.OnObjectCreated += SetCameraTarget;
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

        // OnObjectCreatedイベントにSetCameraTargetメソッドを解除
        CharacterModel.OnObjectCreated -= SetCameraTarget;
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
        // 水平回転（Y軸）
        _currentAngleY += _lookInput.x * rotationSpeed * Time.deltaTime;

        // 垂直回転（X軸）と制限
        _currentAngleX += _lookInput.y * rotationSpeed * Time.deltaTime;
        _currentAngleX = Mathf.Clamp(_currentAngleX, -verticalRotationLimit, verticalRotationLimit);

        // 回転行列を使ってカメラ位置を更新
        Quaternion rotation = Quaternion.Euler(_currentAngleX, _currentAngleY, 0);
        Vector3 direction = rotation * _offset;

        // カメラ位置の更新
        transform.position = _playerTransform.position + direction;

        // プレイヤーを注視
        transform.LookAt(_playerTransform);
    }

    void SetCameraTarget(GameObject target)
    {
        // カメラの視線の先を生成されたオブジェクトに設定
        _playerTransform = target.transform;
    }
}
 