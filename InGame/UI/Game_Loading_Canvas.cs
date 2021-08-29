using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Loading_Canvas : Photon.PunBehaviour
{

    public static Game_Loading_Canvas Instance;

    public GameObject[] User_Slot = new GameObject[8];

    PhotonView pv;
    
    [HideInInspector]
    public int Player_Ready_Count = 0;

    bool Set_Check = false; // setting 체크하기위한 bool 변수

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        pv = this.GetComponent<PhotonView>();
        Set_Check = false;

        for (int i = 0; i < InRoom_Canvas.Room_Join_User.Count; i++)
        {
            User_Slot[i].SetActive(true);
            User_Slot[i].transform.GetChild(0).GetComponent<UILabel>().text = InRoom_Canvas.Room_Join_User[i].Nick_Name;
            User_Slot[i].transform.GetChild(1).GetComponent<UILabel>().text = "Loading...";
            User_Slot[i].transform.GetChild(2).GetChild(0).GetComponent<UISprite>().name = InRoom_Canvas.Room_Join_User[i].Sprite_Icon.ToString(); //추후 수정.
        }
    }

    void Start()
    {
        pv.RPC("Ready_Count", PhotonTargets.AllBuffered, InRoom_Canvas.My_Slot_Index, Manager.Instance.User.Nick_Name, InRoom_Canvas.My_Slot_Index); //아이콘 보내기 추후 수정//레디 상태인지 아닌지 파악.
    }

    public void Open()
    {
        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    [PunRPC]
    public void Ready_Count(int index, string Nick_name, int sprite_icon)
    {
        ++Player_Ready_Count;
        User_Slot[index].transform.GetChild(0).GetComponent<UILabel>().text = Nick_name;
        User_Slot[index].transform.GetChild(1).GetComponent<UILabel>().text = "Ready";
        User_Slot[index].transform.GetChild(2).GetChild(0).GetComponent<UISprite>().name = sprite_icon.ToString();

        if (Player_Ready_Count == InRoom_Canvas.Room_Join_User.Count) //모든 유저들이 씬전환이 끝났을 경우, 마스터클라이언트에서 처리안하는 이유= 마스터클라이언트가 로딩이 더 느릴경우 멈춤현상 발생
        {
            pv.RPC("Set_Start", PhotonTargets.All);
        }
    }

    [PunRPC]
    public void Set_Start()
    {
        if (Set_Check == true) // 모든클라이언트에서 호출하므로 이를 막기위한 세마포역활
            return;
        Set_Check = true;
        InGame.Instance.Play_Canvas_Initialize();
        Game_Manager.Instance.My_Unit = PhotonNetwork.Instantiate("Normal", Game_Manager.Instance.Respon_Positon.GetChild(InRoom_Canvas.My_Slot_Index).transform.position, Quaternion.identity, 0);
        Game_Manager.Instance.My_Unit.AddComponent<move>();
        Game_Manager.Instance.My_Unit.AddComponent<Character_State>();

        //시작전에 모든 클라이언트들이 내가 무슨캐릭을 조종하는지 찾아주자...
        if (PhotonNetwork.isMasterClient)
        {
            Timer_Canvas.Instance.Timer = Manager.Instance.Room.time * 60;
            Timer_Canvas.Instance.Ability_Set_Timer = ((Manager.Instance.Room.time - 1) * 60) + 30;

            Timer_Canvas.Instance.Timer_Count();
            Game_Manager.Instance.Make_Soul_Stone(); //마스터 클라이언트 일경우 소울스톤을 생성하는 메소드 실행.
        }
        Game_Manager.Instance.My_Unit.GetComponent<Character_State>().My_Character_Info.Nick_Name = Manager.Instance.User.Nick_Name;
        Game_Manager.Instance.My_Unit.GetComponent<Character_State>().My_Character_Info.Damege = 10;
        Game_Manager.Instance.My_Unit.GetComponent<Character_State>().My_Character_Info.HP_Maximum = 100;
        Game_Manager.Instance.My_Unit.GetComponent<Character_State>().My_Character_Info.HP = 100;
        Game_Manager.Instance.My_Unit.GetComponent<Character_State>().My_Character_Info.Life_State = Character_State.Life_.Live;
        Game_Manager.Instance.Rand_Skill(Ability.Skill_Rank_Height.Row_Rank); //랜덤 스킬 번호 받아온다.


        Game_Manager.Instance.Setting_Ability(GamePlay_Canvas.Instance.Ability1_Btn, Game_Manager.Instance.My_Unit.GetComponent<Character_State>().My_Character_Info.Row_Rank_Skill, Game_Manager.Instance.My_Unit.GetComponent<Character_State>().My_Character_Info.Row_Skill_Level); // 저급 랭크는 시작하면 바로 줌.
        Game_Manager.Instance.Call_Corutine_Set_Game();
    }

    public void Loading_Initialized()
    {
        for (int i = 0; i < InRoom_Canvas.Room_Join_User.Count; i++)
        {
            if (InRoom_Canvas.Room_Join_User[i] != null)
            {
                User_Slot[i].SetActive(true);
                User_Slot[i].transform.GetChild(0).GetComponent<UILabel>().text = InRoom_Canvas.Room_Join_User[i].ID;
                User_Slot[i].transform.GetChild(1).GetComponent<UILabel>().text = "Loading..";
            }
            else
            {
                User_Slot[i].SetActive(false);
            }
        }
    }
}
