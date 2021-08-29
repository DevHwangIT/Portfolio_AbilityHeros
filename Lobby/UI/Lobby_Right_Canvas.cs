using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby_Right_Canvas : MonoBehaviour, UI_Interface
{

    [HideInInspector]
    public static Lobby_Right_Canvas Instance;

    string Quick_Match_string_Type = "Solo";
    int Quick_Match_int_Type = 0;
    int Quick_Match_Map = 0;

    [Header("Menu_Right_Btn")]
    public GameObject Quick_Match_Setting_MenuRight;
    public GameObject Quick_Match_set_MenuRight;
    public GameObject Quick_Match_MenuRight;
    public GameObject Quick_Matching_MenuRight;
    public GameObject Room_Join_MenuRight;
    public GameObject MyRoom_MenuRight;

    public UILabel Quick_Matching_State_Label;

    public UILabel Setting_Mode_Label;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void Open()
    {
        this.gameObject.SetActive(true);
        this.GetComponent<TweenPosition>().PlayForward();
    }

    public void Close()
    {
        this.GetComponent<TweenPosition>().PlayReverse();
        this.GetComponent<TweenPosition>().callWhenFinished = "Close_Delay";
    }

    public void Close_Delay()
    {
        this.gameObject.SetActive(false);
    }

    int QuickMatching_Sec=0;

    void QuickMatching_Suching()
    {
        Quick_Matching_State_Label.text = QuickMatching_Sec + " Second.. Finding..";

        if (QuickMatching_Sec > 60 && PhotonNetwork.inRoom == false)
        {
            RoomOptions room_setting = new RoomOptions();
            TypedLobby robi_setting = new TypedLobby();
            room_setting.MaxPlayers = 8;
            room_setting.IsOpen = true;
            room_setting.IsVisible = true;
            PhotonNetwork.CreateRoom(Manager.Instance.User.Nick_Name + "of Room" + "!" + "" + "!" + Quick_Match_Map + "!" + Manager.Instance.User.Nick_Name + "!" + Quick_Match_string_Type, room_setting, robi_setting);
            PhotonNetwork.autoCleanUpPlayerObjects = false;
            Match_Canvas.Present_Room.name = Manager.Instance.User.Nick_Name + "of Room";
            Match_Canvas.Present_Room.pw = "";

            //게임 설정한 정보를 Manager Class 에 전달.
            if (Quick_Match_string_Type == "Solo")
            {
                Manager.Instance.Room.One_Team_Count = 8;
            }
            if (Quick_Match_string_Type == "Team")
            {
                Manager.Instance.Room.One_Team_Count = 8 / 2;
                Manager.Instance.Room.Two_Team_Count = 8 / 2;
            }
            Manager.Instance.Room.map = Quick_Match_Map;
            Manager.Instance.Room.mode = Quick_Match_int_Type;
            Manager.Instance.Room.room_name = Manager.Instance.User.Nick_Name + "of Room";
            Manager.Instance.Room.time = 10; //split하는 이유는 text가 X분 식으로 되어 있으므로.

            QuickMatching_Sec = 0;
            CancelInvoke("QuickMatching_Suching");
            return;
        }

        Photon_Manager.Instance.Get_Room_List();
        
        if (Photon_Manager.Instance.Room_List.Length != 0) {
            for (int index = 0; index < Photon_Manager.Instance.Room_List.Length; index++)
            {
                string[] RoomInfo_ = Photon_Manager.Instance.Room_List[index].Name.Split('!');

                if (RoomInfo_[1] == "" && RoomInfo_[2] == Quick_Match_Map.ToString() && RoomInfo_[4] == Quick_Match_string_Type)
                {
                    QuickMatching_Sec = 0;
                    CancelInvoke("QuickMatching_Suching");
                    Quick_Match_Close_Btn();
                    PhotonNetwork.JoinRoom(Photon_Manager.Instance.Room_List[index].Name);
                    return;
                }
            }
        }

        QuickMatching_Sec++;
        Invoke("QuickMatching_Suching", 1.0f);
    }

    public void Quick_Match_Setting_Open_Btn() // 퀵 매치 설정 키기 - 퀵매치돌리는 중인데 설정 누르면 최소되게 처리 버튼
    {
        Quick_Match_MenuRight.SetActive(false);
        Room_Join_MenuRight.SetActive(false);
        MyRoom_MenuRight.SetActive(false);
        Quick_Match_set_MenuRight.SetActive(false);
        Quick_Match_Setting_MenuRight.SetActive(true);
    }

    public void Quick_Match_Setting_Close_Btn() // 퀵 매치 설정 끄기 - 퀵매치돌리는 중인데 설정 누르면 최소되게 처리 버튼
    {
        Quick_Match_MenuRight.SetActive(true);
        Room_Join_MenuRight.SetActive(true);
        MyRoom_MenuRight.SetActive(true);
        Quick_Match_set_MenuRight.SetActive(true);
        Quick_Match_Setting_MenuRight.SetActive(false);
    }

    public void Quick_Match_Open_Btn() //퀵 매치 참가 열기 버튼
    {
        Quick_Match_MenuRight.SetActive(false);
        Quick_Matching_MenuRight.SetActive(true);
        Quick_Match_MenuRight.GetComponent<TweenPosition>().enabled = true;
        Quick_Matching_MenuRight.GetComponent<TweenPosition>().PlayForward();

        // 방 찾기 혹은 생성 큐 돌림
        QuickMatching_Sec = 0;
        Invoke("QuickMatching_Suching", 1.0f);
    }

    int Mode = 0;//int 0이면 개인전 1이면 팀전

    public void Quick_Match_Type_Set_Right_Btn()
    {
        Mode += 1;
        switch (Mode)
        {
            case 0:
                Manager.Instance.Room.mode = 0; //개인전
                Setting_Mode_Label.text = "개인 섬멸전";
                break;

            case 1:
                Manager.Instance.Room.mode = 1; // 팀전
                Setting_Mode_Label.text = "팀 섬멸전";
                break;

            default:
                Mode = 0;
                Manager.Instance.Room.mode = 0; //개인전
                Setting_Mode_Label.text = "개인 섬멸전";
                break;
        }
    }

    public void Quick_Match_Type_Set_Left_Btn()
    {
        Mode -= 1;
        switch (Mode)
        {
            case 0:
                Manager.Instance.Room.mode = 0; //개인전
                Setting_Mode_Label.text = "개인 섬멸전";
                break;

            case 1:
                Manager.Instance.Room.mode = 1; //팀전
                Setting_Mode_Label.text = "팀 섬멸전";
                break;

            default:
                Mode = 1;
                Manager.Instance.Room.mode = 1; //팀전
                Setting_Mode_Label.text = "팀 섬멸전";
                break;
        }
    }

    public void Quick_Match_Close_Btn() //퀵 매치 참가 닫기 버튼
    {
        Quick_Match_MenuRight.SetActive(true);
        Quick_Matching_MenuRight.SetActive(false);
        Quick_Match_MenuRight.GetComponent<TweenPosition>().enabled = true;
        Quick_Match_MenuRight.GetComponent<TweenPosition>().PlayForward();

        // 방 찾기 혹은 생성 큐 멈춤
        CancelInvoke("QuickMatching_Suching");
    }

    public void Match_Room_Btn() //대기방참가 버튼
    {
        Right_Menu_Reset();
        Lobby.Instance.Match_UI_Show_UI();
        Match_Set();
    }

    public void Myroom_Btn() // 마이룸 버튼
    {
        Right_Menu_Reset();
        MyRoom_Set();
    }

    void Right_Menu_Reset() //초기화
    {
        Quick_Matching_MenuRight.SetActive(false);
        Quick_Match_Setting_MenuRight.SetActive(false);
        Quick_Match_set_MenuRight.SetActive(true);
        Quick_Match_MenuRight.SetActive(true);
        Room_Join_MenuRight.SetActive(true);
        MyRoom_MenuRight.SetActive(true);
    }

    void Match_Set()
    {
        Lobby.Instance.Match_UI_Show_UI();
        Match_Canvas.Instance.gameObject.SetActive(true);
        Match_Canvas.Instance.gameObject.GetComponent<TweenPosition>().PlayForward();
    }

    void MyRoom_Set()
    {
        Lobby.Instance.CustomRoom_UI_Show_UI();
    }
}
