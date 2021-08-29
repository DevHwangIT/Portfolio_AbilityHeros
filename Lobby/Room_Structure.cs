using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room_Structure : MonoBehaviour {

    //Lobby 내의 방을 표시하는 UI 각각이 가지기 위한 클래스

    public int number_ = 0;
    public string name_ = "";
    public string pw_ = "";
    public string make_user="";
    public string type="Solo";
    public int count_user_ = 0; //방에 들어간 인원
    public int maximun_ = 0; //최대 인원
    public int Map_Type_Number;
    [HideInInspector]
    public UISprite Map_Image;

    private void Awake()
    {
        Map_Image = this.transform.GetChild(0).GetComponent<UISprite>();
    }

    private void Start()
    {
        Info_Set();
    }

    public void Info_Set()
    {
        if (pw_ == "")
            this.transform.GetChild(0).GetChild(5).gameObject.SetActive(false);
        else
            this.transform.GetChild(0).GetChild(5).gameObject.SetActive(true);

        this.transform.GetChild(0).GetChild(1).GetComponent<UILabel>().text = name_;
        this.transform.GetChild(0).GetChild(2).GetComponent<UILabel>().text = make_user;
        this.transform.GetChild(0).GetChild(3).GetComponent<UILabel>().text = type;
        this.transform.GetChild(0).GetChild(4).GetComponent<UILabel>().text = count_user_.ToString()+"/"+maximun_;

        switch (Map_Type_Number)
        {
            case 0:
                Map_Image.spriteName = "도시";
                break;

            case 1:
                Map_Image.spriteName = "공항";
                break;

            case 2:
                Map_Image.spriteName = "사막";
                break;
        }

        switch (type)
        {
            case "Solo":
                this.GetComponent<UISprite>().spriteName = "게임_개인전";
                break;

            case "Team":
                this.GetComponent<UISprite>().spriteName = "게임_팀전";
                break;

            default:
                break;
        }
    } 
}
