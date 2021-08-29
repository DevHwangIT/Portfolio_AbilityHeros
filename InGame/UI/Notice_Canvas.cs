using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notice_Canvas : MonoBehaviour, UI_Interface
{

    public static Notice_Canvas Instance;

    public UILabel Msg_Label;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    /*
    public IEnumerator Notice_(string key)
    {
        Msg_Label.text = key;
        Open();
        yield return null;
    }
    */
    public void Notice_(string key, float time)
    {
        Msg_Label.text = key;
        Open();
        if (IsInvoking("Close")) //실행중이라면 취소하고 다시실행.
            CancelInvoke("Close");
        Invoke("Close", time);
    }

    public void Open()
    {
        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }
}
