using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado_Trigger : MonoBehaviour {

    float Tornado_Speed = 5.0f;
    AudioSource audio_;
    PhotonView pv_;

    public int level = 1;
    int damege;
    
    bool IsDot_Time;

    private void Start()
    {
        pv_ = this.GetComponent<PhotonView>();
        if (pv_.isMine)
        {
            Invoke("Destroy_Notice", Character_State.Instance.My_Character_Info.High_Skill_Script.Keep_Time);
            level = Character_State.Instance.My_Character_Info.High_Skill_Script.Skill_Level;
            damege = Character_State.Instance.My_Character_Info.High_Skill_Script.Damege;
            IsDot_Time = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector3.forward * Time.deltaTime * Tornado_Speed, Space.Self);
    }
    
    void OnTriggerStay(Collider other)
    {
        if (this.GetComponent<PhotonView>().isMine)
        {
            if (other.tag == "Player")
            {
                if (other.GetComponent<PhotonView>().owner.NickName != PhotonNetwork.player.NickName)
                {
                    if (IsDot_Time == true)
                    {
                        IsDot_Time = false;
                        if (!IsInvoking("IsDot_Set"))
                            Invoke("IsDot_Set", 0.2f);
                        Ability_Trigger.Instance.pv_.RPC("HP_Subtract", PhotonTargets.All, damege, other.gameObject.GetComponent<PhotonView>().owner.NickName, Manager.Instance.User.Nick_Name);
                        other.GetComponent<Animation_Synchronize>().pv_.RPC("Char_Anim_Sync", PhotonTargets.All, Animation_Synchronize.Ainmation_State.Hit, other.gameObject.GetComponent<PhotonView>().owner.NickName);
                    }
                }
            }
        }
    }

    void IsDot_Set()
    {
        IsDot_Time = true;
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
