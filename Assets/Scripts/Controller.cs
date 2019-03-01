using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;

    private int jumpGraceFrames = 0;
    private float jumpGravity = -28;
    private float fallGravity = -40;
    private float jumpSpeed = 15;
    private float jumpReleaseMultiplyer = 0.7f;
    private float runAcceleration = 40;
    private float maxRunSpeed = 8;
    private float maxWalkSpeed = 5;
    private float deceleration = 40;

    private bool jumping = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        controller.detectCollisions = true;
    }

    // Update is called once per frame
    void Update()
    {

        // Apply gravity
        if (jumping)
        {
            moveDirection.y = moveDirection.y + (jumpGravity * Time.deltaTime);
        }
        else
        {
            moveDirection.y = moveDirection.y + (fallGravity * Time.deltaTime);
        }
        
        // Handle Collisions with ground and jump grace frames
        if (controller.isGrounded)
        {
            jumpGraceFrames = 5;
            moveDirection.y = 0;
        }
        else
        {
            if (jumpGraceFrames > 0) jumpGraceFrames--;
        }

        // Jump
        if (jumpGraceFrames > 0)
        {
            if (InputManager._jumpPressed)
            {
                moveDirection += jumpSpeed * Vector3.up;
                jumpGraceFrames = 0;
                jumping = true;
            }
        }

        // Release jump
        if (jumping && (InputManager._jumpReleased || moveDirection.y <= 0))
        {
            jumping = false;
            moveDirection.y *= jumpReleaseMultiplyer;
        }

        // Horizontal movement
        if (InputManager._right && !InputManager._left)
        {
            moveDirection.x = Mathf.Min(maxRunSpeed, moveDirection.x + runAcceleration * Time.deltaTime);
        }
        else if (InputManager._left && !InputManager._right)
        {
            moveDirection.x = Mathf.Max(-maxRunSpeed, moveDirection.x - runAcceleration * Time.deltaTime);
        }
        else
        {
            if (Mathf.Abs(moveDirection.x) <= deceleration * Time.deltaTime)
            {
                moveDirection.x = 0;
            }
            else
            {
                moveDirection.x -= deceleration * Time.deltaTime * Mathf.Sign(moveDirection.x);
            }
        }





        controller.Move(moveDirection * Time.deltaTime);
    }
}
