using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform player = null;
    [SerializeField]private float cameraSmoothing = 5f;
    private Vector3 offset;
    private Vector3 targetPos;
    private Transform pos;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        offset = transform.position - player.transform.position;
        pos = transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (player != null)
        {
            targetPos = player.position + offset;
            pos.position = Vector3.Lerp(pos.position, targetPos, cameraSmoothing * Time.deltaTime);
        }
    }
}