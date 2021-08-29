using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_0 : Ability {

    public Ability_0()
    {
        //순차,능력이름,능력정보,소비소울,랭크,스킬타입,쿨타임 등을 설정.
        Index = 0;
        Name_Key = "Ability_0_Name";
        Skill_Level = 1;
        Info_Key = "Ability_0_Info";
        Spend_Soul = 0;
        Rank = Skill_Rank.None;
        Type = Skill_Type.None;
        Skill_Delay = 0;
    }
    
    // 부모클래스의 Active_Skill_Btn 메소드로 사용 가능여부 파악한 후 호출 됩니다.
    public override void Active_Skill()
    {
        Debug.Log("아직 알수없는 스킬을 사용함.");
        Character_State.Instance.My_Character_Info.Stone_Count -= Spend_Soul;
        Skill_Activation = false;
        return;
    }
}
