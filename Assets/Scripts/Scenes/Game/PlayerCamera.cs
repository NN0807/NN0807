using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    // カメラオブジェクト
    [SerializeField]
    public GameObject mainCamera;

    // 調整
    [SerializeField]
    public Vector3 Offset;

    [SerializeField] private float sensitivity = 100f;
    private Vector2 lookInput;
    private float xRotation = 0f;
    private Transform playerBody;

    private void Awake()
    {
        // プレイヤーのTransformを取得（カメラがプレイヤーに追従する場合）
        playerBody = transform.parent;
    }

    // InputActionから呼び出されるメソッド
    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        float mouseX = lookInput.x * sensitivity * Time.deltaTime;
        float mouseY = lookInput.y * sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);  // 上下の視点制限

        // カメラの上下の動き
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        // プレイヤーの左右の動き
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
