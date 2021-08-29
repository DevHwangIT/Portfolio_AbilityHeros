using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Set_Canvas : MonoBehaviour, UI_Interface
{

    [HideInInspector]
    public static Menu_Set_Canvas Instance;

    public GameObject Cupon_View;

    public UILabel Device_Info;
    public UILabel Language_Label;
    public UISlider Sound_Bar;

    int Language_Index = 0;
    string[] Language_List = { "English", "한국어", "中文简体", "繁體中文", "日本語", "Français", "Idioma español" };
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        Sound_Bar.value = Manager.Instance.Volum;
        Sound_Slider_Bar();
    }

    public void Open()
    {
        Sound_Bar.value = AudioListener.volume;
        this.gameObject.SetActive(true);
        Device_Info.text = "로그인 정보 -" + Manager.Instance.User.Nick_Name + "\n버전정보 - Ability Hero - " + Manager.Client_Version;
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    public void Sound_Slider_Bar()
    {
        AudioListener.volume = Sound_Bar.value;
        Manager.Instance.Volum = Sound_Bar.value;
    }

    public void Lauguage_Left_Btn()
    {
        if (Language_Index > 0)
        {
            Language_Index -= 1;
        }
        else
            Language_Index = Language_List.Length - 1;
        Language_Label.text = Language_List[Language_Index];
    }

    public void Lauguage_Right_Btn()
    {
        if (Language_Index < Language_List.Length - 1)
        {
            Language_Index += 1;
        }
        else
            Language_Index = 0;
        Language_Label.text = Language_List[Language_Index];
    }
    
    public void Close_Btn()
    {
        Lobby.Instance.Lobby_UI_Show_UI();
    }

    public void Cupon_Btn()
    {
        this.gameObject.SetActive(false);
        Cupon_View.SetActive(true);
    }

    public void LogOut_Btn()
    {
        Manager.Instance.Game_Quit();
    }
}
