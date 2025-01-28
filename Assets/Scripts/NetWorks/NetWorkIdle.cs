using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// MonoBehaviourPunCallbacksを継承して、photonViewプロパティを使えるようにする
public class NetWorkIdle : MonoBehaviourPunCallbacks
{
    // カスタムプロパティのキー
    private const string ReadyKey = "IsReady";
    bool allReady = true;
    public Button startButton; // スタートボタン（ホストのみ有効化）

    private void Start()
    {
        // 送受信接続再開
        PhotonNetwork.IsMessageQueueRunning = true;

        // シーンがロードされたら準備完了を自動設定
        SetReadyState(true);

        // スタートボタン初期設定
        if (startButton != null)
        {
            startButton.gameObject.SetActive(false); // ボタンを非表示に
            startButton.onClick.AddListener(OnStartButtonClicked); // ボタンクリックイベント登録
        }
    }

    // 準備完了状態を設定
    private void SetReadyState(bool isReady)
    {
        var properties = new ExitGames.Client.Photon.Hashtable
        {
            { ReadyKey, isReady }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);

        Debug.Log("準備完了状態を設定しました。");
    }

    // ホストが全プレイヤーの準備完了状態を監視
    private void CheckAllPlayersReady()
    {
        if (!PhotonNetwork.IsMasterClient) return; // ホスト以外はスキップ

        allReady = true;

        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (!player.CustomProperties.TryGetValue(ReadyKey, out object isReady) || !(bool)isReady)
            {
                allReady = false; // 1人でも未準備なら全員準備完了ではない
                Debug.Log($"{player.NickName} が準備未完了です。");
            }
        }

        // 全員準備完了の場合、ボタンを有効化
        if (allReady && startButton != null)
        {
            Debug.Log("全員準備完了！ホストがスタートボタンを押せます。");
            // ボタンを表示
            startButton.gameObject.SetActive(true); 
        }
    }

    // カスタムプロパティが更新されたときに呼び出される
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        // ホスト以外は無視
        if (!PhotonNetwork.IsMasterClient) return;

        if (changedProps.ContainsKey(ReadyKey))
        {
            Debug.Log($"{targetPlayer.NickName} の準備完了状態が更新されました: {changedProps[ReadyKey]}");
            CheckAllPlayersReady();
        }
    }

    // スタートボタンを押したときの処理
    private void OnStartButtonClicked()
    {
        Debug.Log("スタートボタンが押されました！ゲームを開始します。");
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
