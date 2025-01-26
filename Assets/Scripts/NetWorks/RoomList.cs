using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;

// ※ルームリストが更新された時に呼ばれるコールバック（OnRoomListUpdate()）の引数からは、
// 更新されたルーム情報の差分を取得できますが、ルームリスト全体を取得する機能はありません。
// そのため、ロビーを利用する場合には、ルームリストをキャッシュするクラス（RoomList）を独自に実装して、
// ロビーに参加している間はいつでもルームリスト全体を取得できるようにしておくと便利です。

// IEnumerable<RoomInfo>インターフェースを実装して、foreachでルーム情報を列挙できるようにする
public class RoomList : IEnumerable<RoomInfo>
{
    // 文字列で名前を検索できる配列
    private Dictionary<string, RoomInfo> dictionary = new Dictionary<string, RoomInfo>();

    public void Update(List<RoomInfo> changedRoomList)
    {
        foreach (var info in changedRoomList)
        {
            if (!info.RemovedFromList)
            {
                dictionary[info.Name] = info;
            }
            else
            {
                dictionary.Remove(info.Name);
            }
        }
    }

    public void Clear()
    {
        dictionary.Clear();
    }

    // 指定したルーム名のルーム情報があれば取得する
    public bool TryGetRoomInfo(string roomName, out RoomInfo roomInfo)
    {
        return dictionary.TryGetValue(roomName, out roomInfo);
    }

    public IEnumerator<RoomInfo> GetEnumerator()
    {
        foreach (var kvp in dictionary)
        {
            yield return kvp.Value;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}