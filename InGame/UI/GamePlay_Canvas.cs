using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlay_Canvas : MonoBehaviour, UI_Interface
{
    public static GamePlay_Canvas Instance;

    [Header("UI_Object")]
    public UILabel Kill_Label;
    public UILabel Soul_Label;
    public UISlider HP_bar;
    public UILabel HP_Label;

    public GameObject Joystick;
    public GameObject Gard_Btn;
    public GameObject Map;
    public GameObject My_Info;
    public Transform Buff_View;
    public UIButton Ability1_Btn;
    public UIButton Ability2_Btn;

    public GameObject Setting_View;
    
    public GameObject High_Skill_Effect_Canvas;

    [Header("Minimap Camera")]
    public GameObject mini_camera;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void LateUpdate()
    {
        if (Game_Manager.Instance.My_Unit != null)
        {
            try
            {
                HP_bar.value = (float)Game_Manager.Instance.My_Unit.GetComponent<Character_State>().My_Character_Info.HP / Game_Manager.Instance.My_Unit.GetComponent<Character_State>().My_Character_Info.HP_Maximum;
                HP_Label.text = Game_Manager.Instance.My_Unit.GetComponent<Character_State>().My_Character_Info.HP + " / " + Game_Manager.Instance.My_Unit.GetComponent<Character_State>().My_Character_Info.HP_Maximum;
            }
            catch (System.DivideByZeroException e)
            {
                HP_bar.value = 0;
                HP_Label.text = "0 / "+Game_Manager.Instance.My_Unit.GetComponent<Character_State>().My_Character_Info.HP_Maximum;
            }
        }
    }

    public void Open()
    {
        this.gameObject.SetActive(true);
        High_Skill_Effect_Close();
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    public void High_Skill_Effect_Open()
    {
        High_Skill_Effect_Canvas.SetActive(true);
        Joystick.gameObject.SetActive(false);
        Gard_Btn.gameObject.SetActive(false);
        Map.gameObject.SetActive(false);
        My_Info.gameObject.SetActive(false);
        Ability1_Btn.gameObject.SetActive(false);
        Ability2_Btn.gameObject.SetActive(false);
    }

    public void High_Skill_Effect_Close()
    {
        High_Skill_Effect_Canvas.SetActive(false);
        Joystick.gameObject.SetActive(true);
        Gard_Btn.gameObject.SetActive(true);
        Map.gameObject.SetActive(true);
        My_Info.gameObject.SetActive(true);
        Ability1_Btn.gameObject.SetActive(true);
        Ability2_Btn.gameObject.SetActive(true);
    }

    bool Set_Camera = false;

    //시점 변환
    public void Camera_Change()
    {
        Person_Camera camera_ = Game_Manager.Instance.three_Camera.GetComponent<Person_Camera>();
        if(Set_Camera)
        {
            camera_.distance = 10f;
            camera_.minHeight = 7.5f;
            camera_.maxHeight = 7.5f;
            Set_Camera = false;
        }
        else
        {
            camera_.distance = 5f;
            camera_.minHeight = 15f;
            camera_.maxHeight = 15f;
            Set_Camera = true;
        }
    }

    public void Setting_View_Open_Btn()
    {
        Setting_View.SetActive(true);
    }

    public void Setting_View_Close_Btn()
    {
        Setting_View.SetActive(false);
    }

    public void Setting_View_Game_Quit_Btn()
    {
        Manager.Instance.Scene_name = "Lobby";
        SceneManager.LoadScene("Loading");
    }
}
