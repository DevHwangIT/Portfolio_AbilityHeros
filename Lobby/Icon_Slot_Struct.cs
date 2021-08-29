using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icon_Slot_Struct : MonoBehaviour
{
    public int Index_;
    public string Name_;
    public string Info_;

    GameObject Icon_Slot_Info_View;

    private void Start()
    {
        Icon_Slot_Info_View = GameObject.Find("Icon_Slot_Info");
    }

    public void Set()
    {
        transform.GetChild(0).GetComponent<UISprite>().spriteName = Index_.ToString();
    }

    public void Inventory_Icon_Slot_Btn()
    {
        Icon_Slot_Info_View.transform.GetChild(0).GetComponent<UILabel>().text = Name_ + "\n" + Info_;
    }
}