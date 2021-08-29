using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exception_Canvas : MonoBehaviour {

    public GameObject Exception_Canvas_Box;
    public UILabel Text_Label;
    
    public void Exception_Message(string message)
    {
        Text_Label.GetComponent<UILocalize>().key = message;
        Exception_Canvas_Box.SetActive(true);
    }

    public void Back_Btn()
    {
        Exception_Canvas_Box.SetActive(false);
    }
}
