using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_3005_Trigger : MonoBehaviour {

    float Throw_Time = 10.0f;
    float Throw_Speed = 10.0f;
    float Roate_Speed = 100.0f;

    AudioSource audio_;
    PhotonView pv_;

    public int keep_time = 1;

    Transform Target_Pos;

    private void Start()
    {
        pv_ = this.GetComponent<PhotonView>();
        if (pv_.isMine)
        {
            keep_time = Character_State.Instance.My_Character_Info.Row_Skill_Script.Keep_Time;
            Invoke("Destroy_Notice", 30.0f);
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
    void Destroy_Object(string Nick, int ID)
    {
        if (pv_.owner.NickName == Nick)
        {
            if (pv_.viewID == ID)
            {
                if (this.gameObject)
                {
                    PhotonNetwork.Destroy(this.gameObject);
                }
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
                    Ability_Trigger.Instance.pv_.RPC("User_Condition_State", PhotonTargets.All, other.GetComponent<PhotonView>().owner.NickName, Condition.Condition_State.fascination, keep_time, 0, 3005);
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
