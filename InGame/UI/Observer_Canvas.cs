using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer_Canvas : MonoBehaviour, UI_Interface
{

    public static Observer_Canvas Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void Open()
    {
        this.gameObject.SetActive(true);
        InGame.Instance.Notice_Canvas_Initialize();
        Notice_Canvas.Instance.Notice_("관전 모드 입니다.", 900.0f);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }
}
