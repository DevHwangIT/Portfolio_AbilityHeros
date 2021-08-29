using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_4002 : Ability {
    
    public Ability_4002()
    {
        //순차,능력이름,능력정보,소비소울,랭크,스킬타입,쿨타임 등을 설정.
        Index = 4002;
        Name_Key = "Ability_4002_Name";
        Skill_Level = 1;
        Info_Key = "Ability_4002_Info";
        Spend_Soul = 1;
        Rank = Skill_Rank.C;
        Type = Skill_Type.Active_Skill;
        Elemental = Skill_Elemental.None;
        Skill_Delay = 60;
        Damege = 0;
        Keep_Time = 0;
    }
    
    // 부모클래스의 Active_Skill_Btn 메소드로 사용 가능여부 파악한 후 호출 됩니다.
    public override void Active_Skill()
    {
        int Recovery = 10 + (Skill_Level * 10);
        if (Random.Range(0, 100) % 2 == 0)
        {
            if (Character_State.Instance.My_Character_Info.HP + Recovery < Character_State.Instance.My_Character_Info.HP_Maximum)
                Character_State.Instance.My_Character_Info.HP += Recovery;
            else
                Character_State.Instance.My_Character_Info.HP = Character_State.Instance.My_Character_Info.HP_Maximum;
        }
        else
        {
            if (Character_State.Instance.My_Character_Info.HP - Recovery > 0)
                Character_State.Instance.My_Character_Info.HP -= Recovery;
            else
                Character_State.Instance.My_Character_Info.HP = 1;
        }

        Delay_Timer = Skill_Delay;
        Skill_Activation = false;
        return;
    }
}
