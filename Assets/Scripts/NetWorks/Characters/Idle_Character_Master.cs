using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class Idle_Character_Master : MonoBehaviourPunCallbacks
{
    // プレイヤーリスト表示用の親オブジェクト
    public Transform playerListParent;
    // プレイヤー情報を表示するPrefab
    public GameObject playerListItemPrefab;

    private void Start()
    {
        // 待機画面でプレイヤーリストを初期化
        UpdatePlayerList();
    }

    // プレイヤーリストを更新
    private void UpdatePlayerList()
    {
        // 子オブジェクトをクリア
        foreach (Transform child in playerListParent)
        {
            Destroy(child.gameObject);
        }

        // 各プレイヤーのデータを取得して表示
        foreach (var player in PhotonNetwork.PlayerList)
        {


            int bodyPart = player.CustomProperties.ContainsKey("BodyPart") ? (int)player.CustomProperties["BodyPart"] : 0;
            int footPart = player.CustomProperties.ContainsKey("LegPart")  ? (int)player.CustomProperties["LegPart"]  : 0;

            // プレイヤー情報の表示用オブジェクトを生成
            var playerItem = Instantiate(playerListItemPrefab, playerListParent);
        }
    }

    // 他プレイヤーのプロパティが変更された場合にリストを更新
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("BodyPart") || changedProps.ContainsKey("LegPart"))
        {
            Debug.Log($"{targetPlayer.NickName} のパーツ情報が更新されました。");
            UpdatePlayerList();
        }
    }
}
