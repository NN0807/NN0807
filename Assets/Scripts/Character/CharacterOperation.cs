using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // Input System関連のAPI

public class CharacterOperation : MonoBehaviour
{
    private HakopanControls inputActions;

    Rigidbody rb;
    float speed = 3.0f;
    [SerializeField] float moveSpeed = 2f;
    private float horizontalInput, verticalInput;

    private const float RotateSpeed = 900f;

    // Start is called before the first frame update
    void Start()
    {
        inputActions = new HakopanControls();
        inputActions.Enable();

        rb = GetComponent<Rigidbody>();

        
    }

    // Update is called once per frame
    void Update()
    {
        var inputMoveAxis = inputActions.Player.Move.ReadValue<Vector2>();
        horizontalInput = inputMoveAxis.x;
        verticalInput = inputMoveAxis.y;
        if (inputActions.Player.Fire.triggered)
        {
            Debug.Log("ファイヤー");
        }

        if (inputActions.Player.Pause.triggered)
        {
            Debug.Log("ポーズ");
        }


        // カメラの方向から、X-Z平面の単位ベクトルを取得
        Vector3 CameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        // 方向キーの入力値とカメラの向きから、移動方向を決定
        Vector3 moveForward = CameraForward * verticalInput + Camera.main.transform.right * horizontalInput;
        // 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
        rb.velocity = moveForward * moveSpeed + new Vector3(0, rb.velocity.y, 0);
        // キャラクターの向きを進行方向に
        if (moveForward != Vector3.zero)
        {
            Quaternion from = transform.rotation;
            Quaternion to = Quaternion.LookRotation(moveForward);
            transform.rotation = Quaternion.RotateTowards(from, to, RotateSpeed * Time.deltaTime);

        }
    }

    // 当たった時に呼ばれる関数
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit"); // ログを表示する
    }
}
