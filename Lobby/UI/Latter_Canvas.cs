using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Latter_Canvas : MonoBehaviour, UI_Interface
{

    [HideInInspector]
    public static Latter_Canvas Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public new void Open()
    {
        this.gameObject.SetActive(true);
    }

    public new void Close()
    {
        this.gameObject.SetActive(false);
    }

    public void Close_Btn()
    {
        Lobby.Instance.Lobby_UI_Show_UI();
    }
}
