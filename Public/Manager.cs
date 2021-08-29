using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{

    public static Manager Instance = null;

    public int IsSuspension = 0;

    public delegate void UIDelegate();

    public GameObject Exception_Box;
    public GameObject Wait_Pannel;

    public float Volum = 1.0f;

    [HideInInspector]
    public bool InOut_Check = false; //DB 접속명단에 기록됬는지 확인하는 변수

    [HideInInspector]
    public int Server_Access_Count = 0;

    //현재 착용중인 아이템 리스트 0번쨰 머리 1번째 상의 2번쨰 하의 3번째 신발 끝.
    public int[] Wear_Item = null;

    [HideInInspector]
    public bool IsGame=false; //현재 인게임인지 아닌지 파악.

    //내가 가지고 있는 아이콘 리스트
    public List<Icon_Info> My_Icon_List = null;
    //내가 가지고 있는 능력 리스트
    public List<Ability_Info> My_Ability_Row_List = null;
    //내가 가지고 있는 능력 리스트
    public List<Ability_Info> My_Ability_High_List = null;
    //인벤토리에 내가 가지고 있는 머리부분 아이템 리스트
    public List<Item_Info> Item_Head_List = null;
    //인벤토리에 내가 가지고 있는 상의부분 아이템 리스트
    public List<Item_Info> Item_Top_List = null;
    //인벤토리에 내가 가지고 있는 하의부분 아이템 리스트
    public List<Item_Info> Item_Pants_List = null;
    //인벤토리에 내가 가지고 있는 신발부분 아이템 리스트
    public List<Item_Info> Item_Shoes_List = null;

    public string[] Rank_List_Name = null;
    public string[] Rank_List_Lv = null;
    public string[] Rank_List_play = null;

    public Room_Info Room = null;
    public User_Form User = null;

    public static string Client_Version = "0.0.1 " + "Ver";

    public Sprite[] Map_Image_Resource = null;

    public string Scene_name = "Start";

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        if (Manager.Instance == this)
        {
            DontDestroyOnLoad(this.gameObject);
        }
        Scene_name = "Start";
        Application.targetFrameRate = 60;


    }

    private void Start()
    {
        Room = new Room_Info();
        User = new User_Form();
        My_Icon_List = new List<Icon_Info>();
        My_Ability_Row_List = new List<Ability_Info>();
        My_Ability_High_List = new List<Ability_Info>();
        Item_Head_List = new List<Item_Info>();
        Item_Top_List = new List<Item_Info>();
        Item_Pants_List = new List<Item_Info>();
        Item_Shoes_List = new List<Item_Info>();
        //this.GetComponent<Localize_Manager>().Check_Languge(Application.systemLanguage);

        Ability_List_Set(); //모든능력 리스트 생성
    }

    public GameObject Loading_Print()
    {
        GameObject Wait_object = Instantiate(Manager.Instance.Wait_Pannel, Vector3.zero, Quaternion.identity);
        Wait_object.transform.parent = GameObject.Find("Pannel").transform;
        Wait_object.transform.localScale = new Vector3(1, 1, 1);
        Wait_Pannel.gameObject.SetActive(true);
        return Wait_object;
    }

    public void Error_Print(string Error_name)
    {
        GameObject Error_ = Instantiate(Manager.Instance.Exception_Box, Vector3.zero, Quaternion.identity);
        Error_.transform.GetChild(1).GetComponent<UILabel>().text = Error_name;
        Error_.GetComponent<UISprite>().enabled = false;
        Error_.GetComponent<UISprite>().depth = 100;
        Error_.GetComponent<UISprite>().enabled = true;
        Error_.gameObject.SetActive(true);
    }

    public void Game_Quit()
    {
        InOut_Check = false;
        if (PhotonNetwork.connected == true)
        {
            PhotonNetwork.Disconnect();
        }
        Start();
        SceneManager.LoadScene("Start");
    }

    //어플리케이션이 종료되면 호출되는 함수! 여기다가 강제종료시 구현하자!
    public void OnApplicationQuit()
    {
        if (InOut_Check == true)
        {
            Debug.Log("접속이 해제");
        }
    }

    void Ability_List_Set()
    {
        //임시 테스트 구문 추후 삭제하세요.
        My_Ability_Row_List.Add(new Manager.Ability_Info(3001, 0, 1)); //건강한몸
        My_Ability_Row_List.Add(new Manager.Ability_Info(3002, 0, 1)); //관심법
        My_Ability_Row_List.Add(new Manager.Ability_Info(3003, 0, 1)); //장애물 생성
        My_Ability_Row_List.Add(new Manager.Ability_Info(3004, 0, 1)); //소울스톤훔치기
        My_Ability_Row_List.Add(new Manager.Ability_Info(3005, 0, 1)); //그라비아 사진첩 투척

        My_Ability_Row_List.Add(new Manager.Ability_Info(4001, 0, 1)); //투척
        My_Ability_Row_List.Add(new Manager.Ability_Info(4002, 0, 1)); //체력회복사과
        My_Ability_Row_List.Add(new Manager.Ability_Info(4003, 0, 1)); //랜덤이동

      //  Manager.Instance.My_Ability_High_List.Add(new Manager.Ability_Info(1, 0, 0)); 아직 미구현한 능력
      
        My_Ability_High_List.Add(new Manager.Ability_Info(1001, 0, 1));//바람
        My_Ability_High_List.Add(new Manager.Ability_Info(1002, 0, 1)); //번개
        My_Ability_High_List.Add(new Manager.Ability_Info(1003, 0, 1)); //어스퀘이크
        
        My_Ability_High_List.Add(new Manager.Ability_Info(2001, 0, 1)); //불장벽
        My_Ability_High_List.Add(new Manager.Ability_Info(2002, 0, 1)); //은신
        My_Ability_High_List.Add(new Manager.Ability_Info(2003, 0, 1)); //열려라 포탈 고치자!
        My_Ability_High_List.Add(new Manager.Ability_Info(2004, 0, 1)); //가스가스!
    }

    public Ability Return_Ability(int index)
    {
        Ability Instance;

        switch (index)
        {
            case 0:
                Instance = new Ability_0();
                return Instance;

            case 1:
                Instance = new Ability_1();
                return Instance;

            case 1001:
                Instance = new Ability_1001();
                return Instance;

            case 1002:
                Instance = new Ability_1002();
                return Instance;

            case 1003:
                Instance = new Ability_1003();
                return Instance;

            case 2001:
                Instance = new Ability_2001();
                return Instance;

            case 2002:
                Instance = new Ability_2002();
                return Instance;

            case 2003:
                Instance = new Ability_2003();
                return Instance;

            case 2004:
                Instance = new Ability_2004();
                return Instance;

            case 3001:
                Instance = new Ability_3001();
                return Instance;
                
            case 3002:
                Instance = new Ability_3002();
                return Instance;
                
            case 3003:
                Instance = new Ability_3003();
                return Instance;
                
            case 3004:
                Instance = new Ability_3004();
                return Instance;

            case 3005:
                Instance = new Ability_3005();
                return Instance;

            case 4001:
                Instance = new Ability_4001();
                return Instance;

            case 4002:
                Instance = new Ability_4002();
                return Instance;

            case 4003:
                Instance = new Ability_4003();
                return Instance;

            default:
                Debug.LogError("클라이언트에 등록된 스킬 선택 범위를 벗어났습니다.");
                return new Ability_0(); //0은 무능력이므로 0레벨 처리.
        }
    }
    public class User_Form
    {
        public string Key; //DB상에서 유저를 찾는 고유의 키값.
        public string Nick_Name; //게임상에서 유저를 찾는 닉네임 및 키값
        public string ID;
        public string PW;
        public int Lv;
        public int Exp;
        public int Exp_Maximum;
        public long Money;
        public long Soul_Stone; //유저가 보유하고 있는 스톤 
        public int Icon; //유저가 장착한 현재 아이콘 스프라이트 넘버
        public string Friends;
        public string Hold_Item_List; //아이템 번호 ex) 1,2,3,1001,2004,3001 이런 식.
        public string country;
        public string User_Platform;
    }

    public class Room_Info
    {
        public string room_name;
        public int map; // 도시 등의 맵
        public int mode; //팀전 개인전
        public int time; //분 단위 시간 저장
        public int One_Team_Count;
        public int Two_Team_Count;
    }

    public class Item_Info
    {
        public int Name_Key;
        public string Part;
        public int Count;
        public string Info;

        public Item_Info(int key, string part, int count, string info)
        {
            Name_Key = key;
            Part = part;
            Count = count;
            Info = info;
        }
    }

    public class Ability_Info
    {
        public int Index;
        public float Exp;
        public int Level;

        public Ability_Info(int index, float exp, int lv)
        {
            Index = index;
            Exp = exp;
            Level = lv;
        }

        public void Find_Ability_Set(int index, float exp, int lv)
        {
            if (index == Index)
            {
                Exp = exp;
                Level = lv;
            }
        }
    }

    public class Icon_Info
    {
        public int Index;
        public string Name;
        public string Info;

        public Icon_Info(int index, string name, string info)
        {
            Index = index;
            Name = name;
            Info = info;
        }
    }
}


