using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch_Colider : MonoBehaviour {
    
    int Damega = 3;

    Animator root_animator;
    PhotonView trigger_pv;
    PhotonView animation_pv;
    AudioSource audio_;
    AudioClip[] Clip;

    public Attack_Type Attack_Part;

    public enum Attack_Type
    {
        Left_Punch=0,
        Right_Punch = 1,
        Left_Leg = 2,
        Right_Leg = 3,
    }

    private void Awake()
    {
        audio_ = this.gameObject.AddComponent<AudioSource>();
        Clip = new AudioClip[3];
        Clip[0] = Resources.Load<AudioClip>("Audio\\Fight_Sound\\Hit1");
        Clip[1] = Resources.Load<AudioClip>("Audio\\Fight_Sound\\Hit2");
        Clip[2] = Resources.Load<AudioClip>("Audio\\Fight_Sound\\Hit3");
        root_animator = transform.root.GetComponent<Animator>();
        audio_.playOnAwake = false;
    }

    private void Start()
    {
        trigger_pv = Ability_Trigger.Instance.pv_;
        animation_pv = Game_Manager.Instance.My_Unit.GetComponent<Animation_Synchronize>().pv_;
        if (this.transform.root.gameObject.GetComponent<PhotonView>().isMine == true) // 주먹에 있는 콜라이더가 내캐릭터에만 이벤트 등록.
        {
            switch (Attack_Part)
            {
                case Attack_Type.Right_Punch:
                    move.Instance.Collider_On_1 += new move.Trigger_(Collider_Setactive_On);
                    break;

                case Attack_Type.Left_Punch:
                    move.Instance.Collider_On_2 += new move.Trigger_(Collider_Setactive_On);
                    break;

                case Attack_Type.Left_Leg:
                    move.Instance.Collider_On_4 += new move.Trigger_(Collider_Setactive_On);
                    break;

                case Attack_Type.Right_Leg:
                    move.Instance.Collider_On_3 += new move.Trigger_(Collider_Setactive_On);
                    break;

            }
            move.Instance.Collider_Off += new move.Trigger_(Collider_Setactive_Off);
            move.Instance.Collider_Off();
        }
        else // 아닐 경우 콜라이더를 항상 꺼둠.
        {
            this.enabled = false;
            Collider_Setactive_Off();
        }
    }

    void Collider_Setactive_On()
    {
        this.gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    void Collider_Setactive_Off()
    {
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
    }


    //해당 스크립트는 캐릭터마다 부여되어있어서 IsMine으로 처리할경우 에러발생.
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.GetComponent<PhotonView>().owner.NickName != Manager.Instance.User.Nick_Name)
            {
                int rand = Random.Range(1, 3);
                trigger_pv.RPC("Punch_Effect", PhotonTargets.All, this.transform.position, rand);

                if (other.GetComponent<Animation_Synchronize>().Present_State != Animation_Synchronize.Ainmation_State.Gard)
                {
                    trigger_pv.RPC("HP_Subtract", PhotonTargets.All, Damega, other.gameObject.GetComponent<PhotonView>().owner.NickName, Manager.Instance.User.Nick_Name);
                    animation_pv.RPC("Char_Anim_Sync", PhotonTargets.All, Animation_Synchronize.Ainmation_State.Hit, other.gameObject.GetComponent<PhotonView>().owner.NickName); //애니메이션 초기화
                }
                else
                {
                    Damega = (int)(Damega * 0.1f);
                    if (Damega <= 1)
                        Damega = 1;
                    trigger_pv.RPC("HP_Subtract", PhotonTargets.All, Damega, other.gameObject.GetComponent<PhotonView>().owner.NickName, Manager.Instance.User.Nick_Name);
                }

                audio_.clip = Clip[rand];
                if (!audio_.isPlaying)
                    audio_.Play();
            }
        }
    }
}
