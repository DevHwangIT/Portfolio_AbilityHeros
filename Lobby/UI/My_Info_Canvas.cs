using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class My_Info_Canvas : MonoBehaviour, UI_Interface
{
    [HideInInspector]
    public static My_Info_Canvas Instance;

    [Header("Info UI")]
    public UISlider Exp_Bar;
    public UILabel Exp_Label;
    public UISprite Sprite_Icon;
    public UILabel Nick;
    public UILabel Lv;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void Start()
    {
        Refresh_Text_UI();
    }

    public void Open()
    {
        this.gameObject.SetActive(true);
        this.GetComponent<TweenPosition>().PlayForward();
        Refresh_Text_UI();
        if (My_Room_Canvas.Instance)
            My_Room_Canvas.Instance.My_Character_Mirror_Initialize();
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }
    
    public void Refresh_Text_UI()
    {
        Exp_Bar.value = (float)System.Math.Truncate(((float)Manager.Instance.User.Exp / Manager.Instance.User.Exp_Maximum) * 100) / 100;
        Exp_Label.text = Exp_Bar.value + " %";
        Sprite_Icon.spriteName = Manager.Instance.User.Icon.ToString();
        Nick.text = Manager.Instance.User.Nick_Name;
        Lv.text = "Lv." + Manager.Instance.User.Lv.ToString();
    }
}
