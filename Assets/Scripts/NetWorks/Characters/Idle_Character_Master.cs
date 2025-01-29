using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class Idle_Character_Master : MonoBehaviourPunCallbacks
{
    // プレイヤー情報を表示するCharacterManager
    public Idle_Character_Manager[] _idle_Character_Manager = new Idle_Character_Manager[4];

    [SerializeField]
    public int _playerCount = 0;

    private void Start()
    {
        // 待機画面でプレイヤーリストを初期化
        UpdatePlayerList();
    }

    // プレイヤーリストを更新
    private void UpdatePlayerList()
    {
        // オブジェクトをクリア
        for (int Index = 0; Index < 4; Index++) 
        {
            _idle_Character_Manager[Index].ModelDestory();
        }

        // 各プレイヤーのデータを取得して表示
        foreach (var player in PhotonNetwork.PlayerList)
        {
            // プレイヤーカウント
            _playerCount++;

            int _bodyPart = player.CustomProperties.ContainsKey("BodyPart") ? (int)player.CustomProperties["BodyPart"] : 0;
            int _legPart  = player.CustomProperties.ContainsKey("LegPart")  ? (int)player.CustomProperties["LegPart"]  : 0;

            // プレイヤー情報の表示用オブジェクトを生成;
            if (player.ActorNumber != _playerCount) 
            {
                _idle_Character_Manager[player.ActorNumber - 1].ModelGenerate(_legPart, _bodyPart);
            }   
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
