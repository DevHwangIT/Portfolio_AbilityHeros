using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lobby_Top_Canvas : MonoBehaviour, UI_Interface
{

    [HideInInspector]
    public static Lobby_Top_Canvas Instance;

    [Header("Info UI")]
    public UILabel Lobi_Money;
    public UILabel Lobi_Crystal;

    [Header("Canvas")]
    public GameObject Info_Canvas;
    public GameObject Market_Canvas;
    public GameObject Advertisement_Canvas;
    public GameObject Latter_Canvas;
    public GameObject News_Canvas;
    public GameObject Menu_Canvas;
    public GameObject MyInfo_Detail_Canvas;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        Refresh_Text_UI();
    }

    public void Open()
    {
        this.gameObject.SetActive(true);
        Refresh_Text_UI();
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    public void Refresh_Text_UI()
    {
        Lobi_Money.text = Manager.Instance.User.Money.ToString();
        Lobi_Crystal.text = Manager.Instance.User.Soul_Stone.ToString();
    }

    //로비로 돌아가기
    public void BacK_Lobby_Btn()
    {
        Lobby.Instance.Lobby_UI_Show_UI();
    }

    //상점 Canvas 노출시키기
    public void Market_Btn()
    {
        Lobby.Instance.Market_UI_Show_UI();
    }

    //편지 Canvas 노출시키기
    public void Latter_Btn()
    {
        Lobby.Instance.Latter_UI_Show_UI();
    }

    //채팅 Canvas 노출시키기
    public void Chat_Btn()
    {
        Lobby.Instance.LobbyChatting_UI_Show_UI();
    }

    //Menu 노출시키기
    public void Setting_Menu_Btn()
    {
        Lobby.Instance.Setting_UI_Show_UI();
    }

    //게임 종료
    public void Quit_Btn()
    {
        GameObject.Destroy(Manager.Instance.gameObject);
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Start");
    }
}
