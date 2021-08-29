using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_3001 : Ability
{

    public Ability_3001()
    {
        //순차,능력이름,능력정보,소비소울,랭크,스킬타입,쿨타임 등을 설정.
        Index = 3001;
        Name_Key = "Ability_3001_Name";
        Skill_Level = 1;
        Info_Key = "Ability_3001_Info";
        Spend_Soul = 0;
        Rank = Skill_Rank.C;
        Type = Skill_Type.Pasive_Skill;
        Elemental = Skill_Elemental.None;
        Skill_Delay = 0;
        Damege = 0;
        Keep_Time = 0;

        if (Character_State.Instance != null)
        {
            Character_State.Instance.My_Character_Info.HP += (Skill_Level * 30);
            Character_State.Instance.My_Character_Info.HP_Maximum += (Skill_Level * 30);
        }
    }

    // 부모클래스의 Active_Skill_Btn 메소드로 사용 가능여부 파악한 후 호출 됩니다.
    public override void Active_Skill()
    {
        return;
    }
    /*
    ~Ability_3001()
    {
        if (Character_State.Instance != null)

        {
            if (Character_State.Instance.My_Character_Info.Life_State == Character_State.Life_.Live)
            {
                if (Character_State.Instance.My_Character_Info.HP - (Skill_Level * 30) <= 0)
                {
                    Character_State.Instance.My_Character_Info.HP = 1;
                    Character_State.Instance.My_Character_Info.HP_Maximum -= (Skill_Level * 30);
                }
                else
                {
                    Character_State.Instance.My_Character_Info.HP -= (Skill_Level * 30);
                    Character_State.Instance.My_Character_Info.HP_Maximum -= (Skill_Level * 30);
                }
            }
        }
    }*/
}
