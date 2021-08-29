using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
//using GooglePlayGames;
//using GooglePlayGames.BasicApi;
using UnityEngine.SceneManagement;

public class Social_Manager : MonoBehaviour
{

    //PC 버전에서는 사용하지않는다.

//    public static Social_Manager Instance;
    
//    GameObject loading_object;

//    private Action<bool,string> signInCallback;    // 로그인 성공 여부 확인을 위한 Callback 함수

//    void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//        }

//#if UNITY_ANDROID
//        PlayGamesPlatform.DebugLogEnabled = true;

//        PlayGamesPlatform.Activate();

//#elif UNITY_IOS
 
//        GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
 
//#endif

//    }

//    private void Start()
//    {
//        // Callback 함수 정의
//        signInCallback = (bool success, string error_code) =>
//        {
//            if (success) //로그인 성공처리
//            {
//                Debug.Log(Manager.Instance.User.ID = Social.localUser.id);

//                loading_object = Manager.Instance.Loading_Print();
//                Manager.Instance.User.ID = Social.localUser.id;
//                Manager.Instance.User.PW = Social.localUser.id + "Android";
//                Manager.Instance.User.User_Platform = "Android";
//                StartCoroutine(DB_Manager.Instance.Load_Data_DB());
//            }
//            else // 실패했으므로 이에 따른 경고창 출력
//            {
//                Manager.Instance.Error_Print(error_code);
//            }
//        };
//    }

//    // 로그인
//    public IEnumerator SignIn()
//    {
//        // 로그아웃 상태면 호출
//        if (!Social.localUser.authenticated)
//        {
//            Social.localUser.Authenticate(signInCallback);
//        }
//        else //이미 아이디가 로그인 상태일경우.
//        {
//            loading_object = Manager.Instance.Loading_Print();
//            Manager.Instance.User.ID = Social.localUser.id;
//            Manager.Instance.User.PW = Social.localUser.id + "Android";
//            Manager.Instance.User.User_Platform = "Android";
//            StartCoroutine(DB_Manager.Instance.Load_Data_DB());
//        }
//        yield return null;
//    }

//    // 로그아웃
//    public void SignOut()
//    {
//        // 로그인 상태면 호출
//        if (Social.localUser.authenticated == true) // 로그인 -> 로그아웃
//        {
//            //((GooglePlayGames.PlayGamesPlatform)Social.Active).SignOut();
//        }
//    }

//    public IEnumerator Call_Create_Login()
//    {
//        Manager.Instance.User.ID = Social.localUser.id;
//        Manager.Instance.User.PW = Social.localUser.id + "Android";
//        Manager.Instance.User.Nick_Name = Social.localUser.userName;
//        Manager.Instance.User.User_Platform = "Android";
//        Destroy(loading_object);
//        StartCoroutine(DB_Manager.Instance.Create_DB());
//        yield return null;
//    }
}
