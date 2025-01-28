using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// MonoBehaviourPunCallbacksを継承して、photonViewプロパティを使えるようにする
public class NetWorkIdle_UI : MonoBehaviourPunCallbacks
{
    // あなた画像
    public Image[] _youImages     = new Image[4];

    // Loading画像
    public Image[] _loadingImages = new Image[4];

    // "ホストを待っています"画像
    public Image _idleHost;
    // "ゲストを待っています"画像
    public Image _idleLocal;

    private void Start()
    {
        // "あなた"画像を一旦全て非表示
        // ロードアイコンを一旦全て表示
        for (int index = 0; index < 4; index++) 
        {
            _youImages[index].gameObject.SetActive(false);
            _loadingImages[index].gameObject.SetActive(true);
        }
    }


    // Update is called once per frame
    void Update()
    {
        // 自身がホストか判定
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("自分はホストです！");

            // 画像の表示非表示設定
            _idleHost.gameObject.SetActive(false);
            _idleLocal.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("自分はホストではありません。");

            // 画像の表示非表示設定
            _idleHost.gameObject.SetActive(true);
            _idleLocal.gameObject.SetActive(false);
        }


        // ローカルプレイヤーオブジェクトを取得する
        var localPlayer = PhotonNetwork.LocalPlayer;

        // 自身のロードアイコンは常に非表示
        _loadingImages[localPlayer.ActorNumber - 1].gameObject.SetActive(false);

        // 自身のプレイヤー番号によって表示する画像を変更させる
        if (localPlayer.ActorNumber == 1)
        {
            _youImages[0].gameObject.SetActive(true);
            _youImages[1].gameObject.SetActive(false);
            _youImages[2].gameObject.SetActive(false);
            _youImages[3].gameObject.SetActive(false);
        }
        if (localPlayer.ActorNumber == 2)
        {
            _youImages[0].gameObject.SetActive(false);
            _youImages[1].gameObject.SetActive(true);
            _youImages[2].gameObject.SetActive(false);
            _youImages[3].gameObject.SetActive(false);
        }
        if (localPlayer.ActorNumber == 3)
        {
            _youImages[0].gameObject.SetActive(false);
            _youImages[1].gameObject.SetActive(false);
            _youImages[2].gameObject.SetActive(true);
            _youImages[3].gameObject.SetActive(false);
        }
        if (localPlayer.ActorNumber == 4)
        {
            _youImages[0].gameObject.SetActive(false);
            _youImages[1].gameObject.SetActive(false);
            _youImages[2].gameObject.SetActive(false);
            _youImages[3].gameObject.SetActive(true);
        }
    }
}
