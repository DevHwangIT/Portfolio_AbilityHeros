using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clothes_Manager : MonoBehaviour {

    public GameObject Hair;
    public GameObject Top;
    public GameObject Pants;
    public GameObject Shoes;

    [HideInInspector]
    Transform Pos;

    // 이렇게 비어있는 게임 오브젝트를 선언 해준다.
    private GameObject Hair_Object;
    private GameObject Top_Object;
    private GameObject Pants_Object;
    private GameObject Shoes_Object;

    PhotonView pv_;

    public enum Part{
        Hair,
        Top,
        Pants,
        Shoes
    }

    //AddEquipment, RemoveEquipment 두가지 메소드를 이용해서 장비를 착용하거나 벗음.

    void Awake()
    {
        pv_ = this.gameObject.GetComponent<PhotonView>();
        Pos = GameObject.Find("Null_Pos").transform;
    }

    private void Start()
    {
        if (this.GetComponent<PhotonView>() == null)
        {
            if (Hair != null)
                Equipment_Set(Part.Hair);
            if (Top != null)
                Equipment_Set(Part.Top);
            if (Pants != null)
                Equipment_Set(Part.Pants);
            if (Shoes != null)
                Equipment_Set(Part.Shoes);
        }
        else
        {
            pv_.RPC("InGame_Wear_Set", PhotonTargets.All, Manager.Instance.User.Nick_Name, Manager.Instance.Wear_Item);
        }
    }

    void Top_Type_Set() //Top부분에 후드,전신옷등을 대비한 처리
    {
        if (!Top.GetComponent<Equipment_Type>())
        {
            return;
        }
        else
        {
            switch (Top.GetComponent<Equipment_Type>().Type)
            {
                case Equipment_Type.Eqipment_Kinds.Head_Top:
                    if (Hair != null)
                        Hair = null;
                    RemoveEquipment(Part.Hair);
                    break;

                case Equipment_Type.Eqipment_Kinds.Top_Pant:
                    if (Pants != null)
                        Pants = null;
                    RemoveEquipment(Part.Pants);
                    break;

                case Equipment_Type.Eqipment_Kinds.Head_Top_Pant:
                    if (Hair != null)
                        Hair = null;
                    if (Pants != null)
                        Pants = null;
                    RemoveEquipment(Part.Hair);
                    RemoveEquipment(Part.Pants);
                    break;

                case Equipment_Type.Eqipment_Kinds.Head_Top_Pant_Shoes:
                    if (Hair != null)
                        Hair = null;
                    if (Pants != null)
                        Pants = null;
                    if (Shoes != null)
                        Shoes = null;
                    RemoveEquipment(Part.Hair);
                    RemoveEquipment(Part.Pants);
                    RemoveEquipment(Part.Shoes);
                    break;
            }
        }
    }
    
    public void Equipment_Set(Part Select)
    {
        RemoveEquipment(Select);
        AddEquipment(Select);
    }

    // 착용 메소드
    private void AddEquipment(Part Select)
    {
        SkinnedMeshRenderer[] BonedObjects;
        switch (Select)
        {
            case Part.Hair:
                Hair_Object = Hair;//GameObject.Instantiate(Hair, Pos.transform.position, Quaternion.identity);
                Hair_Object.transform.parent = Pos;
                Hair_Object.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                //// 머 일단 이렇게 아랫쪽에 있을 SkinnedMeshRenderer를 받아둔다.
                BonedObjects = Hair.GetComponentsInChildren<SkinnedMeshRenderer>();
                // 배열 속에 들어있을 SkinnedMeshRenderer에 대해 뭔가를 해 준다.
                foreach (SkinnedMeshRenderer smr in BonedObjects)
                    ProcessBonedObject(smr, Part.Hair);
                // 머 이렇게 하면 이전에 처리된 오브젝트를 없에준다고 한다. 그냥 랜더 끄는거같은데?
                //Hair.SetActiveRecursively(false);
                break;

            case Part.Top:
                Top_Type_Set(); //후드티 등으로 인해서 머리카락등과 렌더링 겹치는것을 막기위함.
                Top_Object = Top;//GameObject.Instantiate(Top, Pos.transform.position, Quaternion.identity);
                Top_Object.transform.parent = Pos;
                Top_Object.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                BonedObjects = Top.GetComponentsInChildren<SkinnedMeshRenderer>();
                // 배열 속에 들어있을 SkinnedMeshRenderer에 대해 뭔가를 해 준다.
                foreach (SkinnedMeshRenderer smr in BonedObjects)
                    ProcessBonedObject(smr, Part.Top);
                break;

            case Part.Pants:
                Pants_Object = Pants;//GameObject.Instantiate(Pants, Pos.transform.position, Quaternion.identity);
                Pants_Object.transform.parent = Pos;
                Pants_Object.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                BonedObjects = Pants.GetComponentsInChildren<SkinnedMeshRenderer>();
                // 배열 속에 들어있을 SkinnedMeshRenderer에 대해 뭔가를 해 준다.
                foreach (SkinnedMeshRenderer smr in BonedObjects)
                    ProcessBonedObject(smr, Part.Pants);
                break;

            case Part.Shoes:
                Shoes_Object = Shoes;//GameObject.Instantiate(Shoes, Pos.transform.position, Quaternion.identity);
                Shoes_Object.transform.parent = Pos;
                Shoes_Object.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                BonedObjects = Shoes.GetComponentsInChildren<SkinnedMeshRenderer>();
                // 배열 속에 들어있을 SkinnedMeshRenderer에 대해 뭔가를 해 준다.
                foreach (SkinnedMeshRenderer smr in BonedObjects)
                    ProcessBonedObject(smr, Part.Shoes);
                break;
        }
    }

    //장비 제거 메소드
    private void RemoveEquipment(Part Select)
    {
        // 없엘 때는 이렇게 없에면 되는 듯 하다.
        switch (Select)
        {
            case Part.Hair:
                if (Hair_Object != null) {
                    Destroy(Hair_Object);
                    //Destroy(Hair);
                    //Hair.SetActiveRecursively(true); //착용 안된 오브젝트를 나타나게 한다.
                }
                break;

            case Part.Top:
                if (Top_Object != null)
                {
                    Destroy(Top_Object);
                    //Destroy(Top);
                    //Top.SetActiveRecursively(true);
                }
                break;

            case Part.Pants:
                if (Pants_Object != null)
                {
                    Destroy(Pants_Object);
                    //Destroy(Pants);
                    //Pants.SetActiveRecursively(true);
                }
                break;

            case Part.Shoes:
                if (Shoes_Object != null)
                {
                    Destroy(Shoes_Object);
                    //Destroy(Shoes);
                    //Shoes.SetActiveRecursively(true);
                }
                break;
        }
        Pos.DestroyChildren();
    }

    private void ProcessBonedObject(SkinnedMeshRenderer ThisRenderer,Part Select)
    {
        SkinnedMeshRenderer NewRenderer;
        Transform[] MyBones;

        switch (Select)
        {
            case Part.Hair:
                // 캐릭터에게 입힐 바지 오브젝트를 새로 만들자.
                Hair_Object = new GameObject(ThisRenderer.gameObject.name);

                // 이 위치에 새로운 버자(팬티)가 하위 객체로 생성되었다.
                Hair_Object.transform.parent = transform;

                // 스크립트 상에서는 랜더러를 이렇게 추가하는듯 하다. 앞에 아무것도 안 쓰면 public인듯.. 근데 함수 끝나면 사라질거 같은데 안사라짐 -_-?
                NewRenderer = Hair_Object.AddComponent(typeof(SkinnedMeshRenderer)) as SkinnedMeshRenderer;

                // 본도 받아야 한다. 일단 크기만큼 할당을 하자. ※왠지 SkinnedMeshRenderer에 본이 있다.
                MyBones = new Transform[ThisRenderer.bones.Length];

                // 이 함수는 아래에 있다.
                for (int i = 0; i < ThisRenderer.bones.Length; i++)
                    MyBones[i] = FindChildByName(ThisRenderer.bones[i].name, transform); // 하위 본을 전부 맞추는게 편하니 이렇게 하자.
                                                 
                // 랜더러를 할당한다..고 한다. 바지(팬티)의 새로운 랜더로써, 갖가지 기능을 여기서 처리할 수 있다. matrial을 바꿔서 바지 색을 바꾼다던가..(누런 팬티)
                NewRenderer.bones = MyBones;
                NewRenderer.sharedMesh = ThisRenderer.sharedMesh; // 어쨰서 그냥 mesh라는 키워드가 없는지는 모르겠는데 그게 이건가보다.
                NewRenderer.materials = ThisRenderer.sharedMaterials;
                NewRenderer.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                break;

            case Part.Top:
                // 캐릭터에게 입힐 바지 오브젝트를 새로 만들자.
                Top_Object = new GameObject(ThisRenderer.gameObject.name);

                // 이 위치에 새로운 버자(팬티)가 하위 객체로 생성되었다.
                Top_Object.transform.parent = transform;

                // 스크립트 상에서는 랜더러를 이렇게 추가하는듯 하다. 앞에 아무것도 안 쓰면 public인듯.. 근데 함수 끝나면 사라질거 같은데 안사라짐 -_-?
                NewRenderer = Top_Object.AddComponent(typeof(SkinnedMeshRenderer)) as SkinnedMeshRenderer;

                // 본도 받아야 한다. 일단 크기만큼 할당을 하자. ※왠지 SkinnedMeshRenderer에 본이 있다.
                MyBones = new Transform[ThisRenderer.bones.Length];

                // 이 함수는 아래에 있다.
                for (int i = 0; i < ThisRenderer.bones.Length; i++)
                    MyBones[i] = FindChildByName(ThisRenderer.bones[i].name, transform); // 하위 본을 전부 맞추는게 편하니 이렇게 하자.

                // 랜더러를 할당한다..고 한다. 바지(팬티)의 새로운 랜더로써, 갖가지 기능을 여기서 처리할 수 있다. matrial을 바꿔서 바지 색을 바꾼다던가..(누런 팬티)
                NewRenderer.bones = MyBones;
                NewRenderer.sharedMesh = ThisRenderer.sharedMesh; // 어쨰서 그냥 mesh라는 키워드가 없는지는 모르겠는데 그게 이건가보다.
                NewRenderer.materials = ThisRenderer.sharedMaterials;
                NewRenderer.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                break;

            case Part.Pants:
                // 캐릭터에게 입힐 바지 오브젝트를 새로 만들자.
                Pants_Object = new GameObject(ThisRenderer.gameObject.name);

                // 이 위치에 새로운 버자(팬티)가 하위 객체로 생성되었다.
                Pants_Object.transform.parent = transform;

                // 스크립트 상에서는 랜더러를 이렇게 추가하는듯 하다. 앞에 아무것도 안 쓰면 public인듯.. 근데 함수 끝나면 사라질거 같은데 안사라짐 -_-?
                NewRenderer = Pants_Object.AddComponent(typeof(SkinnedMeshRenderer)) as SkinnedMeshRenderer;

                // 본도 받아야 한다. 일단 크기만큼 할당을 하자. ※왠지 SkinnedMeshRenderer에 본이 있다.
                MyBones = new Transform[ThisRenderer.bones.Length];

                // 이 함수는 아래에 있다.
                for (int i = 0; i < ThisRenderer.bones.Length; i++)
                    MyBones[i] = FindChildByName(ThisRenderer.bones[i].name, transform); // 하위 본을 전부 맞추는게 편하니 이렇게 하자.

                // 랜더러를 할당한다..고 한다. 바지(팬티)의 새로운 랜더로써, 갖가지 기능을 여기서 처리할 수 있다. matrial을 바꿔서 바지 색을 바꾼다던가..(누런 팬티)
                NewRenderer.bones = MyBones;
                NewRenderer.sharedMesh = ThisRenderer.sharedMesh; // 어쨰서 그냥 mesh라는 키워드가 없는지는 모르겠는데 그게 이건가보다.
                NewRenderer.materials = ThisRenderer.sharedMaterials;
                NewRenderer.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                break;

            case Part.Shoes:
                // 캐릭터에게 입힐 바지 오브젝트를 새로 만들자.
                Shoes_Object = new GameObject(ThisRenderer.gameObject.name);

                // 이 위치에 새로운 버자(팬티)가 하위 객체로 생성되었다.
                Shoes_Object.transform.parent = transform;

                // 스크립트 상에서는 랜더러를 이렇게 추가하는듯 하다. 앞에 아무것도 안 쓰면 public인듯.. 근데 함수 끝나면 사라질거 같은데 안사라짐 -_-?
                NewRenderer = Shoes_Object.AddComponent(typeof(SkinnedMeshRenderer)) as SkinnedMeshRenderer;

                // 본도 받아야 한다. 일단 크기만큼 할당을 하자. ※왠지 SkinnedMeshRenderer에 본이 있다.
                MyBones = new Transform[ThisRenderer.bones.Length];

                // 이 함수는 아래에 있다.
                for (int i = 0; i < ThisRenderer.bones.Length; i++)
                    MyBones[i] = FindChildByName(ThisRenderer.bones[i].name, transform); // 하위 본을 전부 맞추는게 편하니 이렇게 하자.

                // 랜더러를 할당한다..고 한다. 바지(팬티)의 새로운 랜더로써, 갖가지 기능을 여기서 처리할 수 있다. matrial을 바꿔서 바지 색을 바꾼다던가..(누런 팬티)
                NewRenderer.bones = MyBones;
                NewRenderer.sharedMesh = ThisRenderer.sharedMesh; // 어쨰서 그냥 mesh라는 키워드가 없는지는 모르겠는데 그게 이건가보다.
                NewRenderer.materials = ThisRenderer.sharedMaterials;
                NewRenderer.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                break;
        }
       
    }
    
    // 머 뺑뺑이 돌리면서 찾는다고 한다.
    private Transform FindChildByName(string ThisName, Transform ThisGObj)
    {
        // 리턴용 임시 Transform.
        Transform ReturnObj;
        
        // 검색 조건에 맞으면 리턴한다.
        if (ThisGObj.name == ThisName)
            return ThisGObj.transform;
        
        // 안 맞으면 계층구조의 가로 세로로 계속 찾게 한다.
        // 아무래도, child의 child의... 해서 찾게 하려고 이렇게 하는 듯.

        // 대체 foreach문이 먼진 모르겠는데, 대충 문맥상 in 오른쪽에 들어있는 왼쪽 타입에 대해...라는 구문 같다.
        foreach (Transform child in ThisGObj)
        {
            // 재귀함수?!
            ReturnObj = FindChildByName(ThisName, child);

            if (ReturnObj != null)
                return ReturnObj;
        }

        return null;
    }

    [PunRPC]
    public void InGame_Wear_Set(string player,int[] wear)
    {
        if (this.GetComponent<PhotonView>().owner.NickName == player) {
            Hair = Instantiate(Resources.Load<GameObject>("Custom\\Hair\\Hair_" + wear[0]),Vector3.zero,Quaternion.identity);
            Top = Instantiate(Resources.Load<GameObject>("Custom\\Top\\Top_" + wear[1]), Vector3.zero, Quaternion.identity);
            Pants = Instantiate(Resources.Load<GameObject>("Custom\\Pants\\Pants_" + wear[2]), Vector3.zero, Quaternion.identity);
            Shoes = Instantiate(Resources.Load<GameObject>("Custom\\Shoes\\Shoes_" + wear[3]), Vector3.zero, Quaternion.identity);
            StartCoroutine("Wear_All_Set");
        }
    }

    IEnumerator Wear_All_Set()
    {
        AddEquipment(Part.Hair);
        AddEquipment(Part.Top);
        AddEquipment(Part.Pants);
        AddEquipment(Part.Shoes);
        yield return null;
    }
}
