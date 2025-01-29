using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// MonoBehaviourPunCallbacksを継承して、PUNのコールバックを受け取れるようにする
public class NetWorkSystem_GameScene : MonoBehaviourPunCallbacks
{

    private void Awake()
    {
        // 送受信接続再開
        PhotonNetwork.IsMessageQueueRunning = true;

        // ローカルプレイヤーオブジェクトを取得する
        var localPlayer = PhotonNetwork.LocalPlayer;

        // ネットワークオブジェクト生成
        if (localPlayer.ActorNumber == 1)
        {
            Debug.Log("01生成");
            PhotonNetwork.Instantiate("CharacterManager01", new Vector3(0.0f, 3.0f, 0.0f), Quaternion.identity);
        }
        if (localPlayer.ActorNumber == 2)
        {
            Debug.Log("02生成");
            PhotonNetwork.Instantiate("CharacterManager02", new Vector3(0.0f, 3.0f, 0.0f), Quaternion.identity);
        }
        if (localPlayer.ActorNumber == 3)
        {
            Debug.Log("03生成");
            PhotonNetwork.Instantiate("CharacterManager03", new Vector3(0.0f, 3.0f, 0.0f), Quaternion.identity);
        }
        if (localPlayer.ActorNumber == 4)
        {
            Debug.Log("04生成");
            PhotonNetwork.Instantiate("CharacterManager04", new Vector3(0.0f, 3.0f, 0.0f), Quaternion.identity);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
