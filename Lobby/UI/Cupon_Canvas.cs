using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cupon_Canvas : MonoBehaviour {

    public static Cupon_Canvas Instance;

    public UIInput Cupon_Input_UI;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        Instance.gameObject.SetActive(false);
    }

    public void Cupon_Ok_Btn()
    {
        // 쿠폰 DB 비교 호출
        Debug.Log("추후 구현");
    }

    public void Cupon_Back_Btn()
    {
        Instance.gameObject.SetActive(false);
        Lobby.Instance.Lobby_UI_Show_UI();
    }
}
