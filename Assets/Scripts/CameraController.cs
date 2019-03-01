using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed;
    public float firstDist;
    public float secondDist;
    public float yDist;
    public float yMin;
    GameObject player;
    GameObject shieldParent;
    bool facingRight = true;
    Vector3 pos;
    Vector3 playerPos;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        shieldParent = player.transform.GetChild(0).gameObject;
    }

    void Start()
    {

    }

    void FixedUpdate()
    {
        if (!GameManager._paused)
        {
            pos = transform.position;
            playerPos = player.transform.position;

            if (pos.y >= yMin)
                pos.y = player.transform.position.y + yDist;
            else
                pos.y = yMin;

            if ((InputManager._block || InputManager._aim) && ThrowShield.hasShield)
            {
                if (shieldParent.transform.eulerAngles.z > 90.0f && shieldParent.transform.eulerAngles.z < 270.0f)
                    facingRight = false;
                else
                    facingRight = true;

                if (facingRight)
                {
                    if (pos.x + speed * Time.deltaTime < playerPos.x + firstDist)
                        pos.x += speed * Time.deltaTime;
                    else if (pos.x - speed * Time.deltaTime > playerPos.x + firstDist)
                        pos.x -= speed * Time.deltaTime;
                    else if (pos.x < playerPos.x + firstDist && player.GetComponent<Rigidbody>().velocity.x > 0.0f)
                        pos.x += (playerPos.x + firstDist) - pos.x;
                    else if (pos.x > playerPos.x + firstDist && player.GetComponent<Rigidbody>().velocity.x < 0.0f)
                        pos.x += (playerPos.x + firstDist) - pos.x;
                }
                else
                {
                    if (pos.x - speed * Time.deltaTime > playerPos.x - firstDist)
                        pos.x -= speed * Time.deltaTime;
                    else if (pos.x + speed * Time.deltaTime < playerPos.x - firstDist)
                        pos.x += speed * Time.deltaTime;
                    else if (pos.x > playerPos.x - firstDist && player.GetComponent<Rigidbody>().velocity.x < 0.0f)
                        pos.x += (playerPos.x - firstDist) - pos.x;
                    else if (pos.x < playerPos.x - firstDist && player.GetComponent<Rigidbody>().velocity.x > 0.0f)
                        pos.x += (playerPos.x - firstDist) - pos.x;
                }
            }
            else
            {
                if (facingRight)
                {
                    if (pos.x + speed * Time.deltaTime < playerPos.x + firstDist)
                        pos.x += speed * Time.deltaTime;
                    else if (pos.x < playerPos.x + firstDist && player.GetComponent<Rigidbody>().velocity.x > 0.0f)
                        pos.x += (playerPos.x + firstDist) - pos.x;

                    if (pos.x > playerPos.x + secondDist)
                        facingRight = false;
                }
                else
                {
                    if (pos.x - speed * Time.deltaTime > playerPos.x - firstDist)
                        pos.x -= speed * Time.deltaTime;
                    else if (pos.x > playerPos.x - firstDist && player.GetComponent<Rigidbody>().velocity.x < 0.0f)
                        pos.x += (playerPos.x - firstDist) - pos.x;

                    if (pos.x < playerPos.x - secondDist)
                        facingRight = true;
                }
            }
            if (pos.y >= yMin)
                pos.y = player.transform.position.y + yDist;
            else
                pos.y = yMin;


            transform.position = pos; 
        }
    }
}