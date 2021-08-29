using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Slot_Struct : MonoBehaviour {

    public enum Part
    {
        Attack,
        Defense,
        Utill
    }

    public Part Type_;
    public int index_;
    public string Name_;
    public string info_;
    public int Lv_;
    public float Exp_;

    public void Inventory_Ability_Slot_Btn() //슬롯 버튼을 눌렀을때 해야하는 처리하기.
    {
        My_Room_Canvas.Instance.Present_Skill(Name_, "현재 레벨 -> "+Lv_.ToString(), info_);
    }
}
