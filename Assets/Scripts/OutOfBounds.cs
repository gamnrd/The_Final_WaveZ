using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    [SerializeField] private bool respawnOnContact = true;

    //If player gets out of bounds and hits bottom of world, respawn
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && respawnOnContact)
        {
            RespawnController.Instance.RespawnPlayer();
        }
    }
}
