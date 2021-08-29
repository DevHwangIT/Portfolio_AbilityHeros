using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour{
    /*
    public Transform Stick;
    Vector3 axis;

    float radius;
    Vector3 defaultcenter;

    move script_move;
    Touch Touch_Temp;
    
    Transform X_rotation;
    Transform Y_rotation;
    
    float minimumY = -50F;
    float maximumY = 50F;
    float sensitivity = 3.0F;
    
    float rotationY = 0F;
    float rotationX = 0F;

    private void Start()
    {
        defaultcenter = Stick.position;
    }

    private void Update()
    {
        Touch_Proccessor();
    }

    public void Drag_(Vector3 pos)
    {
        Stick.transform.position = pos;
    }
    
    void Touch_Proccessor()
    {
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch_Temp = Input.GetTouch(i);

                    switch (Touch_Temp.phase)
                    {
                        case TouchPhase.Began: //패드를 넘어서 터치될경우.
                            if ((Touch_Temp.position.x > -500 && Touch_Temp.position.x < -700) || (Touch_Temp.position.y < -325 && Touch_Temp.position.y > -125))
                                break;
                            break;

                        //터치후 움직일경우
                        case TouchPhase.Moved:
                            if ((Touch_Temp.position.x > -500 && Touch_Temp.position.x < -700) || (Touch_Temp.position.y < -325 && Touch_Temp.position.y > -125))
                            {
                                rotationX += Touch_Temp.deltaPosition.x * sensitivity * Time.deltaTime;
                                rotationY += Touch_Temp.deltaPosition.y * sensitivity * Time.deltaTime;
                                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
                                X_rotation.localEulerAngles = new Vector3(0, rotationX, 0);
                                Y_rotation.localEulerAngles = new Vector3(-rotationY, 0, 0);
                            }
                            else
                                Move(new Vector3(Touch_Temp.position.x, Touch_Temp.position.y, 0));
                            break;

                        case TouchPhase.Ended:
                            End();
                            break;
                    }
                }
            }
    }

    //Event Drag 처리.
    public void Move(Vector3 touchPos)
    {
        axis = (touchPos - defaultcenter).normalized;

        float Distance = Vector3.Distance(touchPos, defaultcenter);
        if (Distance > radius)
            Stick.position = defaultcenter + axis * radius;
        else
            Stick.position = defaultcenter + axis * Distance;

        script_move.joystick_axis = axis;
    }

    public void End()
    {
        axis = Vector3.zero;
        Stick.position = defaultcenter;

        script_move.joystick_axis = axis;
    }
    
    bool jumping = true;
    public void Jump_Btn_()
    {
        if (jumping) {
            StartCoroutine(Jumping_Corutine());
        }
    }

    IEnumerator Jumping_Corutine()
    {
        jumping = false;
        script_move.animator_.SetBool("jump", true);
        script_move.Jump_Force = 12f;
        yield return new WaitForEndOfFrame();
        script_move.animator_.SetBool("jump", false);
        script_move.Jump_Force = 0f;
        jumping = true;
    }

    bool attack = true;
    public void Attack_Btn()
    {
        if (attack)
        {
            StartCoroutine(Attack_Coroutine());
        }
    }

    IEnumerator Attack_Coroutine()
    {
        attack = false;
        script_move.animator_.SetBool("attack", true);
        yield return new WaitForEndOfFrame();
        script_move.animator_.SetBool("attack", false);
        attack = true;
    }*/
}
