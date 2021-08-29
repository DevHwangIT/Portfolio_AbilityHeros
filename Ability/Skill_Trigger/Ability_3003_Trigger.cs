using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_3003_Trigger : MonoBehaviour {

    PhotonView pv_;

    // Use this for initialization
    void Start ()
    {
        pv_ = this.GetComponent<PhotonView>();
        if (pv_.isMine)
        {
            Invoke("Destroy_Notice", Character_State.Instance.My_Character_Info.Row_Skill_Script.Keep_Time);
        }
    }


    void Destroy_Notice()
    {
        if (pv_.isMine)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
