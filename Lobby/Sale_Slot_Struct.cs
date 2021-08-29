using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sale_Slot_Struct : MonoBehaviour {
    
    public string Slot_Name;
    
    public string Sale_Code;

    void Awake()
    {
        this.transform.GetChild(0).GetComponent<UILabel>().text = Slot_Name;
        this.gameObject.name = Sale_Code;
    }
}
