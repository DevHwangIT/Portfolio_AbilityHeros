using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_3004 : Ability {

    public Ability_3004()
    {
        //순차,능력이름,능력정보,소비소울,랭크,스킬타입,쿨타임 등을 설정.
        Index = 3004;
        Name_Key = "Ability_3004_Name";
        Skill_Level = 1;
        Info_Key = "Ability_3004_Info";
        Spend_Soul = 0;
        Rank = Skill_Rank.C;
        Type = Skill_Type.Pasive_Skill;
        Elemental = Skill_Elemental.None;
        Skill_Delay = 0;
        Damege = 0;
        Keep_Time = 0;

        if (move.Instance != null)
        {
            move.Instance.Collider_On_1 += Enemy_Get_Stone;
            move.Instance.Collider_On_2 += Enemy_Get_Stone;
            move.Instance.Collider_On_3 += Enemy_Get_Stone;
        }
    }

    public void Enemy_Get_Stone()
    {
        int rand = Random.Range(1, 100);
        if (rand <= (Skill_Level * 10) + 20)  // 100 / 스킬레벨 확률
        {
            Character_State.Instance.My_Character_Info.Stone_Count += 1;
        }
    }

    ~Ability_3004()
    {
        if (move.Instance != null)
        {
            move.Instance.Collider_On_1 -= Enemy_Get_Stone;
            move.Instance.Collider_On_2 -= Enemy_Get_Stone;
            move.Instance.Collider_On_3 -= Enemy_Get_Stone;
        }
    }
}
