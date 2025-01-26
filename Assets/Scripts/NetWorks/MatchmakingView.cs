using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchmakingView : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private RoomListView _roomListView = default;

    [SerializeField]
    private InputField _roomNameInputField = default;

    [SerializeField]
    private Button _createRoomButton = default;

    private CanvasGroup _canvasGroup;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        // ロビーに参加するまでは、入力できないようにする
        _canvasGroup.interactable = false;

        // ルームリスト表示を初期化する
        _roomListView.Init(this);

        _roomNameInputField.onValueChanged.AddListener(OnRoomNameInputFieldValueChanged);
        _createRoomButton.onClick.AddListener(OnCreateRoomButtonClick);
    }

    public override void OnJoinedLobby()
    {
        // ロビーに参加したら、入力できるようにする
        _canvasGroup.interactable = true;
    }

    private void OnRoomNameInputFieldValueChanged(string value)
    {
        // ルーム名が1文字以上入力されている時のみ、ルーム作成ボタンを押せるようにする
        _createRoomButton.interactable = (value.Length > 0);
    }

    private void OnCreateRoomButtonClick()
    {
        // ルーム作成処理中は、入力できないようにする
        _canvasGroup.interactable = false;

        // 入力フィールドに入力したルーム名のルームを作成する
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(_roomNameInputField.text, roomOptions);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        // ルームの作成が失敗したら、再び入力できるようにする
        _roomNameInputField.text = string.Empty;
        _canvasGroup.interactable = true;
    }

    public void OnJoiningRoom()
    {
        // ルーム参加処理中は、入力できないようにする
        _canvasGroup.interactable = false;
    }

    public override void OnJoinedRoom()
    {
        // ルームへの参加が成功したら、UIを非表示にする
        gameObject.SetActive(false);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // ルームへの参加が失敗したら、再び入力できるようにする
        _canvasGroup.interactable = true;
    }
}