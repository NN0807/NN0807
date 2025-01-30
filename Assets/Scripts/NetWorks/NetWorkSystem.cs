using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

// MonoBehaviourPunCallbacksを継承して、PUNのコールバックを受け取れるようにする
public class NetWorkSystem : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        // PhotonServerSettingsの設定内容を使って、マスターサーバーへ接続する
        PhotonNetwork.ConnectUsingSettings();
    }

    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster()
    {
        // ロビー参加
        PhotonNetwork.JoinLobby();
    }

    // ルーム参加後
    public override void OnJoinedRoom()
    {
        // 受信メッセージ処理の実行・一時停止を切り替えることができる。
        PhotonNetwork.IsMessageQueueRunning = false;

        // シーン遷移
        SceneManager.LoadSceneAsync("NetWork_FireStage_Scene", LoadSceneMode.Single);

        //if (PhotonNetwork.IsMasterClient)
        //{
        //    PhotonNetwork.CurrentRoom.SetStartTime(PhotonNetwork.ServerTimestamp);
        //}
    }

    // Photonのサーバーから切断する
    // PhotonNetwork.Disconnect();

    // Photonのサーバーから切断された時に呼ばれるコールバック
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"サーバーとの接続が切断されました: {cause.ToString()}");
    }
}