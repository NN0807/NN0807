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

    public void Initialize(CharacterManager manager)
    {
        Debug.Log("CharacterCollider 初期化");

        // データ設定
        _rigidbody = GetComponent<Rigidbody>();
        characterParamAsset = Resources.Load<CharacterParamAsset>("CharacterParamAsset");

        // 変数初期化
        MoveForward = Vector3.zero;
    }

    public void UpdatePart(CharacterManager manager)
    {
        Debug.Log("CharacterCollider　更新処理");

        // 移動
        Move(manager);

        // 旋回
        Turn();
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
        _rigidbody.velocity = MoveForward * characterParamAsset.MoveSpeed + new Vector3(0, _rigidbody.velocity.y, 0);
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

    // 衝撃処理
    private void Impulse(Vector3 forward, float attack)
    {
        // 吹っ飛ばす
        _rigidbody.AddForce(forward * attack, ForceMode.Impulse);
    }

    // イベント登録をCharacterMove内で行う
    public void RegisterColliderEvent(CharacterCollider collider)
    {
        // イベントに関数を登録
        collider.CollisionAttackEnterEvent += Impulse;
    }

    // 前方方向取得関数
    public Vector3 GetMoveForward() { return MoveForward; }
}
