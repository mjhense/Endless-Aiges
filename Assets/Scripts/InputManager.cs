using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * PS4 controller inputs
 * Buttons:
 *  PS4_Square
 *  PS4_X
 *  PS4_Circle
 *  PS4_Triangle
 *  PS4_L1
 *  PS4_R1
 *  PS4_L2
 *  PS4_R2
 *  PS4_Share
 *  PS4_Options
 *  PS4_L3
 *  PS4_R3
 *  PS4_PS
 *  PS4_PadPress
 * Axes:
 *  PS4_LeftStick_X (-1-left, 0-nothing, 1-right)
 *  PS4_LeftStick_Y (1-up, 0-nothing, -1-down)
 *  PS4_RightStick_X (-1-left, 0-nothing, 1-right)
 *  PS4_RightStick_Y (1-up, 0-nothing, -1-down)
 *  PS4_L2_Axis (1-all the way pressed, -1-resting)
 *  PS4_R2_Axis (1-all the way pressed, -1-resting)
 *  PS4_DPad_X (-1-left, 0-nothing, 1-right)
 *  PS4_DPad_Y (1-up, 0-nothing, -1-down)
 *  PS4_DPad_X_BT (-1-left, 0-nothing, 1-right)
 *  PS4_RightStick_X_BT (-1-left, 0-nothing, 1-right)
 */

