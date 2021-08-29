using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour {

    AsyncOperation scence;

    int List_Index = 0;
    
    public UISprite Loading_Image;
    string[] Sprite_Name = { "Ability_Loading1" };
    public UILabel Loading_State_text;
    public UILabel Loading_text;
    public UISlider ProgressBar;
    string[] loading_hint=
        { "Tip 과도한 게임은 정신건강에 해롭습니다.",
        "Tip 다양한 능력을 활용하여 승리하세요!",
        "Tip 능력은 누가 사용하느냐에 따라서 좋게도 혹은 나쁘게도 사용될 수 있습니다.",
        "Tip 지나친 욕설과 비속어는 게임의 재미를 떨어뜨립니다.",
        "Tip. 필사길 사용에는 소울스톤이 필요합니다!"
    };
    
    private void Start()
    {
        System.GC.Collect();
        Loading_Image.spriteName = Sprite_Name[Random.Range(0, Sprite_Name.Length - 1)];
        int Text_index = Random.Range(0, loading_hint.Length - 1);
        Loading_text.text = loading_hint[Text_index];

        if (PhotonNetwork.Friends != null)
        {
            foreach (FriendInfo info in PhotonNetwork.Friends)
            {
                Debug.Log(info.UserId + " 닉네임 - 현재상태 : " + info.IsOnline);
            }
        }

        StartCoroutine(Call_Loading_Scene());
    }

    IEnumerator Call_Loading_Scene()
    {
        if (PhotonNetwork.connecting == true) //연결중인 상태인지 파악
        {
            Debug.Log("포톤 연결을 확인하는 중입니다.");
            Invoke("Call_Loading_Scene()", 1.0f); //재귀 호출
        }
        else //연결 중 상태가 아님을 확인했음.
        {
            if (PhotonNetwork.connected == true) //연결성공했는지 파악 성공했으면 다음씬 전환
            {
                Debug.Log("포톤 연결에 성공하였습니다..");
                Call_Next_Scence();
            }
            else // 아닐경우 다시 연결 시도
            {
                Debug.Log("포톤 연결을 다시 시도합니다..");
                Photon_Manager.Instance.Photon_Cloud_Connet();
                Invoke("Call_Loading_Scene()", 1.0f); //재귀 호출
            }
        }
        yield return null;
    }

    void Call_Next_Scence()
    {
        scence = SceneManager.LoadSceneAsync(Manager.Instance.Scene_name);
        Loading_State();
    }

    void Loading_State()
    {
        ProgressBar.value = scence.progress;

        if (List_Index > 3)
        {
            List_Index = 0;
        }
        else
            List_Index++;

        switch (List_Index)
        {
            case 0:
                Loading_State_text.text = "Loading maps...  " + (int)(ProgressBar.value * 100) + " %";
                break;

            case 1:
                Loading_State_text.text = "Loading maps...  " + (int)(ProgressBar.value * 100) + " %";
                break;

            case 2:
                Loading_State_text.text = "Loading maps...  " + (int)(ProgressBar.value * 100) + " %";
                break;

            case 3:
                Loading_State_text.text = "Loading maps...  " + (int)(ProgressBar.value * 100) + " %";
                break;

            default:
                break;
        }
        Invoke("Loading_State", 0.1f);
    }
}
