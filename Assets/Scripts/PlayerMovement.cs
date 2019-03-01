using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{


    static bool onGround = false;
    bool jumpingUp = false;
    float runVelX = 9.0f;
    float blockVelX = 5.0f;
    float accelX = 1.0f;
    float gravity = 1.2f;
    float jumpGravity = 0.6f;
    float jumpSpeed = 17.0f;
    float jumpDamping = 0.7f;
    Rigidbody rigid;

    bool wasJumping = false;
    float width = 1.0f;
    int onGroundTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (!GameManager._paused)
        {
            float maxVelX = ((InputManager._block || InputManager._aim) && ThrowShield.hasShield) ? blockVelX : runVelX;
            // If you're pressing right
            if (InputManager._right && !InputManager._left)
            {
                if (rigid.velocity.y > -0.1f && rigid.velocity.y < 0.1f)
                {
                    if (!(InputManager._block && ThrowShield.hasShield))
                        AnimationStateController._state = 2;
                }
                else if (rigid.velocity.y > 0.1f)
                    AnimationStateController._state = 10;
                else if (rigid.velocity.y < -0.1f)
                    AnimationStateController._state = 12;

                if (rigid.velocity.x < maxVelX)
                    rigid.velocity += Vector3.right * accelX;
                else
                    rigid.velocity = new Vector3(maxVelX, rigid.velocity.y, rigid.velocity.z);
            }
            // If you're pressing left
            else if (InputManager._left && !InputManager._right)
            {
                if (rigid.velocity.y > -0.1f && rigid.velocity.y < 0.1f)
                {
                    if (!(InputManager._block && ThrowShield.hasShield))
                        AnimationStateController._state = 3;
                }
                else if (rigid.velocity.y > 0.1f)
                    AnimationStateController._state = 11;
                else if (rigid.velocity.y < -0.1f)
                    AnimationStateController._state = 13;

                if (rigid.velocity.x > -maxVelX)
                    rigid.velocity -= Vector3.right * accelX;
                else
                    rigid.velocity = new Vector3(-maxVelX, rigid.velocity.y, rigid.velocity.z);
            }
            // If you're not pressing right or left
            else
            {
                if (!(InputManager._block && ThrowShield.hasShield))
                {
                    if (rigid.velocity.y > -0.1f && rigid.velocity.y < 0.1f)
                    {
                        if (AnimationStateController._state == 2 || AnimationStateController._state == 4 ||
                           AnimationStateController._state == 6 || AnimationStateController._state == 12)
                            AnimationStateController._state = 0;
                        else if (AnimationStateController._state == 3 || AnimationStateController._state == 5 ||
                            AnimationStateController._state == 7 || AnimationStateController._state == 13)
                            AnimationStateController._state = 1;
                    }
                    else if (rigid.velocity.y > 0.1f)
                    {
                        if (AnimationStateController._state % 2 == 1)
                            AnimationStateController._state = 11;
                        else
                            AnimationStateController._state = 10;
                    }
                    else if (rigid.velocity.y < -0.1f)
                    {
                        if (AnimationStateController._state % 2 == 1)
                            AnimationStateController._state = 13;
                        else
                            AnimationStateController._state = 12;
                    }
                }

                if (Mathf.Abs(rigid.velocity.x) < accelX)
                    rigid.velocity = new Vector3(0.0f, rigid.velocity.y, 0.0f);
                else
                    rigid.velocity -= Mathf.Sign(rigid.velocity.x) * accelX * Vector3.right;
            }

            if (InputManager._jump && !wasJumping && onGround)
            {
                rigid.velocity = new Vector3(rigid.velocity.x, jumpSpeed, 0.0f);
                onGround = false;
                jumpingUp = true;
            }

            if (jumpingUp)
            {
                rigid.velocity += Vector3.down * jumpGravity;
                if (InputManager._jump && rigid.velocity.y > 0)
                {

                }
                else
                {
                    rigid.velocity = new Vector3(rigid.velocity.x, rigid.velocity.y * jumpDamping, 0.0f);
                    jumpingUp = false;
                }
            }
            else
            {
                rigid.velocity += Vector3.down * gravity;
            }

            float dx = Time.fixedDeltaTime * rigid.velocity.x;
            float dy = Time.fixedDeltaTime * rigid.velocity.y;

            bool found = false;
            for (int i = 0; i < 5; i++)
            {
                RaycastHit hit;
                Debug.DrawLine(transform.position + Vector3.right * width * (1.0f / (5.0f - 1.0f) * i - 0.5f), transform.position + Vector3.right * width * (1.0f / (5.0f - 1.0f) * i - 0.5f) + Vector3.down * (1 - dy));
                if (Physics.Raycast(transform.position + Vector3.right * width * (1.0f / (5.0f - 1.0f) * i - 0.5f) + Vector3.down * 0.9f, Vector3.down, out hit, 1 - 0.9f - dy, ~(1 << 2)))
                {
                    onGround = true;
                    onGroundTimer = 5;

                    transform.position = new Vector3(transform.position.x, transform.position.y - hit.distance + 1 - 0.9f, 0.0f);

                    Vector3 parallel = new Vector3(hit.normal.y, -hit.normal.x, 0);

                    rigid.velocity = parallel * Vector3.Dot(parallel, rigid.velocity);

                    if (rigid.velocity.sqrMagnitude < 0.5f && !InputManager._right && !InputManager._left) rigid.velocity = Vector3.zero;

                    found = true;
                    break;
                }
            }
            if (!found)
            {
                if (onGroundTimer != 0)
                {
                    onGroundTimer--;
                }
                else
                {
                    onGround = false;
                }
            }

            wasJumping = InputManager._jump;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Attack")
        {
            if (!InputManager._block)
                GameManager.hits++;
            else if (InputManager._block && !ThrowShield.hasShield)
                GameManager.hits++;
            else if (InputManager._block)
                GameManager.hits += 0.5f;

            if (other.gameObject.name == "LeftAttack")
                rigid.velocity += new Vector3(-15.0f, 5.0f);
            else if (other.gameObject.name == "RightAttack")
                rigid.velocity += new Vector3(15.0f, 5.0f);
        }
    }
}