public class InputManager : MonoBehaviour
{
    public static InputManager _instance;
    public static bool _up, _down, _left, _right;
    public static bool _jumpPressed, _jumpReleased, _jump, _block, _bash, _aim, _release, _blockPressed, _blockReleased, _aimPressed;
    public static bool _pause, _interact, _ps4AnyButtonDown;
    public static float _yAxis, _xAxis;
    public static bool _pickup;
    public static Controls _scheme;
    public Controls scheme;
    public float sensitivity;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(this);
        }

        _scheme = scheme;
    }

    void Start()
    {
        _up = _down = _left = _right = false;
        _jumpPressed = _jump = _block = _bash = _aim = _release = false;
        _pause = _interact = _ps4AnyButtonDown = false;
        _yAxis = _xAxis = 0.0f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetButtonDown("PS4_Share"))
        {
            switch (_scheme)
            {
                case Controls.KEYBOARD:
                    _scheme = Controls.PS4_WIRED;
                    break;
                case Controls.PS4_WIRED:
                    _scheme = Controls.PS4_BLUETOOTH;
                    break;
                case Controls.PS4_BLUETOOTH:
                    _scheme = Controls.KEYBOARD;
                    break;
            }
        }

        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0 || Input.anyKeyDown)
            _scheme = Controls.KEYBOARD;

        _ps4AnyButtonDown = (Input.GetButtonDown("PS4_X") ||
           Input.GetButtonDown("PS4_Square") || Input.GetButtonDown("PS4_Circle") || Input.GetButtonDown("PS4_Triangle") ||
           Input.GetButtonDown("PS4_R1") || Input.GetButtonDown("PS4_L1") || Input.GetButtonDown("PS4_R2") ||
           Input.GetButtonDown("PS4_L2") || Input.GetAxis("PS4_DPad_X") != 0 || Input.GetAxis("PS4_DPad_Y") != 0);

        if (_ps4AnyButtonDown || Input.GetAxis("PS4_RightStick_X") != 0 || Input.GetAxis("PS4_LeftStick_X") != 0)
            _scheme = Controls.PS4_BLUETOOTH;

        switch (_scheme)
        {
            case Controls.KEYBOARD:
                _up = Input.GetKey(KeyCode.W);
                _down = Input.GetKey(KeyCode.S);
                _left = Input.GetKey(KeyCode.A);
                _right = Input.GetKey(KeyCode.D);

                _jump = Input.GetKey(KeyCode.Space);
                _jumpPressed = Input.GetKeyDown(KeyCode.Space);
                _jumpReleased = Input.GetKeyUp(KeyCode.Space);

                _bash = Input.GetMouseButtonDown(0);
                _block = Input.GetMouseButton(0);
                _blockPressed = Input.GetMouseButtonDown(0);
                _blockReleased = Input.GetMouseButtonUp(0);

                _aim = Input.GetMouseButton(1);
                _aimPressed = Input.GetMouseButtonDown(1);
                _release = Input.GetMouseButtonUp(1);

                _pause = Input.GetKeyDown(KeyCode.Escape);
                _interact = Input.GetKeyDown(KeyCode.E);

                _yAxis = Input.GetAxis("Mouse Y") * sensitivity;
                _xAxis = Input.GetAxis("Mouse X") * sensitivity;

                _pickup = Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1);
                break;
            case Controls.PS4_WIRED:
                _up = Input.GetAxis("PS4_DPad_Y") > 0.1f;
                _down = Input.GetAxis("PS4_DPad_Y") < -0.1f;
                _left = (Input.GetAxis("PS4_DPad_X") < -0.1f) || Input.GetAxis("PS4_LeftStick_X") < -0.1f;
                _right = (Input.GetAxis("PS4_DPad_X") > 0.1f) || Input.GetAxis("PS4_LeftStick_X") > 0.1f;

                _jump = Input.GetButton("PS4_X");
                _jumpPressed = Input.GetButtonDown("PS4_X");
                _jumpReleased = Input.GetButtonUp("PS4_X");

                _bash = Input.GetButtonDown("PS4_R1") || Input.GetButtonDown("PS4_R2");
                _block = Input.GetButton("PS4_R1") || Input.GetButton("PS4_R2");
                _blockPressed = Input.GetButtonDown("PS4_R1") || Input.GetButtonDown("PS4_R2");
                _blockReleased = Input.GetButtonUp("PS4_R1") || Input.GetButtonUp("PS4_R2");

                _aim = Input.GetButton("PS4_L1") || Input.GetButton("PS4_L2");
                _aimPressed = Input.GetButtonDown("PS4_L1") || Input.GetButtonDown("PS4_L2");
                _release = Input.GetButtonUp("PS4_L1") || Input.GetButtonUp("PS4_L2");

                _pause = Input.GetButtonDown("PS4_Options");
                _interact = Input.GetButtonDown("PS4_Square");

                _yAxis = Input.GetAxis("PS4_RightStick_Y");
                _xAxis = Input.GetAxis("PS4_RightStick_X");

                _pickup = Input.GetButtonDown("PS4_L1") || Input.GetButtonDown("PS4_R1");
                break;
            case Controls.PS4_BLUETOOTH:
                _up = Input.GetAxis("PS4_DPad_Y") > 0.1f;
                _down = Input.GetAxis("PS4_DPad_Y") < -0.1f;
                _left = (Input.GetAxis("PS4_DPad_X_BT") < -0.1f) || Input.GetAxis("PS4_LeftStick_X") < -0.1f;
                _right = (Input.GetAxis("PS4_DPad_X_BT") > 0.1f) || Input.GetAxis("PS4_LeftStick_X") > 0.1f;

                _jump = Input.GetButton("PS4_X");
                _jumpPressed = Input.GetButtonDown("PS4_X");
                _jumpReleased = Input.GetButtonUp("PS4_X");

                _bash = Input.GetButtonDown("PS4_R1") || Input.GetButtonDown("PS4_R2");
                _block = Input.GetButton("PS4_R1") || Input.GetButton("PS4_R2");
                _blockPressed = Input.GetButtonDown("PS4_R1") || Input.GetButtonDown("PS4_R2");
                _blockReleased = Input.GetButtonUp("PS4_R1") || Input.GetButtonUp("PS4_R2");

                _aim = Input.GetButton("PS4_L1") || Input.GetButton("PS4_L2");
                _aimPressed = Input.GetButtonDown("PS4_L1") || Input.GetButtonDown("PS4_L2");
                _release = Input.GetButtonUp("PS4_L1") || Input.GetButtonUp("PS4_L2");

                _pause = Input.GetButtonDown("PS4_Options");
                _interact = Input.GetButtonDown("PS4_Square");

                _yAxis = Input.GetAxis("PS4_RightStick_Y_BT");
                _xAxis = Input.GetAxis("PS4_RightStick_X_BT");

                _pickup = Input.GetButtonDown("PS4_R1") || Input.GetButtonDown("PS4_L1");
                break;
        }
    }
}