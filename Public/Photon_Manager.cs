using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon;

public class Photon_Manager : Photon.PunBehaviour {

    public static Photon_Manager Instance;

    public RoomInfo[] Room_List;

    PhotonView pv_;

    [HideInInspector]
    public Room_Create_Cavas Room_Create_Canvas_;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        Photon_Cloud_Connet(); //서버와의 연결
        pv_ = this.gameObject.AddComponent<PhotonView>();
    }

    public void Photon_Cloud_Connet()
    {
        if (PhotonNetwork.connected == false && PhotonNetwork.connecting == false) //연결되어있거나 연결중이 아닐경우에 다시 연결시도.
        {
            PhotonNetwork.ConnectUsingSettings(Manager.Client_Version);
        }
    }
    
    public void Onjoin_User()
    {
        InRoom_Canvas.Room_Join_User.Clear();
        InRoom_Canvas.User_Slot User = new InRoom_Canvas.User_Slot(PhotonNetwork.player.ID, PhotonNetwork.player.ID.ToString().Split('-')[0], Manager.Instance.User.Nick_Name, 1, PhotonNetwork.player.IsMasterClient, false, InRoom_Canvas.Team_.One_Team, Manager.Instance.Wear_Item, Manager.Instance.User.Icon);
        if (User.Team_Type == InRoom_Canvas.Team_.One_Team)
        {
            InRoom_Canvas.Instance.Calling_Notify_Master(PhotonNetwork.player.ID, User.ID, User.Nick_Name, User.Lv, User.Room_Master, User.Ready_State, false, User.wear_item, User.Sprite_Icon);
            //pv_.RPC("Join_Notify_Master", PhotonTargets.MasterClient, PhotonNetwork.player.ID, User.ID, User.Nick_Name, User.Lv, User.Room_Master,User.Ready_State, false);
        }
        else
        {
            InRoom_Canvas.Instance.Calling_Notify_Master(PhotonNetwork.player.ID, User.ID, User.Nick_Name, User.Lv, User.Room_Master, User.Ready_State, true, User.wear_item, User.Sprite_Icon);
            //pv_.RPC("Join_Notify_Master", PhotonTargets.MasterClient, PhotonNetwork.player.ID, User.ID, User.Nick_Name, User.Lv, User.Room_Master,User.Ready_State, true);
        }
    }

    public void Get_Room_List()
    {
        Room_List = PhotonNetwork.GetRoomList();
    }

    public void Join_Room(string Room_Name, string Room_PW, string Map_type, string Nick_Name, string mode)
    {
        PhotonNetwork.JoinRoom(Room_Name + "!" + Room_PW + "!" + Map_type + "!" + Nick_Name + "!" + mode);
    }

    public void Join_Robby()
    {
        PhotonNetwork.LeaveRoom();
        Manager.Instance.IsGame = false; //방에 입장해서 현재 인 게임 중 인지를 파악하기 위함
    }

    //--------------------------------------------- 포톤 클라우드 오버라이드 메소드 --------------------

    // ------------------ 게임방 접속 관련 콜백 처리 ------------------------


    //룸에서 나갔을 때
    public override void OnLeftRoom()
    {
        Manager.Instance.IsGame = false; //방에 입장해서 현재 인 게임 중 인지를 파악하기 위함
    }

    //로비에서 나갔을 때
    public override void OnLeftLobby()
    {
        base.OnLeftLobby();
    }

    //로비에 접속했을 때
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
    }

    //방이 만들어졌을 때
    public override void OnCreatedRoom()
    {
        Lobby.Instance.WaitingGameRoom_UI_Show_UI();
        InRoom_Canvas.Instance.Joined_Set();
    }

    //방에 입장하였을 때
    public override void OnJoinedRoom() //자동 호출되는 콜백함수
    {
        if (PhotonNetwork.player.IsMasterClient == true)
        {
            InRoom_Canvas.Room_Join_User.Clear();
            InRoom_Canvas.User_Slot User = new InRoom_Canvas.User_Slot(PhotonNetwork.player.ID, PhotonNetwork.player.ID.ToString().Split('-')[0], Manager.Instance.User.Nick_Name, Manager.Instance.User.Lv, PhotonNetwork.player.IsMasterClient, false, InRoom_Canvas.Team_.One_Team, Manager.Instance.Wear_Item, Manager.Instance.User.Icon);
            InRoom_Canvas.Room_Join_User.Add(User);
            InRoom_Canvas.Instance.Refresh_Join_UI();
        }
        else
        {
            InRoom_Canvas.Instance.Joined_Set();
            Onjoin_User(); // 내가 플레이어로서 방의 입장하였을 경우 이를 이미 접속한 유저에게 알림.
        }
    }

    //마스터클라이언트가 변경되었을때
    public override void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
    }

    //다른 플레이어가 방에 접속했을때
    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        base.OnPhotonPlayerConnected(newPlayer);
    }

    //다른 플레이어가 방에서 접속 종료시
    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        if (PhotonNetwork.isMasterClient == true) //내가 마스터 클라이언트일경우
        {   
            if (Manager.Instance.IsGame == true) //인게임에서 게임 중에 나갔을 경우 처리.
            {
                Game_Manager.Instance.User_Disconnected_InGame();
            }
            else // 로비에서 인게임 시작전에 나갔을 경우 처리.
            {
                InRoom_Canvas.Instance.Out_Notify_Other();
            }
        }
    }

    //마스터로 방에 접속하였을 때
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
    }

    //방에 입장하는것을 실패하였을 때
    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        if (codeAndMsg[0].ToString() == ErrorCode.GameFull.ToString())
            Manager.Instance.Error_Print("인원이 가득찬 방 입니다.");
        else
            base.OnPhotonJoinRoomFailed(codeAndMsg);
    }

    //방을 만드는 것이 실패하였을 때
    public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
    {
        base.OnPhotonCreateRoomFailed(codeAndMsg);
    }


    // ------------------ 포톤클라우드 접속 관련 콜백 처리 ------------------------

    //포톤클라우드 최대 접속자 수를 넘어섰을때
    public override void OnPhotonMaxCccuReached()
    {
        Manager.Instance.Error_Print("서버 이용자 수가 많아 현재 접속할 수 없습니다. 나중에 다시 시도해주세요.");
        base.OnPhotonMaxCccuReached();
    }

    //포톤에 접속되었을때
    public override void OnConnectedToPhoton()
    {
        base.OnConnectedToPhoton();
    }

    //포톤 연결이 종료되었을 때
    public override void OnDisconnectedFromPhoton()
    {
        if (Manager.Instance.IsGame == false)
            SceneManager.LoadScene("Start");
        else
        {
            Application.Quit();
        }
        /*
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Application.Quit();
        } else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork) //데이터로 연결
        {
        }
        else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork) //와이파이로 연결
        {

        }*/
    }

    //포톤 연결에 실패하였을때
    public override void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        base.OnFailedToConnectToPhoton(cause);
    }

    /*
    //네트워크 오브젝트 생성시에
    public override void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        base.OnPhotonInstantiate(info);
    }

    //방 목록 업데이트 수신시
    public override void OnReceivedRoomListUpdate()
    {
        base.OnReceivedRoomListUpdate();
    }
    */
}
