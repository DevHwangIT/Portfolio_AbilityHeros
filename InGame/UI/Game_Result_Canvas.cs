using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Result_Canvas : MonoBehaviour, UI_Interface
{
    public static Game_Result_Canvas Instance;

    public GameObject Drow_Title;
    public GameObject Win_Title;
    public GameObject Lose_Title;

    public GameObject GameType_Label;
    public GameObject Play_Time_Label;

    public GameObject Kill_Label;
    public GameObject Damege_Label;
    public GameObject Exp_Label;
    public GameObject TeamBouns_Label;

    public GameObject GetGold_Label;
    public GameObject GetExp_Label;


    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void Open()
    {
        this.gameObject.SetActive(true);
        //승리 패배 처리
        switch (Game_Manager.Instance.Game_Result)
        {
            case Game_Manager.Result_Type.None:
                break;

            case Game_Manager.Result_Type.Draw:
                Drow_Title.SetActive(true);
                Win_Title.SetActive(false);
                Lose_Title.SetActive(false);
                break;

            case Game_Manager.Result_Type.Win:
                Drow_Title.SetActive(false);
                Win_Title.SetActive(true);
                Lose_Title.SetActive(false);
                Set_Result_Proccess();
                break;

            case Game_Manager.Result_Type.Lose:
                Drow_Title.SetActive(false);
                Win_Title.SetActive(false);
                Lose_Title.SetActive(true);
                Set_Result_Proccess();
                break;
        }
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    // 판당 골드 산정 법칙 - (( 총 플레이시간 / 10 ) 
    // 판당 경험치 산정 법칙 - (( 총 플레이시간 / 10 )*킬수) + 팀전 보너스 

    void Set_Result_Proccess()
    {
        int Total_Get_Exp = 0;
        int Total_Get_Gold = 0;

        Play_Time_Label.GetComponent<UILabel>().text = Localization.Get("Time") + " " + Timer_Canvas.Instance.Timer / 60 + " : " + Timer_Canvas.Instance.Timer % 60; //플레이타임 라벨
        Kill_Label.GetComponent<UILabel>().text = Character_State.Instance.Kill_Count.ToString()+" P"; // 잡은 적 라벨
        Damege_Label.GetComponent<UILabel>().text = Character_State.Instance.Send_Damege_Count+" P"; // 준 데미지 라벨
        Exp_Label.GetComponent<UILabel>().text = ((Manager.Instance.Room.time - Timer_Canvas.Instance.Timer) / 10).ToString() +" EXP"; // 경험치 라벨
        TeamBouns_Label.GetComponent<UILabel>().text = "50 EXP"; //팀보너스 경험치 라벨
        Total_Get_Gold = (Manager.Instance.Room.time - Timer_Canvas.Instance.Timer) / 10; // 토탈 획득 골드 값

        if (Manager.Instance.Room.mode == 0) //개인전
        {
            GameType_Label.GetComponent<UILabel>().text = Localization.Get("Solo") + "_"; // 게임타입 라벨 - 개인전
            TeamBouns_Label.SetActive(false);
            Total_Get_Exp = (Manager.Instance.Room.time - Timer_Canvas.Instance.Timer) / 10;
        }
        else
        {
            GameType_Label.GetComponent<UILabel>().text = Localization.Get("Team") + "_"; //게임 타입 라벨 - 팀전
            TeamBouns_Label.SetActive(true);
            Total_Get_Exp = (Manager.Instance.Room.time - Timer_Canvas.Instance.Timer) / 10 + 50;
        }
        
        switch (Manager.Instance.Room.map)
        {
            case 0:
                GameType_Label.GetComponent<UILabel>().text += Localization.Get("City");
                break;

            default:
                GameType_Label.GetComponent<UILabel>().text += "Unknow";
                break;
        }
        GetExp_Label.GetComponent<UILabel>().text = "+ "+Total_Get_Exp+" EXP";
        GetGold_Label.GetComponent<UILabel>().text = "+ " + Total_Get_Gold + " G";
    }

    public void Back_Lobby_Btn()
    {
        Photon_Manager.Instance.Join_Robby();
        Manager.Instance.Scene_name = "Lobby";
        SceneManager.LoadScene("Loading");
    }
}
