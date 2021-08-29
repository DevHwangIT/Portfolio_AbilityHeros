using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_2002 : Ability
{

    public Ability_2002()
    {
        //순차,능력이름,능력정보,소비소울,랭크,스킬타입,쿨타임 등을 설정.
        Index = 2002;
        Name_Key = "Ability_2002_Name";
        Skill_Level = 1;
        Info_Key = "Ability_2002_Info";
        Spend_Soul = 2;
        Rank = Skill_Rank.B;
        Type = Skill_Type.Active_Skill;
        Elemental = Skill_Elemental.Dark;
        Skill_Delay = 30;
        Damege = 0;
        Keep_Time = 10 + (Skill_Level * 2);
    }

    // 부모클래스의 Active_Skill_Btn 메소드로 사용 가능여부 파악한 후 호출 됩니다.
    public override void Active_Skill()
    {
        PhotonNetwork.Instantiate("Skill_\\Skill_Resources\\Hide_Effect", Character_State.Instance.gameObject.transform.position, Character_State.Instance.gameObject.transform.rotation, 0);
        Ability_Trigger.Instance.pv_.RPC("Ability2002_Trigger", PhotonTargets.All, Manager.Instance.User.Nick_Name, Keep_Time);
        Ability_Trigger.Instance.User_Condition_State(Manager.Instance.User.Nick_Name, Condition.Condition_State.None, Keep_Time, 0, Index);
        Delay_Timer = Skill_Delay;
        Character_State.Instance.My_Character_Info.Stone_Count -= Spend_Soul;
        Skill_Activation = false;
        return;
    }
}
