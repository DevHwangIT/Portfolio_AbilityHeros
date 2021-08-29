using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_2004_Trigger : MonoBehaviour {

    PhotonView pv_;
    ParticleSystem ps_;
    
    private void Start()
    {
        pv_ = this.GetComponent<PhotonView>();
        if (pv_.isMine)
        {
            Invoke("Destroy_Notice", Character_State.Instance.My_Character_Info.High_Skill_Script.Keep_Time);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (this.GetComponent<PhotonView>().isMine)
        {
            if (other.tag == "Player")
            {
                if (other.GetComponent<PhotonView>().owner.NickName != PhotonNetwork.player.NickName)
                {
                    Ability_Trigger.Instance.pv_.RPC("Ability2004_Trigger_Start", PhotonTargets.All, other.GetComponent<PhotonView>().owner.NickName);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (this.GetComponent<PhotonView>().isMine)
        {
            if (other.tag == "Player")
            {
                if (other.GetComponent<PhotonView>().owner.NickName != PhotonNetwork.player.NickName)
                {
                    Ability_Trigger.Instance.pv_.RPC("Ability2004_Trigger_End", PhotonTargets.All, other.GetComponent<PhotonView>().owner.NickName);
                }
            }
        }
    }

    void Destroy_Notice()
    {
        pv_.RPC("Destroy_Object", PhotonTargets.All, pv_.owner.NickName, pv_.viewID);
    }

    [PunRPC]
    void Destroy_Object(string Nick, int ID)
    {
        if (pv_.owner.NickName == Nick)
        {
            if (pv_.viewID == ID)
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }
}
