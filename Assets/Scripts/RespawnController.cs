using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnController : MonoBehaviour
{
    private Transform respawnPoint;
    public Transform player;
    public Transform playerins;
    public Transform StartPoint;
    public static RespawnController Instance;

    private void Start()
    {
        respawnPoint = StartPoint;
    }

    private void Awake()
    {
        Instance = this;
    }

    public void setCheckpoint(Transform newCheckPoint)
    {
        respawnPoint = newCheckPoint;
    }

    public void RespawnPlayer()
    {
        player.transform.position = respawnPoint.transform.position;
        playerins.transform.position = respawnPoint.transform.position;
    }
}
