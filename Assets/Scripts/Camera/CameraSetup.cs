using UnityEngine;
using Cinemachine;

public class CameraSetup : MonoBehaviour
{
    // カメラ
    public CinemachineFreeLook _freeLookCamera;

    // カメラの初期位置を調整するオフセット値を設定
    Vector3 _positionOffset = new Vector3( 0.0f,    4.6f, 4.0f); 
    Vector3 _lookAtOffset   = new Vector3(55.0f, -180.0f, 0.0f);


    void OnEnable()
    {
        // OnObjectCreatedイベントにSetCameraTargetメソッドを登録
        // オブジェクト生成時にカメラのターゲットを設定するため
        // イベントを"static"として宣言されているため、
        // クラスのインスタンスを作成せずに直接アクセス出来る
        CharacterModel.OnObjectCreated += SetCameraTarget;
    }

    void OnDisable()
    {
        // OnObjectCreatedイベントにSetCameraTargetメソッドを解除
        CharacterModel.OnObjectCreated -= SetCameraTarget;
    }

    void SetCameraTarget(GameObject target)
    {
        // カメラの追従対象を生成されたオブジェクトに設定
        _freeLookCamera.Follow = target.transform;
        // カメラの視線の先を生成されたオブジェクトに設定
        _freeLookCamera.LookAt = target.transform;

        // Followターゲットにオフセットを適用
        //_virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset      = _positionOffset;

        // LookAtターゲットにオフセットを適用
        //_virtualCamera.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset = _lookAtOffset;
    }
}