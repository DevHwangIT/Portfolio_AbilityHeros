using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InRoom_Canvas : MonoBehaviour {

    //Room_Canvas 가 활성화 되었을때 내부 처리 구현하는 클래스

    // 자신이 있는 룸에 대한 정보를 담는 클래스 , 게임내의 정보클래스는 따로 만들자.

    public static InRoom_Canvas Instance;

    public class User_Slot
    {
        public int index_key = 0; //PhotonNetwork.Player.ID가 값으로 들어가며, 이 값으로 나의 인덱스값을 찾아낸다.
        public string ID = "없음"; //리스트 내에서 내가 어디에있는지 찾기 위한 ID 삽입
        public string Nick_Name = "없음";
        public int Lv = 1;
        public bool Room_Master = false;
        public bool Ready_State = false; //레디 상태인지 아닌지 체크하는 Bool
        public Team_ Team_Type;
        public int[] wear_item;
        public int Sprite_Icon;

        public User_Slot(int key, string id, string name, int level, bool master, bool ready, Team_ team, int[] wear_,int Icon)
        {
            index_key = key;
            ID = id;
            Nick_Name = name;
            Lv = level;
            Room_Master = master;
            Ready_State = ready;
            Team_Type = team;
            wear_item = wear_;
            Sprite_Icon = Icon;
        }
    }

    public struct Struct_User_Slot
    {
        public int[] Index_Key;
        public int length;
        public string[] ID;
        public string[] Nick_Name;
        public int[] Lv;
        public bool[] Room_Master;
        public bool[] Ready_State;
        public bool[] Team_Type;
        public int[][] wear_item;
        public int[] Icon;
    }

    public Struct_User_Slot Serialize(List<User_Slot> user_list) //List를 기본자료형으로 직렬화하여 보내기 위함.
    {
        Struct_User_Slot Struct_Slot = new Struct_User_Slot();
        Struct_Slot.Index_Key = new int[user_list.Count];
        Struct_Slot.ID = new string[user_list.Count];
        Struct_Slot.Nick_Name = new string[user_list.Count];
        Struct_Slot.Lv = new int[user_list.Count];
        Struct_Slot.Room_Master = new bool[user_list.Count];
        Struct_Slot.Ready_State = new bool[user_list.Count];
        Struct_Slot.Team_Type = new bool[user_list.Count];
        Struct_Slot.wear_item = new int[user_list.Count][];
        Struct_Slot.Icon = new int[user_list.Count];
        for (int i = 0; i < user_list.Count; i++)
        {
            Struct_Slot.wear_item[i] = new int[4];
        }

        for (int index = 0; index < user_list.Count; index++)
        {
            Struct_Slot.Index_Key[index] = user_list[index].index_key;
            Struct_Slot.ID[index] = user_list[index].ID;
            Struct_Slot.Nick_Name[index] = user_list[index].Nick_Name;
            Struct_Slot.Lv[index] = user_list[index].Lv;
            Struct_Slot.Room_Master[index] = user_list[index].Room_Master;
            Struct_Slot.Ready_State[index] = user_list[index].Ready_State;
            Struct_Slot.Icon[index] = user_list[index].Sprite_Icon;
            for (int i=0; i<4; i++)
            {
                Struct_Slot.wear_item[index][i] = user_list[index].wear_item[i];
            }
            if (user_list[index].Team_Type == Team_.One_Team)
            {
                Struct_Slot.Team_Type[index] = false;
            }
            else
            {
                Struct_Slot.Team_Type[index] = true;
            }
        }
        Struct_Slot.length = user_list.Count;
        return Struct_Slot;
    }

    public User_Slot DeSerialize(int key, string id, string nick, int lv, bool master, bool ready, bool team, int[] wear,int icon_) //받은 패킷들을 다시 조합해서 User_Slot 변환
    {
        User_Slot User_Serialize;

        if (team == false)
        {
            User_Serialize = new User_Slot(key, id, nick, lv, master, ready, Team_.One_Team, wear,icon_);
        }
        else
        {
            User_Serialize = new User_Slot(key, id, nick, lv, master, ready, Team_.Two_Team, wear, icon_);
        }
        return User_Serialize;
    }

    public User_Slot[] DeSerialize(int[] key, string[] id, string[] nick, int[] lv, bool[] master, bool[] ready, bool[] team, int length,int[][] wear, int[] icon) //받은 패킷들을 다시 조합해서 User_Slot[] 변환
    {
        User_Slot[] User_Serialize = new User_Slot[length];
        for (int index = 0; index < length; index++) {
            if (team[index] == false)
            {
                User_Serialize[index] = new User_Slot(key[index], id[index], nick[index], lv[index], master[index], ready[index], Team_.One_Team, wear[index], icon[index]);
            }
            else
            {
                User_Serialize[index] = new User_Slot(key[index], id[index], nick[index], lv[index], master[index], ready[index], Team_.Two_Team, wear[index], icon[index]);
            }
        }
        return User_Serialize;
    }

    [Serializable]
    public enum Team_ //1팀과 2팀
    {
        One_Team = 0,
        Two_Team = 1
    }

    public static List<User_Slot> Room_Join_User = new List<User_Slot>();

    [HideInInspector]
    public bool Ready_State = false; // 나 자신이 방장이 아닌 플레이어일때, 준비완료 버튼을 눌렀는지 체크, 방장일 경우 해당 변수는 항상 False 값을 갖는다.
    public bool Game_Start = false; // 모든플레이어가 준비완료이고 게임을 시작해도 되는지 체크

    [Header("UI Object")]
    public GameObject Team_InRoom;
    public GameObject Solo_InRoom;

    public UISprite Room_Map_Image;
    public UILabel Room_Title_Label;
    public UILabel Room_Info_Label;

    [Header("Solo_List Object")]
    public GameObject User_Team;

    [Header("Team_List Object")]
    public GameObject One_Team;
    public GameObject Two_Team;

    [Header("others")]
    public GameObject Start_Failed_Message;
    public UILabel Start_Btn_Text;

    [HideInInspector]
    public PhotonView Room_PV;

    public static int Present_Solo_Team_Count = 0;
    [HideInInspector]
    public int Solo_Maximum_Count = 12;

    public GameObject Character_Render_List; //캐릭터 랜더링 부분 관여하는 오브젝트.

    [HideInInspector]
    public RenderTexture[] Render_Character_Texture;

    public static int Present_One_Team_Count = 0; //현재 1팀에 있는 사람의 수 MAaximum_Value
    public static int Present_Two_Team_Count = 0; //현재 2팀에 있는 사람의 수 MAaximum_Value

    [HideInInspector]
    public int One_Team_Maximum_Count = 8;
    [HideInInspector]
    public int Two_Team_Maximum_Count = 8;
    public static int My_Slot_Index;
    public static Team_ My_Team;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        Start_Failed_Message.SetActive(false);
        this.gameObject.SetActive(false);
        Room_PV = this.gameObject.GetComponent<PhotonView>();
    }

    public void Open()
    {
        this.gameObject.SetActive(true);
        if (Manager.Instance.Room.mode == 0) //개인전
        {
            Team_InRoom.SetActive(false); //캔버스 표시
            Solo_InRoom.SetActive(true);
        }
        else //팀전
        {
            Team_InRoom.SetActive(true); //캔버스 표시
            Solo_InRoom.SetActive(false);
        }
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    public void Refresh_Join_UI() // 접속한 유저 목록을 바탕으로 화면상에서 다시한번 정렬해서 표시해줌.
    {
        //모드에 따른 방 설정
        Debug.Log(Manager.Instance.Room.mode + " <- 0은 솔로 1은 팀전으로 맵이 셋팅됨");
        Joined_Set();
        switch (Manager.Instance.Room.mode)
        {
            case 0:
                Solo_Maximum_Count = Manager.Instance.Room.One_Team_Count;
                int Solo_Number = 0; // For문에서 인덱스에 사용하기 위한 지역변수.
                int Solo_ready_count = 0; // 준비 상태가 몇명인지 체크
                Present_Solo_Team_Count = 0;// 
                My_Slot_Index = Find_index_In_Room();

                Reset_List_UI(); //화면상에 보이는 유저ui 리스트 초기화.

                for (int index = 0; index < Room_Join_User.Count; index++)
                {
                    if (Room_Join_User[index] != null) // 리스트중 비어있는 항목이 있을수도 있으므로 확인하기 위함.
                    {
                        User_Team.transform.GetChild(Solo_Number).GetChild(0).GetComponent<UILabel>().color = Color.white;
                        User_Team.transform.GetChild(Solo_Number).GetChild(0).GetComponent<UILabel>().text = Room_Join_User[index].Nick_Name;
                        User_Team.transform.GetChild(Solo_Number).GetChild(4).GetComponent<UILabel>().text = "Lv." + Room_Join_User[index].Lv.ToString();
                        User_Team.transform.GetChild(Solo_Number).GetChild(5).GetChild(0).GetComponent<UISprite>().spriteName = Room_Join_User[index].Sprite_Icon.ToString(); //아이콘

                        if (Room_Join_User[index].Room_Master == true) //방장일 경우 방장임을 표시해줌.
                        {
                            User_Team.transform.GetChild(Solo_Number).GetChild(0).GetComponent<UILabel>().color = Color.red;
                            User_Team.transform.GetChild(Solo_Number).GetChild(3).GetComponent<UISprite>().spriteName = "방장";
                        }
                        else
                        {
                            User_Team.transform.GetChild(Solo_Number).GetChild(3).GetComponent<UISprite>().spriteName = "강퇴";
                        }
                        
                        if (Room_Join_User[index].Ready_State == true) //일반유저인데 레디상태 일 경우
                            User_Team.transform.GetChild(Solo_Number).GetChild(1).GetComponent<UISprite>().spriteName = "준비완료_마크";
                        else  //일반유저인데 레디상태가 아닐경우
                            User_Team.transform.GetChild(Solo_Number).GetChild(1).GetComponent<UISprite>().spriteName = null;
                        User_Team.transform.GetChild(Solo_Number).gameObject.SetActive(true);
                        Solo_Number++;
                        Present_Solo_Team_Count++;

                        if (Room_Join_User[index].Ready_State == true)
                            ++Solo_ready_count;
                    }
                }

                for (int i = 0; i < Present_Solo_Team_Count; i++)
                {
                    if (i != My_Slot_Index)
                    {
                        Character_Render_List.transform.GetChild(i + 1).GetChild(0).GetComponent<Clothes_Manager>().Hair = Instantiate(Resources.Load<GameObject>("Custom\\Hair\\Hair_" + Room_Join_User[i].wear_item[0]), Vector3.zero, Quaternion.identity);
                        Character_Render_List.transform.GetChild(i + 1).GetChild(0).GetComponent<Clothes_Manager>().Top = Instantiate(Resources.Load<GameObject>("Custom\\Top\\Top_" + Room_Join_User[i].wear_item[1]), Vector3.zero, Quaternion.identity);
                        Character_Render_List.transform.GetChild(i + 1).GetChild(0).GetComponent<Clothes_Manager>().Pants = Instantiate(Resources.Load<GameObject>("Custom\\Pants\\Pants_" + Room_Join_User[i].wear_item[2]), Vector3.zero, Quaternion.identity);
                        Character_Render_List.transform.GetChild(i + 1).GetChild(0).GetComponent<Clothes_Manager>().Shoes = Instantiate(Resources.Load<GameObject>("Custom\\Shoes\\Shoes_" + Room_Join_User[i].wear_item[3]), Vector3.zero, Quaternion.identity);
                        Character_Render_List.transform.GetChild(i + 1).GetChild(0).GetComponent<Clothes_Manager>().Equipment_Set(Clothes_Manager.Part.Hair);
                        Character_Render_List.transform.GetChild(i + 1).GetChild(0).GetComponent<Clothes_Manager>().Equipment_Set(Clothes_Manager.Part.Top);
                        Character_Render_List.transform.GetChild(i + 1).GetChild(0).GetComponent<Clothes_Manager>().Equipment_Set(Clothes_Manager.Part.Pants);
                        Character_Render_List.transform.GetChild(i + 1).GetChild(0).GetComponent<Clothes_Manager>().Equipment_Set(Clothes_Manager.Part.Shoes);
                    }
                    else
                    {
                        Character_Render_List.transform.GetChild(0).GetChild(0).GetComponent<Clothes_Manager>().Hair = Instantiate(Resources.Load<GameObject>("Custom\\Hair\\Hair_" + Room_Join_User[i].wear_item[0]), Vector3.zero, Quaternion.identity);
                        Character_Render_List.transform.GetChild(0).GetChild(0).GetComponent<Clothes_Manager>().Top = Instantiate(Resources.Load<GameObject>("Custom\\Top\\Top_" + Room_Join_User[i].wear_item[1]), Vector3.zero, Quaternion.identity);
                        Character_Render_List.transform.GetChild(0).GetChild(0).GetComponent<Clothes_Manager>().Pants = Instantiate(Resources.Load<GameObject>("Custom\\Pants\\Pants_" + Room_Join_User[i].wear_item[2]), Vector3.zero, Quaternion.identity);
                        Character_Render_List.transform.GetChild(0).GetChild(0).GetComponent<Clothes_Manager>().Shoes = Instantiate(Resources.Load<GameObject>("Custom\\Shoes\\Shoes_" + Room_Join_User[i].wear_item[3]), Vector3.zero, Quaternion.identity);
                        Character_Render_List.transform.GetChild(0).GetChild(0).GetComponent<Clothes_Manager>().Equipment_Set(Clothes_Manager.Part.Hair);
                        Character_Render_List.transform.GetChild(0).GetChild(0).GetComponent<Clothes_Manager>().Equipment_Set(Clothes_Manager.Part.Top);
                        Character_Render_List.transform.GetChild(0).GetChild(0).GetComponent<Clothes_Manager>().Equipment_Set(Clothes_Manager.Part.Pants);
                        Character_Render_List.transform.GetChild(0).GetChild(0).GetComponent<Clothes_Manager>().Equipment_Set(Clothes_Manager.Part.Shoes);
                    }
                } //방에 들어온 유저들의 정보대로 해당 유저의 캐릭터 아바타를 그린다.

                Render_Character_Texture = Resources.LoadAll<RenderTexture>("Render_Image\\");

                for (int i = 0; i < Present_Solo_Team_Count; i++)
                {
                    if (i != My_Slot_Index)
                    {
                        User_Team.transform.GetChild(i).GetChild(2).GetComponent<UITexture>().mainTexture = Render_Character_Texture[i];
                    }
                    else
                    {
                        User_Team.transform.GetChild(i).GetChild(2).GetComponent<UITexture>().mainTexture = Render_Character_Texture[7];
                    }
                } // 그려진 아바타 정보를 불러와서 UI상에서 보여준다.

                if ((Solo_ready_count == (Room_Join_User.Count - 1)) && (Room_Join_User.Count != 1)) // 방에 접속해있는 유저가 전부 레디상태 일 경우 -1 하는이유는 방장을 제외한 모든 유저이므로.
                    Game_Start = true;
                else // 아닐 경우
                    Game_Start = false;

                Ready_Btn_Set();
                break;

            case 1:
                int One_Team_Number = 0;
                int Two_Team_Number = 0;
                Present_One_Team_Count = 0;
                Present_Two_Team_Count = 0;
                One_Team_Maximum_Count = Manager.Instance.Room.One_Team_Count;
                Two_Team_Maximum_Count = Manager.Instance.Room.Two_Team_Count;
                int ready_count = 0; // 준비 상태가 몇명인지 체크
                My_Slot_Index = Find_index_In_Room();

                Reset_List_UI(); //화면상에 보이는 유저ui 리스트 초기화.

                for (int index = 0; index < Room_Join_User.Count; index++)
                {
                    if (Room_Join_User[index] != null) // 리스트중 비어있는 항목이 있을수도 있으므로 확인하기 위함.
                    {
                        if (Room_Join_User[index].Team_Type == Team_.One_Team)
                        {
                            One_Team.transform.GetChild(One_Team_Number).GetChild(0).GetComponent<UILabel>().color = Color.white;
                            One_Team.transform.GetChild(One_Team_Number).GetChild(0).GetComponent<UILabel>().text = Room_Join_User[index].Nick_Name; //닉네임
                            One_Team.transform.GetChild(One_Team_Number).GetChild(4).GetComponent<UILabel>().text = "Lv." + Room_Join_User[index].Lv.ToString(); // 레벨
                            One_Team.transform.GetChild(One_Team_Number).GetChild(5).GetChild(0).GetComponent<UISprite>().spriteName = Room_Join_User[index].Sprite_Icon.ToString(); //아이콘
                            if (Room_Join_User[index].Room_Master == true) //방장일 경우 방장임을 표시해줌.
                            {
                                One_Team.transform.GetChild(One_Team_Number).GetChild(0).GetComponent<UILabel>().color = Color.red;
                                One_Team.transform.GetChild(One_Team_Number).GetChild(3).GetComponent<UISprite>().spriteName = "방장";
                                One_Team.transform.GetChild(One_Team_Number).GetChild(1).GetComponent<UISprite>().spriteName = null;
                            }
                            else
                            {
                                One_Team.transform.GetChild(One_Team_Number).GetChild(3).GetComponent<UISprite>().spriteName = "강퇴";
                                if (Room_Join_User[index].Ready_State == true) //일반유저인데 레디상태 일 경우
                                    One_Team.transform.GetChild(One_Team_Number).GetChild(1).GetComponent<UISprite>().spriteName = "준비완료_마크";
                                else  //일반유저인데 레디상태가 아닐경우
                                    One_Team.transform.GetChild(One_Team_Number).GetChild(1).GetComponent<UISprite>().spriteName = null;
                            }
                            One_Team.transform.GetChild(One_Team_Number).gameObject.SetActive(true);
                            One_Team_Number++;
                            Present_One_Team_Count++;
                        }
                        else
                        {
                            if (Room_Join_User[index].Team_Type == Team_.Two_Team)
                            {
                                Two_Team.transform.GetChild(Two_Team_Number).GetChild(0).GetComponent<UILabel>().color = Color.white;
                                Two_Team.transform.GetChild(Two_Team_Number).GetChild(0).GetComponent<UILabel>().text = Room_Join_User[index].Nick_Name; //닉네임
                                Two_Team.transform.GetChild(Two_Team_Number).GetChild(4).GetComponent<UILabel>().text = "Lv." + Room_Join_User[index].Lv.ToString(); //레벨
                                Two_Team.transform.GetChild(Two_Team_Number).GetChild(5).GetChild(0).GetComponent<UISprite>().spriteName = Room_Join_User[index].Sprite_Icon.ToString(); //아이콘
                                if (Room_Join_User[index].Room_Master == true) //방장일 경우 방장임을 표시해줌.
                                {
                                    Two_Team.transform.GetChild(Two_Team_Number).GetChild(0).GetComponent<UILabel>().color = Color.red;
                                    Two_Team.transform.GetChild(Two_Team_Number).GetChild(3).GetComponent<UISprite>().spriteName = "방장";
                                    Two_Team.transform.GetChild(Two_Team_Number).GetChild(1).GetComponent<UISprite>().spriteName = null;
                                }
                                else
                                {
                                    Two_Team.transform.GetChild(Two_Team_Number).GetChild(3).GetComponent<UISprite>().spriteName = "강퇴";
                                    if (Room_Join_User[index].Ready_State == true) //일반유저인데 레디상태 일 경우
                                        Two_Team.transform.GetChild(Two_Team_Number).GetChild(1).GetComponent<UISprite>().spriteName = "준비완료_마크";
                                    else  //일반유저인데 레디상태가 아닐경우
                                        Two_Team.transform.GetChild(Two_Team_Number).GetChild(1).GetComponent<UISprite>().spriteName = null;
                                }
                                Two_Team.transform.GetChild(Two_Team_Number).gameObject.SetActive(true);
                                Two_Team_Number++;
                                Present_Two_Team_Count++;
                            }
                        }
                        if (Room_Join_User[index].Ready_State == true)
                            ++ready_count;
                    }
                }
                
                for (int i = 0; i < Present_One_Team_Count+Present_Two_Team_Count; i++)
                {
                    if (i != My_Slot_Index)
                    {
                        Character_Render_List.transform.GetChild(i + 1).GetChild(0).GetComponent<Clothes_Manager>().Hair = Instantiate(Resources.Load<GameObject>("Custom\\Hair\\Hair_" + Room_Join_User[i].wear_item[0]), Vector3.zero, Quaternion.identity);
                        Character_Render_List.transform.GetChild(i + 1).GetChild(0).GetComponent<Clothes_Manager>().Top = Instantiate(Resources.Load<GameObject>("Custom\\Top\\Top_" + Room_Join_User[i].wear_item[1]), Vector3.zero, Quaternion.identity);
                        Character_Render_List.transform.GetChild(i + 1).GetChild(0).GetComponent<Clothes_Manager>().Pants = Instantiate(Resources.Load<GameObject>("Custom\\Pants\\Pants_" + Room_Join_User[i].wear_item[2]), Vector3.zero, Quaternion.identity);
                        Character_Render_List.transform.GetChild(i + 1).GetChild(0).GetComponent<Clothes_Manager>().Shoes = Instantiate(Resources.Load<GameObject>("Custom\\Shoes\\Shoes_" + Room_Join_User[i].wear_item[3]), Vector3.zero, Quaternion.identity);
                        Character_Render_List.transform.GetChild(i + 1).GetChild(0).GetComponent<Clothes_Manager>().Equipment_Set(Clothes_Manager.Part.Hair);
                        Character_Render_List.transform.GetChild(i + 1).GetChild(0).GetComponent<Clothes_Manager>().Equipment_Set(Clothes_Manager.Part.Top);
                        Character_Render_List.transform.GetChild(i + 1).GetChild(0).GetComponent<Clothes_Manager>().Equipment_Set(Clothes_Manager.Part.Pants);
                        Character_Render_List.transform.GetChild(i + 1).GetChild(0).GetComponent<Clothes_Manager>().Equipment_Set(Clothes_Manager.Part.Shoes);
                    }
                    else
                    {
                        Character_Render_List.transform.GetChild(0).GetChild(0).GetComponent<Clothes_Manager>().Hair = Instantiate(Resources.Load<GameObject>("Custom\\Hair\\Hair_" + Room_Join_User[i].wear_item[0]), Vector3.zero, Quaternion.identity);
                        Character_Render_List.transform.GetChild(0).GetChild(0).GetComponent<Clothes_Manager>().Top = Instantiate(Resources.Load<GameObject>("Custom\\Top\\Top_" + Room_Join_User[i].wear_item[1]), Vector3.zero, Quaternion.identity);
                        Character_Render_List.transform.GetChild(0).GetChild(0).GetComponent<Clothes_Manager>().Pants = Instantiate(Resources.Load<GameObject>("Custom\\Pants\\Pants_" + Room_Join_User[i].wear_item[2]), Vector3.zero, Quaternion.identity);
                        Character_Render_List.transform.GetChild(0).GetChild(0).GetComponent<Clothes_Manager>().Shoes = Instantiate(Resources.Load<GameObject>("Custom\\Shoes\\Shoes_" + Room_Join_User[i].wear_item[3]), Vector3.zero, Quaternion.identity);
                        Character_Render_List.transform.GetChild(0).GetChild(0).GetComponent<Clothes_Manager>().Equipment_Set(Clothes_Manager.Part.Hair);
                        Character_Render_List.transform.GetChild(0).GetChild(0).GetComponent<Clothes_Manager>().Equipment_Set(Clothes_Manager.Part.Top);
                        Character_Render_List.transform.GetChild(0).GetChild(0).GetComponent<Clothes_Manager>().Equipment_Set(Clothes_Manager.Part.Pants);
                        Character_Render_List.transform.GetChild(0).GetChild(0).GetComponent<Clothes_Manager>().Equipment_Set(Clothes_Manager.Part.Shoes);
                    }
                }//방에 들어온 유저들의 정보대로 해당 유저의 캐릭터 아바타를 그린다.

                Render_Character_Texture = Resources.LoadAll<RenderTexture>("Render_Image\\");

                for (int i = 0; i < Present_One_Team_Count + Present_Two_Team_Count; i++)
                {
                    if (Room_Join_User[i].Team_Type == Team_.One_Team)
                    {
                        if (i != My_Slot_Index)
                        {
                            One_Team.transform.GetChild(i).GetChild(2).GetComponent<UITexture>().mainTexture = Render_Character_Texture[i];
                        }
                        else
                        {
                            One_Team.transform.GetChild(i).GetChild(2).GetComponent<UITexture>().mainTexture = Render_Character_Texture[7];
                        }
                    }
                    else
                    {
                        if (i != My_Slot_Index)
                        {
                            Two_Team.transform.GetChild(i).GetChild(2).GetComponent<UITexture>().mainTexture = Render_Character_Texture[i];
                        }
                        else
                        {
                            Two_Team.transform.GetChild(i).GetChild(2).GetComponent<UITexture>().mainTexture = Render_Character_Texture[7];
                        }
                    }
                } // 그려진 아바타 정보를 불러와서 UI상에서 보여준다.

                if ((ready_count == (Room_Join_User.Count - 1)) && (Room_Join_User.Count != 1)) // 방에 접속해있는 유저가 전부 레디상태 일 경우 -1 하는이유는 방장을 제외한 모든 유저이므로.
                    Game_Start = true;
                else // 아닐 경우
                    Game_Start = false;

                Ready_Btn_Set();
                break;

            default:
                Debug.Log("모드 부분 참조 에러 발생");
                break;
        }
    }


    void Reset_List_UI()
    {
        switch (Manager.Instance.Room.mode)
        {
            case 0:
                for (int index = 0; index < Solo_Maximum_Count; index++)
                {
                    User_Team.transform.GetChild(index).gameObject.SetActive(false);
                }
                    break;

            case 1:
                for (int index = 0; index < One_Team_Maximum_Count; index++)
                {
                    One_Team.transform.GetChild(index).gameObject.SetActive(false);
                    Two_Team.transform.GetChild(index).gameObject.SetActive(false);
                }
                break;
        }
    }

    public int Find_index_In_Room() //룸 안에 나와 일치하는 명단이 없을 경우 0 값을 반환한다.
    {
        for (int index = 0; index < Room_Join_User.Count; index++)
        {
            if (Room_Join_User[index].index_key == PhotonNetwork.player.ID)
            {
                return index;
            }
        }
        return 999;
    }

    bool Join_User_Find_Delate()
    {
        for (int index = 0; index < Room_Join_User.Count; index++)
        {
            if (Room_Join_User[index].index_key == PhotonNetwork.player.ID)
            {
                return true; // 확인만하고 반환. 어차피 나가는 클라이언트는 리스트 수정하나마나 이므로.!
            }
        }
        return false;
    }

    public void Change_Room_Master()
    {
        int index = Find_index_In_Room();
        int next_root; // 다음 방장을 이어받을 배열의 위치를 저장
        if ((index + 1) >= Room_Join_User.Count) //내가 배열의 마지막번호일 경우 0번 아닐경우 나 다음의 번호에게 방장의 권한을 준다.
            next_root = 0;
        else
        {
            next_root = index + 1;
        }
        PhotonNetwork.SetMasterClient(PhotonPlayer.Find(Room_Join_User[next_root].index_key));
    }

    public void Calling_Notify_Master(int photon_ID, string Slot_ID, string Nick_Name, int Lv, bool Room_Master, bool Ready_State, bool type, int[] wear, int icon)
    {
        Room_PV.RPC("Join_Notify_Master", PhotonTargets.MasterClient, photon_ID, Slot_ID, Nick_Name, Lv, Room_Master, Ready_State, type, wear, icon);
    }

    public void Out_Notify_Other()
    {

        // RPC로 현재 룸에있는 유저들의 정보를 요구하고 받아서 누가 나갔는지 파악하고 그에 따른 UI새로 그리는 함수 호출

        Photon_Manager.Instance.OnJoinedRoom(); //패킷으로 받을 경우 자신보다 다른 클라이언트가 패킷을 더 빨리 전달받아서 꼬이는 것을 막고자 직접 호출.
        Room_PV.RPC("Call_Room_ReJoin", PhotonTargets.Others);
        
        /* 삭제해도되는 부분 위에 것이 정상 작동되면..
        int index = Find_index_In_Room();
        if (PhotonNetwork.player.IsMasterClient == true) //나갈시에 내가 방장인경우
        {
            Change_Room_Master();
            Room_PV.RPC("Check_Room_Master", PhotonTargets.Others);
            if (Join_User_Find_Delate())
            {
                Room_PV.RPC("Refresh_Out_User", PhotonTargets.All, index); //나간 유저를 다른유저들에게 알려버림!.
                return true;
            }
        }
        if (Join_User_Find_Delate()) {
            Room_PV.RPC("Refresh_Out_User", PhotonTargets.All, index); //나간 유저를 다른유저들에게 알려버림!.
            return true;
        }
        return false;*/
    }

    public void Solo_Kick_Button(string Slot)
    {
        int Slot_Index = 0;

        for (int i=0; i < User_Team.transform.childCount; i++)
        {
            if (User_Team.transform.GetChild(i).gameObject.name == Slot)
                Slot_Index = i;
        }

        if (PhotonNetwork.isMasterClient)
        {
            Room_PV.RPC("RoomMaster_Kick_Recive", PhotonTargets.Others, Room_Join_User[Slot_Index].Nick_Name);
        }
    }

    public void One_Team_Kick_Button(string Slot)
    {
        int Slot_Index = 0;

        for (int i = 0; i < One_Team.transform.childCount; i++)
        {
            if (One_Team.transform.GetChild(i).gameObject.name == Slot)
                Slot_Index = i;
        }

        if (PhotonNetwork.isMasterClient)
        {
            Room_PV.RPC("RoomMaster_Kick_Recive", PhotonTargets.Others, Room_Join_User[Slot_Index].Nick_Name);
        }
    }

    public void Two_Team_Kick_Button(string Slot)
    {
        int Slot_Index = 0;

        for (int i = 0; i < Two_Team.transform.childCount; i++)
        {
            if (Two_Team.transform.GetChild(i).gameObject.name == Slot)
                Slot_Index = i;
        }

        if (PhotonNetwork.isMasterClient)
        {
            Room_PV.RPC("RoomMaster_Kick_Recive", PhotonTargets.Others, Room_Join_User[Slot_Index].Nick_Name);
        }
    }

    [PunRPC]
    void RoomMaster_Kick_Recive(string Target) // 유저 강퇴 처리
    {
        if (!PhotonNetwork.isMasterClient) //방장이 아닐경우 방장이 자기자신의 버튼을 눌렀을 경우를 막기위함.
        {
            if (Target == Manager.Instance.User.Nick_Name)
            {
                Back_Lobby_Btn();
            }
        }
    }

    [PunRPC]
    void Call_InRoom_ReJoin()
    {
        Photon_Manager.Instance.OnJoinedRoom();
    }
    
    [PunRPC]
    public void Join_Notify_Master(int key, string id, string nick_name, int lv, bool master, bool ready, bool type, int[] wear_item, int icon) // 룸내의 마스터만 사용하는 메소드 알림.
    {
        User_Slot user = DeSerialize(key, id, nick_name, lv, master, ready, type, wear_item, icon);
        if (Present_One_Team_Count >= One_Team_Maximum_Count) //1팀
        {
            user.Team_Type = Team_.Two_Team;
        }

        if (Present_Two_Team_Count >= Two_Team_Maximum_Count) //2팀
        {
            user.Team_Type = Team_.One_Team;
        }

        Room_Join_User.Add(user);
        Refresh_Join_UI();
        Struct_User_Slot Packet_List = Serialize(Room_Join_User);
        Room_PV.RPC("Refresh_Join_User_List", PhotonTargets.All, Packet_List.Index_Key, Packet_List.ID, Packet_List.Nick_Name, Packet_List.Lv, Packet_List.Room_Master, Packet_List.Ready_State, Packet_List.Team_Type, Packet_List.length, One_Team_Maximum_Count, Two_Team_Maximum_Count, Manager.Instance.Room.map, Packet_List.wear_item, Packet_List.Icon); //접속한 유저를 다른유저들에게 알려버림!.
    }

    [PunRPC]
    public void Calling_Refersh_Room_Ui() // 룸내의 유저리스트 화면을 초기화하는 메소드 호출.
    {
        Refresh_Join_UI();
    }
    
    [PunRPC]
    public void Refresh_Join_User_List(int[] key, string[] id, string[] nick, int[] lv, bool[] master, bool[] ready, bool[] type, int length, int h_maximum, int r_maximum, int map, int[][] wear, int[] icon) // 방에 접속한 모든 인물에게 알림.
    {
        Manager.Instance.Room.One_Team_Count = h_maximum;
        Manager.Instance.Room.Two_Team_Count = r_maximum;
        Manager.Instance.Room.map = map;
        Room_Join_User.Clear();
        Room_Join_User.AddRange(DeSerialize(key, id, nick, lv, master, ready, type, length, wear,icon));
        Refresh_Join_UI(); //바뀐 데이터를 바탕으로 화면상에 출력해줌.
    }

    [PunRPC]
    public void Refresh_User_Ready(int index, bool ready)
    {
        Room_Join_User[index].Ready_State = ready;
        Calling_Refersh_Room_Ui();
    }

    [PunRPC]
    public void Refresh_User_Team(int index, bool team)
    {
        if (team == false)
        {
            Room_Join_User[index].Team_Type = Team_.One_Team;
        }
        else
        {
            Room_Join_User[index].Team_Type = Team_.Two_Team;
        }
        Calling_Refersh_Room_Ui();
    }

    [PunRPC]
    public void Refresh_Out_User(int index)
    {
        Room_Join_User.RemoveAt(index);
        Calling_Refersh_Room_Ui();
    }
    
    [PunRPC]
    public void Refresh_Room_Master(int index)
    {
        Room_Join_User[index].Room_Master = true;
        Calling_Refersh_Room_Ui();
    }

    [PunRPC]
    public void Check_Room_Master()
    {
        int index = Find_index_In_Room();
        if (PhotonNetwork.player.IsMasterClient == true)
        {
            Debug.Log("내가 룸마스터가 되었습니다.");
            Room_Join_User[index].Room_Master = true;
            Room_PV.RPC("Refresh_Room_Master", PhotonTargets.Others, index);
        }
    }

    [PunRPC]
    public void Call_Game_Start(string name, int map,int mode, int time, int h_count, int r_count)
    {
        PhotonNetwork.room.IsOpen = false;
        PhotonNetwork.room.IsVisible = false;
        Manager.Instance.Room.room_name = name;
        Manager.Instance.Room.map = map;
        Manager.Instance.Room.mode = mode;
        Manager.Instance.Room.time = time;
        Manager.Instance.Room.One_Team_Count = h_count;
        Manager.Instance.Room.Two_Team_Count = r_count;
        My_Team = Room_Join_User[My_Slot_Index].Team_Type;
        PhotonNetwork.player.UserId = Manager.Instance.User.ID;
        PhotonNetwork.player.NickName = Manager.Instance.User.Nick_Name;
        switch (Manager.Instance.Room.map)
        {
            case 0:
                Manager.Instance.Scene_name = "City";
                break;
        }
        SceneManager.LoadScene("City");
    }

    public void Moved_One_Team_Btn()
    {
        int index = Find_index_In_Room();
        if (Room_Join_User[index].Team_Type != Team_.One_Team && Present_One_Team_Count < One_Team_Maximum_Count)
        {
            Room_Join_User[index].Team_Type = Team_.One_Team;
            Refresh_Join_UI();
            Room_PV.RPC("Refresh_User_Team", PhotonTargets.Others, index, false); //다른유저들에게 알려버림!.}
        }
    }

    public void Moved_Two_Team_Btn()
    {
        int index = Find_index_In_Room();
        if (Room_Join_User[index].Team_Type != Team_.Two_Team && Present_Two_Team_Count < Two_Team_Maximum_Count)
        {
            Room_Join_User[index].Team_Type = Team_.Two_Team;
            Refresh_Join_UI();
            Room_PV.RPC("Refresh_User_Team", PhotonTargets.Others, index, true); //다른유저들에게 알려버림!.
        }
    }

    public void Ready_Btn_Set()
    {
        if (PhotonNetwork.player.IsMasterClient == false && Ready_State == true) //준비 완료 -> 준비 해제
        {
            Start_Btn_Text.color = Color.gray;
            Start_Btn_Text.text = Localization.Get("Ready");
        }
        else if (PhotonNetwork.player.IsMasterClient == false && Ready_State == false) //준비 해제 -> 준비 완료
        {
            Start_Btn_Text.color = Color.white;
            Start_Btn_Text.text = Localization.Get("Ready");
        }

        if (PhotonNetwork.player.IsMasterClient == true && Game_Start == true)
        {
            Start_Btn_Text.color = Color.red;
            Start_Btn_Text.text = Localization.Get("Start");
        }
        else if (PhotonNetwork.player.IsMasterClient == true && Game_Start == false)
        {
            Start_Btn_Text.color = Color.gray;
            Start_Btn_Text.text = Localization.Get("Start");
        }
    }

    public void Back_Lobby_Btn() //로비로 되돌아감.
    {
        Start_Failed_Message.SetActive(false);
        Chat_Proccess.Room_Chat_List.Clear();
        Ready_State = false;
        Game_Start = false;
        Photon_Manager.Instance.Join_Robby();
        Lobby.Instance.Lobby_UI_Show_UI();
    }

    public void Game_Start_Btn()
    {
        int index = Find_index_In_Room();
        //내가 방장이 아닐떄 처리항목
        if (PhotonNetwork.player.IsMasterClient == false && Ready_State == false)
        {
            Ready_State = true;
            Ready_Btn_Set();
            Room_Join_User[index].Ready_State = true;
            Calling_Refersh_Room_Ui();
            Room_PV.RPC("Refresh_User_Ready", PhotonTargets.Others, index, true); //접속한 유저를 다른유저들에게 알려버림!.
            //내가 준비완료 상태임을 다른 플레이어들에게 알림 처리할것
        }
        else if (PhotonNetwork.player.IsMasterClient == false && Ready_State == true)
        {
            Ready_State = false;
            Ready_Btn_Set();
            Room_Join_User[index].Ready_State = false;
            Calling_Refersh_Room_Ui();
            Room_PV.RPC("Refresh_User_Ready", PhotonTargets.Others, index, false);
        }

        //내가 방장일때 처리 항목
        if (Game_Start == true && PhotonNetwork.player.IsMasterClient == true)
        {
            Room_PV.RPC("Call_Game_Start", PhotonTargets.All, Manager.Instance.Room.room_name, Manager.Instance.Room.map, Manager.Instance.Room.mode,Manager.Instance.Room.time, Manager.Instance.Room.One_Team_Count, Manager.Instance.Room.Two_Team_Count);
        }
        else if (Game_Start == false && PhotonNetwork.player.IsMasterClient == true)
        {
            Start_Failed_Message.SetActive(true);
            Start_Failed_Message.GetComponent<UILabel>().text = "준비되지 않은 플레이어가 존재합니다.";
            if ((Present_One_Team_Count == 0 || Present_Two_Team_Count == 0) && (Manager.Instance.Room.mode == 1))
            {
                Start_Failed_Message.GetComponent<UILabel>().text = "각 팀에 한명 이상은 존재해야 합니다.";
            }
            if (Room_Join_User.Count <= 1)
            {
                Start_Failed_Message.GetComponent<UILabel>().text = "최소한 2명 이상의 플레이어가 존재해야 합니다.";
            }
        }
    }

    public void Joined_Set()
    {
        Lobby.Instance.WaitingGameRoom_UI_Show_UI();

        Room_Title_Label.text = Manager.Instance.Room.room_name;
        if (PhotonNetwork.isMasterClient)
            Start_Btn_Text.text = Localization.Get("Start");
        else
            Start_Btn_Text.text = Localization.Get("Ready");

        switch (Manager.Instance.Room.map)
        {
            case 0:
                Room_Map_Image.spriteName = "도시";
                Room_Info_Label.text = Localization.Get("Map") + " : " + Localization.Get("City") + "\n" + Localization.Get("Time limit") + " : " + Manager.Instance.Room.time + " " + Localization.Get("Minute");
                break;

            default:
                Room_Map_Image.spriteName = "도시";
                Room_Info_Label.text = Localization.Get("Map") + " : " + Localization.Get("City") + "\n" + Localization.Get("Time limit") + " : " + Manager.Instance.Room.time + " " + Localization.Get("Minute");
                break;
        }
    }
}
