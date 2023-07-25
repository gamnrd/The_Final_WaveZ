using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerNear : MonoBehaviour
{
    private SphereCollider sphereCollider;
    [SerializeField] private float radius;
    public bool isPlayerNear = false;

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = radius;
    }

    private void OnDrawGizmosSelected()
    {
        //Draw range that player needs to be within to spawn zombies
        Gizmos.color = new Color(0, 1, 0, 0.1f);
        Gizmos.DrawSphere(transform.position, radius);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }
}
