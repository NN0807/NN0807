using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour,ICharacterPart
{
    // データアセット
    public CharacterParamAsset characterParamAsset;

    // 前方方向
    private Vector3 MoveForward;

    // 剛体
    [SerializeField]
    private Rigidbody _rigidbody;

    // ダッシュフラグ
    private bool IsDashing;
    // コピーフラグ
    private bool IsCopying;

    // 歩き速度
    private float _walkSpeed;
    // ダッシュ速度
    private float _dashSpeed;

    // 衝撃を受けたかの判定フラグ
    [SerializeField]
    private bool _impulseFlag = false;

    // 衝撃処理時間
    private float _impulseTime = 0.0f;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(CharacterManager manager)
    {
        Debug.Log("CharacterMove 初期化");

        // データ設定
        _rigidbody = GetComponent<Rigidbody>();
        characterParamAsset = Resources.Load<CharacterParamAsset>("CharacterParamAsset");

        // 変数初期化
        MoveForward   = Vector3.zero;
        _walkSpeed    = 0.0f;
        _dashSpeed    = 0.0f;
        _impulseTime  = 0.0f;
    }

    public void UpdatePart(CharacterManager manager)
    {
        Debug.Log("CharacterMove　更新処理");

        // 攻撃中は移動も旋回も出来なくする
        if (!manager.IsCurrentlyAttacking())     
        {
            // 移動
            if (!_impulseFlag) Move(manager);

            // 旋回
            Turn();
        }
        else
        {
            // 速力リセット
            //_rigidbody.velocity = Vector3.zero;
        }

        // 衝撃を受けたらタイマー起動
        if (_impulseFlag) _impulseTime += Time.deltaTime;
        // 衝撃を受けてから0.5秒経過したら
        if (_impulseTime > 0.5f)
        {
            // タイマーとフラグをリセット
            _impulseTime = 0.0f;
            _impulseFlag = false;
        }
    }

    // 移動処理
    private void Move(CharacterManager manager)
    {
        // カメラの方向から、X-Z平面の単位ベクトルを取得
        Vector3 CameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        // 方向キーの入力値とカメラの向きから、移動方向を決定
        MoveForward = CameraForward * manager.GetVerticalInput() +
            Camera.main.transform.right * manager.GetHorizontalInput();

        // 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す
        if (!IsDashing)
        {
            _rigidbody.velocity = MoveForward * (characterParamAsset.MoveSpeed) + new Vector3(0, _rigidbody.velocity.y, 0);
            _walkSpeed = Mathf.Max(_rigidbody.velocity.x / MoveForward.x, _rigidbody.velocity.y / MoveForward.y, _rigidbody.velocity.z / MoveForward.z);
        }
    }

    // 旋回処理
    private void Turn()
    {
        // キャラクターの向きを進行方向に
        if (MoveForward != Vector3.zero)
        {
            Quaternion from = transform.rotation;
            Quaternion to = Quaternion.LookRotation(MoveForward);
            transform.rotation = Quaternion.Lerp(from, to, characterParamAsset.RotateSpeed * Time.deltaTime);
        }
    }

    // ダッシュ処理
    private void ActivateDash()
    {
        // ダッシュフラグON
        IsDashing = true;

        if (!_impulseFlag)
        {
            // 最大ダッシュ速度を越えないように、現在の速度を算出する
            _dashSpeed = Mathf.Min((_walkSpeed + _dashSpeed) + characterParamAsset.Acceleration * Time.deltaTime,
            characterParamAsset.MaxDashSpeed);

            // 移動方向にダッシュスピードを掛ける
            _rigidbody.velocity = MoveForward * _dashSpeed;
        }     
    }

    // ダッシュ終了処理
    private void DeactivateDash()
    {
        // ダッシュフラグOFF
        IsDashing = false;

        // ダッシュ速度リセット
        _dashSpeed = 0.0f;
    }

    // 衝撃処理
    public void SetImpulse()
    {
        // 衝撃フラグ設定
        _impulseFlag = true;
    }

    // イベント登録をCharacterMove内で行う
    public void RegisterOperationEvent(CharacterOperation operation)
    {
        if (!operation) return;
        // イベントに関数を登録
        operation.ActivateDashEvent   += ActivateDash;
        operation.DeactivateDashEvent += DeactivateDash;
    }

    // 前方方向取得関数
    public Vector3 GetMoveForward() { return MoveForward; }
}
