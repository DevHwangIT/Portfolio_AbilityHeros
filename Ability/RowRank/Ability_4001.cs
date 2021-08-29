using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_4001 : Ability
{
    public Ability_4001()
    {
        //순차,능력이름,능력정보,소비소울,랭크,스킬타입,쿨타임 등을 설정.
        Index = 4001;
        Name_Key = "Ability_4001_Name";
        Skill_Level = 1;
        Info_Key = "Ability_4001_Info";
        Spend_Soul = 0;
        Rank = Skill_Rank.D;
        Type = Skill_Type.Active_Skill;
        Elemental = Skill_Elemental.None;
        Skill_Delay = 5;
        Damege = 3 + Skill_Level;
        Keep_Time = 0;
    }
    
    // 부모클래스의 Active_Skill_Btn 메소드로 사용 가능여부 파악한 후 호출 됩니다.
    public override void Active_Skill()
    {
        PhotonNetwork.Instantiate("Skill_\\Skill_Resources\\Skill_Stone_Tigger", Character_State.Instance.gameObject.transform.position, Character_State.Instance.gameObject.transform.rotation, 0); //돌맹이 객체 생성 후 던짐
        Character_State.Instance.gameObject.GetComponent<move>().animator_.Play("Throw");
        Delay_Timer = Skill_Delay;
        Skill_Activation = false;
        return;
    }
}
