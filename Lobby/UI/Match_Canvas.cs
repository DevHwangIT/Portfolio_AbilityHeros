using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match_Canvas : MonoBehaviour, UI_Interface
{

    [HideInInspector]
    public static Match_Canvas Instance;

    int Present_Page = 1;

    //로비내의 방의 정보를 담기위한 구조체.
    public struct Room_Struct
    {
        public int number;
        public string name;
        public string pw;
        public string name_;
        public string type;
        public int user_count;
        public int maximum;
        public int map_type;
    }

    public static Room_Struct Present_Room;

    [Header("Lobi_Object")]
    public UILabel Room_Count;
    public GameObject[] Room_Info = new GameObject[10];

    public GameObject Access_Canvas;
    public UIInput Access_Password;
    public GameObject Access_Fail_Label;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        Present_Room = new Room_Struct();
        Present_Page = 1;
        Call_Room_Refresh();
    }

    public void Open()
    {
        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    void Call_Room_Refresh()
    {
        Lobi_Room_Refresh();
        Invoke("Call_Room_Refresh", 3.0f);
    }

    public void Lobi_Room_Refresh()
    {
        Photon_Manager.Instance.Get_Room_List();
        if (Photon_Manager.Instance.Room_List.Length != 0)
            Room_Count.text = Present_Page + "/" + (Photon_Manager.Instance.Room_List.Length / 4 + 1);
        else
            Room_Count.text = Present_Page + "/" + Present_Page;

        for (int i = 0; i < Room_Info.Length; i++)
        {
            try
            {
                Room_Info[i].SetActive(true);

                if (i * Room_Info.Length > Photon_Manager.Instance.Room_List.Length)
                    throw new System.IndexOutOfRangeException();

                string[] Room_Name_Pw = Photon_Manager.Instance.Room_List[i * Room_Info.Length].Name.Split('!');


                //방만든사람 하고 타입 팀전인지 개인전인지 파악하는것 추가하기.
                Room_Info[i].GetComponent<Room_Structure>().number_ = i + (Present_Page - 1 * Photon_Manager.Instance.Room_List.Length);
                Room_Info[i].GetComponent<Room_Structure>().name_ = Room_Name_Pw[0];
                Room_Info[i].GetComponent<Room_Structure>().count_user_ = Photon_Manager.Instance.Room_List[i * Room_Info.Length].PlayerCount;
                Room_Info[i].GetComponent<Room_Structure>().maximun_ = Photon_Manager.Instance.Room_List[i * Room_Info.Length].MaxPlayers;
                Room_Info[i].GetComponent<Room_Structure>().pw_ = Room_Name_Pw[1];
                System.Int32.TryParse(Room_Name_Pw[2], out Room_Info[i].GetComponent<Room_Structure>().Map_Type_Number);
                Room_Info[i].GetComponent<Room_Structure>().make_user = Room_Name_Pw[3];
                Room_Info[i].GetComponent<Room_Structure>().type = Room_Name_Pw[4];
                Room_Info[i].GetComponent<Room_Structure>().Info_Set(); //화면상에 설정된 내용으로 UI표기.
            }
            catch (System.IndexOutOfRangeException)
            {
                Room_Info[i].SetActive(false);
            }
        }
    }

    public void User_Join_Room(GameObject Room)
    {
        Present_Room.number = Room.transform.GetComponent<Room_Structure>().number_;
        Present_Room.name = Room.transform.GetComponent<Room_Structure>().name_;
        Present_Room.name_ = Room.transform.GetComponent<Room_Structure>().make_user;
        Present_Room.pw = Room.transform.GetComponent<Room_Structure>().pw_;
        Present_Room.user_count = Room.transform.GetComponent<Room_Structure>().count_user_;
        Present_Room.maximum = Room.transform.GetComponent<Room_Structure>().maximun_;
        Present_Room.map_type = Room.transform.GetComponent<Room_Structure>().Map_Type_Number;
        Present_Room.type = Room.transform.GetComponent<Room_Structure>().type;

        switch (Present_Room.type)
        {
            case "Solo":
                Manager.Instance.Room.mode = 0;
                break;

            case "Team":
                Manager.Instance.Room.mode = 1;
                break;

            default:
                break;
        }

        if (Present_Room.pw == "")
        { //비밀번호가 없으므로 그냥 조인룸.
            Photon_Manager.Instance.Join_Room(Present_Room.name, Present_Room.pw, Present_Room.map_type.ToString(), Present_Room.name_, Present_Room.type);
        }
        else
        {
            Access_Fail_Label.SetActive(false);
            Access_Password_Room();
        }
    }

    //잠금 방을 입장하려고 하였을때
    void Access_Password_Room()
    {
        Access_Canvas.SetActive(true);
    }

    //잠금 방 입장하기 이후 비밀번호 입력화면에서 뒤로가기 눌렀을 때
    public void Access_Room_Accept()
    {
        if (Access_Password.value == Present_Room.pw)
            Photon_Manager.Instance.Join_Room(Present_Room.name, Present_Room.pw, Present_Room.map_type.ToString(), Present_Room.name_, Present_Room.type);
        else
        {
            Access_Fail_Label.SetActive(true);
            Access_Password.value = "";
        }
    }

    //잠금 방 입장하기 이후 비밀번호 입력화면에서 뒤로가기 눌렀을 때
    public void Access_Room_Back()
    {
        Access_Canvas.SetActive(false);
    }

    public void Page_Left_Btn()
    {
        if (Present_Page < Photon_Manager.Instance.Room_List.Length / 8)
            Present_Page++;
        else
            Present_Page = 1;

        Lobi_Room_Refresh();
    }

    public void Page_Right_Btn()
    {
        if (Present_Page > 1)
            Present_Page--;
        else
            Present_Page = Photon_Manager.Instance.Room_List.Length / 8 + 1;

        Lobi_Room_Refresh();
    }
}
