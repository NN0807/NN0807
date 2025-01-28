using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerCamera : MonoBehaviour
{
    // データアセット
    public CharacterParamAsset characterParamAsset;

    // プレイヤーオブジェクト
    public Transform _playerTransform;

    public Transform _playerDeadTargetPos;

    // カメラとプレイヤーの相対位置
    private Vector3 _offset;            

    // 演出開始フラグ
    public bool _playEffect = false;

    // コルーチンフラグ
    private bool _coroutineFlag = false;

    private Vector3      _startOffset = new Vector3     ( 0.0f,    2.0f, -16.0f); // 初期位置オフセット
    private Vector3        _endOffset = new Vector3     ( 0.0f,    2.0f,  -5.0f); // 最終位置オフセット
    private Quaternion _startRotation = Quaternion.Euler(60.0f,    0.0f,   0.0f); // 初期角度
    private Quaternion   _endRotation = Quaternion.Euler(30.0f, -180.0f,   0.0f); // 最終角度

    private Quaternion       Rotation;

    // 白画像
    public Image _whiteBack;
    // 白画像の透明値
    public float _whiteBackAlpha;

    void Awake()
    {
        // データ設定
        characterParamAsset = Resources.Load<CharacterParamAsset>("CharacterParamAsset");

        // カメラの初期オフセット
        _offset  = _startOffset;
        Rotation = _startRotation;

        // 画像初期設定
        _whiteBackAlpha  = 1.0f;
        _whiteBack.color = new Color(1.0f, 1.0f, 1.0f, _whiteBackAlpha);
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
        // プレイヤーが死亡したら
        if (_playerTransform == null)
        {
            // デスカメラに切り替える
            _playerTransform = _playerDeadTargetPos;
            // 真下を向く
            Rotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);
            _offset.y = 0;
        }

        // 演出開始
        if (Time.time > 1.5f) 
        {
            if (!_coroutineFlag)
            {
                StartCoroutine(
                ChangeTransform());

                _coroutineFlag = true;
            }

            // 透明値更新処理
            if (_whiteBackAlpha > 0.0f) _whiteBackAlpha -= Time.deltaTime * 2.0f;
            _whiteBack.color = new Color(1.0f, 1.0f, 1.0f, _whiteBackAlpha);
        }

        // 回転行列を使って目標位置を計算
        // カメラ位置の更新
        transform.position = _playerTransform.position + (Rotation * _offset);

        // プレイヤーを注視
        transform.LookAt(_playerTransform);
    }

    void SetCameraTarget(GameObject target)
    {
        // カメラの視線の先を生成されたオブジェクトに設定
        _playerTransform = target.transform;
    }

    private System.Collections.IEnumerator ChangeTransform()
    {
        float _duration    = 1.0f; // 1秒
        float _elapsedTime = 0.0f;

        while (_elapsedTime < _duration)
        {
            _elapsedTime += Time.deltaTime;
            float t = _elapsedTime / _duration;

            // オフセットを線形補間
            _offset  =    Vector3.Lerp(_startOffset,   _endOffset,   t);

            // 回転を線形補間
            Rotation = Quaternion.Lerp(_startRotation, _endRotation, t);

            yield return null; // 次のフレームまで待機
        }
    }
}
 