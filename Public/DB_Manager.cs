using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.SceneManagement;

public class DB_Manager : MonoBehaviour {

    public static DB_Manager Instance;

    //로그인 관련 php
    const string LoginUrl = "http://AbilitHero/Login.php";
    const string CreateUrl = "http://AbilityHero/CreateUser.php";
    const string Create_Item_DB_Url = "http://AbilityHero/CreateUserItemDB.php";
    const string RankUrl = "http://iop1063.cafe24.com/AbilityHero/Ranking_Server.php";

    //아이템 관련 php
    const string GetItem_ListUrl = "http://AbilityHero/Get_Item_List.php";
    const string GetIcon_ListUrl = "http:///AbilityHero/Get_Icon_List.php";
    const string GetAbility_ListUrl = "http://AbilityHero/Get_Ability_List.php";
    const string Change_Wear_DB_Url = "http://AbilityHero/ChangeWearDB.php";

    //구매 관련 php
    const string Buy_Item_Url = "http://AbilityHero/Add_Item.php";
    const string Buy_Upgrade_Ability_Url = "http://AbilityHero/Ability_Upgrade.php";
    const string SpendCrystal = "http://AbilityHero/SpendStone.php";
    const string SpendMoney = "http://AbilityHero/SpendMoney.php";
    const string Add_Gold_Url = "http://AbilityHero/Add_Gold.php";
    const string Add_Stone_Url = "http://AbilityHero/Add_Stone.php";
    
    //로그 기록 관련 php
    const string Get_LogUrl = "http://AbilityHero/Get_Log.php";

