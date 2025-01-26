using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

public class RoomListMaster : MonoBehaviourPunCallbacks
{
    private RoomList _roomList = new RoomList();

    public override void OnJoinedLobby()
    {
        _roomList.Clear();
    }

    public override void OnRoomListUpdate(List<RoomInfo> changedRoomList)
    {
        _roomList.Update(changedRoomList);
    }

    public override void OnLeftLobby()
    {
        _roomList.Clear();
    }
}