using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment_Type : MonoBehaviour {

    public Eqipment_Kinds Type;

    public enum Eqipment_Kinds
    {
        Head_Top,
        Top_Pant,
        Head_Top_Pant,
        Head_Top_Pant_Shoes,
    }
}
