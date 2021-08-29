using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_3003 : Ability {

    public Ability_3003()
    {
        //순차,능력이름,능력정보,소비소울,랭크,스킬타입,쿨타임 등을 설정.
        Index = 3003;
        Name_Key = "Ability_3003_Name";
        Skill_Level = 1;
        Info_Key = "Ability_3003_Info";
        Spend_Soul = 2;
        Rank = Skill_Rank.C;
        Type = Skill_Type.Active_Skill;
        Elemental = Skill_Elemental.None;
        Skill_Delay = 30;
        Damege = 0;
        Keep_Time = 20 + ((Skill_Level / 2) * 5);
    }

    // 부모클래스의 Active_Skill_Btn 메소드로 사용 가능여부 파악한 후 호출 됩니다.
    public override void Active_Skill()
    {
        GameObject Trigger = PhotonNetwork.Instantiate("Skill_\\Skill_Resources\\Ability_3003_Trigger", Character_State.Instance.gameObject.transform.position + Character_State.Instance.gameObject.transform.forward, Character_State.Instance.gameObject.transform.rotation, 0); //돌맹이 객체 생성 후 던짐
        if (Skill_Level <= 3) //스킬레벨이 3보다 같거나 작으면 장애물 하나를 끔.
        {
            Trigger.transform.GetChild(2).transform.gameObject.SetActive(false);
        }
        if (Skill_Level <= 1) //스킬레벨이 1보다 같거나 작으면 장애물 하나를 끔 결론적으로 2개.
        {
            Trigger.transform.GetChild(1).transform.gameObject.SetActive(false);
        }
        Delay_Timer = Skill_Delay;
        Character_State.Instance.My_Character_Info.Stone_Count -= Spend_Soul;
        Skill_Activation = false;
        return;
    }
}
