using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private Transform respawnLocation;

    private void Awake()
    {
        if(respawnLocation == null) respawnLocation = transform.GetChild(0).transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            RespawnController.Instance.setCheckpoint(respawnLocation);
        }
    }
}
