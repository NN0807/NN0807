using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetWorkSysyemCustomize : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        // 送受信接続再開
        PhotonNetwork.IsMessageQueueRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
