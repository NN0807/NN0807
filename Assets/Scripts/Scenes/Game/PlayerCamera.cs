using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    // データアセット
    public CharacterParamAsset characterParamAsset;

    // プレイヤーオブジェクト
    public Transform _playerTransform;

    // プレイヤーからの距離
    [SerializeField]
    public float _distance = 5.0f;

    // カメラとプレイヤーの相対位置
    private Vector3 _offset;            

    // 初期の垂直回転角度（斜め下）
    [SerializeField]
    public float _currentAngleX =   30.0f;

    // 初期の水平回転角度（正面）
    [SerializeField]
    public float _currentAngleY = -180.0f;

    // スムーズに追従するスピード
    [SerializeField]
    public float _smoothSpeed = 1.0f;  

    void Awake()
    {
        // データ設定
        characterParamAsset = Resources.Load<CharacterParamAsset>("CharacterParamAsset");

        // カメラの初期オフセット
        _offset = new Vector3(0.0f, 2.0f, -_distance);
    }

    // ゲーム実行時にこのオブジェクトが存在していたら実行される
    void OnEnable()
    {
        if (characterParamAsset == null)
        {
            Debug.LogError("CharacterParamAsset が見つかりません！");
        }

        // OnObjectCreatedイベントにSetCameraTargetメソッドを登録
        // オブジェクト生成時にカメラのターゲットを設定するため
        // イベントを"static"として宣言されているため、
        // クラスのインスタンスを作成せずに直接アクセス出来る
        CharacterModel.OnObjectCreated += SetCameraTarget;
    }

    // ゲーム実行中にこのオブジェクトが削除されたら実行される
    void OnDisable()
    {
        // OnObjectCreatedイベントにSetCameraTargetメソッドを解除
        CharacterModel.OnObjectCreated -= SetCameraTarget;
    }

    void Update()
    {
        // 回転行列を使って目標位置を計算
        Quaternion rotation = Quaternion.Euler(_currentAngleX, _currentAngleY, 0);
        Vector3 desiredPosition = _playerTransform.position + (rotation * _offset);

        // 現在の位置と目標位置を補間
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed * Time.deltaTime);

        // カメラ位置の更新
        //transform.position = smoothedPosition;
        transform.position = _playerTransform.position + (rotation * _offset); ;

        // プレイヤーを注視
        transform.LookAt(_playerTransform);
    }

    void SetCameraTarget(GameObject target)
    {
        // カメラの視線の先を生成されたオブジェクトに設定
        _playerTransform = target.transform;
    }
}
 