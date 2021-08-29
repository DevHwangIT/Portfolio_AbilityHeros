using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Stone_Trigger : MonoBehaviour {

    float Throw_Time = 10.0f;
    float Throw_Speed = 10.0f;
    float Roate_Speed = 100.0f;

    AudioSource audio_;
    PhotonView pv_;

    public int level = 1;
    int damege;

    Transform Target_Pos;

    private void Start()
    {
        pv_ = this.GetComponent<PhotonView>();
        if (pv_.isMine)
        {
            level = Character_State.Instance.My_Character_Info.Row_Skill_Script.Skill_Level;
            damege = Character_State.Instance.My_Character_Info.Row_Skill_Script.Damege;
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector3.forward * Time.deltaTime * Throw_Speed, Space.Self);
    }

    void Destroy_Notice()
    {
        pv_.RPC("Destroy_Object", PhotonTargets.All, pv_.owner.NickName, pv_.viewID);
    }

    [PunRPC]
    void Destroy_Object(string Nick,int ID)
    {
        if (pv_.owner.NickName == Nick)
        {
            if (pv_.viewID == ID)
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
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
                    Ability_Trigger.Instance.pv_.RPC("HP_Subtract", PhotonTargets.All, damege, other.gameObject.GetComponent<PhotonView>().owner.NickName, Manager.Instance.User.Nick_Name);
                    if (Random.Range(1, 100) < (1 + level)) // % 확률로 기절 시킴.
                        Ability_Trigger.Instance.pv_.RPC("User_Condition_State", PhotonTargets.All, other.GetComponent<PhotonView>().owner.NickName, Condition.Condition_State.Stun, 1, 1, 4001);
                }
            }

            if (other.GetComponent<PhotonView>().owner.NickName == Manager.Instance.User.Nick_Name)
            {
                return;
            }
            else
            {
                Destroy_Notice();
            }
        }
    }
}
