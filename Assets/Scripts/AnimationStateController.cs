using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    public static int _state = 0;
    Animator anim;
    GameObject extraShield;
    int prevState = 0;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        extraShield = GameObject.FindGameObjectWithTag("ExtraShield");
    }
    
    void Update()
    {
        if (ThrowShield.hasShield && !InputManager._block)
            extraShield.SetActive(true);
        else
            extraShield.SetActive(false);

        if (prevState != _state)
        {
            switch (_state)
            {
                //Revive
                case -1:
                    anim.SetInteger("state", -1);
                    gameObject.transform.localPosition = new Vector3(0.25f, -1.0f, 0.0f);
                    gameObject.transform.localEulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
                    break;
                //Idle right
                case 0:
                    anim.SetInteger("state", 0);
                    gameObject.transform.localPosition = new Vector3(0.25f, -1.0f, 0.0f);
                    gameObject.transform.localEulerAngles = new Vector3(0.0f, 120.0f, 0.0f);
                    break;
                //Idle left
                case 1:
                    anim.SetInteger("state", 0);
                    gameObject.transform.localPosition = new Vector3(-0.25f, -1.0f, 0.0f);
                    gameObject.transform.localEulerAngles = new Vector3(0.0f, -120.0f, 0.0f);
                    break;
                //Run right
                case 2:
                    anim.SetInteger("state", 1);
                    gameObject.transform.localPosition = new Vector3(0.25f, -1.0f, 0.0f);
                    gameObject.transform.localEulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
                    break;
                //Run left
                case 3:
                    anim.SetInteger("state", 1);
                    gameObject.transform.localPosition = new Vector3(-0.25f, -1.0f, 0.0f);
                    gameObject.transform.localEulerAngles = new Vector3(0.0f, -90.0f, 0.0f);
                    break;
                //Block right
                case 4:
                    anim.SetInteger("state", 2);
                    gameObject.transform.localPosition = new Vector3(0.25f, -1.0f, 0.0f);
                    gameObject.transform.localEulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
                    break;
                //Block left
                case 5:
                    anim.SetInteger("state", 2);
                    gameObject.transform.localPosition = new Vector3(-0.25f, -1.0f, 0.0f);
                    gameObject.transform.localEulerAngles = new Vector3(0.0f, -90.0f, 0.0f);
                    break;
                //Throwing right
                case 8:
                    anim.SetInteger("state", 3);
                    gameObject.transform.localPosition = new Vector3(0.25f, -1.0f, 0.0f);
                    gameObject.transform.localEulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
                    break;
                //Throwing left
                case 9:
                    anim.SetInteger("state", 3);
                    gameObject.transform.localPosition = new Vector3(-0.25f, -1.0f, 0.0f);
                    gameObject.transform.localEulerAngles = new Vector3(0.0f, -90.0f, 0.0f);
                    break;
                //Jumping right
                case 10:
                    anim.SetInteger("state", 5);
                    gameObject.transform.localPosition = new Vector3(0.25f, -1.0f, 0.0f);
                    gameObject.transform.localEulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
                    break;
                //Jumping left
                case 11:
                    anim.SetInteger("state", 5);
                    gameObject.transform.localPosition = new Vector3(-0.25f, -1.0f, 0.0f);
                    gameObject.transform.localEulerAngles = new Vector3(0.0f, -90.0f, 0.0f);
                    break;
                //Falling right
                case 12:
                    anim.SetInteger("state", 6);
                    gameObject.transform.localPosition = new Vector3(0.25f, -1.0f, 0.0f);
                    gameObject.transform.localEulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
                    break;
                //Falling left
                case 13:
                    anim.SetInteger("state", 6);
                    gameObject.transform.localPosition = new Vector3(-0.25f, -1.0f, 0.0f);
                    gameObject.transform.localEulerAngles = new Vector3(0.0f, -90.0f, 0.0f);
                    break;
                //BlockWalk right
                case 14:
                    break;
                //BlockWalk left
                case 15:
                    break;
            }
        }
        prevState = _state;
    }
}