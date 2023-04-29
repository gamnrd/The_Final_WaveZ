using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnController : MonoBehaviour
{
    private Transform respawnPoint;
    private Transform player;
    private Transform playerBody;
    [SerializeField] private Transform StartPoint;
    public static RespawnController Instance;

    private void Awake()
    {
        Instance = this;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerBody = player.GetChild(0).transform;
    }

    private void Start()
    {
        respawnPoint = StartPoint;
    }

    public void setCheckpoint(Transform newCheckPoint)
    {
        respawnPoint = newCheckPoint;
    }

    public void RespawnPlayer()
    {
        player.transform.position = respawnPoint.transform.position;
        playerBody.transform.position = respawnPoint.transform.position;
    }
}
