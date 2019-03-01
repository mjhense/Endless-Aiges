using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public float maxDist;
    public float coolDown;
    BoxCollider bashColl;
    Animator anim;
    GameObject player;
    GameObject shieldParent;
    GameObject shield;
    Vector3 newPos;
    Vector3 playPos;
    bool canBash = true;
    float zAngle;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        shieldParent = player.transform.GetChild(0).gameObject;
        shield = shieldParent.transform.GetChild(0).gameObject;
        shieldParent.transform.GetChild(1).gameObject.SetActive(false);
        bashColl = shieldParent.transform.GetChild(1).gameObject.GetComponent<BoxCollider>();
        anim = transform.parent.GetChild(2).gameObject.GetComponent<Animator>();
        shield.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    IEnumerator BashTimer()
    {
        canBash = false;
        bashColl.enabled = true;
        shieldParent.transform.GetChild(1).gameObject.SetActive(true);
        shieldParent.transform.GetChild(1).gameObject.GetComponent<BashEffect>().StartEffect();
        yield return new WaitForSeconds(0.1f);
        bashColl.enabled = false;
        yield return new WaitForSeconds(coolDown);
        canBash = true;
    }

    void Update()
    {
        if (!GameManager._paused)
        {
            if (canBash && InputManager._bash && ThrowShield.hasShield)
                StartCoroutine(BashTimer());

            if (InputManager._blockPressed && ThrowShield.hasShield)
                shield.SetActive(true);
            if (InputManager._blockReleased)
                shield.SetActive(false);

            switch (InputManager._scheme)
            {
                case Controls.KEYBOARD:
                    TargetPosition();
                    break;
                case Controls.PS4_WIRED:
                case Controls.PS4_BLUETOOTH:
                    TargetPositionController();
                    break;
            }

            zAngle = 180 * Mathf.Atan2(newPos.y, newPos.x) / Mathf.PI;
            shieldParent.transform.eulerAngles = new Vector3(0.0f, 0.0f, zAngle);

            if (InputManager._block && ThrowShield.hasShield)
            {
                if (zAngle > 90.0f || zAngle < -90.0f)
                    AnimationStateController._state = 5;
                else
                    AnimationStateController._state = 4;
            }
        }
    }

    private void TargetPosition()
    {
        newPos = transform.localPosition;
        float mag = Mathf.Sqrt(Mathf.Pow(newPos.x, 2.0f) + Mathf.Pow(newPos.y, 2.0f));

        float xComp = 0.0f;
        if (mag != 0.0f)
            xComp = newPos.x / mag;

        if ((newPos.x <= maxDist * xComp + 0.1f && xComp >= 0.0f) ||
            (newPos.x >= maxDist * xComp - 0.1f && xComp < 0.0f))
            newPos.x += InputManager._xAxis;
        else
            newPos.x = maxDist * xComp;

        float yComp = 0.0f;
        if (mag != 0.0f)
            yComp = newPos.y / mag;

        if ((newPos.y <= maxDist * yComp + 0.1f && yComp >= 0.0f) ||
            (newPos.y >= maxDist * yComp - 0.1f && yComp < 0.0f))
            newPos.y += InputManager._yAxis;
        else
            newPos.y = maxDist * yComp;

        transform.localPosition = newPos;
    }

    private void TargetPositionController()
    {
        newPos = transform.localPosition;
        float mag = Mathf.Sqrt(Mathf.Pow(InputManager._xAxis, 2.0f) + Mathf.Pow(InputManager._yAxis, 2.0f));

        if (mag != 0.0f)
        {
            newPos.x = maxDist * InputManager._xAxis / mag;
            newPos.y = maxDist * InputManager._yAxis / mag;
        }

        transform.localPosition = newPos;
    }
}