using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition : MonoBehaviour {
    
    public enum Condition_State //캐릭터의 상태이상을 나타냄.
    {
        None_Hit = 0, //일반적인 피격 효과
        Sleep = 1, //수면 효과
        Confusion = 2, // 혼란 상태 효과
        Freeze = 3, // 얼음 상태 효과
        Burn = 4, //화상 상태 효과
        Shock = 5, //감전 상태 효과
        fascination = 6, //매혹 상태 효과
        Blooding = 7, // 블러딩 상태 효과
        Stun = 8, //기절 상태 효과
        Air_Bone = 9, //공중 상태 효과
        Heling = 10, //체력 회복 효과
        SpeedUP = 11, //움직임 증가 효과
        None=50
    }

    public void Set(string sprite,int time, int damege, Condition_State Type)
    {
        Icon_Sprite = sprite;
        Time_ = time;
        Present_Time = time;
        Dot_Damege = damege;
        State = Type;
    }

    GameObject UI_Icon;
    GameObject Particle_Object;

    string Icon_Sprite;
    float Time_ = 1;
    float Present_Time = 1;
    int Dot_Damege = 1;
    Condition_State State;
    
    // Use this for initialization
    public void Run()
    {
        Vector3 Effect_pos = move.Instance.transform.position;

        UI_Icon = Instantiate(Resources.Load<GameObject>("Prefeb\\Buff"), Vector3.one, Quaternion.identity);
        UI_Icon.transform.parent = GamePlay_Canvas.Instance.Buff_View;
        UI_Icon.transform.localPosition = Vector3.one;
        UI_Icon.transform.localScale = Vector3.one;
        GamePlay_Canvas.Instance.Buff_View.GetComponent<UIGrid>().Reposition();
        UI_Icon.transform.GetChild(0).GetComponent<UISprite>().spriteName = Icon_Sprite;

        switch (State)
        {
            case Condition.Condition_State.None_Hit:
                break;

            case Condition.Condition_State.Air_Bone:
                move.Instance.Is_Trigger = true;
                Effect_pos.y += 1;
                Particle_Object = PhotonNetwork.Instantiate("Condition_State\\Stun", Effect_pos, Quaternion.identity, 0);
                Game_Manager.Instance.My_Unit.GetComponent<Animation_Synchronize>().pv_.RPC("Char_Anim_Sync", PhotonTargets.All, Animation_Synchronize.Ainmation_State.Abnormal, Manager.Instance.User.Nick_Name);
                break;

            case Condition.Condition_State.Blooding:
                Particle_Object = PhotonNetwork.Instantiate("Condition_State\\Blooding", Effect_pos, Quaternion.identity, 0);
                break;

            case Condition.Condition_State.Sleep:
                move.Instance.Is_Trigger = true;
                Particle_Object = PhotonNetwork.Instantiate("Condition_State\\Sleep", Effect_pos, Quaternion.identity, 0);
                break;

            case Condition.Condition_State.Burn:
                Particle_Object = PhotonNetwork.Instantiate("Condition_State\\Burn", Effect_pos, Quaternion.identity, 0);
                break;

            case Condition.Condition_State.Confusion:
                move.Instance.Is_Trigger = true;
                Effect_pos.y += 1;
                Particle_Object = PhotonNetwork.Instantiate("Condition_State\\Confusion", Effect_pos, Quaternion.identity, 0);
                Game_Manager.Instance.My_Unit.GetComponent<Animation_Synchronize>().pv_.RPC("Char_Anim_Sync", PhotonTargets.All, Animation_Synchronize.Ainmation_State.Abnormal, Manager.Instance.User.Nick_Name);
                break;

            case Condition.Condition_State.fascination:
                move.Instance.Is_Trigger = true;
                Effect_pos.y += 1;
                Particle_Object = PhotonNetwork.Instantiate("Condition_State\\Fascination", Effect_pos, Quaternion.identity, 0);
                Game_Manager.Instance.My_Unit.GetComponent<Animation_Synchronize>().pv_.RPC("Char_Anim_Sync", PhotonTargets.All, Animation_Synchronize.Ainmation_State.Abnormal, Manager.Instance.User.Nick_Name);
                break;

            case Condition.Condition_State.Freeze:
                move.Instance.Is_Trigger = true;
                Effect_pos.y += 1;
                Particle_Object = PhotonNetwork.Instantiate("Condition_State\\Fascination", Effect_pos, Quaternion.identity, 0);
                Game_Manager.Instance.My_Unit.GetComponent<Animation_Synchronize>().pv_.RPC("Char_Anim_Sync", PhotonTargets.All, Animation_Synchronize.Ainmation_State.Abnormal, Manager.Instance.User.Nick_Name);
                break;

            case Condition.Condition_State.Shock:
                move.Instance.Is_Trigger = true;
                Particle_Object = PhotonNetwork.Instantiate("Condition_State\\Shock", Effect_pos, Quaternion.identity, 0);
                break;

            case Condition.Condition_State.Stun:
                move.Instance.Is_Trigger = true;
                Effect_pos.y += 1;
                Particle_Object = PhotonNetwork.Instantiate("Condition_State\\Stun", Effect_pos, Quaternion.identity, 0);
                Game_Manager.Instance.My_Unit.GetComponent<Animation_Synchronize>().pv_.RPC("Char_Anim_Sync", PhotonTargets.All, Animation_Synchronize.Ainmation_State.Abnormal, Manager.Instance.User.Nick_Name);
                break;

            case Condition.Condition_State.Heling:
                Particle_Object = PhotonNetwork.Instantiate("Condition_State\\Heling", Effect_pos, Quaternion.identity, 0);
                break;

            case Condition.Condition_State.SpeedUP:
                Particle_Object = PhotonNetwork.Instantiate("Condition_State\\SpeedUp", Effect_pos, Quaternion.identity, 0);
                break;

            case Condition.Condition_State.None:
                break;
        }

        Invoke("State_Timer", 1.0f);
    }

    void State_Timer()
    {
        Present_Time--;
        UI_Icon.GetComponent<UISprite>().fillAmount = Present_Time / Time_;

        if (Present_Time >= 1)
        {
            Invoke("State_Timer", 1.0f);
        }
        else
        {
            Call_Destroy();
        }
    }

    void Call_Destroy()
    {
        if (Character_State.Instance.My_Character_Info.HP > 0)
            Game_Manager.Instance.My_Unit.GetComponent<Animation_Synchronize>().pv_.RPC("Char_Anim_Sync", PhotonTargets.All, Animation_Synchronize.Ainmation_State.Wait, Manager.Instance.User.Nick_Name);
        move.Instance.Is_Trigger = false;
        Character_State.Instance.Unit_Condition_List.Remove(this);
        Destroy(UI_Icon);
        PhotonNetwork.Destroy(Particle_Object);
    }
}
