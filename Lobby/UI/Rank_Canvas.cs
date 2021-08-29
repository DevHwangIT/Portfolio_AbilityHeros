using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rank_Canvas : MonoBehaviour, UI_Interface
{

    [HideInInspector]
    public static Rank_Canvas Instance;

    public GameObject My_Rank;
    
    public GameObject Top_1by10;

    // Use this for initialization
    void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        StartCoroutine(DB_Manager.Instance.Ranking_DB());
    }

    public void Open()
    {
        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    public void Close_Btn()
    {
        Lobby.Instance.Lobby_UI_Show_UI();
    }

    public void Refresh_Ranking()
    {
        for (int i = 0; i < Manager.Instance.Rank_List_Lv.Length; i++)
        {
            Top_1by10.transform.GetChild(i).GetChild(1).GetComponent<UILabel>().text = Manager.Instance.Rank_List_Lv[i];
            Top_1by10.transform.GetChild(i).GetChild(2).GetComponent<UILabel>().text = Manager.Instance.Rank_List_Name[i];
        }

        My_Rank.transform.GetChild(1).GetComponent<UILabel>().text = "Lv. " + Manager.Instance.User.Lv;
        My_Rank.transform.GetChild(2).GetComponent<UILabel>().text = "???";
    }
}
