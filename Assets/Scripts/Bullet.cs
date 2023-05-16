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
                Destroy(Instantiate(Resources.Load("FX_BloodSplat"), transform.position, transform.rotation), 0.5f);
            }
            //Else spawn sparks
            else
            {
                Destroy(Instantiate(Resources.Load("FX_Spark"), transform.position, transform.rotation), 0.5f);
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

    public void MoveToPool()
    {
        //transform.SetParent(GameObject.Find("Bullet Pool").transform);
    }
}
