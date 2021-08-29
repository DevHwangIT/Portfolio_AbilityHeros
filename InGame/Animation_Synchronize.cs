using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Synchronize : Photon.PunBehaviour {
    
    public enum Ainmation_State
    {
        Wait = 0,
        Run = 1,
        Hit = 2,
        Attack = 3,
        Jump = 6,
        NuckBack=7,
        Throw = 8,
        Gard = 9,
        Death=10,

        Sky_Hands = 20,
        Get_Move = 21,
        Magic_Attack = 22,
        Skill_Physical_Buff = 23,
        Skill_Magic_Buff = 24,
        Abnormal = 25,
        Prayer = 26,
        Magic_Book_Failed = 27,
        Magic_Book_Success = 28,
        Trascendece = 29,
        Hard_Attack=30,

        Win_Pose_1 = 501,
        Win_Pose_2 = 502,
        Win_Pose_3 = 503,
        Win_Pose_4 = 504,
        Win_Pose_5 = 505,
    }

    public Ainmation_State Present_State;

    Animator animator_;
    public PhotonView pv_;
    AudioSource audio_;

    public AudioClip Attack1_Clip;
    public AudioClip Attack2_Clip;
    public AudioClip Attack3_Clip;
    
    private void Awake()
    {
        audio_ = this.GetComponent<AudioSource>();
        animator_ = this.GetComponent<Animator>();
        pv_ = this.GetComponent<PhotonView>();
        Attack1_Clip = Resources.Load<AudioClip>("Audio\\Fight_Sound\\Attack1");
        Attack2_Clip = Resources.Load<AudioClip>("Audio\\Fight_Sound\\Attack2");
        Attack3_Clip = Resources.Load<AudioClip>("Audio\\Fight_Sound\\Attack3");
        Present_State = Ainmation_State.Wait;
    }

    [PunRPC]
    public void Char_Anim_Sync(Ainmation_State state, string ID)
    { //캐릭터 애니메이션 동기화.
        if (pv_.owner.NickName == ID)  {
            animator_.SetInteger("State", (int)state);
            Present_State = state;
        }
    }

    [PunRPC]
    public void Attack_Anim_Sync(string ID, int attack_)
    { //캐릭터 애니메이션 동기화.
        if (pv_.owner.NickName == ID)
        {
            switch (attack_)
            {
                case 0:
                    animator_.SetInteger("Attack_Combo", 0);
                    audio_.clip = Attack1_Clip;
                    if (!audio_.isPlaying)
                        audio_.Play();
                    break;

                case 1:
                    animator_.SetInteger("Attack_Combo", 1);
                    audio_.clip = Attack2_Clip;
                    if (!audio_.isPlaying)
                        audio_.Play();
                    break;

                case 2:
                    animator_.SetInteger("Attack_Combo", 2);
                    audio_.clip = Attack3_Clip;
                    if (!audio_.isPlaying)
                        audio_.Play();
                    break;
            }
        }
    }
}
