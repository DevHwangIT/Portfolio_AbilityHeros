using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyChat_Canvas : MonoBehaviour, UI_Interface
{
    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    public void Open()
    {
        this.gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
