using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_1 : Ability {

    public Ability_1()
    {
        //순차,능력이름,능력정보,소비소울,랭크,스킬타입,쿨타임 등을 설정.
        Index = 1;
        Name_Key = "Ability_1_Name";
        Skill_Level = 1;
        Info_Key = "Ability_1_Info";
        Spend_Soul = 20;
        Rank = Skill_Rank.S;
        Type = Skill_Type.Active_Skill;
        Elemental = Skill_Elemental.Dark;
        Skill_Delay = 60;
        Damege = 0;
        Keep_Time = 30;
    }

    Vector3 User_Pos;

    [HideInInspector]
    public string Warring_Text = "누군가 비열한 승리 능력을 발동하였습니다. 30초내에 저지하지 않을 경우 패배하게 됩니다.";
    
    public override void Active_Skill()
    {
        Debug.LogError("구현되지 않은 스킬입니다.");
        /*
        Instantiate(Skill_Effect, User_Pos, Skill_Effect.transform.rotation);
        AudioSource audio_ = this.GetComponent<AudioSource>();
        audio_.clip = Skill_Sound;
        audio_.loop = true;
        audio_.Play();
        Character_State.Instance.My_Character_Info.Stone_Count -= Spend_Soul;
        Invoke("Game_Over", 30.0f);*/
    }
    
    void Skill_Execute()
    {
        Debug.Log("게임종료");
    }
}
