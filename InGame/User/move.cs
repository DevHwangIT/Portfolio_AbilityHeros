using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : Photon.PunBehaviour {

    public static move Instance;

    public float speed = 4.0f;

    public float Jump_Force = 8.0f;

    public bool Is_Trigger = false;

    [HideInInspector]
    public Animator animator_;
    [HideInInspector]
    public CharacterController controller;
    AudioSource audio_;

    public Ability Skill_1;
    public Ability Skill_2;

    public float rotationX = 0F;
    public float gravity = 40.0f;
    public Vector3 moveDirection = Vector3.zero;
    
    public Vector3 joystick_axis;

    public UIButton Attack_Button;
    public UIButton Jump_Button;
    public UIButton Gard_Button;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        animator_ = this.GetComponent<Animator>();
        controller = this.GetComponent<CharacterController>();
        Attack_Button = GameObject.Find("Attack_Btn").GetComponent<UIButton>();
        Jump_Button = GameObject.Find("Jump_Btn").GetComponent<UIButton>();
        Gard_Button = GameObject.Find("Gard_Btn").GetComponent<UIButton>();
        Event_Setup();
    }

    void Event_Setup()
    {
        EventDelegate Attack_eventBtn = new EventDelegate(gameObject.GetComponent<move>(), "Attack_Btn");
        EventDelegate.Add(Attack_Button.GetComponent<UIButton>().onClick, Attack_eventBtn);
        EventDelegate Jump_eventBtn = new EventDelegate(gameObject.GetComponent<move>(), "Jump_Btn");
        EventDelegate.Add(Jump_Button.GetComponent<UIButton>().onClick, Jump_eventBtn);
        EventDelegate Gard_Press_eventBtn = new EventDelegate(gameObject.GetComponent<move>(), "Press_Gard_Btn");
        EventDelegate.Add(Gard_Button.GetComponent<UIEventTrigger>().onPress, Gard_Press_eventBtn);
        EventDelegate Gard_Release_eventBtn = new EventDelegate(gameObject.GetComponent<move>(), "Release_Gard_Btn");
        EventDelegate.Add(Gard_Button.GetComponent<UIEventTrigger>().onRelease, Gard_Release_eventBtn);
        audio_ =this.GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (Is_Trigger == false && Press_Btn == false && isGard == false)
        {
            //사용자가 입력값 확인 후 그에 따른 걷기 애니메이션 동작
            if (joystick_axis != Vector3.zero)
            {
                Game_Manager.Instance.My_Unit.GetComponent<Animation_Synchronize>().pv_.RPC("Char_Anim_Sync", PhotonTargets.All, Animation_Synchronize.Ainmation_State.Run, PhotonNetwork.player.NickName);
            }
            else
            {
                if (animator_.GetInteger("State") == 1) //달리기 무한 루프상태막기.
                {
                    Game_Manager.Instance.My_Unit.GetComponent<Animation_Synchronize>().pv_.RPC("Char_Anim_Sync", PhotonTargets.All, Animation_Synchronize.Ainmation_State.Wait, PhotonNetwork.player.NickName);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Is_Trigger == false)
        {
            if (isGard == false && Press_Btn == false)//가드 상태가 아닐경우에 움직임 처리.
            {
                moveDirection = new Vector3(joystick_axis.x, 0, joystick_axis.y);
                moveDirection *= speed;

                if (moveDirection.x != 0 || moveDirection.z != 0)
                    this.transform.rotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z));// * Time.smoothDeltaTime)
            }
            else
            {
                moveDirection = Vector3.zero;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime * 10.0f;
        controller.Move(moveDirection * Time.deltaTime);
    }
    
    public void Jump_Btn()
    {
        if (controller.isGrounded)
        {
            Game_Manager.Instance.My_Unit.GetComponent<Animation_Synchronize>().pv_.RPC("Char_Anim_Sync", PhotonTargets.All, Animation_Synchronize.Ainmation_State.Jump, PhotonNetwork.player.NickName);
            moveDirection.y = Jump_Force;
        }
    }

    bool routine_Isplaying = false; //공격 코루틴 실행중인지 파악을 위한 Boolean
    public bool Press_Btn = false; //콤보 코루틴 실행중인지 파악을 위한 Boolean

    public void Attack_Btn()
    {
        if (Is_Trigger == false) //피격중 혹은 트리거 상태가 아닐떄.
        {
            Press_Btn = true;
            Game_Manager.Instance.My_Unit.GetComponent<Animation_Synchronize>().pv_.RPC("Char_Anim_Sync", PhotonTargets.All, Animation_Synchronize.Ainmation_State.Attack, PhotonNetwork.player.NickName);
            if (routine_Isplaying == false)
            {
                StartCoroutine(Attack_Delay());
            }
        }
    }

    public bool isGard;

    public void Press_Gard_Btn()
    {
        if (isGard == false && Game_Manager.Instance.My_Unit.GetComponent<Animation_Synchronize>().Present_State != Animation_Synchronize.Ainmation_State.Hit && Is_Trigger == false)
        {
            Game_Manager.Instance.My_Unit.GetComponent<Animation_Synchronize>().pv_.RPC("Char_Anim_Sync", PhotonTargets.All, Animation_Synchronize.Ainmation_State.Gard, PhotonNetwork.player.NickName);
            isGard = true;
        }
    }

    public void Release_Gard_Btn()
    {
        if (isGard == true)
        {
            Game_Manager.Instance.My_Unit.GetComponent<Animation_Synchronize>().pv_.RPC("Char_Anim_Sync", PhotonTargets.All, Animation_Synchronize.Ainmation_State.Wait, PhotonNetwork.player.NickName);
            isGard = false;
        }
    }

    public delegate void Trigger_();
    public Trigger_ Collider_On_1;
    public Trigger_ Collider_On_2;
    public Trigger_ Collider_On_3; //오른발
    public Trigger_ Collider_On_4; //왼발
    public Trigger_ Collider_Off;
     
    IEnumerator Attack_Delay()
    {
        routine_Isplaying = true;

        if (!audio_.isPlaying)
            audio_.Play();

        if (animator_.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Dash_Attack(Move)")
        {
            if (animator_.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Attack1_1")
            {
                Game_Manager.Instance.My_Unit.GetComponent<Animation_Synchronize>().pv_.RPC("Attack_Anim_Sync", PhotonTargets.All, PhotonNetwork.player.NickName, 1);
                Collider_On_2(); //어택 주먹 박스 콜라이더 활성화
            }
            else if (animator_.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Attack1_2")
            {
                Game_Manager.Instance.My_Unit.GetComponent<Animation_Synchronize>().pv_.RPC("Attack_Anim_Sync", PhotonTargets.All, PhotonNetwork.player.NickName, 2);
                Collider_On_3(); //어택 주먹 박스 콜라이더 활성화
            }
            else
            {
                Game_Manager.Instance.My_Unit.GetComponent<Animation_Synchronize>().pv_.RPC("Attack_Anim_Sync", PhotonTargets.All, PhotonNetwork.player.NickName, 0);
                Collider_On_1(); //어택 주먹 박스 콜라이더 활성화
            }
        }
        else
        {
            Collider_On_3();
            UIJoystick.Instance.GetComponent<SphereCollider>().enabled = false;
        }

        Press_Btn = false;

        yield return new WaitForSeconds(animator_.GetCurrentAnimatorClipInfo(0)[0].clip.length / 2);//애니메이션 재생시간동안에는 공격 코루틴 호출 불가하게 설정

        routine_Isplaying = false; // 대략적인 코루틴이 끝나서 처리가 가능함을 알림.

        UIJoystick.Instance.GetComponent<SphereCollider>().enabled = true;

        if (Press_Btn == true)
        { //버튼이 눌렸을 경우.
            Press_Btn = false; //누른 것을 처리했으므로 다시 안눌린 상태로 변환.
            Attack_Btn(); // 다음 공격 행동 호출
        }
        else
        {
            Game_Manager.Instance.My_Unit.GetComponent<Animation_Synchronize>().pv_.RPC("Char_Anim_Sync", PhotonTargets.All, Animation_Synchronize.Ainmation_State.Wait, PhotonNetwork.player.NickName); //애니메이션 초기화
            Game_Manager.Instance.My_Unit.GetComponent<Animation_Synchronize>().pv_.RPC("Attack_Anim_Sync", PhotonTargets.All, PhotonNetwork.player.NickName, 0); //공격모션 초기화
            Collider_Off(); //어택 주먹 박스 콜라이더 비활성화
        }
    }
}
