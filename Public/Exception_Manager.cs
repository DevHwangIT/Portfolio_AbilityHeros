using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exception_Manager : MonoBehaviour {
    
    UILabel Error_Message;

    public AudioClip Error;
    AudioSource Audio_;

    private void Start()
    {
        Audio_ = this.GetComponent<AudioSource>();
        this.transform.parent = GameObject.Find("Pannel").transform;
        this.transform.localScale = new Vector3(1, 1, 1);
        Audio_.clip = Error;
        Audio_.Play();
        Error_Message = this.transform.GetChild(1).GetComponent<UILabel>();
    }

    public void Back_Btn()
    {
        Destroy(this.gameObject);
    }
}
