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
        //�� ��ȣ
        uint m_number;
        //�� �̸�
        string m_name;
        //��й�ȣ
        string m_password;

        //�ִ� �ο�
        int m_maxplayercount;
        //�����ο�
        int m_playercount;

        string m_room_master_name;
        GameMode m_game_mode;
        MapType m_map_select;

        //�� ��ݻ���
        bool m_IsLock;
    }

    //�����κ��� ���� ���� �����Ѵ�.
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
