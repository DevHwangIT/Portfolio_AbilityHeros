using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul_Stone : MonoBehaviour {

    public enum Stone
    {
        Red=0,
        Green=1,
        Blue=2
    }

    PhotonView pv_;

    AudioSource audio_;

    public AudioClip Get_Sound;
    
    public Stone Stone_Type = Stone.Green;

    public float Speed = 100.0f;

    private void Start()
    {
        pv_ = this.GetComponent<PhotonView>();
        audio_ = this.GetComponent<AudioSource>();
    }

    private void Update()
    {
        this.transform.Rotate(Vector3.up * Time.deltaTime * Speed);
    }

    [PunRPC]
    public void Get_Stone(string name)
    {
        //누가 먹었는지 이름으로 판별.
        if (name == Character_State.Instance.My_Character_Info.Nick_Name) {
            switch (this.Stone_Type)
            {
                // 블루 스톤일경우의 처리
                case Stone.Blue:
                    Character_State.Instance.My_Character_Info.Stone_Count++; // 이렇게 처리하면 안된다. 블루 스톤 개수 하나 추가.
                    break;

                // 그린 스톤일경우의 처리
                case Stone.Green:
                    Game_Manager.Instance.Rand_Skill(Ability.Skill_Rank_Height.Row_Rank);
                    Game_Manager.Instance.Setting_Ability
                        (
                        GamePlay_Canvas.Instance.Ability1_Btn,
                        Game_Manager.Instance.My_Unit.GetComponent<Character_State>().My_Character_Info.Row_Rank_Skill,
                        Game_Manager.Instance.My_Unit.GetComponent<Character_State>().My_Character_Info.Row_Skill_Level

                        )
                    ;
                    break;

                // 레드 스톤일경우의 처리
                case Stone.Red:
                    Game_Manager.Instance.Rand_Skill(Ability.Skill_Rank_Height.High_Rank);
                    Game_Manager.Instance.Setting_Ability
                        (
                        GamePlay_Canvas.Instance.Ability2_Btn,
                        Game_Manager.Instance.My_Unit.GetComponent<Character_State>().My_Character_Info.High_Rank_Skill,
                        Game_Manager.Instance.My_Unit.GetComponent<Character_State>().My_Character_Info.High_Skill_Level
                        );
                    break;

                default:
                    break;
            }
        }
    }

    [PunRPC]
    public void Destroy_Stone(int id, int view)
    {
        //마스터 클라이언트로 오브젝트를 찾지않는 이유-> 게임 도중 마스터클라이언트가 변경될 수 있으므로
        if (pv_.ownerId == id) { //해당 오브젝트가 자신이 맞는지 파악.
            if (pv_.viewID == view) { // 해당 오브젝트 생성이 자신인지 파악.
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }
    
    //유저가 스톤을 획득
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {            
            this.GetComponent<BoxCollider>().enabled = false; //혹여라도 처리 중 재획득하는것을 방지.
            audio_.clip = Get_Sound; //획득 사운드 추가.
            audio_.Play();
            pv_.RPC("Get_Stone", PhotonTargets.All, other.GetComponent<PhotonView>().owner.NickName);
            StartCoroutine(Call_Get_Stone(audio_.clip.length, other));
        }
    }

    IEnumerator Call_Get_Stone(float time,Collider other)
    {
        yield return new WaitForSeconds(time/2);
        pv_.RPC("Destroy_Stone", PhotonTargets.All, this.gameObject.GetComponent<PhotonView>().ownerId, this.gameObject.GetComponent<PhotonView>().viewID);
    }
}
