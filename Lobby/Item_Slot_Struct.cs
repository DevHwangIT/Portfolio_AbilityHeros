using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Slot_Struct : MonoBehaviour {

    public enum Part
    {
        Hair,
        Top,
        Pants,
        Shoes
    }

    public Part Type_;
    public string Name_;
    public string info_;
    public int index_;

    public void Set()
    {
        switch (Type_)
        {
            case Part.Hair:
                transform.GetChild(0).GetComponent<UISprite>().atlas = My_Room_Canvas.Instance.Hair_Atlas;
                transform.GetChild(0).GetComponent<UISprite>().spriteName = Name_;
                break;

            case Part.Top:
                transform.GetChild(0).GetComponent<UISprite>().atlas = My_Room_Canvas.Instance.Top_Atlas;
                transform.GetChild(0).GetComponent<UISprite>().spriteName = Name_;
                break;

            case Part.Pants:
                transform.GetChild(0).GetComponent<UISprite>().atlas = My_Room_Canvas.Instance.Pants_Atlas;
                transform.GetChild(0).GetComponent<UISprite>().spriteName = Name_;
                break;

            case Part.Shoes:
                transform.GetChild(0).GetComponent<UISprite>().atlas = My_Room_Canvas.Instance.Shoes_Atlas;
                transform.GetChild(0).GetComponent<UISprite>().spriteName = Name_;
                break;
        }
    }

    public void Inventory_Item_Slot_Btn()
    {
        int Index = 0;
        switch (this.GetComponent<Item_Slot_Struct>().Type_)
        {
            case Part.Hair:
                Manager.Instance.Wear_Item[0] = index_;
                break;

            case Part.Top:
                Manager.Instance.Wear_Item[1] = index_;

                break;

            case Part.Pants:
                Manager.Instance.Wear_Item[2] = index_;
                break;

            case Part.Shoes:
                Manager.Instance.Wear_Item[3] = index_;
                break;
        }
        StartCoroutine(DB_Manager.Instance.Change_Wear_Item());
        GameObject.Find("MyRoom_Canvas").GetComponent<My_Room_Canvas>().Present_Wear_Setting();
    }
}
