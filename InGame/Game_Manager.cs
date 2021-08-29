using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_Manager : Photon.PunBehaviour {

    public static Game_Manager Instance;
        
    [Header("InGame_Object")]
    public Transform Respon_Positon;
    public Transform Soul_Respon;

    [HideInInspector]
    public GameObject My_Unit;

    public Person_Camera three_Camera;
    float distace;
    float Min_height;
    float Max_height;

    public UIJoystick Joy_stick_Script;

    public bool[] Death_Check;

    public enum Result_Type
    {
        None = 0,
        Draw=1,
        Win = 2,
        Lose = 3
    }

    public Result_Type Game_Result = Result_Type.None;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        Death_Check = new bool[InRoom_Canvas.Room_Join_User.Count];
        for (int i = 0; i < Death_Check.Length; i++)
        {
            Death_Check[i] = false;
        }
        Manager.Instance.IsGame = true; //방에 입장해서 현재 인 게임 중 인지를 파악하기 위함
    }

    public void User_Disconnected_InGame() //게임 중에 유저가 나갔을 경우. 해당 플레이어를 죽은 것으로 처리해버리고 게임 속행.
    {
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            int Count = 0;
            for (int j = 0; j < InRoom_Canvas.Room_Join_User.Count; j++)
            {
                if (PhotonNetwork.playerList[i].NickName != InRoom_Canvas.Room_Join_User[j].Nick_Name)
                    ++Count;

                if (Count >= PhotonNetwork.playerList.Length)
                {
                    //Game_Manager.Instance.pv.RPC("User_Death_Boolean", PhotonTargets.MasterClient, i);// 나간 플레이어 리스트 넘버 삽입.
                }
            }
        }
    }

    [PunRPC]
    public void User_Death_Boolean(int index)
    {
        int[] Win_List;
        Death_Check[index] = true;

        switch (Manager.Instance.Room.mode)
        {
            case 0: //개인전
                Win_List = new int[1];
                int Death_Count = 0;
                for (int i=0; i<Death_Check.Length; i++) //플레이어 리스트를 돌리고 죽었는지 체크
                {
                    if (Death_Check[i] == true)
                        Death_Count++;
                    else
                        Win_List[0] = i; // for문을 돌면서 결국 마지막까지 죽지않은 플레이어의 index값을 갖는다
                }

                if (Death_Count >= Death_Check.Length - 1) //플레이어가 한명만 살아남았을 경우 게임 끝났음을 알린다.
                {
                    //pv.RPC("Result_Set", PhotonTargets.All, Win_List, false);
                }
                break;

            case 1: //팀전
                int OneTeam_Death_Count = 0;
                int TwoTeam_Death_Count = 0;

                for (int i = 0; i < Death_Check.Length; i++) //플레이어 리스트를 돌리고 죽었는지 체크
                {
                    if (Death_Check[i] == true && InRoom_Canvas.Room_Join_User[i].Team_Type == InRoom_Canvas.Team_.One_Team)
                        OneTeam_Death_Count++;
                    if (Death_Check[i] == true && InRoom_Canvas.Room_Join_User[i].Team_Type == InRoom_Canvas.Team_.Two_Team)
                        TwoTeam_Death_Count++;
                }

                if (OneTeam_Death_Count >= InRoom_Canvas.Present_One_Team_Count || TwoTeam_Death_Count >= InRoom_Canvas.Present_Two_Team_Count) // 1팀이나 2팀의 죽은 팀 수가 해당 팀수의 구성인원보다 같거나 크면 게임이 끝났으므로, 이에 대한 승리 팀 처리
                { 
                    int Win_Indexer = 0; //승리팀 인덱스에 기록시 인덱서 역활
                    if (OneTeam_Death_Count >= InRoom_Canvas.Present_One_Team_Count) // 1번팀이 전멸하였으므로 2번팀을 찾아서  승리 리스트에 추가.
                    {
                        Win_List = new int[InRoom_Canvas.Present_Two_Team_Count]; //승리팀 인원수 만큼 리스트 메모리 할당
                        for (int i = 0; i < Death_Check.Length; i++)
                        {
                            if (InRoom_Canvas.Room_Join_User[i].Team_Type == InRoom_Canvas.Team_.Two_Team) //승리팀의 인덱스 번호를 리스트에 기록
                            {
                                Win_List[Win_Indexer] = i;
                                Win_Indexer++;
                            }
                        }
                    }
                    else // 2번팀이 전멸하였으므로 1번팀을 찾아서  승리 리스트에 추가.
                    {
                        Win_List = new int[InRoom_Canvas.Present_One_Team_Count]; //승리팀 인원수 만큼 리스트 메모리 할당
                        for (int i = 0; i < Death_Check.Length; i++)
                        {
                            if (InRoom_Canvas.Room_Join_User[i].Team_Type == InRoom_Canvas.Team_.One_Team) //승리팀의 인덱스 번호를 리스트에 기록
                            {
                                Win_List[Win_Indexer] = i;
                                Win_Indexer++;
                            }
                        }
                    }
                    //pv.RPC("Result_Set", PhotonTargets.All, Win_List, false); // 승리팀 유저 리스트 인덱스 값을 넘겨주고 게임이 끝났음을 알린다.
                }
                break;
        }
    }

    [PunRPC]
    public void Result_Set(int[] Win_index, bool Draw_Check)
    {
        /*
        for (int index = 0; index < Win_index.Length; index++)
        {
            Debug.Log(Win_index[index].ToString());
            Debug.Log(InRoom_Canvas.Room_Join_User[Win_index[index]].ID);
        }
        Debug.Log(Game_Result.ToString());*/

        if (Draw_Check == false) //무승부가 아닐경우
        {
            for (int index = 0; index < Win_index.Length; index++)
            {
                if (InRoom_Canvas.Room_Join_User[Win_index[index]].Nick_Name == Manager.Instance.User.Nick_Name) //승리 명단에 내가 있을 경우
                {
                    Game_Result = Result_Type.Win;
                    InGame.Instance.Result_Canvas_Initialize(); //결과창을 불러오는 UI 호출
                    return;
                }
                else
                {
                    Game_Result = Result_Type.Lose;
                }
            }
        }
        else //무승부 일경우
        {
            Game_Result = Result_Type.Draw;
        }
        
        InGame.Instance.Result_Canvas_Initialize(); //결과창을 불러오는 UI 호출
    }

    public void Make_Soul_Stone() //몇 초후에 생성할건지, 몇개를?
    {
        Soul_Stone.Stone type = Soul_Stone.Stone.Blue; //기본값 초기화.

        int value = Random.Range(0, 100); // 무슨 종류의 스톤을 만들어 낼건지 5% 10% 85%

        if (value < 10)
            type = Soul_Stone.Stone.Red;
        else if (value >= 10 && value < 30)
            type = Soul_Stone.Stone.Green;
        else
            type = Soul_Stone.Stone.Blue;

        int count = Random.Range(1, 2);
        int place = Random.Range(0, Soul_Respon.childCount - 1);

        switch (type)
        {
            case Soul_Stone.Stone.Blue:
                PhotonNetwork.Instantiate("SoulStone/BlueStone", Soul_Respon.GetChild(place).transform.position, Quaternion.identity, 0);
                break;

            case Soul_Stone.Stone.Green:
                PhotonNetwork.Instantiate("SoulStone/GreenStone", Soul_Respon.GetChild(place).transform.position, Quaternion.identity, 0);
                break;

            case Soul_Stone.Stone.Red:
                PhotonNetwork.Instantiate("SoulStone/RedStone", Soul_Respon.GetChild(place).transform.position, Quaternion.identity, 0);
                break;

            default:
                PhotonNetwork.Instantiate("SoulStone/BlueStone", Soul_Respon.GetChild(place).transform.position, Quaternion.identity, 0);
                break;
        }
        Invoke("Make_Soul_Stone", Random.Range(30, 60));
    }

    //내가 가지고 있는 스킬안에서 랜덤으로 선택함.
    public void Rand_Skill(Ability.Skill_Rank_Height Height)
    {
        if (Height == Ability.Skill_Rank_Height.High_Rank)
        {
            if (Manager.Instance.My_Ability_High_List.Count != 0)
            {
                int High_Index = Random.Range(0, Manager.Instance.My_Ability_High_List.Count - 1);
                Character_State.Instance.My_Character_Info.High_Rank_Skill = Manager.Instance.My_Ability_High_List[High_Index].Index;
                Character_State.Instance.My_Character_Info.High_Skill_Level = Manager.Instance.My_Ability_High_List[High_Index].Level;
            }
            else
            {
                Character_State.Instance.My_Character_Info.High_Rank_Skill = 0;
                Character_State.Instance.My_Character_Info.High_Skill_Level = 1;
            }
        }
        else
        {
            if (Manager.Instance.My_Ability_Row_List.Count != 0) {
                int Row_Index = Random.Range(0, Manager.Instance.My_Ability_Row_List.Count - 1);
                Character_State.Instance.My_Character_Info.Row_Rank_Skill = Manager.Instance.My_Ability_Row_List[Row_Index].Index;
                Character_State.Instance.My_Character_Info.Row_Skill_Level = Manager.Instance.My_Ability_Row_List[Row_Index].Level;
            }
            else
            {
                Character_State.Instance.My_Character_Info.Row_Rank_Skill = 0;
                Character_State.Instance.My_Character_Info.Row_Skill_Level = 1;
            }
        }
    }
    
    //일정 시간초 후에 Timer_Canvas에서 호출
    public void High_Ability_Set()
    {
        Rand_Skill(Ability.Skill_Rank_Height.High_Rank);
        My_Unit.GetComponent<Character_State>().My_Character_Info.High_Rank_Skill = Character_State.Instance.My_Character_Info.High_Rank_Skill;
        Setting_Ability(GamePlay_Canvas.Instance.Ability2_Btn, My_Unit.GetComponent<Character_State>().My_Character_Info.High_Rank_Skill, My_Unit.GetComponent<Character_State>().My_Character_Info.High_Skill_Level);
        Notice_Canvas.Instance.Notice_("상위 랭크 스킬이 개방되었습니다.", 3.0f);
    }

    public void Call_Corutine_Set_Game()
    {
        StartCoroutine(Set_Game());
    }
            
    public IEnumerator Set_Game()
    {
        Notice_Canvas.Instance.Close();
        Joy_stick_Script.move_Script = My_Unit.GetComponent<move>();
        three_Camera.GetComponent<Person_Camera>().target = My_Unit.transform;

        GameObject[] Unit_List = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject Unit in Unit_List)
        {
            //관전하려는 캐릭터가 살아있고, 그 캐릭터가 나와 같은 팀일 경우 관전.
            if (Unit.GetComponent<Character_State>().My_Character_Info.Team_Type == Game_Manager.Instance.My_Unit.GetComponent<Character_State>().My_Character_Info.Team_Type)
            {
                GameObject Unit_Pos = GameObject.Instantiate(Resources.Load<GameObject>("Prefeb\\Blue_Mini_Map_Pos"), Vector3.zero, Quaternion.identity);
                Unit_Pos.transform.parent = Unit.transform;
                Unit_Pos.transform.localPosition = new Vector3(0, 96, 0);
                Unit_Pos.transform.localScale = Vector3.one;
            }
            else
            {
                GameObject Unit_Pos = GameObject.Instantiate(Resources.Load<GameObject>("Prefeb\\Red_Mini_Map_Pos"), Vector3.zero, Quaternion.identity);
                Unit_Pos.transform.parent = Unit.transform;
                Unit_Pos.transform.localPosition = new Vector3(0, 95, 0);
                Unit_Pos.transform.localScale = Vector3.one;
            }
        }
        yield return 0;
    }

    public void Result_Proccess()
    {
        InGame.Instance.Result_Canvas_Initialize();
    }

    //Ability Info 버튼 누르고있을경우
    public void Preesed_Ability_Open_Btn(Transform pos)
    {
        //나타내기 전에 정보를 새로고침 한다.
        pos.GetChild(0).transform.gameObject.SetActive(true);
    }

    //Ability Info 버튼 누르고있을경우
    public void Preesed_Ability_Close_Btn(Transform pos)
    {
        //나타내기 전에 정보를 새로고침 한다.
        pos.GetChild(0).transform.gameObject.SetActive(false);
    }

    public void Camera_Vibration_Set()
    {
        distace = three_Camera.distance;
        Min_height = three_Camera.minHeight;
        Max_height = three_Camera.maxHeight;

        three_Camera.distance = 3;
        three_Camera.maxHeight = 1;
        three_Camera.minHeight = 30;
        //카메라가 제자리 찾으려는것을 이용한 흔들림 효과.
    }

    public void Camera_Vibration_ReSet()
    {
        three_Camera.distance=distace;
        three_Camera.minHeight=Min_height;
        three_Camera.maxHeight = Max_height;
    }

    //인덱스 번호를 바탕으로 스킬을 설정.
    public void Setting_Ability(UIButton Btn_Object,int Ability_index, int Ability_level)
    {
        EventDelegate Ability_Event;

        
        Btn_Object.GetComponent<Ability_Button>().Ability_Instance = null;


        Btn_Object.GetComponent<Ability_Button>().Ability_Instance = null;
        Btn_Object.GetComponent<UIButton>().onClick.Clear();

        Ability ability = Manager.Instance.Return_Ability(Ability_index);

        Btn_Object.transform.gameObject.GetComponent<Ability_Button>().Ability_Ititialize(ability, Ability_level);

        if (ability.Rank == Ability.Skill_Rank.S || ability.Rank == Ability.Skill_Rank.A || ability.Rank == Ability.Skill_Rank.B)
        {
            Character_State.Instance.My_Character_Info.High_Skill_Script = ability;
            Character_State.Instance.My_Character_Info.High_Skill_Level = Ability_level;
        }
        else
        {
            Character_State.Instance.My_Character_Info.Row_Skill_Script = ability;
            Character_State.Instance.My_Character_Info.Row_Skill_Level = Ability_level;
        }

        Ability_Event = new EventDelegate(Btn_Object.GetComponent<Ability_Button>(), "Use_Ability");
        Btn_Object.GetComponent<UIButton>().onClick.Add(Ability_Event);
    }
}
