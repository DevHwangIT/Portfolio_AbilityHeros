using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Social_Canvas : MonoBehaviour, UI_Interface
{

    [HideInInspector]
    public static Social_Canvas Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void Open()
    {
        this.gameObject.SetActive(true);
        this.GetComponent<TweenPosition>().PlayForward();
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    //public void Close_Delay()
    //{
    //    this.gameObject.SetActive(false);
    //}

    //다하고 친구기능 추가하기..
}
