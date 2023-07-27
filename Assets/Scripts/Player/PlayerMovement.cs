using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//Requires the Inputcontroller class
[RequireComponent(typeof(InputController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float movSpeed;
    private Rigidbody rb;
    [SerializeField] private bool isGrounded = true;
    [SerializeField] private LayerMask groundLayers;
    Vector3 movement;

    float hInput, vInput;
    public FixedJoystick moveJoystick;

    private Camera playerCamera;


    public float GroundedOffset = -0.14f;
    public float GroundedRadius = 0.28f;

    [SerializeField] private InputController input;

    //Aim mouse
    private Ray ray;
    Plane ground = new Plane(Vector3.up, Vector3.zero);
    float rayLength;
    Vector3 facing;
    Vector3 aimDirection;
    Transform pos;

    private PlayerHealth playerHealth;



    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        rb = GetComponent<Rigidbody>();
        playerCamera = Camera.main;
        input = GetComponent<InputController>();
        pos = transform;
    }

    private void Start()
    {
        GameUI.Instance.ToggleTouchControls(input.platform == Platform.Mobile);
        InvokeRepeating("OnGround", 0f, 0.25f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //If player is not alive, disable movement
        if (!playerHealth.GetPlayerAlive()) return;

        //Move player based on inputs
        switch (input.platform)
        {
            case Platform.PC:
                AimMouse();
                if (input.move.x != 0 || input.move.y != 0)
                {
                    MovePlayer(input.move.x, input.move.y);
                }
                break;



            case Platform.Mobile:
                hInput = moveJoystick.Horizontal;
                vInput = moveJoystick.Vertical;

                if (input.joystickAim.x != 0 || input.joystickAim.y != 0)
                {
                    AimJoystick();
                }

                if (hInput != 0 || vInput != 0)
                {
                    MovePlayer(hInput, vInput);
                }
                break;



            case Platform.Console:
                if (input.joystickAim.x != 0 || input.joystickAim.y != 0)
                {
                    AimJoystick();
                }

                if (input.move.x != 0 || input.move.y != 0)
                {
                    MovePlayer(input.move.x, input.move.y);
                }
                break;



            case Platform.Web:
                AimMouse();
                if (input.move.x != 0 || input.move.y != 0)
                {
                    MovePlayer(input.move.x, input.move.y);
                }
                break;

            default:
                break;
        }
    }

    private void AimJoystick()
    {
        aimDirection = new Vector3(input.joystickAim.x * (-1), 0f, input.joystickAim.y * (-1)).normalized;
        pos.rotation = Quaternion.Slerp(pos.rotation, Quaternion.LookRotation(aimDirection), 1f);
        if (aimDirection != Vector3.zero)
        {
            
        }
    }

    private void AimMouse()
    {
        ray = playerCamera.ScreenPointToRay(input.mouseAim);

        if (ground.Raycast(ray, out rayLength))
        {
            facing = ray.GetPoint(rayLength);
            pos.LookAt(new Vector3(facing.x, pos.position.y, facing.z));
        }
    }


    #region Aim Mouse
    //Get the mouse position and have the player face it
    private void FaceMouse()
    {
        ray = playerCamera.ScreenPointToRay(input.mouseAim);

        if (ground.Raycast(ray, out rayLength))
        {
            facing = ray.GetPoint(rayLength);
            pos.LookAt(new Vector3(facing.x, pos.position.y, facing.z));
        }
    }
    #endregion Aim Mouse



    //Get player controls
    public void PauseGame(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //If paused, unpause
            if (PauseScreen.Instance.GetPaused())
            {
                PauseScreen.Instance.ResumeGame();
            }
            //If unpaused, pause
            else
            {
                PauseScreen.Instance.PauseGame();
            }
        }
    }

    private void MovePlayer(float x, float y)
    {
        Vector3 currentVelocity = rb.velocity;
        movement = new Vector3(-1 * x, 0, -1 * y);
        if (movement != Vector3.zero)
        {
            movement *= movSpeed;
            Vector3 velocityChange = (movement - currentVelocity);
            velocityChange = new Vector3(velocityChange.x, GetGravity(), velocityChange.z);
            rb.velocity = velocityChange;
        }
    }

    private void MovePlayerWithAim()
    {
        //Controller or touch controls
        /*if (input.platform == Platform.Console || input.platform == Platform.Mobile)
        {
            aimDirection = new Vector3 (input.joystickAim.x * (-1) , 0f, input.joystickAim.y * (-1));

            if (aimDirection != Vector3.zero)
            {
                pos.rotation = Quaternion.Slerp(pos.rotation, Quaternion.LookRotation(aimDirection), 1f);
            }
        }*/


        //Using Mouse and keyboard
        /*if (input.platform == Platform.PC || input.platform == Platform.Web)
        {
            ray = playerCamera.ScreenPointToRay(input.mouseAim);

            if (ground.Raycast(ray, out rayLength))
            {
                facing = ray.GetPoint(rayLength);
                pos.LookAt(new Vector3(facing.x, pos.position.y, facing.z));
            }
        }*/
        /*
        Vector3 currentVelocity = rb.velocity;
        if (input.platform == Platform.Mobile)
        {
            movement = new Vector3((-1) * moveJoystick.Horizontal, 0, (-1) * moveJoystick.Vertical).normalized;
        }
        else
        {
            movement = new Vector3((-1) * input.move.x, 0, (-1) * input.move.y);
        }
        */
       /* if (movement != Vector3.zero)
        {*//*
            movement *= movSpeed;
            Vector3 velocityChange = (movement - currentVelocity);
            velocityChange = new Vector3(velocityChange.x, GetGravity(), velocityChange.z);
            rb.velocity = velocityChange;*/
       // }

    }

    private bool OnGround()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(pos.position.x, pos.position.y - GroundedOffset,
            pos.position.z);
        isGrounded = Physics.CheckSphere(spherePosition, GroundedRadius, groundLayers,
            QueryTriggerInteraction.Ignore);
        return isGrounded;
    }


    private float GetGravity()
    {
        float y = 0f;

        //If the player is on the ground don't use gravity
        if (isGrounded)
        {
            y = 0f;
        }
        //If the player is off the ground apply gravity
        else
        {
            y = -10;

            //If the y velocity is positive, set it negative
            if (y > 0f)
            {
                y *= -1f;
            }
        }
        return y;
    }
}
