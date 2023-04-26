using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime;
    public GameObject blood, spark;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        
        if (other.gameObject.tag == "Zombie")
        {
            Destroy(Instantiate(blood, transform.position, transform.rotation), 0.5f);
        }
        else
        {
            Destroy(Instantiate(spark, transform.position, transform.rotation), 0.5f);
        }
        Destroy(gameObject);
    }
}
