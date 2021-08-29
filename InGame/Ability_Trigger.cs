using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Trigger : MonoBehaviour
{
    //능력 및 기타 게임에 영향을 미치는 트리거를 작동시키는 스크립트.
    public static Ability_Trigger Instance;

    public delegate void Call_Event();

    //Event
    public Call_Event HP_Subtract_Event;
    public Call_Event User_Condition_Event;
    public Call_Event User_Position_Event;
    public Call_Event User_Rotation_Event;


    [HideInInspector]
    public PhotonView pv_;

    AudioSource audio_;

    public GameObject Punch_Effect1;
    public GameObject Punch_Effect2;
    public GameObject Punch_Effect3;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        pv_ = this.GetComponent<PhotonView>();
        audio_ = this.GetComponent<AudioSource>();
    }

    private void Start()
    {
        HP_Subtract_Event = new Call_Event(Ability_Trigger.Instance.Call_HP_Subtract);
        User_Condition_Event = new Call_Event(Ability_Trigger.Instance.Call_Condition_State);
    }

    //스킬로 인한 캐릭터 에어본 추후 구현
    IEnumerator Unit_Parabola()
    {
        yield return null;
    }

    [PunRPC]
    public void Punch_Effect(Vector3 pos, int type)
    {
        switch (type)
        {
            case 0:
                Instantiate(Punch_Effect1, pos, Quaternion.identity);
                break;

            case 1:
                Instantiate(Punch_Effect2, pos, Quaternion.identity);
                break;

            case 2:
                Instantiate(Punch_Effect3, pos, Quaternion.identity);
                break;

            default:
                break;
        }
    }

    int Hp_Sub_Damege_;
    string Hp_Sub_Recive_User;
    string Hp_Sub_Send_User;

    [PunRPC]
    public void HP_Subtract(int damege, string Recive_User, string Send_User)
    {
        Hp_Sub_Damege_=damege;
        Hp_Sub_Recive_User=Recive_User;
        Hp_Sub_Send_User=Send_User;
        HP_Subtract_Event();
    }

    public void Call_HP_Subtract()
    {
        if (Hp_Sub_Recive_User == Manager.Instance.User.Nick_Name)
        {
            if ((Character_State.Instance.My_Character_Info.HP - Hp_Sub_Damege_) <= 0)
            {
                pv_.RPC("Kill_Log_Send", PhotonTargets.All, Hp_Sub_Send_User);
            }
            Character_State.Instance.My_Character_Info.HP -= Hp_Sub_Damege_;
        }
    }

    string Condition_Recive_User;
    Condition.Condition_State Condition_State;
    int Condition_Time;
    int Condition_Damege;
    int Condition_Sprite;

    [PunRPC]
    public void User_Condition_State(string Recive_User, Condition.Condition_State state, int time_, int damege_, int sprite_) //지속시간 및 강도 처리.
    {
        if (Recive_User == Manager.Instance.User.Nick_Name)
        {
            Condition_Recive_User = Recive_User;
            Condition_State = state;
            Condition_Time = time_;
            Condition_Damege = damege_;
            Condition_Sprite = sprite_;
            Call_Condition_State();
        }
    }

    public void Call_Condition_State()
    {
        Condition Unit_State = Game_Manager.Instance.My_Unit.AddComponent<Condition>();
        Unit_State.Set(Condition_Sprite.ToString(), Condition_Time, Condition_Damege, Condition_State);
        Character_State.Instance.Unit_Condition_List.Add(Unit_State);
        Unit_State.Run();
    }


    [PunRPC] //추후 수정...아직미완성
    public void Hit_User_Translate(string Recive_User, float force)
    {
        if (Recive_User == Manager.Instance.User.Nick_Name)
        {/*
            move.Instance.Is_Trigger = true;
            StartCoroutine(Hit_Move_Trigger_End(force));*/
        }
    }

    [PunRPC] //추후 수정 아직미완성.
    public void Hit_User_Rotate(string Recive_User, float force)
    {
        if (Recive_User == Manager.Instance.User.Nick_Name)
        {/*
            move.Instance.Trigger_From = move.Instance.transform.localPosition;
            move.Instance.Trigger_to = new Vector3(move.Instance.transform.localPosition.x, move.Instance.transform.localPosition.y + force, move.Instance.transform.localPosition.z - force);
            move.Instance.Is_Trigger = true;
            Animation_Synchronize.Instance.pv_.RPC("Char_Anim_Sync", PhotonTargets.All, Animation_Synchronize.Ainmation_State.Death, PhotonNetwork.player.NickName);*/
        }
    }
    /*
    IEnumerator Hit_Move_Trigger_End(float time)
    {
        yield return new WaitForSeconds(time);
        move.Instance.Is_Trigger = false;
        move.Instance.moveDirection = Vector3.zero;
        Game_Manager.Instance.My_Unit.GetComponent<Animation_Synchronize>().pv_.RPC("Char_Anim_Sync", PhotonTargets.All, Animation_Synchronize.Ainmation_State.Wait, PhotonNetwork.player.NickName);
    }
    */
    [PunRPC]
    public void Kill_Log_Send(string Recive_User)
    {
        if (Recive_User == Manager.Instance.User.Nick_Name)
        {
            Character_State.Instance.Kill_Count += 1;
        }
    }

    //Ability_2002 트리거
    GameObject Active_Unit;
    string Active_User_;

    [PunRPC]
    public void Ability2002_Trigger(string Active_User,int time)
    {
        Active_User_ = Active_User;
        GameObject[] Unit=GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject i in Unit)
        {
            Active_Unit = i; //누구 오브젝트인지 찾고 기억
            if (Active_Unit.GetComponent<PhotonView>().owner.NickName == Active_User)
            {
                foreach (SkinnedMeshRenderer render in i.transform.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    if (Active_User_ != Manager.Instance.User.Nick_Name)
                    {
                        render.enabled = false;
                    }

                    Invoke("Ability_2002_Callback", time);
                }
            }
        }
    }

    void Ability_2002_Callback()
    {
        foreach (SkinnedMeshRenderer render in Active_Unit.transform.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            if (Active_User_ != Manager.Instance.User.Nick_Name)
            {
                render.enabled = true;
            }
        }
    }

    [PunRPC]
    public void Ability2003_Trigger_Send(string recive, Vector3 pos)
    {
        if (recive == Manager.Instance.User.Nick_Name) //이 능력을 발동시킨 유저의 경우 해당 진행
        {
            Character_State.Instance.gameObject.transform.position = pos;
        }
    }

    GameObject Dark_Screen;

    [PunRPC]
    public void Ability2004_Trigger_Start(string Recive)
    {
        if (Recive == Manager.Instance.User.Nick_Name)
        {
            if (Dark_Screen == null)
            {
                Dark_Screen = GameObject.Instantiate(Resources.Load<GameObject>("Skill_\\Skill_Resources\\Dark_Screen"), GamePlay_Canvas.Instance.transform.position, Quaternion.identity);
                Dark_Screen.transform.parent = GamePlay_Canvas.Instance.transform;
                Dark_Screen.transform.localPosition = Vector3.zero;
                Dark_Screen.transform.localScale = Vector3.one;
            }
            else
            {
                Dark_Screen.SetActive(true);
                Dark_Screen.GetComponent<TweenAlpha>().PlayForward();
            }
        }
    }

    [PunRPC]
    public void Ability2004_Trigger_End(string Recive)
    {
        if (Recive == Manager.Instance.User.Nick_Name)
        {
            if (Dark_Screen != null)
            {
                Dark_Screen.SetActive(false);
            }
        }
    }

    [PunRPC]
    public void Ability3002_Trigger_Send(string Send_User)
    {
        Instance.pv_.RPC("Ability3002_Trigger_Recive", PhotonTargets.All, Character_State.Instance.My_Character_Info.HP, Send_User, Manager.Instance.User.Nick_Name);
    }

    public int Ability_3002_count = 0;
    
    [PunRPC]
    public void Ability3002_Trigger_Recive(int hp, string recive, string send)
    {
        if (recive == Manager.Instance.User.Nick_Name) //이 능력을 발동시킨 유저의 경우 해당 진행
        {
            if (Ability_3002_count > Character_State.Instance.My_Character_Info.Row_Skill_Level)
            {
                ++Ability_3002_count;
                Chatting_Canvas.Instance.Recive_Chat("", "User : " + send + " | HP - " + hp, Chatting_Canvas.Chat_Type.Self);
            }
        }
    }
}
