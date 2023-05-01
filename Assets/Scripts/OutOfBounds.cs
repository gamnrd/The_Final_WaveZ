using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    //If player gets out of bounds and hits bottom of world, respawn
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            RespawnController.Instance.RespawnPlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //If a zombies box colider colides with the trigger
        if ((other.gameObject.CompareTag("Zombie")) && other == other.gameObject.GetComponent<BoxCollider>())
        {
            other.transform.localPosition = Vector3.zero;
        }
    }
}
