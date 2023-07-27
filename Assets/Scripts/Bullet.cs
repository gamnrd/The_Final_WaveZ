using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : AutoDestroyPoolableObject
{
    //[SerializeField] private float lifetime;
    [SerializeField] private float bulletSpeed = 30;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (gameObject.activeSelf)
        {
            //If the bullet hits a zombie spawn blood
            if (other.gameObject.CompareTag("Zombie"))
            {
                PoolableObject instance = ParticlePoolManager.instance.bloodPool.GetObject();
                if (instance != null)
                {
                    instance.transform.localPosition = transform.position;
                    instance.transform.rotation = transform.rotation;
                }
            }
            //Else spawn sparks
            else
            {
                PoolableObject instance = ParticlePoolManager.instance.sparkPool.GetObject();
                if (instance != null)
                {
                    instance.transform.localPosition = transform.position;
                    instance.transform.rotation = transform.rotation;
                }
            }

            Disable();
        }
    }


    public override void OnEnable()
    {
        base.OnEnable();
        //Move bullet
        rb.velocity = (GameObject.Find("FirePoint").GetComponent<Transform>().forward * bulletSpeed);
    }

    public override void OnDisable()
    {
        //Invoke("MoveToPool", 0.1f);
        rb.velocity = Vector3.zero;
        base.OnDisable();
    }
}
