using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_2003_Trigger : MonoBehaviour {

    int Portal_Number = 1;
    public static GameObject Portal_1;
    public static GameObject Portal_2;

    PhotonView pv_;

    private void Awake()
    {
        pv_ = this.GetComponent<PhotonView>();
    }

    // Use this for initialization
    void Start()
    {
        if (pv_.isMine)
        {
            if (Portal_1 == null && this.GetComponent<PhotonView>().isMine)
            {
                Portal_1 = this.gameObject;
                Portal_Number = 1;
            }
            else if (Portal_2 == null && this.GetComponent<PhotonView>().isMine)
            {
                Portal_2 = this.gameObject;
                Portal_Number = 2;
            }
            else if (this.GetComponent<PhotonView>().isMine)
            {
                PhotonNetwork.Destroy(Portal_1);
                PhotonNetwork.Destroy(Portal_2);
                Portal_1 = this.gameObject;
                Portal_Number = 1;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Portal_1 != null && Portal_2 != null)
        {
            if (other.tag == "Player") 
            {
                if (pv_)
                {
                    if (Portal_Number == 1)
                    {
                        Ability_Trigger.Instance.pv_.RPC("Ability2003_Trigger_Send", PhotonTargets.All, other.GetComponent<PhotonView>().owner.NickName, Portal_2.transform.position + (Character_State.Instance.gameObject.transform.forward * 3));
                    }
                    else
                    {
                        Ability_Trigger.Instance.pv_.RPC("Ability2003_Trigger_Send", PhotonTargets.All, other.GetComponent<PhotonView>().owner.NickName, Portal_1.transform.position + (Character_State.Instance.gameObject.transform.forward * 3));
                    }

                    if (Portal_1 != null)
                        PhotonNetwork.Destroy(Portal_1);

                    if (Portal_2 != null)
                        PhotonNetwork.Destroy(Portal_2);
                }
            }
        }
    }
}
