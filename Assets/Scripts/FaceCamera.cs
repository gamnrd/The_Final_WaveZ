using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;
    }

    void Update()
    {
        //make the objects transform face the user at all times
        transform.LookAt(new Vector3(playerCamera.transform.position.x, transform.position.y, playerCamera.transform.position.z));
        transform.localRotation *= Quaternion.Euler(0, 180, 0);
    }
}
