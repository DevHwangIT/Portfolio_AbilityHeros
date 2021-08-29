using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Market_Canvas : MonoBehaviour, UI_Interface {

    [HideInInspector]
    public static Market_Canvas Instance;

    [Header("Market Object")]
    public UISprite BG;
    public GameObject Character_List;
    public GameObject Ability_List;
    public GameObject Crystal_List;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    
    // Use this for initialization
    void Start ()
    {
        Character_Tab_Btn();
    }

    public void Open()
    {
        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    public void Character_Tab_Btn()
    {
        Close_List_View();
        Character_List.SetActive(true);
    }

    public void Ability_Tab_Btn()
    {
        Close_List_View();
        Ability_List.SetActive(true);
    }

    public void Crystal_Tab_Btn()
    {
        Close_List_View();
        Crystal_List.SetActive(true);
    }
    
    void Close_List_View()
    {
        Character_List.SetActive(false);
        Ability_List.SetActive(false);
        Crystal_List.SetActive(false);
    }

    public void Info_Top_Btn()
    {
        Lobby.Instance.Market_UI_Show_UI();
        Close_List_View();
        Crystal_Tab_Btn();
    }

    //public delegate void Market_Product_Buy(string product);

    //public Market_Product_Buy Call_Back_Product;

    //public void Product_Buy_Button(string Product_name) //어떠한 제품을 구매버튼 눌렀는지 파악하고 자원에서 차감.
    //{
    //    Call_Back_Product = new Market_Product_Buy(Product_Pay);
    //    switch (Product_name)
    //    {
    //        case Market_Manager.Random_Head_Custom: //랜덤 머리박스 구매
    //            StartCoroutine(DB_Manager.Instance.Spend_Money_DB(1000, "구입 전 골드:" + Manager.Instance.User.Money, "상점 결제 - 랜덤 머리 박스", Product_name));
    //            break;

    //        case Market_Manager.Random_Top_Custom: //랜덤 상의박스 구매
    //            StartCoroutine(DB_Manager.Instance.Spend_Money_DB(1000, "구입 전 골드:" + Manager.Instance.User.Money, "상점 결제 - 랜덤 상의 박스", Product_name));
    //            break;

    //        case Market_Manager.Random_Pant_Custom: //랜덤 하의박스 구매
    //            StartCoroutine(DB_Manager.Instance.Spend_Money_DB(1000, "구입 전 골드:" + Manager.Instance.User.Money, "상점 결제 - 랜덤 하의 박스", Product_name));
    //            break;

    //        case Market_Manager.Random_Shoes_Custom: //랜덤 신발박스 구매
    //            StartCoroutine(DB_Manager.Instance.Spend_Money_DB(1000, "구입 전 골드:" + Manager.Instance.User.Money, "상점 결제 - 랜덤 신발 박스", Product_name));
    //            break;

    //        case Market_Manager.Sale_Icon_1: //랜덤 아이콘 박스 구매.
    //            StartCoroutine(DB_Manager.Instance.Spend_Money_DB(5000, "아직 미구현", "아직 미구현", Product_name));
    //            break;
                
    //        case Market_Manager.Sale_Soul_Stone_10: //( 현금 결제 ) 10 소울스톤 구입
    //            IAP_Manager.Instance.BuyProductID(Market_Manager.Sale_Soul_Stone_10);
    //            break;

    //        case Market_Manager.Sale_Soul_Stone_50: //( 현금 결제 ) 50 소울스톤 구입
    //            IAP_Manager.Instance.BuyProductID(Market_Manager.Sale_Soul_Stone_50);
    //            break;

    //        case Market_Manager.Sale_Soul_Stone_100: //( 현금 결제 ) 100 소울스톤 구입
    //            IAP_Manager.Instance.BuyProductID(Market_Manager.Sale_Soul_Stone_100);
    //            break;

    //        case Market_Manager.Sale_Gold_1000: //1천 골드 구매
    //            StartCoroutine(DB_Manager.Instance.Spend_Stone_DB(10, "구입 전 소울 스톤 :" + Manager.Instance.User.Soul_Stone, "상점 결제 - 1000 골드", Product_name));
    //            break;

    //        case Market_Manager.Sale_Gold_10000: //1만 골드 구매
    //            StartCoroutine(DB_Manager.Instance.Spend_Money_DB(50, "구입 전 소울 스톤 :" + Manager.Instance.User.Soul_Stone, "상점 결제 - 10,000 골드", Product_name));
    //            break;

    //        case Market_Manager.Sale_Gold_50000: //5만 골드 구매
    //            StartCoroutine(DB_Manager.Instance.Spend_Money_DB(100, "구입 전 소울 스톤 :" + Manager.Instance.User.Soul_Stone, "상점 결제 - 50,000 골드", Product_name));
    //            break;

    //        case Market_Manager.Sale_Event_1: //이벤트 아이템 구매 (아직 미구현)
    //            break;

    //        case Market_Manager.Sale_Event_2: //이벤트 아이템 구매 (아직 미구현)
    //            break;

    //        case Market_Manager.Sale_Event_3: //이벤트 아이템 구매 (아직 미구현)
    //            break;

    //        default:
    //            Debug.LogError("잘못된 상품 구매");
    //            break;
    //    }
    //}

    //enum Item_Rank
    //{
    //    S=0,
    //    A=1,
    //    B=2,
    //    C=3,
    //    D=4
    //}

    //Item_Rank Random_Persent_Type()
    //{
    //    float Rand = UnityEngine.Random.Range(0.0f, 100f);
    //    Item_Rank Rank;
    //    if (Rand <= 1.0f) // 1%
    //    {
    //        Rank = Item_Rank.S;
    //    }
    //    else if (Rand > 1.0f && Rand <= 5.0f) //5%
    //    {
    //        Rank = Item_Rank.A;
    //    }
    //    else if (Rand > 5.0f && Rand <= 15.0f) //10%
    //    {
    //        Rank = Item_Rank.B;
    //    }
    //    else if (Rand > 15.0f && Rand <= 40.0f) //25%
    //    {
    //        Rank = Item_Rank.C;
    //    }
    //    else // 나머지 60%
    //    {
    //        Rank = Item_Rank.D;
    //    }

    //    return Rank;
    //}

    //public void Product_Pay(string Product_name) // 차감이 성공적으로 이루어졌을 경우 그에 따른 아이템 지급.
    //{
    //    Item_Rank Rank = Random_Persent_Type();
    //    int Rand_Index = 10000;

    //    //아래는 아이템 인덱스 추출을 위한 임시 지역 변수
    //    GameObject[] Random_Object;
    //    string[] Random_ALL_List; //모든 오브젝트의 이름을 문자열로 추출.
    //    List<int> Select_Rank_List; //추출한 오브젝트로부터 아이템 인덱스 추출
    //    List<int> Rank_Item;


    //    Manager.Instance.Error_Print("해당 기능은 변환실패로 인해 현재 막아놓은 기능입니다.!");

    //    switch (Product_name)
    //    {
    //        case Market_Manager.Random_Head_Custom: //랜덤 머리박스 구매
    //            Random_Object = Resources.LoadAll<GameObject>("Custom\\Hair");
    //            Random_ALL_List = Random_Object.ToStringFull().Split(','); //모든 오브젝트의 이름을 문자열로 추출.
    //            Select_Rank_List = new List<int>(); //추출한 오브젝트로부터 아이템 인덱스 추출
                
    //            foreach (string name in Random_ALL_List)
    //            {
    //                int name_index = 0;
    //                System.Int32.TryParse(name.Replace("Hair_", ""), out name_index);
    //                Select_Rank_List.Add(name_index);
    //                //여기가 문제다 변환이 안되고있습니다.!! 현재 추측되는 원인으로는 name 저부분이 GameObject형태로 인식을 해서인것으로 파악됩니다.
    //            }
                
    //            Rank_Item = new List<int>(); //추출한 아이템 인덱스중 해당 등급에 해당하는 아이템만 골라서 추출

    //            switch (Rank)
    //            {
    //                case Item_Rank.S:
    //                    foreach (int Item_Number in Select_Rank_List)
    //                    {
    //                        if (10000 < Item_Number && Item_Number < 12001)
    //                        {
    //                            Rank_Item.Add(Item_Number);
    //                        }
    //                    }
    //                    Rand_Index = UnityEngine.Random.Range(0, Rank_Item.Count - 1);
    //                    break;

    //                case Item_Rank.A:
    //                    foreach (int Item_Number in Select_Rank_List)
    //                    {
    //                        if (12000 < Item_Number && Item_Number < 14001)
    //                        {
    //                            Rank_Item.Add(Item_Number);
    //                        }
    //                    }
    //                    Rand_Index = UnityEngine.Random.Range(0, Rank_Item.Count - 1);
    //                    break;

    //                case Item_Rank.B:
    //                    foreach (int Item_Number in Select_Rank_List)
    //                    {
    //                        if (14000 < Item_Number && Item_Number < 16001)
    //                        {
    //                            Rank_Item.Add(Item_Number);
    //                        }
    //                    }
    //                    Rand_Index = UnityEngine.Random.Range(0, Rank_Item.Count - 1);
    //                    break;

    //                case Item_Rank.C:
    //                    foreach (int Item_Number in Select_Rank_List)
    //                    {
    //                        if (16000 < Item_Number && Item_Number < 18001)
    //                        {
    //                            Rank_Item.Add(Item_Number);
    //                        }
    //                    }
    //                    Rand_Index = UnityEngine.Random.Range(0, Rank_Item.Count - 1);
    //                    break;

    //                case Item_Rank.D:
    //                    foreach (int Item_Number in Select_Rank_List)
    //                    {
    //                        if (18000 < Item_Number && Item_Number < 20000)
    //                        {
    //                            Rank_Item.Add(Item_Number);
    //                        }
    //                    }
    //                    Rand_Index = UnityEngine.Random.Range(0, Rank_Item.Count - 1);
    //                    break;

    //                default:
    //                    foreach (int Item_Number in Select_Rank_List)
    //                    {
    //                        if (18000 < Item_Number && Item_Number < 20000)
    //                        {
    //                            Rank_Item.Add(Item_Number);
    //                        }
    //                    }
    //                    Rand_Index = UnityEngine.Random.Range(0, Rank_Item.Count - 1);
    //                    break;
    //            }
                
    //            if (Rand_Index == 0)
    //            {
    //                Manager.Instance.Error_Print("존재하지 않는 아이템 랭크 입니다.");
    //                return;
    //            }

    //            StartCoroutine(DB_Manager.Instance.Add_Item_DB(Rand_Index, "상점 - 머리 랜덤 박스", "결제로 획득", "상점 결제"));
    //            StartCoroutine(DB_Manager.Instance.Get_Item_List_DB());
    //            Manager.Instance.Error_Print("Test구문 - " + Rand_Index + " 번째 아이템 획득");
    //            break;

    //        case Market_Manager.Random_Top_Custom: //랜덤 상의박스 구매
    //            Random_Object = Resources.LoadAll<GameObject>("Custom\\Top");
    //            Random_ALL_List = Random_Object.ToStringFull().Split(','); //모든 오브젝트의 이름을 문자열로 추출.
    //            Select_Rank_List = new List<int>(); //추출한 오브젝트로부터 아이템 인덱스 추출


    //            foreach (string name in Random_ALL_List)
    //            {
    //                int name_index = 0;
    //                System.Int32.TryParse(name.Replace("Top_", ""), out name_index);
    //                Select_Rank_List.Add(name_index);
    //            }

    //            Rank_Item = new List<int>(); //추출한 아이템 인덱스중 해당 등급에 해당하는 아이템만 골라서 추출

    //            switch (Rank)
    //            {
    //                case Item_Rank.S:
    //                    foreach (int Item_Number in Select_Rank_List)
    //                    {
    //                        if (30000 < Item_Number && Item_Number < 32001)
    //                        {
    //                            Rank_Item.Add(Item_Number);
    //                        }
    //                    }
    //                    Rand_Index = UnityEngine.Random.Range(0, Rank_Item.Count - 1);
    //                    break;

    //                case Item_Rank.A:
    //                    foreach (int Item_Number in Select_Rank_List)
    //                    {
    //                        if (32000 < Item_Number && Item_Number < 34001)
    //                        {
    //                            Rank_Item.Add(Item_Number);
    //                        }
    //                    }
    //                    Rand_Index = UnityEngine.Random.Range(0, Rank_Item.Count - 1);
    //                    break;

    //                case Item_Rank.B:
    //                    foreach (int Item_Number in Select_Rank_List)
    //                    {
    //                        if (34000 < Item_Number && Item_Number < 36001)
    //                        {
    //                            Rank_Item.Add(Item_Number);
    //                        }
    //                    }
    //                    Rand_Index = UnityEngine.Random.Range(0, Rank_Item.Count - 1);
    //                    break;

    //                case Item_Rank.C:
    //                    foreach (int Item_Number in Select_Rank_List)
    //                    {
    //                        if (36000 < Item_Number && Item_Number < 38001)
    //                        {
    //                            Rank_Item.Add(Item_Number);
    //                        }
    //                    }
    //                    Rand_Index = UnityEngine.Random.Range(0, Rank_Item.Count - 1);
    //                    break;

    //                case Item_Rank.D:
    //                    foreach (int Item_Number in Select_Rank_List)
    //                    {
    //                        if (38000 < Item_Number && Item_Number < 40000)
    //                        {
    //                            Rank_Item.Add(Item_Number);
    //                        }
    //                    }
    //                    Rand_Index = UnityEngine.Random.Range(0, Rank_Item.Count - 1);
    //                    break;

    //                default:
    //                    foreach (int Item_Number in Select_Rank_List)
    //                    {
    //                        if (38000 < Item_Number && Item_Number < 40000)
    //                        {
    //                            Rank_Item.Add(Item_Number);
    //                        }
    //                    }
    //                    Rand_Index = UnityEngine.Random.Range(0, Rank_Item.Count - 1);
    //                    break;
    //            }

    //            if (Rand_Index == 0)
    //            {
    //                Manager.Instance.Error_Print("존재하지 않는 아이템 랭크 입니다.");
    //                return;
    //            }

    //            StartCoroutine(DB_Manager.Instance.Add_Item_DB(Rand_Index, "상점 - 상의 랜덤 박스", "결제로 획득", "상점 결제"));
    //            StartCoroutine(DB_Manager.Instance.Get_Item_List_DB());
    //            Manager.Instance.Error_Print("Test구문 - " + Rand_Index + " 번째 아이템 획득");
    //            break;

    //        case Market_Manager.Random_Pant_Custom: //랜덤 하의박스 구매
    //            Random_Object = Resources.LoadAll<GameObject>("Custom\\Pants");
    //            Random_ALL_List = Random_Object.ToStringFull().Split(','); //모든 오브젝트의 이름을 문자열로 추출.
    //            Select_Rank_List = new List<int>(); //추출한 오브젝트로부터 아이템 인덱스 추출


    //            foreach (string name in Random_ALL_List)
    //            {
    //                int name_index = 0;
    //                System.Int32.TryParse(name.Replace("Pants", ""), out name_index);
    //                Select_Rank_List.Add(name_index);
    //            }

    //            Rank_Item = new List<int>(); //추출한 아이템 인덱스중 해당 등급에 해당하는 아이템만 골라서 추출

    //            switch (Rank)
    //            {
    //                case Item_Rank.S:
    //                    foreach (int Item_Number in Select_Rank_List)
    //                    {
    //                        if (40000 < Item_Number && Item_Number < 42001)
    //                        {
    //                            Rank_Item.Add(Item_Number);
    //                        }
    //                    }
    //                    Rand_Index = UnityEngine.Random.Range(0, Rank_Item.Count - 1);
    //                    break;

    //                case Item_Rank.A:
    //                    foreach (int Item_Number in Select_Rank_List)
    //                    {
    //                        if (42000 < Item_Number && Item_Number < 44001)
    //                        {
    //                            Rank_Item.Add(Item_Number);
    //                        }
    //                    }
    //                    Rand_Index = UnityEngine.Random.Range(0, Rank_Item.Count - 1);
    //                    break;

    //                case Item_Rank.B:
    //                    foreach (int Item_Number in Select_Rank_List)
    //                    {
    //                        if (44000 < Item_Number && Item_Number < 46001)
    //                        {
    //                            Rank_Item.Add(Item_Number);
    //                        }
    //                    }
    //                    Rand_Index = UnityEngine.Random.Range(0, Rank_Item.Count - 1);
    //                    break;

    //                case Item_Rank.C:
    //                    foreach (int Item_Number in Select_Rank_List)
    //                    {
    //                        if (46000 < Item_Number && Item_Number < 48001)
    //                        {
    //                            Rank_Item.Add(Item_Number);
    //                        }
    //                    }
    //                    Rand_Index = UnityEngine.Random.Range(0, Rank_Item.Count - 1);
    //                    break;

    //                case Item_Rank.D:
    //                    foreach (int Item_Number in Select_Rank_List)
    //                    {
    //                        if (48000 < Item_Number && Item_Number < 50000)
    //                        {
    //                            Rank_Item.Add(Item_Number);
    //                        }
    //                    }
    //                    Rand_Index = UnityEngine.Random.Range(0, Rank_Item.Count - 1);
    //                    break;

    //                default:
    //                    foreach (int Item_Number in Select_Rank_List)
    //                    {
    //                        if (48000 < Item_Number && Item_Number < 50000)
    //                        {
    //                            Rank_Item.Add(Item_Number);
    //                        }
    //                    }
    //                    Rand_Index = UnityEngine.Random.Range(0, Rank_Item.Count - 1);
    //                    break;
    //            }

    //            if (Rand_Index == 0)
    //            {
    //                Manager.Instance.Error_Print("존재하지 않는 아이템 랭크 입니다.");
    //                return;
    //            }

    //            StartCoroutine(DB_Manager.Instance.Add_Item_DB(Rand_Index, "상점 - 하의 랜덤 박스", "결제로 획득", "상점 결제"));
    //            StartCoroutine(DB_Manager.Instance.Get_Item_List_DB());
    //            Manager.Instance.Error_Print("Test구문 - " + Rand_Index + " 번째 아이템 획득");
    //            break;

    //        case Market_Manager.Random_Shoes_Custom: //랜덤 신발박스 구매
    //            Random_Object = Resources.LoadAll<GameObject>("Custom\\Shoes");
    //            Random_ALL_List = Random_Object.ToStringFull().Split(','); //모든 오브젝트의 이름을 문자열로 추출.
    //            Select_Rank_List = new List<int>(); //추출한 오브젝트로부터 아이템 인덱스 추출


    //            foreach (string name in Random_ALL_List)
    //            {
    //                int name_index = 0;
    //                System.Int32.TryParse(name.Replace("Shoes_", ""), out name_index);
    //                Select_Rank_List.Add(name_index);
    //            }

    //            Rank_Item = new List<int>(); //추출한 아이템 인덱스중 해당 등급에 해당하는 아이템만 골라서 추출

    //            switch (Rank)
    //            {
    //                case Item_Rank.S:
    //                    foreach (int Item_Number in Select_Rank_List)
    //                    {
    //                        if (50000 < Item_Number && Item_Number < 52001)
    //                        {
    //                            Rank_Item.Add(Item_Number);
    //                        }
    //                    }
    //                    Rand_Index = UnityEngine.Random.Range(0, Rank_Item.Count - 1);
    //                    break;

    //                case Item_Rank.A:
    //                    foreach (int Item_Number in Select_Rank_List)
    //                    {
    //                        if (52000 < Item_Number && Item_Number < 54001)
    //                        {
    //                            Rank_Item.Add(Item_Number);
    //                        }
    //                    }
    //                    Rand_Index = UnityEngine.Random.Range(0, Rank_Item.Count - 1);
    //                    break;

    //                case Item_Rank.B:
    //                    foreach (int Item_Number in Select_Rank_List)
    //                    {
    //                        if (54000 < Item_Number && Item_Number < 56001)
    //                        {
    //                            Rank_Item.Add(Item_Number);
    //                        }
    //                    }
    //                    Rand_Index = UnityEngine.Random.Range(0, Rank_Item.Count - 1);
    //                    break;

    //                case Item_Rank.C:
    //                    foreach (int Item_Number in Select_Rank_List)
    //                    {
    //                        if (56000 < Item_Number && Item_Number < 58001)
    //                        {
    //                            Rank_Item.Add(Item_Number);
    //                        }
    //                    }
    //                    Rand_Index = UnityEngine.Random.Range(0, Rank_Item.Count - 1);
    //                    break;

    //                case Item_Rank.D:
    //                    foreach (int Item_Number in Select_Rank_List)
    //                    {
    //                        if (58000 < Item_Number && Item_Number < 60000)
    //                        {
    //                            Rank_Item.Add(Item_Number);
    //                        }
    //                    }
    //                    Rand_Index = UnityEngine.Random.Range(0, Rank_Item.Count - 1);
    //                    break;

    //                default:
    //                    foreach (int Item_Number in Select_Rank_List)
    //                    {
    //                        if (58000 < Item_Number && Item_Number < 60000)
    //                        {
    //                            Rank_Item.Add(Item_Number);
    //                        }
    //                    }
    //                    Rand_Index = UnityEngine.Random.Range(0, Rank_Item.Count - 1);
    //                    break;
    //            }

    //            if (Rand_Index == 0)
    //            {
    //                Manager.Instance.Error_Print("존재하지 않는 아이템 랭크 입니다.");
    //                return;
    //            }

    //            StartCoroutine(DB_Manager.Instance.Add_Item_DB(Rand_Index, "상점 - 신발 랜덤 박스", "결제로 획득", "상점 결제"));
    //            StartCoroutine(DB_Manager.Instance.Get_Item_List_DB());
    //            Manager.Instance.Error_Print("Test구문 - "+Rand_Index+" 번째 아이템 획득");
    //            break;

    //        case Market_Manager.Sale_Icon_1:
    //            break;
                
    //        case Market_Manager.Sale_Soul_Stone_10: //( 현금 결제 ) 10 소울스톤 구입
    //            IAP_Manager.Instance.BuyProductID(Market_Manager.Sale_Soul_Stone_10);
    //            break;

    //        case Market_Manager.Sale_Soul_Stone_50: //( 현금 결제 ) 50 소울스톤 구입
    //            IAP_Manager.Instance.BuyProductID(Market_Manager.Sale_Soul_Stone_50);
    //            break;

    //        case Market_Manager.Sale_Soul_Stone_100: //( 현금 결제 ) 100 소울스톤 구입
    //            IAP_Manager.Instance.BuyProductID(Market_Manager.Sale_Soul_Stone_100);
    //            break;

    //        case Market_Manager.Sale_Gold_1000: //1천 골드 구매
    //            StartCoroutine(DB_Manager.Instance.Add_Gold_DB(1000, "결제로 획득", "상점 결제"));
    //            StartCoroutine(DB_Manager.Instance.Load_Data_DB()); //DB로부터 정보를 새로고침
    //            Manager.Instance.Error_Print("1000골드 획득");
    //            break;

    //        case Market_Manager.Sale_Gold_10000: //1만 골드 구매
    //            StartCoroutine(DB_Manager.Instance.Add_Gold_DB(5000, "결제로 획득", "상점 결제"));
    //            StartCoroutine(DB_Manager.Instance.Load_Data_DB()); //DB로부터 정보를 새로고침
    //            Manager.Instance.Error_Print("5,000골드 획득");
    //            break;

    //        case Market_Manager.Sale_Gold_50000: //5만 골드 구매
    //            StartCoroutine(DB_Manager.Instance.Add_Gold_DB(10000, "결제로 획득", "상점 결제"));
    //            StartCoroutine(DB_Manager.Instance.Load_Data_DB()); //DB로부터 정보를 새로고침
    //            Manager.Instance.Error_Print("10,000골드 획득");
    //            break;

    //        case Market_Manager.Sale_Event_1: //이벤트 아이템 구매 (아직 미구현)
    //            break;

    //        case Market_Manager.Sale_Event_2: //이벤트 아이템 구매 (아직 미구현)
    //            break;

    //        case Market_Manager.Sale_Event_3: //이벤트 아이템 구매 (아직 미구현)
    //            break;

    //        default:
    //            Debug.LogError("잘못된 상품 지급");
    //            break;
    //    }
    //    StartCoroutine(DB_Manager.Instance.Load_Data_DB());
    //    Lobby_Top_Canvas.Instance.Refresh_Text_UI();
    //    My_Room_Canvas.Instance.Refresh_My_Info();
    //}
}
