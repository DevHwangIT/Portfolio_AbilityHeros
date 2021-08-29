using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_2001 : Ability
{
    public Ability_2001()
    {
        //순차,능력이름,능력정보,소비소울,랭크,스킬타입,쿨타임 등을 설정.
        Index = 2001;
        Name_Key = "Ability_2001_Name";
        Skill_Level = 1;
        Info_Key = "Ability_2001_Info";
        Spend_Soul = 3;
        Rank = Skill_Rank.B;
        Type = Skill_Type.Active_Skill;
        Elemental = Skill_Elemental.Fire;
        Skill_Delay = 15;
        Damege = Skill_Level * 3;
        Keep_Time = 10;
    }

    // 부모클래스의 Active_Skill_Btn 메소드로 사용 가능여부 파악한 후 호출 됩니다.
    public override void Active_Skill()
    {
        PhotonNetwork.Instantiate("Skill_\\Skill_Resources\\FireWall", Character_State.Instance.gameObject.transform.position, Character_State.Instance.gameObject.transform.rotation, 0); //돌맹이 객체 생성 후 던짐
        Character_State.Instance.gameObject.GetComponent<Animation_Synchronize>().Char_Anim_Sync(Animation_Synchronize.Ainmation_State.Sky_Hands,Manager.Instance.User.Nick_Name);
        Delay_Timer = Skill_Delay;
        Character_State.Instance.My_Character_Info.Stone_Count -= Spend_Soul;
        Skill_Activation = false;
        return;
    }
}
