using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_4003 : Ability {

    public Ability_4003()
    {
        //순차,능력이름,능력정보,소비소울,랭크,스킬타입,쿨타임 등을 설정.
        Index = 4003;
        Name_Key = "Ability_4003_Name";
        Skill_Level = 1;
        Info_Key = "Ability_4003_Info";
        Spend_Soul = 1;
        Rank = Skill_Rank.D;
        Type = Skill_Type.Active_Skill;
        Elemental = Skill_Elemental.None;
        Skill_Delay = 30;
        Damege = 0;
        Keep_Time = 0;
    }

    // 부모클래스의 Active_Skill_Btn 메소드로 사용 가능여부 파악한 후 호출 됩니다.
    public override void Active_Skill()
    {
        Transform Respon = GameObject.Find("Respon").transform;
        Game_Manager.Instance.My_Unit.transform.position = Respon.transform.GetChild((int)(Random.Range(0, (float)Respon.childCount))).transform.position;

        Delay_Timer = Skill_Delay;
        Skill_Activation = false;
        return;
    }
}
