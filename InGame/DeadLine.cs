using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeadLine : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player":
                if (other.GetComponent<Character_State>() != null)
                other.GetComponent<Character_State>().My_Character_Info.HP = 0;
                break;
        }
    }
}
