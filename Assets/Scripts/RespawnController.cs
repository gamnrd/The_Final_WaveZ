using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnController : MonoBehaviour
{
    private Transform respawnPoint;
    private Transform player;
    private Transform playerBody;
    [SerializeField] private Transform StartPoint;
    public static RespawnController Instance = null;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        //StartPoint = player.position;
        playerBody = player.GetChild(0).transform;
    }

    private void Start()
    {
        respawnPoint = StartPoint;
        //respawnPoint.position = player.position;
    }

    public void setCheckpoint(Transform newCheckPoint)
    {
        respawnPoint = newCheckPoint;
    }

    public void RespawnPlayer()
    {
        player.transform.position = respawnPoint.position;
        playerBody.transform.localPosition = Vector3.zero; //respawnPoint.position;
    }
}
