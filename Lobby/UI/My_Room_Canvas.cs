using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class My_Room_Canvas : MonoBehaviour, UI_Interface
{
    public static My_Room_Canvas Instance;

    [Header("My Character")]
    public Transform My_Unit;
    public int Tern_Speed;

    [Header("Atlas_Prefeb")]
    public UIAtlas Hair_Atlas;
    public UIAtlas Top_Atlas;
    public UIAtlas Pants_Atlas;
    public UIAtlas Shoes_Atlas;

    [Header("MyRoom_GameObject_Tap")]
    public GameObject Custom_Btn;
    public GameObject Ability_Btn;
    public GameObject Icon_Btn;
    public GameObject Custom_Tap;
    public GameObject Ability_Tap;
    public GameObject Icon_Tap;

    [Header("Present_Wear_Slot")]
    public GameObject Head_Present_Slot;
    public GameObject Top_Present_Slot;
    public GameObject Pants_Present_Slot;
    public GameObject Shoes_Present_Slot;

    [Header("Skill_Label")]
    public UILabel Title_Labal;
    public UILabel Info_Label;
    public UILabel Level_Label;

    [Header("Slot_Prefeb_Object")]
    public GameObject Item_Slot;
    public GameObject Ability_Slot;
    public GameObject Icon_Slot;

    [Header("UI Object Parent")]
    public Transform Head_UI_Parent;
    public Transform Top_UI_Parent;
    public Transform Pants_UI_Parent;
    public Transform Shoes_UI_Parent;

    public Transform Ability_UI_Parent;
    public Transform Icon_UI_Parent;

    [HideInInspector]
    public Ability_Slot_Struct Ability_Select_Slot; //추후 업그레이드 부분에 있어서 현재 선택된 슬롯 값 저장

    // Use this for initialization
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        Refresh_My_Info();
    }

    public void Open()
    {
        this.gameObject.SetActive(true);
        Custom_Tap_Btn();
        My_Character_Mirror_Initialize();
    }

    public void Close()
    {
        My_Character_Mirror_Initialize();
        this.gameObject.SetActive(false);
    }    

    public void Close_Btn()
    {
        Lobby.Instance.Lobby_UI_Show_UI();
    }

    public void My_Character_Mirror_Initialize()
    {
        My_Unit.transform.rotation = Quaternion.identity;
        My_Unit.transform.Rotate(Vector3.up * 180);
    }

    public void Mirror_Tern_Right()
    {
        My_Unit.transform.Rotate(Vector3.up * Tern_Speed);
    }

    public void Mirror_Tern_Left()
    {
        My_Unit.transform.Rotate(Vector3.down * Tern_Speed);
    }

    public void Custom_Tap_Btn()
    {
        Custom_Tap.SetActive(true);
        Ability_Tap.SetActive(false);
        Icon_Tap.SetActive(false);
        Custom_Btn.GetComponent<UISprite>().spriteName = "메뉴_활성화";
        Ability_Btn.GetComponent<UISprite>().spriteName = "메뉴_비활성화";
        Icon_Btn.GetComponent<UISprite>().spriteName = "메뉴_비활성화";
    }

    public void Ability_Tap_Btn()
    {
        Custom_Tap.SetActive(false);
        Ability_Tap.SetActive(true);
        Icon_Tap.SetActive(false);
        Custom_Btn.GetComponent<UISprite>().spriteName = "메뉴_비활성화";
        Ability_Btn.GetComponent<UISprite>().spriteName = "메뉴_활성화";
        Icon_Btn.GetComponent<UISprite>().spriteName = "메뉴_비활성화";
    }

    public void Icon_Tap_Btn()
    {
        Custom_Tap.SetActive(false);
        Ability_Tap.SetActive(false);
        Icon_Tap.SetActive(true);
        Custom_Btn.GetComponent<UISprite>().spriteName = "메뉴_비활성화";
        Ability_Btn.GetComponent<UISprite>().spriteName = "메뉴_비활성화";
        Icon_Btn.GetComponent<UISprite>().spriteName = "메뉴_활성화";
    }

    public void Refresh_My_Info()
    {
        Slot_Clean_Child_Object();
        Cloth_Add_Slot();
        Ability_Add_Slot();
        Icon_Add_Slot();
        Present_Wear_Setting();
    }

    //현재 내가 누른 스킬에 대해서 적용
    public void Present_Skill(string Title, string Lv, string Info)
    {
        Title_Labal.text = Title;
        Info_Label.text = Info;
        Level_Label.text = Lv;
    }
    
    //현재 내캐릭터가 입고있는 옷에 대해서 화면상에 보여줌.
    public void Present_Wear_Setting()
    {
        Head_Present_Slot.transform.GetChild(0).GetComponent<UISprite>().spriteName = "Hair_" + Manager.Instance.Wear_Item[0];
        Top_Present_Slot.transform.GetChild(0).GetComponent<UISprite>().spriteName = "Top_" + Manager.Instance.Wear_Item[1];
        Pants_Present_Slot.transform.GetChild(0).GetComponent<UISprite>().spriteName = "Pants_" + Manager.Instance.Wear_Item[2];
        Shoes_Present_Slot.transform.GetChild(0).GetComponent<UISprite>().spriteName = "Shoes_" + Manager.Instance.Wear_Item[3];

        //0번은 파츠를 안입은 상태.
        if (Manager.Instance.Wear_Item[0] != 0)
        {
            My_Unit.GetComponent<Clothes_Manager>().Hair = Instantiate(Resources.Load<GameObject>("Custom\\Hair\\Hair_" + Manager.Instance.Wear_Item[0]), Vector3.zero, Quaternion.identity);
            My_Unit.GetComponent<Clothes_Manager>().Equipment_Set(Clothes_Manager.Part.Hair);
        }

        if (Manager.Instance.Wear_Item[1] != 0)
        {
            My_Unit.GetComponent<Clothes_Manager>().Top = Instantiate(Resources.Load<GameObject>("Custom\\Top\\Top_" + Manager.Instance.Wear_Item[1]), Vector3.zero, Quaternion.identity);
            My_Unit.GetComponent<Clothes_Manager>().Equipment_Set(Clothes_Manager.Part.Top);
        }

        if (Manager.Instance.Wear_Item[2] != 0)
        {
            My_Unit.GetComponent<Clothes_Manager>().Pants = Instantiate(Resources.Load<GameObject>("Custom\\Pants\\Pants_" + Manager.Instance.Wear_Item[2]), Vector3.zero, Quaternion.identity);
            My_Unit.GetComponent<Clothes_Manager>().Equipment_Set(Clothes_Manager.Part.Pants);
        }

        if (Manager.Instance.Wear_Item[3] != 0)
        {
            My_Unit.GetComponent<Clothes_Manager>().Shoes = Instantiate(Resources.Load<GameObject>("Custom\\Shoes\\Shoes_" + Manager.Instance.Wear_Item[3]), Vector3.zero, Quaternion.identity);
            My_Unit.GetComponent<Clothes_Manager>().Equipment_Set(Clothes_Manager.Part.Shoes);
        }
    }

    //Manager.cs 내의 정보를 바탕으로 Slot을 만들어 추가한다.
    void Cloth_Add_Slot()
    {
        for (int index = 0; index < Manager.Instance.Item_Head_List.Count; index++)
        {
            GameObject Slot_ = GameObject.Instantiate(Item_Slot, Item_Slot.transform.position, Quaternion.identity);
            Slot_.GetComponent<Item_Slot_Struct>().Name_ = "Hair_" + Manager.Instance.Item_Head_List[index].Name_Key;
            Slot_.GetComponent<Item_Slot_Struct>().index_ = Manager.Instance.Item_Head_List[index].Name_Key;
            Slot_.GetComponent<Item_Slot_Struct>().Type_ = Item_Slot_Struct.Part.Hair;
            Slot_.name = "Hair_" + Manager.Instance.Item_Head_List[index].Name_Key;
            Slot_.transform.parent = Head_UI_Parent;
            Slot_.transform.localScale = Vector3.one;

            Slot_.GetComponent<Item_Slot_Struct>().Set();
        }

        for (int index = 0; index < Manager.Instance.Item_Top_List.Count; index++)
        {
            GameObject Slot_ = GameObject.Instantiate(Item_Slot, Item_Slot.transform.position, Quaternion.identity);
            Slot_.GetComponent<Item_Slot_Struct>().Name_ = "Top_" + Manager.Instance.Item_Top_List[index].Name_Key;
            Slot_.GetComponent<Item_Slot_Struct>().index_ = Manager.Instance.Item_Top_List[index].Name_Key;
            Slot_.GetComponent<Item_Slot_Struct>().Type_ = Item_Slot_Struct.Part.Top;
            Slot_.name = "Top_" + Manager.Instance.Item_Top_List[index].Name_Key;
            Slot_.transform.parent = Top_UI_Parent;
            Slot_.transform.localScale = Vector3.one;

            Slot_.GetComponent<Item_Slot_Struct>().Set();
        }

        for (int index = 0; index < Manager.Instance.Item_Pants_List.Count; index++)
        {
            GameObject Slot_ = GameObject.Instantiate(Item_Slot, Item_Slot.transform.position, Quaternion.identity);
            Slot_.GetComponent<Item_Slot_Struct>().Name_ = "Pants_" + Manager.Instance.Item_Pants_List[index].Name_Key;
            Slot_.GetComponent<Item_Slot_Struct>().index_ = Manager.Instance.Item_Pants_List[index].Name_Key;
            Slot_.GetComponent<Item_Slot_Struct>().Type_ = Item_Slot_Struct.Part.Pants;
            Slot_.name = "Pants_" + Manager.Instance.Item_Pants_List[index].Name_Key;
            Slot_.transform.parent = Pants_UI_Parent;
            Slot_.transform.localScale = Vector3.one;
            
            Slot_.GetComponent<Item_Slot_Struct>().Set();
        }

        for (int index = 0; index < Manager.Instance.Item_Shoes_List.Count; index++)
        {
            GameObject Slot_ = GameObject.Instantiate(Item_Slot, Item_Slot.transform.position, Quaternion.identity);
            Slot_.GetComponent<Item_Slot_Struct>().Name_ = "Shoes_" + Manager.Instance.Item_Shoes_List[index].Name_Key;
            Slot_.GetComponent<Item_Slot_Struct>().index_ = Manager.Instance.Item_Shoes_List[index].Name_Key;
            Slot_.GetComponent<Item_Slot_Struct>().Type_ = Item_Slot_Struct.Part.Shoes;
            Slot_.name = "Shoes_" + Manager.Instance.Item_Shoes_List[index].Name_Key;
            Slot_.transform.parent = Shoes_UI_Parent;
            Slot_.transform.localScale = Vector3.one;

            Slot_.GetComponent<Item_Slot_Struct>().Set();
        }
    }

    //Manager.cs 내의 정보를 바탕으로 Slot을 만들어 추가한다.
    void Ability_Add_Slot()
    {
        for (int index = 0; index < Manager.Instance.My_Ability_High_List.Count; index++) //상위랭크 스킬
        {
            GameObject Slot_ = GameObject.Instantiate(Ability_Slot, Ability_Slot.transform.position, Quaternion.identity);
            Slot_.transform.parent = Ability_UI_Parent;
            Slot_.name = "Ability_" + Manager.Instance.My_Ability_High_List[index].Index.ToString();
            Slot_.transform.localScale = Vector3.one;

            Slot_.GetComponent<Ability_Slot_Struct>().Ability_Instance = Manager.Instance.Return_Ability(Manager.Instance.My_Ability_High_List[index].Index);

            Ability Slot_Ablity = Slot_.GetComponent<Ability_Slot_Struct>().Ability_Instance;

            for (int i = 0; i < Manager.Instance.My_Ability_High_List.Count; i++)
            {
                if (Slot_Ablity.Index == Manager.Instance.My_Ability_High_List[i].Index)
                {
                    Slot_Ablity.Skill_Level = Manager.Instance.My_Ability_High_List[i].Level;
                    Slot_Ablity.Skill_Exp = Manager.Instance.My_Ability_High_List[i].Exp;
                }
            }
        }

        for (int index = 0; index < Manager.Instance.My_Ability_Row_List.Count; index++) //하위랭크 스킬
        {
            GameObject Slot_ = GameObject.Instantiate(Ability_Slot, Ability_Slot.transform.position, Quaternion.identity);
            Slot_.transform.parent = Ability_UI_Parent;
            Slot_.name = "Ability_" + Manager.Instance.My_Ability_Row_List[index].Index.ToString();
            Slot_.transform.localScale = Vector3.one;

            Slot_.GetComponent<Ability_Slot_Struct>().Ability_Instance = Manager.Instance.Return_Ability(Manager.Instance.My_Ability_Row_List[index].Index);

            Ability Slot_Ablity = Slot_.GetComponent<Ability_Slot_Struct>().Ability_Instance;

            for (int i = 0; i < Manager.Instance.My_Ability_Row_List.Count; i++)
            {
                if (Slot_Ablity.Index == Manager.Instance.My_Ability_Row_List[i].Index)
                {
                    Slot_Ablity.Skill_Level = Manager.Instance.My_Ability_Row_List[i].Level;
                    Slot_Ablity.Skill_Exp = Manager.Instance.My_Ability_Row_List[i].Exp;
                }
            }
        }
    }
    
    //Manager.cs 내의 정보를 바탕으로 Slot을 만들어 추가한다.
    void Icon_Add_Slot()
    {
        for (int index = 0; index < Manager.Instance.My_Icon_List.Count; index++)
        {
            GameObject Slot_ = GameObject.Instantiate(Icon_Slot, Icon_Slot.transform.position, Quaternion.identity);
            
            Slot_.GetComponent<Icon_Slot_Struct>().Index_ = Manager.Instance.My_Icon_List[index].Index;
            Slot_.GetComponent<Icon_Slot_Struct>().Name_ = Manager.Instance.My_Icon_List[index].Name;
            Slot_.GetComponent<Icon_Slot_Struct>().Info_ = Manager.Instance.My_Icon_List[index].Info;

            Slot_.transform.parent = Icon_UI_Parent;
            Slot_.name = "Icon" + Manager.Instance.My_Icon_List[index].Index.ToString();
            Slot_.transform.localScale = Vector3.one;

            Slot_.GetComponent<Icon_Slot_Struct>().Set();
        }
    }

    //슬롯을 초기화 시킨다.
    void Slot_Clean_Child_Object()
    {
        if (Head_UI_Parent.childCount != 0)
        {
            for (int index = 0; index < Head_UI_Parent.childCount; index++)
            {
                Head_UI_Parent.GetChild(index).DestroyChildren();
            }
        }

        if (Top_UI_Parent.childCount != 0)
        {
            for (int index = 0; index < Top_UI_Parent.childCount; index++)
            {
                Top_UI_Parent.GetChild(index).DestroyChildren();
            }
        }

        if (Pants_UI_Parent.childCount != 0)
        {
            for (int index = 0; index < Pants_UI_Parent.childCount; index++)
            {
                Pants_UI_Parent.GetChild(index).DestroyChildren();
            }
        }

        if (Shoes_UI_Parent.childCount != 0)
        {
            for (int index = 0; index < Shoes_UI_Parent.childCount; index++)
            {
                Shoes_UI_Parent.GetChild(index).DestroyChildren();
            }
        }

        if (Ability_UI_Parent.childCount != 0)
        {
            for (int index = 0; index < Ability_UI_Parent.childCount; index++)
            {
                Ability_UI_Parent.GetChild(index).DestroyChildren();
            }
        }

        if (Icon_UI_Parent.childCount != 0)
        {
            for (int index = 0; index < Icon_UI_Parent.childCount; index++)
            {
                Icon_UI_Parent.GetChild(index).DestroyChildren();
            }
        }
    }
}