    bool Overlap_Check = true;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
    }

    public IEnumerator Referesh_DB()
    {
        string id = Manager.Instance.User.ID;
        string pw = Manager.Instance.User.PW;
        string plat = Manager.Instance.User.User_Platform;
        WWWForm form = new WWWForm();
        form.AddField("input_id", id);
        form.AddField("input_pw", pw);
        form.AddField("input_plat", plat);

        WWW webRequest = new WWW(LoginUrl, form);
        yield return webRequest;
        string Request = webRequest.text.Trim();  // Trim사용 이유는 웹에서 받아올때 보이지 않는 공백이 첨부되므로.
        string[] packet = Request.Split('/');
        if (packet[0] == "Success") // 성공여부,아이디,비밀번호,닉네임,레벨,경험치,돈,아이템리스트,친구목록 순서 \n 기준으로 으로 반환됨
        {
            Manager.Instance.User.Key = packet[1];
            Manager.Instance.User.Nick_Name = packet[2];
            Manager.Instance.User.Lv = System.Int32.Parse(packet[3]);
            Manager.Instance.User.Exp = System.Int32.Parse(packet[4]);
            Manager.Instance.User.Money = System.Int32.Parse(packet[5]);
            Manager.Instance.User.Friends = packet[6];
            Manager.Instance.User.country = packet[7];
            System.Int32.TryParse(packet[8], out Manager.Instance.Wear_Item[0]);
            System.Int32.TryParse(packet[9], out Manager.Instance.Wear_Item[1]);
            System.Int32.TryParse(packet[10], out Manager.Instance.Wear_Item[2]);
            System.Int32.TryParse(packet[11], out Manager.Instance.Wear_Item[3]);
            Debug.Log("DB로부터 정보를 새로고침하였습니다.");
        }
        else
        {
            Debug.Log("DB와 연결을 실패하였습니다.");
        }
    }

    public IEnumerator Load_Data_DB()
    {
        string id = Manager.Instance.User.ID;
        string pw = Manager.Instance.User.PW;
        string platform = Manager.Instance.User.User_Platform;
        WWWForm form = new WWWForm();
        form.AddField("input_id", id);
        form.AddField("input_pw", pw);
        form.AddField("input_plat", platform);

        WWW webRequest = new WWW(LoginUrl, form);
        yield return webRequest;
        string Request = webRequest.text.Trim();  // Trim사용 이유는 웹에서 받아올때 보이지 않는 공백이 첨부되므로.
        string[] packet = Request.Split('/');
        if (packet[0] == "Success") // 성공여부,아이디,비밀번호,닉네임,레벨,경험치,돈,아이템리스트,친구목록 순서 \n 기준으로 으로 반환됨
        {
            Manager.Instance.User.Key = packet[1];
            Manager.Instance.User.Nick_Name = packet[2];
            Manager.Instance.User.Lv = System.Int32.Parse(packet[3]);
            Manager.Instance.User.Exp = System.Int32.Parse(packet[4]);
            Manager.Instance.User.Money = System.Int32.Parse(packet[5]);
            Manager.Instance.User.Soul_Stone = System.Int32.Parse(packet[6]);

            Manager.Instance.IsSuspension = System.Int32.Parse(packet[7]); //정지여부 판별

            Manager.Instance.User.Friends = packet[8];
            Manager.Instance.User.Icon = System.Int32.Parse(packet[9]);

            Manager.Instance.Wear_Item = new int[4];
            System.Int32.TryParse(packet[10], out Manager.Instance.Wear_Item[0]);
            System.Int32.TryParse(packet[11], out Manager.Instance.Wear_Item[1]);
            System.Int32.TryParse(packet[12], out Manager.Instance.Wear_Item[2]);
            System.Int32.TryParse(packet[13], out Manager.Instance.Wear_Item[3]);

            PhotonNetwork.player.NickName = Manager.Instance.User.Nick_Name;

            Manager.Instance.InOut_Check = true;

            //내가 이미 접속중인지 아닌지를 파악하기 위함.
            PhotonNetwork.playerName = Manager.Instance.User.Nick_Name;
            string[] Friends = new string[1] { Manager.Instance.User.Nick_Name };
            PhotonNetwork.FindFriends(Friends);

            if (Manager.Instance.User.Lv == 1)
                Manager.Instance.User.Exp_Maximum = 100;
            else
                Manager.Instance.User.Exp_Maximum = (Manager.Instance.User.Lv * 100) + ((Manager.Instance.User.Lv - 1) * 100);

            Overlap_Check = true;
            yield return Overlap_Corutine(); // 중복검사 결과 코루틴 : FindFriends의 경우 Update 문으로 처리하기때문에 딜레이발생 결국 이를 해결하기위함.

            Debug.LogWarning("Json 자동로그인 추후 다시구현하세요!");
            //JsonManager.JsonManager_Script.Info_Write(); //로그인한 기록을 Json으로 저장 추후 자동로그인을 위함.
            StartCoroutine(DB_Manager.Instance.Get_Item_List_DB());
            StartCoroutine(DB_Manager.Instance.Get_Ability_List_DB());
            StartCoroutine(DB_Manager.Instance.Get_Icon_List_DB());

            if (Manager.Instance.InOut_Check == true && Manager.Instance.IsSuspension == 0 && Overlap_Check == false) //이미 있는 아이디로 파악되어 로그인 성공처리
            {
                Manager.Instance.Scene_name = "Lobby";
                SceneManager.LoadScene("Loading");
            }

            if (Manager.Instance.IsSuspension != 0)
            {
                //계정 정지상태로 파악하고 이를 알림
                Manager.Instance.Error_Print("현재 계정은 게임 규정 위반으로 인해 정지된 상태입니다. \n 자세한 사항은 문의해주시기 바랍니다.");
            }

            if (Overlap_Check == true) 
            {
                //이미 접속한 상태로 파악하고 이를 알림
                Manager.Instance.Error_Print("현재 게임에 접속중인 상태입니다. \n 잠시후 다시 시도해주세요. \n 반복될 경우 문의해주시기 바랍니다.");
            }
        }
        else
        {
            Manager.Instance.Error_Print("로그인에 실패하였습니다.");
            Manager.Instance.InOut_Check = false;
            if (Manager.Instance.User.User_Platform != "FineStudio")
            {
                //StartCoroutine(Social_Manager.Instance.Call_Create_Login());
            }
        }
    }

