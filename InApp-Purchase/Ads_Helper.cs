using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class Ads_Helper : MonoBehaviour
{
    public static Ads_Helper Instance;

    private const string android_game_id = "";
    private const string ios_game_id = "";
    private const string rewarded_video_id = "";

    void Start()
    {
        if (Instance == null)
            Instance = this;
        Initialize();
        if (SceneManagerHelper.ActiveSceneName == "Loading")
        {
            ShowRewardedAd();
        }
    }

    private void Initialize()
    {
#if UNITY_ANDROID
        Advertisement.Initialize(android_game_id);
#elif UNITY_IOS
        Advertisement.Initialize(ios_game_id);
#endif
    }

    public void ShowRewardedAd()
    {
        if (Advertisement.IsReady(rewarded_video_id))
        {
            var options = new ShowOptions {resultCallback = HandleShowResult};

            Advertisement.Show(rewarded_video_id, options);
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
            {
                StartCoroutine(DB_Manager.Instance.Add_Gold_DB(500, "광고를 통한 골드획득", "게임 광고")); //광고를 보고 매개변수 값만큼 돈을 증가시켜서 DB에 저장
                StartCoroutine(DB_Manager.Instance.Referesh_DB()); //수정된 값이 있으므로 이를 DB에서 다시 가져와서 적용시킴.
                //Lobi_Top.Lobi_Top_Script.Invoke("Refresh_Text_UI", 1.0f);
                // to do ...
                // 광고 시청이 완료되었을 때 처리
                break;
            }
            case ShowResult.Skipped:
            {
                Manager.Instance.Error_Localize_Print("Exception");

                // to do ...
                // 광고가 스킵되었을 때 처리

                break;
            }
            case ShowResult.Failed:
            {
                Manager.Instance.Error_Localize_Print("Exception");

                // to do ...
                // 광고 시청에 실패했을 때 처리

                break;
            }
        }
    }
}
