using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class RoomListView : MonoBehaviourPunCallbacks
{
    // 最大ルーム作成数
    private const int MaxElements = 20;

    // 作成されたルームを表示するオブジェクト
    [SerializeField]
    private RoomListViewElement _elementPrefab = default;

    // ルームリストスクリプト
    private RoomList _roomList = new RoomList();

    // 作成されたルームを表示するオブジェクトリスト
    private List<RoomListViewElement> _elementList = new List<RoomListViewElement>(MaxElements);

    // スクロールバー
    private ScrollRect _scrollRect;

    // 初期化関数
    public void Init(MatchmakingView parentView)
    {
        // 設定
        _scrollRect = GetComponent<ScrollRect>();

        // ルームリスト要素（ルーム参加ボタン）を生成して初期化する
        for (int index = 0; index < MaxElements; index++)
        {
            // ルームオブジェクトを生成
            var element = Instantiate(_elementPrefab, _scrollRect.content);
            // ルームの初期化
            element.Init(parentView);
            // 一旦非表示
            element.Hide();
            // ルームリストに追加
            _elementList.Add(element);
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> changedRoomList)
    {
        _roomList.Update(changedRoomList);

        // 存在するルームの数だけルームリスト要素を表示する
        int index = 0;
        foreach (var roomInfo in _roomList)
        {
            _elementList[index++].Show(roomInfo);
        }

        // 残りのルームリスト要素を非表示にする
        for (int i = index; i < MaxElements; i++)
        {
            _elementList[i].Hide();
        }
    }
}