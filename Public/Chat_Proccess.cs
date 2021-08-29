using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chat_Proccess : Photon.PunBehaviour {

    public static Chat_Proccess Instance;

    /*
     900= Lobby-View_ID
     901= Room_View_ID
     902= InGame_View_ID
     */

    public static List<string> Room_Chat_List = new List<string>(); //방안에서의 채팅 리스트 배열
    public static List<string> InGame_Chat_List = new List<string>(); //게임 내부의 채팅 리스트 배열

    [Header("Lobby Scence")]
    public GameObject Room_Chat_View;

    [Header("InGame Scence")]
    public GameObject InGame_Chat_View;

    [Header("Public Object")]
    public UIInput Chat_InputField;
    public UILabel Chat_Text;

    PhotonView PV = null;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
    }

    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    public void Room_Chat_Btn_Press()
    {
        if (Chat_InputField.value != "")
        {
            //추후에 보낼때 자신의 닉네임 합쳐서 보내기.
            Send_Chat(Manager.Instance.User.Nick_Name + " : " + Chat_InputField.value, PhotonTargets.All);
            Chat_InputField.value = "";//채팅 패킷을 보내고 나서 필드값은 초기화
        }
    }

    public void InGame_Chat_Btn_Press()
    {
        if (Chat_InputField.value != "")
        {
            //추후에 보낼때 자신의 닉네임 합쳐서 보내기.
            Send_Chat(Manager.Instance.User.Nick_Name + " : " + Chat_InputField.value, PhotonTargets.All);
            Chat_InputField.value = "";//채팅 패킷을 보내고 나서 필드값은 초기화
        }
    }

    public void Send_Chat(string msg,PhotonTargets target)
    {
        if(PV!=null)
        {
            PV.RPC("Recv_Chat", target, msg);
        }
    }

    [PunRPC]
    public void Recv_Chat(string msg, PhotonMessageInfo info)
    {
        //받을때 로비인지 방인지, 인게임인지 파악하고 해당리스트에 집어넣기.
        UILabel Msg_Text = Instantiate(Chat_Text, Vector2.zero, Quaternion.identity);
        Msg_Text.text = msg;

        switch (info.photonView.viewID)
        {
            case 901:
                Room_Chat_List.Add(msg);
                Msg_Text.transform.SetParent(Room_Chat_View.transform);
                break;

            case 902:
                InGame_Chat_List.Add(msg);
                Debug.Log("인게임 채팅메시지처리");
                break;

            default:
                break;
        }
    }
}
