using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJoystickController : MonoBehaviour
{
    //Movement
    public float movSpeed = 8000;
    public Transform direction;
    float hInput, vInput;
    public FixedJoystick moveJoystick;
    public FixedJoystick aimJoystick;
    private Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        //Get player rigid body and set drag
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.drag = 5;
    }

    //Fixed update is called before Update
    private void FixedUpdate()
    {
        if (PlayerHealth.Instance.alive == true)
        {
            MovePlayer();
            FaceJoystick();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (PlayerHealth.Instance.alive == true)
        {
            PlayerInput();
        }
    }

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
    private void PlayerInput()
    {
        //Joystick Controls
        hInput = moveJoystick.Horizontal * -1;
        vInput = moveJoystick.Vertical * -1;
    }

    private void MovePlayer()
    {
        Vector3 movement;
        movement = new Vector3(hInput, 0.0f, vInput).normalized;
        rb.AddForce(movement * movSpeed);
    }
}
