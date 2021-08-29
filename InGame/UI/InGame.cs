using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame : MonoBehaviour {

    public static InGame Instance;

    public delegate void UI_Delegate();
    
    public AudioSource[] BGM_List;

    public UI_Delegate Play_Canvas_Open;
    public UI_Delegate play_Canvas_Close;

    public UI_Delegate Observer_Canvas_Open;
    public UI_Delegate Observer_Canvas_Close;

    public UI_Delegate Timer_Canvas_Open;
    public UI_Delegate Timer_Canvas_Close;

    public UI_Delegate Notice_Canvas_Open;
    public UI_Delegate Notice_Canvas_Close;

    public UI_Delegate Result_Canvas_Open;
    public UI_Delegate Result_Canvas_Close;

    public UI_Delegate Loading_Canvas_Open;
    public UI_Delegate Loading_Canvas_Close;

    public UI_Delegate InGame_UI_Initialize;
    public UI_Delegate Play_Canvas_Initialize;
    public UI_Delegate Observer_Canvas_Initialize;
    public UI_Delegate Timer_Canvas_Initialize;
    public UI_Delegate Notice_Canvas_Initialize;
    public UI_Delegate Result_Canvas_Initialize;
    public UI_Delegate Loading_Canvas_Initialize;

    public UI_Delegate Skill_Effect_Open;
    public UI_Delegate Skill_Effect_Close;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void Start()
    {
        StartCoroutine(Event_Set());
        InGame_UI_Initialize();
        Loading_Canvas_Open();
        Game_Loading_Canvas.Instance.Loading_Initialized();
    }

    IEnumerator Event_Set()
    {
        Play_Canvas_Open = new UI_Delegate(GamePlay_Canvas.Instance.Open);
        play_Canvas_Close = new UI_Delegate(GamePlay_Canvas.Instance.Close);

        Observer_Canvas_Open = new UI_Delegate(Observer_Canvas.Instance.Open);
        Observer_Canvas_Close = new UI_Delegate(Observer_Canvas.Instance.Close);

        Timer_Canvas_Open = new UI_Delegate(Timer_Canvas.Instance.Open);
        Timer_Canvas_Close = new UI_Delegate(Timer_Canvas.Instance.Close);

        Notice_Canvas_Open = new UI_Delegate(Notice_Canvas.Instance.Open);
        Notice_Canvas_Close = new UI_Delegate(Notice_Canvas.Instance.Close);

        Result_Canvas_Open = new UI_Delegate(Game_Result_Canvas.Instance.Open);
        Result_Canvas_Close = new UI_Delegate(Game_Result_Canvas.Instance.Close);

        Loading_Canvas_Open = new UI_Delegate(Game_Loading_Canvas.Instance.Open);
        Loading_Canvas_Close = new UI_Delegate(Game_Loading_Canvas.Instance.Close);

        Skill_Effect_Open = new UI_Delegate(GamePlay_Canvas.Instance.High_Skill_Effect_Open);
        Skill_Effect_Close = new UI_Delegate(GamePlay_Canvas.Instance.High_Skill_Effect_Close);

        InGame_UI_Initialize = play_Canvas_Close;
        InGame_UI_Initialize += Observer_Canvas_Close;
        InGame_UI_Initialize += Timer_Canvas_Close;
        InGame_UI_Initialize += Notice_Canvas_Close;
        InGame_UI_Initialize += Result_Canvas_Close;
        InGame_UI_Initialize += Loading_Canvas_Close;

        Play_Canvas_Initialize = InGame_UI_Initialize;
        Play_Canvas_Initialize += Play_Canvas_Open;
        Play_Canvas_Initialize += Timer_Canvas_Open;

        Observer_Canvas_Initialize = InGame_UI_Initialize;
        Observer_Canvas_Initialize += Observer_Canvas_Open;
        Observer_Canvas_Initialize += Timer_Canvas_Open;
        
        Timer_Canvas_Initialize += Timer_Canvas_Open;
        
        Notice_Canvas_Initialize += Notice_Canvas_Open;

        Result_Canvas_Initialize = InGame_UI_Initialize;
        Result_Canvas_Initialize += Result_Canvas_Open;

        Loading_Canvas_Initialize = InGame_UI_Initialize;
        Loading_Canvas_Initialize += Loading_Canvas_Open;

        yield return null;
    }
}
