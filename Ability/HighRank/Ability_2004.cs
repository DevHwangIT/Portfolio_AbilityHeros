using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_2004 : Ability {

    public Ability_2004()
    {
        //순차,능력이름,능력정보,소비소울,랭크,스킬타입,쿨타임 등을 설정.
        Index = 2004;
        Name_Key = "Ability_2004_Name";
        Skill_Level = 1;
        Info_Key = "Ability_2004_Info";
        Spend_Soul = 1;
        Rank = Skill_Rank.B;
        Type = Skill_Type.Active_Skill;
        Elemental = Skill_Elemental.Light;
        Skill_Delay = 30;
        Damege = 0;
        Keep_Time = 10 + (Skill_Level * 2);
    }

    // 부모클래스의 Active_Skill_Btn 메소드로 사용 가능여부 파악한 후 호출 됩니다.
    public override void Active_Skill()
    {
        PhotonNetwork.Instantiate("Skill_\\Skill_Resources\\Ability_2004_Trigger", Character_State.Instance.gameObject.transform.position + Character_State.Instance.gameObject.transform.forward, Character_State.Instance.gameObject.transform.rotation, 0);
        Delay_Timer = Skill_Delay;
        Character_State.Instance.My_Character_Info.Stone_Count -= Spend_Soul;
        Skill_Activation = false;
        return;
    }
}
