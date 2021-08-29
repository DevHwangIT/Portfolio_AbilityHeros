using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_3002 : Ability {

    public Ability_3002()
    {
        //순차,능력이름,능력정보,소비소울,랭크,스킬타입,쿨타임 등을 설정.
        Index = 3002;
        Name_Key = "Ability_3002_Name";
        Skill_Level = 1;
        Info_Key = "Ability_3002_Info";
        Spend_Soul = 1;
        Rank = Skill_Rank.C;
        Type = Skill_Type.Pasive_Skill;
        Elemental = Skill_Elemental.None;
        Skill_Delay = 0;
        Damege = 0;
        Keep_Time = 0;
    }

    // 부모클래스의 Active_Skill_Btn 메소드로 사용 가능여부 파악한 후 호출 됩니다.
    public override void Active_Skill()
    {
        Ability_Trigger.Instance.Ability_3002_count = 0;
        Ability_Trigger.Instance.pv_.RPC("Ability3002_Trigger_Send", PhotonTargets.Others, Manager.Instance.User.Nick_Name);
        Delay_Timer = Skill_Delay;
        Character_State.Instance.My_Character_Info.Stone_Count -= Spend_Soul;
        Skill_Activation = false;
        return;
    }
}
