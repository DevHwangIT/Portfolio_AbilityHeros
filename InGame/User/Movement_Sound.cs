using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Sound : MonoBehaviour {

    public AudioClip wood_walk_sound;
    public AudioClip wood_run_sound;
    public AudioClip tile_walk_sound;
    public AudioClip tile_run_sound;
    public AudioClip steel_walk_sound;
    public AudioClip steel_run_sound;
    public AudioClip blood_walk_sound;
    public AudioClip rain_walk_sound;

    AudioSource audio_;
    move move_script;

    void Start()
    {
        move_script = GameObject.Find("User").GetComponent<move>();
        audio_ = this.GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        switch (other.transform.tag)
        {
            case "Wood":
                audio_.clip = wood_walk_sound;
                break;

            default:
                audio_.clip = null;
                break;
        }
        if (!audio_.isPlaying && audio_.clip != null)
            audio_.Play();
    }
    
    /*
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.transform.tag + " 형식의 태그에 충돌중입니다.");
        switch (collision.transform.tag)
        {
            case "Wood":
                if (move_script.isrun_ == true)
                    audio_.clip = wood_run_sound;
                else
                    audio_.clip = wood_walk_sound;
                break;

            case "Tile":
                if (move_script.isrun_ == true)
                    audio_.clip = tile_run_sound;
                else
                    audio_.clip = tile_walk_sound;
                break;

            case "Steel":
                if (move_script.isrun_ == true)
                    audio_.clip = steel_run_sound;
                else
                    audio_.clip = steel_walk_sound;
                break;

            default:
                audio_.clip = null;
                break;
        }
        if (!audio_.clip)
            audio_.Play();
    }
    
    void OnCollisionExit(Collision collision)
    {
        if (audio_.isPlaying)
            audio_.Stop();
    }
    */
}
