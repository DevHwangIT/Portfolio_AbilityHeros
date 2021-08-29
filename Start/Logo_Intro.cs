using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Logo_Intro : MonoBehaviour {

    public static Logo_Intro Instance = null;

    public UILabel Version_Text;
    [Header("UI_GameObject")]
    public GameObject Touch_Screen_UI;
    public GameObject Select_Login_UI;
    public GameObject Fine_Login_UI;
    public GameObject MemberShip_UI;

    [Header("Login UI_InputField")]
    public UIInput ID;
    public UIInput PW;

    [Header("Creat UI_InputField")]
    public UIInput Create_Name;
    public UIInput Create_ID;
    public UIInput Create_PW;
    public UIToggle Check_Toggle;

    public GameObject Cavas;
    public GameObject Camera;

    public Manager.UIDelegate Intro_Initialize;

    // Use this for initialization
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 60;
        Version_Text.text = Manager.Client_Version;
        Intro_Initialize = new Manager.UIDelegate(Initialize_UI);
    }

    public void Start()
    {
        Intro_Initialize();
    }

    public void Initialize_UI()
    {
        Touch_Screen_UI.SetActive(true);
        Select_Login_UI.SetActive(false);
        Fine_Login_UI.SetActive(false);
        MemberShip_UI.SetActive(false);
    }
    
    public void Touch_To_Screen_Btn()
    {
        Touch_Screen_UI.SetActive(false);
        Select_Login_UI.SetActive(true);
    }

    public void Select_Login_UI_Back_Btn()
    {
        Touch_Screen_UI.SetActive(true);
        Select_Login_UI.SetActive(false);
    }
    
    public void Select_Login_UI_MemberShip_Btn()
    {
        Select_Login_UI.SetActive(false);
        MemberShip_UI.SetActive(true);
    }

    public void Select_Fine_Login_UI_Btn()
    {
        Manager.Instance.User.User_Platform = "FineStudio";
        Select_Login_UI.SetActive(false);
        Fine_Login_UI.SetActive(true);
    }

    //public void Select_Google_Login_UI_Btn()
    //{
    //    Manager.Instance.User.User_Platform = "Android";
    //    StartCoroutine(Social_Manager.Instance.SignIn());
    //}

    public void Fine_Login_UI_Login_Btn()
    {
        Manager.Instance.User.ID = ID.value;
        Manager.Instance.User.PW = PW.value;
        StartCoroutine(DB_Manager.Instance.Load_Data_DB());
    }

    public void Fine_Login_UI_Back_Btn()
    {
        Select_Login_UI.SetActive(true);
        Fine_Login_UI.SetActive(false);
    }
    
    public void MemberShip_UI_Back_Btn()
    {
        MemberShip_UI.SetActive(false);
        Select_Login_UI.SetActive(true);
    }

    public void MemberShip_UI_Accept_Btn()
    {
        if (Create_ID.value == "")
        {
            Manager.Instance.Error_Print("아이디를 확인해주십시오.");
            return;
        }
        if (Create_PW.value == "")
        {
            Manager.Instance.Error_Print("비밀번호를 확인해주십시오");
            return;
        }
        if (Create_Name.value == "")
        {
            Manager.Instance.Error_Print("닉네임을 확인해주십시오.");
            return;
        }

        if (Create_ID.value.Contains("!") || Create_ID.value.Contains("/")) //해당 특수문자가 들어갈경우 회원가입 불가능
        {
            Manager.Instance.Error_Print("아이디에 특수문자가 있는지 확인해주십시오.");
            return;
        }
        if (Create_PW.value.Contains("!") || Create_PW.value.Contains("/")) //해당 특수문자가 들어갈경우 회원가입 불가능
        {
            Manager.Instance.Error_Print("비밀번호에 특수문자가 있는지 확인해주십시오.");
            return;
        }
        if (Create_Name.value.Contains("!") || Create_Name.value.Contains("/")) //해당 특수문자가 들어갈경우 회원가입 불가능
        {
            Manager.Instance.Error_Print("닉네임에 특수문자가 있는지 확인해주십시오.");
            return;
        }

        if (Check_Toggle.value == false) //해당 특수문자가 들어갈경우 회원가입 불가능
        {
            Manager.Instance.Error_Print("약관에 동의하여주십시오.");
            return;
        }
        Manager.Instance.User.ID = Create_ID.value;
        Manager.Instance.User.PW = Create_PW.value;
        Manager.Instance.User.Nick_Name = Create_Name.value;
        StartCoroutine(DB_Manager.Instance.Create_DB());
        Logo_Intro.Instance.MemberShip_UI_Back_Btn();
    }
}
