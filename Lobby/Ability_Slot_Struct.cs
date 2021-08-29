using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Slot_Struct : MonoBehaviour {

    public Ability Ability_Instance;

    GameObject Ability_Slot_Info_View;

    private void Start()
    {
        Ability_Slot_Info_View = GameObject.Find("Ability_Slot_Info");
        this.transform.GetChild(0).GetComponent<UISprite>().spriteName = Ability_Instance.Index.ToString();
    }

    public void Inventory_Ability_Slot_Btn()
    {
        int Index = 0;

        My_Room_Canvas.Instance.Ability_Select_Slot = this; // 현재 누른 슬롯 정보를 표현.
        
        Ability_Slot_Info_View.transform.GetChild(0).GetComponent<UILocalize>().key = Ability_Instance.Name_Key;
        Ability_Slot_Info_View.transform.GetChild(0).GetComponent<UILabel>().text = Localization.Get(Ability_Instance.Name_Key) + "  Lv. " + Ability_Instance.Skill_Level;
        Ability_Slot_Info_View.transform.GetChild(1).GetComponent<UILocalize>().key = Ability_Instance.Info_Key;
        Ability_Slot_Info_View.transform.GetChild(1).GetComponent<UILabel>().text = Localization.Get(Ability_Instance.Info_Key);

        Ability_Slot_Info_View.transform.GetChild(2).GetComponent<UILabel>().text = "소모소울 " + Ability_Instance.Spend_Soul + ", 데미지 " + Ability_Instance.Damege+ "\n지속시간 " + Ability_Instance.Keep_Time + ", 쿨타임 " + Ability_Instance.Delay_Timer;
        if (Ability_Instance.Skill_Level >= 5) // Max Level;
        {
            Ability_Slot_Info_View.transform.GetChild(2).transform.gameObject.SetActive(false);
        }
        else
        {
            Ability_Slot_Info_View.transform.GetChild(2).transform.gameObject.SetActive(true);
        }
    }
}
