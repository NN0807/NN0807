using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomListViewElement : MonoBehaviour
{
    // ルーム名
    [SerializeField]
    private Text _roomName      = default;

    // プレイヤー人数
    [SerializeField]
    private Text _playerCounter = default;

    // マッチングロビースクリプト
    private MatchmakingView _matchmakingView;

    // ルーム参加ボタン
    private Button _createButton;

    // 初期化関数
    public void Init(MatchmakingView parentView)
    {
        // 設定
        _matchmakingView = parentView;
        _createButton = GetComponent<Button>();
        // イベント登録
        _createButton.onClick.AddListener(OnButtonClick);
    }

    // ルーム作成関数
    private void OnButtonClick()
    {
        // ルーム参加処理中は、入力できないようにする
        _matchmakingView.OnJoiningRoom();

        // ボタンに対応したルーム名のルームに参加する
        PhotonNetwork.JoinRoom(_roomName.text);
    }

    public void Show(RoomInfo roomInfo)
    {
        // ルーム名を設定
        _roomName.text = roomInfo.Name;
        //playerCounter.SetText("{0} / {1}", roomInfo.PlayerCount, roomInfo.MaxPlayers);
        // ルーム人数を設定
        _playerCounter.text = string.Format("{0} / {1}", roomInfo.PlayerCount, roomInfo.MaxPlayers);

        // ルームが満員でない時のみ、参加ボタンを押せるようにする
        _createButton.interactable = (roomInfo.PlayerCount < roomInfo.MaxPlayers);

        // ルーム表示
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        // ルーム非表示
        gameObject.SetActive(false);
    }
}