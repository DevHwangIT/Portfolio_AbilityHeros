using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class News_Canvas : MonoBehaviour, UI_Interface
{

    [HideInInspector]
    public static News_Canvas Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
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

    public void News_Web_Connect_Btn()
    {
        Application.OpenURL("http://finestudio.company");
    }
}
