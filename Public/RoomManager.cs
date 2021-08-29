using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class RoomManager : MonoBehaviour
{
    enum GameMode
    {
        Solo = 0,
        Team = 1
    }

    enum MapType
    {
        City = 0
    }

    struct RoomInfo
    {
        //방 번호
        uint m_number;
        //방 이름
        string m_name;
        //비밀번호
        string m_password;

        //최대 인원
        int m_maxplayercount;
        //현재인원
        int m_playercount;

        string m_room_master_name;
        GameMode m_game_mode;
        MapType m_map_select;

        //방 잠금상태
        bool m_IsLock;
    }

    //서버로부터 받은 맵을 관리한다.
    List<RoomInfo> Room_List = new List<RoomInfo>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
