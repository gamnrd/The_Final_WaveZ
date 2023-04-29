using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    [SerializeField] private int healAmount = 3;

    private void OnTriggerEnter(Collider other)
    {
        //If collided with a health pack
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>().HealPlayer(healAmount);
            Destroy(gameObject);
        }
    }
}
