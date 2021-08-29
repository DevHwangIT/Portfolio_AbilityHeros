using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_State : Photon.PunBehaviour
{

    public static Character_State Instance;

    public List<Condition> Unit_Condition_List;

    public int Recive_Damege_Count = 0; //받은 데미지
    public int Send_Damege_Count = 0; //적에게 준 데미지
    public int Kill_Count = 0; // 킬 카운트

    public enum Life_
    {
        Live = 0,
        Dead = 1
    }

    public class Character
    {
        public string Nick_Name;
        public int Damege; //기본 공격데미지
        public int Kill_Count;
        public int Stone_Count;
        public int HP;
        public int HP_Maximum;
        public int High_Rank_Skill; // 스킬 번호
        public int High_Skill_Level;
        public Ability High_Skill_Script;
        public int Row_Rank_Skill; // 스킬 번호
        public int Row_Skill_Level;
        public Ability Row_Skill_Script;
        public Life_ Life_State;
        public InRoom_Canvas.Team_ Team_Type;
    }

    public Character My_Character_Info;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        My_Character_Info = new Character();
        Recive_Damege_Count = 0;
        Send_Damege_Count = 0;
        Kill_Count = 0;
        Unit_Condition_List = new List<Condition>();

        Debug.Log("테스트문단");
        My_Character_Info.Stone_Count = 10;
    }

    //경험치 공식

    bool Destroy_isrunnig = true; //파괴 여부 파악

    private void LateUpdate()
    {
        GamePlay_Canvas.Instance.Kill_Label.text = My_Character_Info.Kill_Count.ToString();
        GamePlay_Canvas.Instance.Soul_Label.text = My_Character_Info.Stone_Count.ToString();

        if (My_Character_Info.HP >= 1)
            My_Character_Info.Life_State = Life_.Live;
        else
            My_Character_Info.Life_State = Life_.Dead;

        if (My_Character_Info.Life_State == Life_.Dead)
        {
            StartCoroutine(Destroy_Unit());
        }
    }

    IEnumerator Destroy_Unit()
    {
        if (Destroy_isrunnig == true)
        {
            Destroy_isrunnig = false;
            Observer_Set(); // 관전모드로 변경
            yield return new WaitForEndOfFrame();
            Game_Manager.Instance.My_Unit.GetComponent<Animation_Synchronize>().pv_.RPC("Char_Anim_Sync", PhotonTargets.All, Animation_Synchronize.Ainmation_State.Death, PhotonNetwork.player.NickName);
            Game_Manager.Instance.My_Unit.GetComponent<CharacterController>().enabled = false;
        }
    }

    public void Observer_Set()
    {
        GameObject[] Unit_List = GameObject.FindGameObjectsWithTag("Player");

        Game_Manager.Instance.My_Unit.GetComponent<CharacterController>().enabled = false;

        
        switch (Manager.Instance.Room.mode)
        {   

            case 0: //개인전에 자기 캐릭터 사망했을 경우
                foreach (GameObject Unit in Unit_List)
                {
                    if (Unit.GetComponent<Animation_Synchronize>().Present_State != Animation_Synchronize.Ainmation_State.Death)
                    {
                        Game_Manager.Instance.three_Camera.target = Unit.transform;
                        break;
                    }
                }
                break;

            case 1: //팀전에 자기 캐릭터 사망했을 경우
                foreach (GameObject Unit in Unit_List)
                {
                    //관전하려는 캐릭터가 살아있고, 그 캐릭터가 나와 같은 팀일 경우 관전에 대한건 추후수정.
                    if (Unit.GetComponent<Animation_Synchronize>().Present_State != Animation_Synchronize.Ainmation_State.Death)
                    {
                        Game_Manager.Instance.three_Camera.target = Unit.transform;
                        break;
                    }
                }
                break;
        }

        InGame.Instance.Observer_Canvas_Initialize();
        //Game_Manager.Instance.pv.RPC("User_Death_Boolean", PhotonTargets.MasterClient, Room_Setting.My_Slot_Index);
        //옵저버 가능여부 파악하고 게임 승리 패배 무승부 변경
    }
}