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

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        offset = transform.position - player.transform.position;
        pos = transform;
    }


    // Update is called once per frame
    //void LateUpdate()
    void Update()
    {
        if (player != null)
        {
            targetPos = player.position + offset;
            pos.position = Vector3.Lerp(pos.position, targetPos, cameraSmoothing * Time.deltaTime);
        }
    }


    /*
     * 
     * 
    // Update is called once per frame
    //void LateUpdate()
    private void FixedUpdate()
    {
        if (player != null)
        {
            targetPos = player.position + offset;
            //pos.position = Vector3.Lerp(pos.position, targetPos, cameraSmoothing * Time.deltaTime);
            pos.position = targetPos;
        }
    }*/

}