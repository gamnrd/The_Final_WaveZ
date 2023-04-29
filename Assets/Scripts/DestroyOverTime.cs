using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    [SerializeField] private float lifetime;

    // Update is called once per frame
    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