IEnumerator Overlap_Corutine()
    {
        int access_count = 0;
        
        if (PhotonNetwork.Friends != null)
        {
            foreach (FriendInfo info in PhotonNetwork.Friends)
            {
                if (info.IsOnline == true)
                    access_count++;
            }
            
            if (access_count == 0)
            {
                Overlap_Check = false;
            }
            else
            {
                Overlap_Check = true;
            }

            Debug.Log(PhotonNetwork.Friends.ToArray().ToStringFull());
            Debug.Log(access_count + "명이 접속.");

            StopCoroutine(Overlap_Corutine());
            yield return null;
        }
        else
        {
            yield return new WaitForSeconds(1.0f);
            StartCoroutine("Overlap_Corutine");
        }
    }

    public IEnumerator Create_DB()
    {
        string id = Manager.Instance.User.ID;
        string pw = Manager.Instance.User.PW;
        string name = Manager.Instance.User.Nick_Name;
        string plat = Manager.Instance.User.User_Platform;
        string country = Manager.Instance.User.country; // country 설정은 Localize_manager.cs에서 처리.
        WWWForm form = new WWWForm();
        form.AddField("input_id", id);
        form.AddField("input_pw", pw);
        form.AddField("input_name", name);
        form.AddField("input_plat", plat);
        form.AddField("input_country", country);

        WWW webRequest = new WWW(CreateUrl, form);

        yield return webRequest;

        string packet = webRequest.text.Trim(); // Trim사용 이유는 웹에서 받아올때 보이지 않는 공백이 첨부되므로.
        switch (packet)
        {
            case "Success":
                Manager.Instance.Error_Print("회원가입에 성공하였습니다.");
                StartCoroutine(Load_Data_DB());
                StartCoroutine(Create_Inven_DB());
                break;

            case "Fail_id":
                Manager.Instance.Error_Print("생성할 수 없는 아이디 입니다.");
                break;

            case "Fail_name":
                Manager.Instance.Error_Print("생성할수 없는 닉네임 입니다.");
                break;

            case "Failed_Create":
                Manager.Instance.Error_Print("회원가입 중 오류가 발생하였습니다.");
                break;

            default:
                Manager.Instance.Error_Print(packet + " - 알수없는 오류가 발생하였습니다.");
                break;

        }
    }

    public IEnumerator Create_Inven_DB()
    {
        WWWForm form = new WWWForm();

        form.AddField("input_id", Manager.Instance.User.Key);

        WWW webRequest = new WWW(Create_Item_DB_Url, form);

        yield return webRequest;

        string packet = webRequest.text.Trim(); // Trim사용 이유는 웹에서 받아올때 보이지 않는 공백이 첨부되므로.
        switch (packet)
        {
            case "Success":
                break;

            case "Fail":
                break;

            default:
                Debug.LogError("﻿Success" == packet.ToString());
                break;

        }
    }

    public IEnumerator Change_Wear_Item()
    {
        string id = Manager.Instance.User.Key;
        string wear_head = Manager.Instance.Wear_Item[0].ToString();
        string wear_top = Manager.Instance.Wear_Item[1].ToString();
        string wear_pant = Manager.Instance.Wear_Item[2].ToString();
        string wear_shoes = Manager.Instance.Wear_Item[3].ToString();

        WWWForm form = new WWWForm();

        form.AddField("input_id", Manager.Instance.User.Key);
        form.AddField("wear_head", wear_head);
        form.AddField("wear_top", wear_top);
        form.AddField("wear_pant", wear_pant);
        form.AddField("wear_shoes", wear_shoes);

        WWW webRequest = new WWW(Change_Wear_DB_Url, form);

        yield return webRequest;
    }

    //랭크를 가져옴
    public IEnumerator Ranking_DB()
    {
        WWWForm form = new WWWForm();
        form.AddField("country", Manager.Instance.User.country);

        WWW webRequest = new WWW(RankUrl, form);
        yield return webRequest;
        string Request = webRequest.text.Trim();  // Trim사용 이유는 웹에서 받아올때 보이지 않는 공백이 첨부되므로.
        string[] packet = Request.Split('/');

        if (packet[0] == "Success")
        {
            string[] rank_data = new string[packet.Length - 2]; // 메모리 동적 할당
            Manager.Instance.Rank_List_Name = new string[packet.Length - 2];
            Manager.Instance.Rank_List_Lv = new string[packet.Length - 2];
            Manager.Instance.Rank_List_play = new string[packet.Length - 2];
            for (int index = 0; index < packet.Length - 3; index++)
            {
                rank_data[index] = packet[index + 2];
            }
            for (int count = 0; count < rank_data.Length - 1; count++)
            {
                string[] split_rank = rank_data[count].Split('!');
                Manager.Instance.Rank_List_Name[count] = split_rank[0];
                Manager.Instance.Rank_List_Lv[count] = "Lv." + split_rank[1];

                int play_count = 0;
                int win_count = 0;
                int lose_count;
                System.Int32.TryParse(split_rank[2], out play_count);
                System.Int32.TryParse(split_rank[3], out win_count);
                lose_count = play_count - win_count;
                Manager.Instance.Rank_List_play[count] = win_count + "승 / " + lose_count + "패 / " + play_count + "전";
            }
            Rank_Canvas.Instance.Refresh_Ranking();
            Debug.Log("랭크기록을 성공적으로 불러왔습니다.");
            yield return true;
        }
        else
        {
            Debug.Log("랭크기록을 불러오는데 실패하였습니다.");
        }
    }

    //아이템 리스트를 가져옴
    public IEnumerator Get_Item_List_DB()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", Manager.Instance.User.Key);
        WWW webRequest = new WWW(GetItem_ListUrl, form);
        yield return webRequest;
        string Request = webRequest.text.Trim();  // Trim사용 이유는 웹에서 받아올때 보이지 않는 공백이 첨부되므로.
        string[] data_pack = Request.Split('!');
        Debug.Log(Request.ToString());
        if (data_pack[0] == "Success")
        {
            int H_index = 0, T_index = 0, P_index = 0, S_index = 0;
            for (int i = 1; i < data_pack.Length; i++)
            {
                if (data_pack[i] != null)
                {
                    string[] packet = data_pack[i].Split('/');

                    int key, count;
                    switch (packet[0]) //무슨 아이템부위인지 판별
                    {
                        case "Head":
                            System.Int32.TryParse(packet[1], out key);
                            System.Int32.TryParse(packet[3], out count);
                            Manager.Instance.Item_Head_List.Add(new Manager.Item_Info(key, "Hair", count, packet[2]));
                            H_index++;
                            break;

                        case "Top":
                            System.Int32.TryParse(packet[1], out key);
                            System.Int32.TryParse(packet[3], out count);
                            Manager.Instance.Item_Top_List.Add(new Manager.Item_Info(key, "Top", count, packet[2]));
                            T_index++;
                            break;

                        case "Pant":
                            System.Int32.TryParse(packet[1], out key);
                            System.Int32.TryParse(packet[3], out count);
                            Manager.Instance.Item_Pants_List.Add(new Manager.Item_Info(key, "Pant", count, packet[2]));
                            P_index++;
                            break;

                        case "Shoes":
                            System.Int32.TryParse(packet[1], out key);
                            System.Int32.TryParse(packet[3], out count);
                            Manager.Instance.Item_Shoes_List.Add(new Manager.Item_Info(key, "Shoes", count, packet[2]));
                            S_index++;
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    Debug.Log("보유 아이템 기록을 불러오는데 실패하였습니다.");
                }
            }
            yield return true;
        }
    }

    //아이콘 리스트를 가져옴
    public IEnumerator Get_Icon_List_DB()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", Manager.Instance.User.Key);
        WWW webRequest = new WWW(GetIcon_ListUrl, form);
        yield return webRequest;
        string Request = webRequest.text.Trim();  // Trim사용 이유는 웹에서 받아올때 보이지 않는 공백이 첨부되므로.

        string[] data_pack = Request.Split('!');
        if (data_pack[0] == "Success")
        {
            for (int i = 1; i < data_pack.Length-1; i++)
            {
                string[] Icon_Packet = data_pack[i].Split('/');

                Manager.Instance.My_Icon_List.Add(new Manager.Icon_Info(System.Int32.Parse (Icon_Packet[0]), Icon_Packet[1], Icon_Packet[2]));
            }
        }
        yield return true;
    }

    //능력 리스트를 가져옴
    public IEnumerator Get_Ability_List_DB()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", Manager.Instance.User.Key);
        WWW webRequest = new WWW(GetAbility_ListUrl, form);
        yield return webRequest;
        string Request = webRequest.text.Trim();  // Trim사용 이유는 웹에서 받아올때 보이지 않는 공백이 첨부되므로.
        string[] data_pack = Request.Split('!');

        if (data_pack[0] == "Success")
        {
            for (int i = 1; i < data_pack.Length-1; i++)
            {
                string[] Ability_Packet = data_pack[i].Split('/');
                int Ability_Key = System.Int32.Parse(Ability_Packet[0]);
                int Ability_Lv = System.Int32.Parse(Ability_Packet[1]);
                double Ability_Exp = System.Double.Parse(Ability_Packet[2]);

                if (Ability_Key < 3001) //High 능력 찾기
                {
                    for (int j =0; j < Manager.Instance.My_Ability_High_List.Count; j++)
                    {
                        if (Manager.Instance.My_Ability_High_List[j].Index == Ability_Key)
                        {
                            Manager.Instance.My_Ability_High_List[j].Index = Ability_Key;
                            Manager.Instance.My_Ability_High_List[j].Level = Ability_Lv;
                            Manager.Instance.My_Ability_High_List[j].Exp = (float)Ability_Exp;
                        }
                    }
                }
                else //Row 능력 찾기
                {
                    for (int j = 0; j < Manager.Instance.My_Ability_Row_List.Count; j++)
                    {
                        if (Manager.Instance.My_Ability_Row_List[j].Index == Ability_Key)
                        {
                            Manager.Instance.My_Ability_Row_List[j].Index = Ability_Key;
                            Manager.Instance.My_Ability_Row_List[j].Level = Ability_Lv;
                            Manager.Instance.My_Ability_Row_List[j].Exp = (float)Ability_Exp;
                        }
                    }
                }

            }
            yield return true;
        }
    }

    public IEnumerator Add_Item_DB(int item_number, string item_info, string Info, string Route) //타입, 아이템 정보, 지불가격, 보상으로 줘야될 것.
    {
        WWWForm form = new WWWForm();
        form.AddField("id", Manager.Instance.User.Key);
        form.AddField("item", item_number);

        if (9999 < item_number == item_number < 20000)
            form.AddField("part", "Head");
        else if (29999 < item_number == item_number < 40000)
            form.AddField("part", "Top");
        else if (39999 < item_number == item_number < 50000)
            form.AddField("part", "Pant");
        else if (49999 < item_number == item_number < 60000)
            form.AddField("part", "Shoes");

        form.AddField("info", item_info);

        WWW webRequest = new WWW(Buy_Item_Url, form);
        yield return webRequest;
        string packet = webRequest.text.Trim();  // Trim사용 이유는 웹에서 받아올때 보이지 않는 공백이 첨부되므로.
        if (packet == "Success")
        {
            //결제 성공 메시지 뛰우게하기.
            Manager.Instance.Error_Print("성공");
        }
    }

    public IEnumerator Upgrade_Ability_DB(int item_number, string Info, string Route) //타입, 지불가격, 보상으로 줘야될 것.
    {
        WWWForm form = new WWWForm();
        form.AddField("id", Manager.Instance.User.Key);
        WWW webRequest = new WWW(Buy_Upgrade_Ability_Url, form);
        yield return webRequest;
        string packet = webRequest.text.Trim();  // Trim사용 이유는 웹에서 받아올때 보이지 않는 공백이 첨부되므로.
        if (packet == "Success")
        {
            StartCoroutine(Write_Log_DB(Info, Route));
        }
    }

    //아이템 구매를 처리 - 결제 로그 남기고, 수정처리
    public IEnumerator Write_Log_DB(string Info, string Route)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", Manager.Instance.User.Key);
        form.AddField("info", Info);
        form.AddField("route", Route);
        WWW webRequest = new WWW(Get_LogUrl, form);
        yield return webRequest;
    }

    public IEnumerator Add_Gold_DB(int Add, string Info, string Route)
    {
        string key = Manager.Instance.User.Key;
        int Add_money = Add; //얼마를 더해줄지 정하기
        WWWForm form = new WWWForm();
        form.AddField("input_id", key);
        form.AddField("input_money", Add_money);

        WWW webRequest = new WWW(Add_Gold_Url, form);

        yield return webRequest;
        string packet = webRequest.text.Trim(); // Trim사용 이유는 웹에서 받아올때 보이지 않는 공백이 첨부되므로.
        switch (packet)
        {
            case "Success":
                StartCoroutine(Write_Log_DB(Info, Route));
                break;

            case "Fail":
                Debug.Log("DB에 접속하였으나 수정에 실패하였습니다.");
                break;

            default:
                break;
        }
    }

    public IEnumerator Add_Stone_DB(int Add, string Info, string Route)
    {
        string key = Manager.Instance.User.Key;
        int Add_money = Add; //얼마를 더해줄지 정하기
        WWWForm form = new WWWForm();
        form.AddField("input_id", key);
        form.AddField("input_money", Add_money);

        WWW webRequest = new WWW(Add_Stone_Url, form);

        yield return webRequest;
        string packet = webRequest.text.Trim(); // Trim사용 이유는 웹에서 받아올때 보이지 않는 공백이 첨부되므로.
        switch (packet)
        {
            case "Success":
                StartCoroutine(Write_Log_DB(Info, Route));
                break;

            case "Fail":
                Debug.Log("DB에 접속하였으나 수정에 실패하였습니다.");
                break;

            default:
                break;
        }
    }

    public IEnumerator Spend_Money_DB(int cost, string Info, string Route,string Item_Code) //차감할 가격, 차감되기 전 값 , 얻은 경로, 아이템 코드
    {
        string key = Manager.Instance.User.Key;
        WWWForm form = new WWWForm();
        form.AddField("input_id", key);
        form.AddField("input_money", cost);

        WWW webRequest = new WWW(SpendMoney, form);

        yield return webRequest;
        string packet = webRequest.text.Trim();  // Trim사용 이유는 웹에서 받아올때 보이지 않는 공백이 첨부되므로.

        if (packet == "Success")
        {
            StartCoroutine(Write_Log_DB(Info, Route));
            //Market_Canvas.Instance.Call_Back_Product(Item_Code);
        }
        else if (packet == "Null Key")
        {
            Manager.Instance.Error_Print("구입에 실패하였습니다. 나중에 다시 시도해주세요."); //예외적인 상황 발생 알림
        }
        else if (packet == "Failed Buy")
        {
            Manager.Instance.Error_Print("소지금이 부족하여 실패하였습니다."); //소지금 부족 알림
        }
        else
        {
            Manager.Instance.Error_Print("오류로 인하여 실패하였습니다."); //예외적인 상황 발생 알림
        }
    }

    public IEnumerator Spend_Stone_DB(int cost, string Info, string Route, string Item_Code) //차감할 가격, 차감 되기 전 값, 얻은 경로, 아이템 코드
    {
        string key = Manager.Instance.User.Key;
        WWWForm form = new WWWForm();
        form.AddField("input_id", key);
        form.AddField("input_money", cost);

        WWW webRequest = new WWW(SpendCrystal, form);

        yield return webRequest;
        string packet = webRequest.text.Trim();  // Trim사용 이유는 웹에서 받아올때 보이지 않는 공백이 첨부되므로.
        if (packet == "Success")
        {
            StartCoroutine(Write_Log_DB(Info, Route));
            //Market_Canvas.Instance.Call_Back_Product(Item_Code);
        }
        else if (packet == "Null Key")
        {
            Manager.Instance.Error_Print("구입에 실패하였습니다. 나중에 다시 시도해주세요."); //예외적인 상황 발생 알림
        }
        else if (packet == "Failed Buy")
        {
            Manager.Instance.Error_Print("소지금이 부족하여 실패하였습니다."); //소지금 부족 알림
        }
        else
        {
            Manager.Instance.Error_Print("오류로 인하여 실패하였습니다."); //예외적인 상황 발생 알림
        }
    }
}
