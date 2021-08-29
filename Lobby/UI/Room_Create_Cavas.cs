using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class Room_Create_Cavas : MonoBehaviour, UI_Interface
{

    public static Room_Create_Cavas Instance;

    [Header("Make_Room_Object")]
    public UIInput Room_Name;
    public UIInput Room_PW;

    public UILabel Mode_Label;
    public UISprite Map_Image;

    string Convert_To_String_Type; //Type_Mode와 같이 팀전 개인전 등을 표현.. 포톤용으로 만든 변수
    public int Type_mode = 0; //팀전 개인전 등...
    public int Map_Type = 0; // 맵 선택
    [HideInInspector]
    public int Time_ = 10; // 시간 제한
    public int Maximum = 8; // 최대 입장가능 수

    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
        }
    }

    private void Start()
    {
        Set_Info_Refresh();
    }
    
    public void Open()
    {
        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    public void Set_Info_Refresh()
    {
        switch (Type_mode)
        {
            case 0:
                Mode_Label.text = Localization.Get("Solo");
                Convert_To_String_Type = "Solo";
                break;

            case 1:
                Mode_Label.text = Localization.Get("Team");
                Convert_To_String_Type = "Team";
                break;
                
            default:
                if (Type_mode > 1) {
                    Type_mode = 0;
                    Mode_Label.text = Localization.Get("Solo");
                    Convert_To_String_Type = "Solo";
                }
                else
                {
                    Type_mode = 1;
                    Mode_Label.text = Localization.Get("Team");
                    Convert_To_String_Type = "Team";
                }
                break;
        }

        Map_Image.spriteName = "도시"; //추후 삭제 맵선택

        switch (Map_Type)
        {
            case 0:
                Map_Image.spriteName = "도시";
                break;
                /*
            case 1:
                Map_Image.spriteName = "공항";
                break;

            case 2:
                Map_Image.spriteName = "사막";
                break;
                */
            default:
                Map_Type = 0;
                Map_Image.spriteName = "도시";
                break;
        }
    }
    
    public void Make_Room_Btn()
    {
        if (Room_Name.value.Length < 5)
        {
            Manager.Instance.Error_Print("방 이름의 최소 길이는 5글자 이상입니다.");
            return;
        }
        RoomOptions room_setting = new RoomOptions();
        TypedLobby robi_setting = new TypedLobby();
        room_setting.MaxPlayers = System.Byte.Parse(Maximum.ToString());//임시
        room_setting.IsOpen = true;
        room_setting.IsVisible = true;
        PhotonNetwork.CreateRoom(Room_Name.value + "!" + Room_PW.value + "!" + Map_Type.ToString() + "!" + Manager.Instance.User.Nick_Name + "!" + Convert_To_String_Type, room_setting, robi_setting);
        PhotonNetwork.autoCleanUpPlayerObjects = false;
        Match_Canvas.Present_Room.name = Room_Name.value;
        Match_Canvas.Present_Room.pw = Room_PW.value;

        //게임 설정한 정보를 Manager Class 에 전달.
        if (Type_mode == 0)
        {
            Manager.Instance.Room.One_Team_Count = Maximum;
        }
        if (Type_mode == 1) {
            Manager.Instance.Room.One_Team_Count = Maximum / 2;
            Manager.Instance.Room.Two_Team_Count = Maximum / 2;
        }
        Manager.Instance.Room.map = Map_Type;
        Manager.Instance.Room.mode = Type_mode;
        Manager.Instance.Room.room_name = Room_Name.value;
        Manager.Instance.Room.time = Time_; //split하는 이유는 text가 X분 식으로 되어 있으므로.
    }
    
    public void Mode_Left_Btn()
    {
        Type_mode -= 1;
        Set_Info_Refresh();
    }

    public void Mode_Right_Btn()
    {
        Type_mode += 1;
        Set_Info_Refresh();
    }

    public void Map_Left_Btn()
    {
        Map_Type -= 1;
        Set_Info_Refresh();
    }

    public void Map_Right_Btn()
    {
        Map_Type += 1;
        Set_Info_Refresh();
    }
}
