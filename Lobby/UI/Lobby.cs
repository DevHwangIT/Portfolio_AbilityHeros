using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby : MonoBehaviour
{
    [HideInInspector]
    public static Lobby Instance;

    [Header("Sound")]
    public AudioSource Room_BGM;
    public AudioSource Lobby_BGM;


    #region Canvas객체

    [Header("UI_Instance")]
    [SerializeField]
    private Lobby_Top_Canvas LobbyTop_Instance;
    [SerializeField]
    private Lobby_Right_Canvas LobbyRight_Instance;
    [SerializeField]
    private My_Info_Canvas MyInfo_Instance;
    [SerializeField]
    private Social_Canvas Social_Instance;
    [SerializeField]
    private My_Room_Canvas MyRoom_Instance;
    [SerializeField]
    private Rank_Canvas Rank_Instance;
    [SerializeField]
    private Market_Canvas Market_Instance;
    [SerializeField]
    private Latter_Canvas Latter_Instance;
    [SerializeField]
    private Menu_Set_Canvas MenuSet_Instance;
    [SerializeField]
    private Match_Canvas Match_Instance;
    [SerializeField]
    private Room_Create_Cavas CreateRoom_Instance;
    [SerializeField]
    private LobbyChat_Canvas LobbyChat_Instance;
    [SerializeField]
    private News_Canvas News_Instance;

    //In room
    [SerializeField]
    private InRoom_Canvas InRoom_Instance;

    #endregion


    #region UI 이벤트 등록

    //모든 UI
    public Manager.UIDelegate All_UI_Close_UI;
    //로비 진입시 UI
    public Manager.UIDelegate Lobby_UI_Show_UI;
    //상점 진입시 UI
    public Manager.UIDelegate Market_UI_Show_UI;
    //우편함 진입시 UI
    public Manager.UIDelegate Latter_UI_Show_UI;
    //커스텀 룸(아이템,커스텀마이징 등) UI
    public Manager.UIDelegate CustomRoom_UI_Show_UI;
    //로비 채팅서버 UI
    public Manager.UIDelegate LobbyChatting_UI_Show_UI;
    //설정창 UI
    public Manager.UIDelegate Setting_UI_Show_UI;
    //대기방 UI
    public Manager.UIDelegate Match_UI_Show_UI;
    //InRoom UI
    public Manager.UIDelegate WaitingGameRoom_UI_Show_UI;

    #endregion


    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void Start()
    {
        StartCoroutine(Event_Initialize());
    }

    IEnumerator Event_Initialize()
    {
        //모든 UI
        All_UI_Close_UI += LobbyTop_Instance.Close;
        All_UI_Close_UI += LobbyRight_Instance.Close;
        All_UI_Close_UI += MyInfo_Instance.Close;
        All_UI_Close_UI += Social_Instance.Close;
        All_UI_Close_UI += MyRoom_Instance.Close;
        All_UI_Close_UI += Rank_Instance.Close;
        All_UI_Close_UI += Market_Instance.Close;
        All_UI_Close_UI += Latter_Instance.Close;
        All_UI_Close_UI += MenuSet_Instance.Close;
        All_UI_Close_UI += Match_Instance.Close;
        All_UI_Close_UI += CreateRoom_Instance.Close;
        All_UI_Close_UI += LobbyChat_Instance.Close;
        All_UI_Close_UI += News_Instance.Close;

        //로비 진입시 UI
        Lobby_UI_Show_UI += All_UI_Close_UI;
        Lobby_UI_Show_UI += LobbyTop_Instance.Open;
        Lobby_UI_Show_UI += LobbyRight_Instance.Open;
        Lobby_UI_Show_UI += Social_Instance.Open;
        Lobby_UI_Show_UI += MyInfo_Instance.Open;

        //상점 진입시 UI
        Market_UI_Show_UI += All_UI_Close_UI;
        Market_UI_Show_UI += LobbyTop_Instance.Open;
        Market_UI_Show_UI += Market_Instance.Open;

        //커스텀 룸(아이템,커스텀마이징 등) UI
        CustomRoom_UI_Show_UI += All_UI_Close_UI;
        CustomRoom_UI_Show_UI += MyRoom_Instance.Open;

        //우편함 진입시 UI
        Latter_UI_Show_UI += All_UI_Close_UI;
        Latter_UI_Show_UI += Latter_Instance.Open;

        //로비 채팅서버 UI
        LobbyChatting_UI_Show_UI += All_UI_Close_UI;
        LobbyChatting_UI_Show_UI += LobbyChat_Instance.Open;

        //설정창 UI
        Setting_UI_Show_UI += All_UI_Close_UI;
        Setting_UI_Show_UI += MenuSet_Instance.Open;

        //대기방 UI
        Match_UI_Show_UI += All_UI_Close_UI;
        Match_UI_Show_UI += Match_Instance.Open;
        Match_UI_Show_UI += CreateRoom_Instance.Open;

        //InRoom UI
        WaitingGameRoom_UI_Show_UI += All_UI_Close_UI;
        WaitingGameRoom_UI_Show_UI += InRoom_Instance.Open;

        All_UI_Close_UI();  // 활성화된 UI가 있을경우 종료하기
        Lobby_UI_Show_UI(); // 로비진입시 UI 표시
        yield return null;
    }
}
