using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chatting_Canvas : MonoBehaviour {

    public static Chatting_Canvas Instance;

    [HideInInspector]
    public PhotonView pv_;

    public GameObject Chat_View;
    public UIInput Chat_Label;

    public enum Chat_Type
    {
        All,
        My_Team,
        Self
    } 

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    // Use this for initialization
    void Start () {
        pv_ = this.GetComponent<PhotonView>();
	}

    public void Chat_Send()
    {
        pv_.RPC("Recive_Chat", PhotonTargets.All, Manager.Instance.User.Nick_Name, Chat_Label.value, Chat_Type.All);
        Chat_Label.value = "";
    }

    [PunRPC]
    public void Recive_Chat(string name, string Msg, Chat_Type Type_)
    {
        //추후 팀전인지 모두말인지 등등 파악해서 구분 보이기.
        GameObject Msg_object = GameObject.Instantiate(Resources.Load<GameObject>("Prefeb\\User_Chat"), Vector3.one, Quaternion.identity);
        Msg_object.transform.parent = Chat_View.transform;
        Msg_object.transform.localPosition = Vector3.one;
        Msg_object.transform.localScale = Vector3.one;
        Chat_View.GetComponent<UIGrid>().Reposition();

        switch (Type_)
        {
            case Chat_Type.All:
                Msg_object.GetComponent<UILabel>().text = "[FF0064][All][-] " + "[3232FF]" + name + "[-]\n" + "[FFFFFF]" + Msg + "[-]";
                break;

            case Chat_Type.My_Team:
                Msg_object.GetComponent<UILabel>().text = "[FF0064][Team][-] " + "[3232FF]" + name + "[-]\n" + "[FFFFFF]" + Msg + "[-]";
                break;

            case Chat_Type.Self:
                Msg_object.GetComponent<UILabel>().text = "[FF0064][System][-]" + Msg;
                break;
        }
    }
}
