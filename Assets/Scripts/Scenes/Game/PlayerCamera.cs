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
    private float _distance = -16.0f;

    // カメラとプレイヤーの相対位置
    public Vector3 _offset;            

    // 初期の垂直回転角度（斜め下）
    [SerializeField]
    public float _currentAngleX =   30.0f;

    // 初期の水平回転角度（正面）
    [SerializeField]
    public float _currentAngleY = -180.0f;

    // 演出開始フラグ
    public bool _playEffect = false;

    // FIGTH画像の拡縮値と色
    private Vector3 _targetPos = new Vector3(0.0f, 2.0f, 5.0f);

    // イージング時間
    [SerializeField]
    private float _easingTime = 1.0f;

    // コルーチンフラグ
    private bool _coroutineFlag = false;

    // offset y2 z-16 AngleX60 Y0

    void Awake()
    {
        // データ設定
        characterParamAsset = Resources.Load<CharacterParamAsset>("CharacterParamAsset");

        // カメラの初期オフセット
        _offset = new Vector3(0.0f, 2.0f, -5.0f);
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
        // 演出開始
        if (_playEffect)
        {
            if (!_coroutineFlag)
            {
                StartCoroutine(
                Move(_offset, _targetPos, _easingTime, Easing.Ease.InOutSine, true));

                _coroutineFlag = true;
            }
        }


        // 回転行列を使って目標位置を計算
        Quaternion rotation = Quaternion.Euler(_currentAngleX, _currentAngleY, 0);
        Vector3 desiredPosition = _playerTransform.position + (rotation * _offset);

        // カメラ位置の更新
        transform.position = _playerTransform.position + (rotation * _offset);

        // プレイヤーを注視
        transform.LookAt(_playerTransform);
    }

    void SetCameraTarget(GameObject target)
    {
        // カメラの視線の先を生成されたオブジェクトに設定
        _playerTransform = target.transform;
    }

    // 指定したTransformの座標を更新する
    // 引数 Transform, 目的値の座標、何秒で動かすか、イージングの種類、移動先が絶対座標かどうか
    public IEnumerator Move(Vector3 initializePos, Vector3 destinationPos, float seconds, Easing.Ease easing, bool absolute)
    {
        // イージング関数の取得
        var Ease = Easing.GetEasingMethod(easing);

        // 現在点と移動先の設定
        Vector3 staPos = initializePos;
        Vector3 endPos = absolute ? destinationPos : staPos + destinationPos;
        // 初期地点と目標地点の差
        Vector3 difPos = endPos - staPos;

        // N秒かけて移動させる
        float e = 0;
        while (true)
        {
            yield return null;
            e += Time.deltaTime / seconds;
            if (e >= 1.0f)
            {
                initializePos = endPos;
                break;
            }
            Vector3 nextPos = staPos + Ease(e) * difPos;
            initializePos = nextPos;
        }
    }
}
 