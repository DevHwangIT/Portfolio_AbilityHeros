using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Button : MonoBehaviour {

    public Ability Ability_Instance;//현재 적용된 능력을 담는 객체

    string Fire_Charge_Path = "Skill_\\Default\\Charge\\Fire_Charge";
    string Earth_Charge_Path = "Skill_\\Default\\Charge\\Earth_Charge";
    string Wind_Charge_Path = "Skill_\\Default\\Charge\\Wind_Charge";
    string Water_Charge_Path = "Skill_\\Default\\Charge\\Water_Charge";
    string Light_Charge_Path = "Skill_\\Default\\Charge\\Light_Charge";
    string Dark_Charge_Path = "Skill_\\Default\\Charge\\Dark_Charge";

    // Use this for initialization
    void Start()
    {
        Delay_Counting(); //스킬 쿨타임처리. Activation의 경우 상속받은 프로퍼티에서 알아서 처리.
    }

    private void LateUpdate()
    {
        if (this.gameObject.GetComponent<UISprite>() && Ability_Instance != null)
        {
            this.gameObject.GetComponent<UISprite>().spriteName = Ability_Instance.Index.ToString();
            this.gameObject.GetComponent<UISprite>().fillAmount = Ability_Instance.Delay_Nomalize();
        }
    }

    public void Use_Ability()
    {
        if (move.Instance.Is_Trigger == false)
        {
            UIJoystick.Instance.ResetJoystick();
            UIJoystick.Instance.GetComponent<SphereCollider>().enabled = false;
            if (Ability_Instance.Rank == Ability.Skill_Rank.S || Ability_Instance.Rank == Ability.Skill_Rank.A || Ability_Instance.Rank == Ability.Skill_Rank.B)
            {
                High_Effect_Start();
            }
            else
            {
                Ability_Instance.Active_Skill_Btn();
            }
        }
    }

    //스킬 쿨타임 처리 메소드
    public void Delay_Counting()
    {
        if (Ability_Instance != null) {
            if (Ability_Instance.Delay_Timer > 0)
            {
                --Ability_Instance.Delay_Timer;
                if (Ability_Instance.Delay_Timer <= 0)
                {
                    Ability_Instance.Delay_Timer = 0; // 쿨타임 0으로 변경
                    Ability_Instance.Skill_Activation = true; //스킬 사용가능 설정.
                }
            }
        }
        Invoke("Delay_Counting", 1.0f);
    }

    GameObject effect;

    public void High_Effect_Start()
    {
        if (Ability_Instance.Elemental != Ability.Skill_Elemental.None)
        {
            move.Instance.Is_Trigger = true; //스킬 사용중 움직임 방지.

            if (Ability_Instance.Skill_Activation == true)
            {
                string Effect_Path = "";
                switch (Ability_Instance.Elemental)
                {
                    case Ability.Skill_Elemental.Fire:
                        Effect_Path = Fire_Charge_Path;
                        break;

                    case Ability.Skill_Elemental.Water:
                        Effect_Path = Water_Charge_Path;
                        break;

                    case Ability.Skill_Elemental.Earth:
                        Effect_Path = Earth_Charge_Path;
                        break;

                    case Ability.Skill_Elemental.Wind:
                        Effect_Path = Wind_Charge_Path;
                        break;

                    case Ability.Skill_Elemental.Light:
                        Effect_Path = Light_Charge_Path;
                        break;

                    case Ability.Skill_Elemental.Dark:
                        Effect_Path = Dark_Charge_Path;
                        break;

                    case Ability.Skill_Elemental.None:
                        break;
                }
                effect = GameObject.Instantiate(Resources.Load<GameObject>(Effect_Path), Character_State.Instance.gameObject.transform.position, Quaternion.identity);
                InGame.Instance.Skill_Effect_Open(); //UI상에 효과 나타냄
                Game_Manager.Instance.Camera_Vibration_Set(); // 카메라 떨림효과.
                Game_Manager.Instance.My_Unit.GetComponent<Animation_Synchronize>().pv_.RPC("Char_Anim_Sync", PhotonTargets.All, Animation_Synchronize.Ainmation_State.Magic_Book_Success, Manager.Instance.User.Nick_Name);
            }
            Invoke("High_Effect_End", 1.0f); //이펙트 딜레이
        }
    }

    void High_Effect_End()
    {
        Game_Manager.Instance.My_Unit.GetComponent<Animation_Synchronize>().Char_Anim_Sync(Animation_Synchronize.Ainmation_State.Wait, Manager.Instance.User.Nick_Name);
        Game_Manager.Instance.My_Unit.GetComponent<Animation_Synchronize>().pv_.RPC("Char_Anim_Sync", PhotonTargets.Others, Animation_Synchronize.Ainmation_State.Wait, Manager.Instance.User.Nick_Name);
        InGame.Instance.Skill_Effect_Close();
        Game_Manager.Instance.Camera_Vibration_ReSet();
        Destroy(effect); //이펙트 오브젝트 제거.
        move.Instance.Is_Trigger = false; //스킬 사용중 움직임 방지해제.
        Ability_Instance.Active_Skill_Btn();
    }

    public void Ability_Ititialize(Ability ability,int level)
    {
        Ability_Instance = ability; //능력 객체 할당
        Ability_Instance.Skill_Level = level;
        
        this.transform.gameObject.GetComponent<UISprite>().spriteName = Ability_Instance.Index.ToString(); //화면에 보이는 버튼의 스프라이트 이미지 설정
        this.transform.parent.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetComponent<UISprite>().spriteName = Ability_Instance.Index.ToString(); //능력정보 창 스프라이트 이미지 설정
        this.transform.parent.GetChild(1).GetChild(0).GetChild(1).GetComponent<UILocalize>().key = Ability_Instance.Name_Key; //능력정보 창 스프라이트 이름 설정
        this.transform.parent.GetChild(1).GetChild(0).GetChild(2).GetComponent<UILocalize>().key = Ability_Instance.Info_Key; //능력정보 창 스프라이트 정보 설정


        if (Ability_Instance.Rank == Ability.Skill_Rank.S || Ability_Instance.Rank == Ability.Skill_Rank.A)
        {
            Character_State.Instance.My_Character_Info.High_Skill_Script = Ability_Instance; //스킬 하이랭크 스크립트 지정.
        }
        else
        {
            Character_State.Instance.My_Character_Info.Row_Skill_Script = Ability_Instance; //스킬 로우랭크 스크립트 지정.
        }
    }
}
