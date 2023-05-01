using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyboardController : MonoBehaviour
{
    //Movement
    public float movSpeed = 6000;
    public Transform direction;
    float hInput, vInput;
    private Rigidbody rb;
    public GameObject pauseMenu;
    private PlayerHealth playerHealth;



    // Start is called before the first frame update
    void Start()
    {
        //Get player rigid body and set drag
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        playerHealth = GetComponent<PlayerHealth>();
        //rb.drag = 5;
    }


    //Fixed update is called before Update
    private void FixedUpdate()
    {
        if (playerHealth.GetPlayerAlive())
        {
            MovePlayer();
            FaceMouse();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (playerHealth.GetPlayerAlive())
        {
            PlayerInput();
        }
    }


    #region Aim Joystick
    //Get the mouse position and have the player face it
    private void FaceMouse()
    {
        Ray camera = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (ground.Raycast(camera, out rayLength))
        {
            Vector3 facing = camera.GetPoint(rayLength);
            transform.LookAt(new Vector3(facing.x, transform.position.y, facing.z));
        }
    }
    #endregion 

    //Get player controls
    private void PlayerInput()
    {
        //Move
        //Keyboard Controls
        hInput = Input.GetAxisRaw("Horizontal") * -1;
        vInput = Input.GetAxisRaw("Vertical") * -1;

        //Pause Game
        if (Input.GetKeyUp(KeyCode.Escape))
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

    private void MovePlayer()
    {
        Vector3 movement;
        movement = new Vector3(hInput, 0.0f, vInput).normalized;
        rb.AddForce(movement * movSpeed);
    }
}
