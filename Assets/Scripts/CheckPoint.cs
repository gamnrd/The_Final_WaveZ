using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public Transform SpawnLocation;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Entered");
            RespawnController.Instance.setCheckpoint(SpawnLocation);
        }
    }
}
