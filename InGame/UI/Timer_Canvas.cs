using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer_Canvas : Photon.PunBehaviour {

    public static Timer_Canvas Instance;

    PhotonView pv;

    public int Timer;
    [HideInInspector]
    public int Ability_Set_Timer=0;

    public bool IsAbility = false; // 능력 할당됬는지 파악

    public UILabel Timer_Text;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        pv = this.GetComponent<PhotonView>();
        IsAbility = false;
    }

    public void Open()
    {
        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    public void Timer_Count()
    {
        if (Timer > 0)
        {
            Timer--;
        }
        else
        {
            int[] null_list=new int[1]; //의미없는 쓰레기값
            //Game_Manager.Instance.pv.RPC("Result_Set", PhotonTargets.All, null_list, true);
        }
        pv.RPC("Timer_Set", PhotonTargets.All, Timer);

        if (Timer < Ability_Set_Timer && IsAbility == false)
        {
            IsAbility = true;
            pv.RPC("Call_High_Ability_Set", PhotonTargets.All);
        }

        Invoke("Timer_Count", 1.0f);
    }

    [PunRPC]
    public void Timer_Set(int Time_Vlaue)
    {
        Timer_Text.text = Time_Vlaue / 60 + " : " + Time_Vlaue % 60;
    }

    [PunRPC]
    public void Call_High_Ability_Set()
    {
        Game_Manager.Instance.High_Ability_Set();
        Notice_Canvas.Instance.Notice_("지금부터 상급 능력이 개방 됩니다.", 3.0f);
    }
}
