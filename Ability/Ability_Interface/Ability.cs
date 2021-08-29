using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : IAbilty
{
    
    //순차,능력이름,능력정보,소비소울,랭크,스킬타입,쿨타임 등을 설정.
    protected Ability()
    {
        Index = 0;
        Name_Key = "";
        Skill_Level = 1;
        Skill_Exp = 0;
        Info_Key = "";
        Spend_Soul = 0;
        Rank = Skill_Rank.None;
        Type = Skill_Type.None;
        Elemental = Skill_Elemental.None;
        Skill_Activation = true;
        Skill_Delay = 0;
        Delay_Timer = 0;
        Damege = 0;
        Keep_Time = 0;
    }

    //스킬 발동 구현
    public void Active_Skill_Btn()
    {
        if (Skill_Activation == true)
        {
            //스킬 사용조건이 충분한지 확인.
            if (Character_State.Instance.My_Character_Info.Stone_Count >= Spend_Soul)
            {
                Delay_Timer = Skill_Delay;
                Active_Skill();
            }
            else
            {
                Notice_Canvas.Instance.Notice_("소울 스톤이 부족합니다.", 3.0f);
            }
        }
        UIJoystick.Instance.GetComponent<SphereCollider>().enabled = true;
    }

    //overide 구현
    public virtual void Active_Skill() { }
    
    //스킬 목차 인덱스번호
    public int Index { get; set; }

    //스킬 이름
    public string Name_Key { get; set; }

    public int Skill_Level { get; set; }

    public float Skill_Exp { get; set; }

    //스킬 설명 정보
    public string Info_Key { get; set; }

    //사용을 위해 필요한 소울 개수
    public int Spend_Soul { get; set; }

    //스킬 사용 활성화 여부
    public bool Skill_Activation { get; set; }

    //스킬 쿨타임
    public int Skill_Delay { get; set; }

    //스킬 쿨타임 남은 시간
    public int Delay_Timer { get; set; }

    //스킬 데미지
    public int Damege { get; set; }

    //스킬 지속시간
    public int Keep_Time { get; set; }

    public float Delay_Nomalize()
    {
        try
        {
            float nomalize = (float)Delay_Timer / (float)Skill_Delay;
            return 1 - nomalize;
        }
        catch (System.DivideByZeroException e)
        {
            return 1.0f;
        }
    }

    //스킬 랭크
    public Skill_Rank Rank { get; set; }

    //스킬 타입 = 액티브, 패시브
    public Skill_Type Type { get; set; }

    public Skill_Elemental Elemental { get; set; }

    public Skill_Rank_Height Skill_Height
    {
        get
        {
            return Skill_Height;
        }
        set
        {
            if (Rank == Skill_Rank.S || Rank == Skill_Rank.A)
            {
                Skill_Height = Skill_Rank_Height.High_Rank;
            }
            else
            {
                Skill_Height = Skill_Rank_Height.Row_Rank;
            }
        }
    }

    public Sprite Skill_Icon;

    public GameObject Skill_Effect;

    public AudioClip Skill_Sound;

    public enum Skill_Type
    {
        Pasive_Skill=0,
        Active_Skill=1,
        None=2
    }

    public enum Skill_Rank
    {
        S=0,
        A=1,
        B=2,
        C=3,
        D=4,
        None=5
    }

    public enum Skill_Rank_Height
    {
        High_Rank=0,
        Row_Rank=1
    }

    public enum Skill_Elemental
    {
        Fire,
        Earth,
        Water,
        Wind,
        Dark,
        Light,
        None
    }
}
