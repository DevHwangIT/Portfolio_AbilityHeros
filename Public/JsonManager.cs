using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
//using Litjson;

public class JsonManager : MonoBehaviour
{/*
    //Json으로 플레이어의 정보를 읽어오고 자동로그인이 되게끔 구성.
    
    public class Json_UserInfo
    {
        public string ID;
        public string PW;
        public string Platform;

        public Json_UserInfo(string id, string pw, string platform)
        {
            ID = id;
            PW = pw;
            Platform = platform;
        }
    }

    public Json_UserInfo Login_Info;

    [HideInInspector]
    public string Info_Path;

    public static JsonManager Instance;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            Info_Path = Application.dataPath + "\\Resource\\PlayerInfo.json";
        }
    }

    // Use this for initialization
    void Start()
    {
        if (File.Exists(Info_Path)) //저장된 파일이 존재할 경우
        {
            Manager.Manager_Script.Error_Print(Info_Path+" 해당 경로에 파일이 존재함. 그래서 불러올게요");
            StartCoroutine("Load_Info");
        }
    }

    IEnumerator Load_Info()
    {
        GameObject Load_Pannel = Manager.Manager_Script.Loading_Print();
        string jsonStr = File.ReadAllText(Info_Path);
        JsonData playerData = JsonMapper.ToObject(jsonStr);
        //json정보로 부터 Info 불러와서 저장
        Login_Info = new Json_UserInfo(playerData["ID"].ToString(), playerData["PW"].ToString(), playerData["Platform"].ToString());
        //manager.cs 에 있는 곳에도 저장.
        Manager.Manager_Script.User.ID = Login_Info.ID;
        Manager.Manager_Script.User.PW = Login_Info.PW;
        Manager.Manager_Script.User.User_Platform = Login_Info.Platform;
        GameObject.Destroy(Load_Pannel);
        StartCoroutine(DB_Manager.DB_Scrpit.Load_Data_DB());
        //받아온 정보를 바탕으로 로그인 시도. 안됬을 경우 로그인 새로 유도
        yield return null;
    }

    public void Info_Write() //json으로 로그인된 유저의 데이터를 저장해둠.
    {
        Login_Info = new Json_UserInfo(Manager.Manager_Script.User.ID, Manager.Manager_Script.User.PW, Manager.Manager_Script.User.User_Platform);

        JsonData infoJson = JsonMapper.ToJson(Login_Info);
        
        File.WriteAllText(Info_Path, infoJson.ToString());
    }*/
}
