using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthQueke_Trigger : MonoBehaviour {

    PhotonView pv_;
    ParticleSystem ps_;

    BoxCollider col_;

    public int level = 1;
    int damege;

    private void Start()
    {
        pv_ = this.GetComponent<PhotonView>();
        ps_ = this.GetComponent<ParticleSystem>();
        if (pv_.isMine)
        {
            level = Character_State.Instance.My_Character_Info.High_Skill_Script.Skill_Level;
            damege = Character_State.Instance.My_Character_Info.High_Skill_Script.Damege;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!ps_.IsAlive())
        {
            Destroy_Notice();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (this.GetComponent<PhotonView>().isMine)
        {
            if (other.tag == "Player")
            {
                if (other.GetComponent<PhotonView>().owner.NickName != PhotonNetwork.player.NickName)
                {
                    Ability_Trigger.Instance.pv_.RPC("HP_Subtract", PhotonTargets.All, damege, other.gameObject.GetComponent<PhotonView>().owner.NickName, Manager.Instance.User.Nick_Name);
                    Ability_Trigger.Instance.pv_.RPC("User_Condition_State", PhotonTargets.All, other.GetComponent<PhotonView>().owner.NickName, Condition.Condition_State.Stun, 2, (level / 2 + 1), 1003);
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
