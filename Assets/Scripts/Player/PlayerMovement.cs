using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float movSpeed;
    [SerializeField] private Transform direction;
    [SerializeField] private float hInput, vInput;

    [Header("Touch Controls")]
    [SerializeField] public bool usingTouch;
    [SerializeField] private FixedJoystick moveJoystick;
    [SerializeField] private FixedJoystick aimJoystick;

    private Rigidbody rb;
    [SerializeField] public GameObject pauseMenu;
    private PlayerHealth playerHealth;


    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        rb = GetComponent<Rigidbody>();
    }



    // Start is called before the first frame update
    void Start()
    {
        if (usingTouch)
        {
            movSpeed = 8000;
        }
        else
        {
            movSpeed = 6000;
        }
    }


    //Fixed update is called before Update
    private void FixedUpdate()
    {
        if (playerHealth.GetPlayerAlive())
        {
            MovePlayer();
            if (usingTouch)
            {
                FaceJoystick();
            }
            else
            {
                FaceMouse();
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (playerHealth.GetPlayerAlive())
        {
            playerInput();
            
        }
    }


    #region Aim Mouse
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
    #endregion Aim Mouse

    #region Aim Joystick
    //Get the mouse position and have the player face it
    private void FaceJoystick()
    {
        //Joystick Aim
        float aimH = aimJoystick.Horizontal;
        float aimV = aimJoystick.Vertical;

        Vector2 xy = ConvertDirection(Camera.main.transform.position, aimH, aimV);
        Vector3 direction = new Vector3(xy.x, 0, xy.y).normalized;
        Vector3 lookDirection = transform.position + direction;
        transform.LookAt(lookDirection);
    }

    private Vector2 ConvertDirection(Vector3 camera, float h, float v)
    {
        Vector2 aimDirection = new Vector2(h, v).normalized;
        Vector2 cameraPos = new Vector2(camera.x, camera.z);
        Vector2 cameraToPlayer = (Vector2.zero - cameraPos).normalized;
        float angle = Vector2.SignedAngle(cameraToPlayer, new Vector2(0, 1));
        Vector2 finalDirection = RotateVector(aimDirection, -angle);
        return finalDirection;
    }

    public Vector2 RotateVector(Vector2 v, float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        float x = v.x * Mathf.Cos(radian) - v.y * Mathf.Sin(radian);
        float y = v.x * Mathf.Sin(radian) + v.y * Mathf.Cos(radian);
        return new Vector2(x, y);
    }
    #endregion



    //Get player controls
    private void playerInput()
    {
        //Move
        if (usingTouch)
        {
            //Joystick Controls
            hInput = moveJoystick.Horizontal * -1;
            vInput = moveJoystick.Vertical * -1;
        }
        else
        {
            //Keyboard Controls
            hInput = Input.GetAxisRaw("Horizontal") * -1;
            vInput = Input.GetAxisRaw("Vertical") * -1;
        }

        //Pause Game
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            //If not paused, pause
            if (pauseMenu.activeSelf == false)
            {
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
                UIController.Instance.SetScreens("pause");
            }
            //If paused, unpause
            else if (pauseMenu.activeSelf == true)
            {
                Time.timeScale = 1;
                pauseMenu.SetActive(false);
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
