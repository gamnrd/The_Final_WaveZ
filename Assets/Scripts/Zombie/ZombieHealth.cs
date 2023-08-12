using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieHealth : DestroyPoolableObject
{
    [SerializeField] public float curHealth;

    //Sound
    [Header("Sound")]
    public AudioClip die;

    //Death
    [Header("Death")]
    public bool isAlive;

    [Header("Components")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CapsuleCollider col;
    [SerializeField] private AudioSource src;
    [SerializeField] private GameObject pool;
    [SerializeField] private NavMeshAgent navAgent;
    [SerializeField] private ZombieMovement movement;
    [SerializeField] public ZombieStats stats = new ZombieStats();
    [SerializeField] private Animator anim;

    [Header("Events")]
    [SerializeField] private GameEvent onZombieDeath;

    PoolableObject instance;

    private void Awake()
    {
        if (pool == null) pool = GameObject.Find("Zombie Pool");
        if (navAgent == null) navAgent = GetComponent<NavMeshAgent>();
        if (movement == null) movement = GetComponent<ZombieMovement>();
        if (anim == null) anim = GetComponent<Animator>();
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (col == null) col = GetComponent<CapsuleCollider>();
        if (src == null) src = GetComponent<AudioSource>();
        curHealth = stats.maxHealth;
    }

    public override void OnEnable()
    {
        //Set alive
        isAlive = true;
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        col.isTrigger = false;
        movement.enabled = true;

        curHealth = stats.maxHealth;

        base.OnEnable();
    }
    public override void OnDisable()
    {
        isAlive = false;
        Invoke("MoveToPool", 0.1f);
        movement.enabled = false;
        base.OnDisable();
    }

    //Zombie takes damage
    public void DamageEnemy(float damageAmount)
    {
        if (!isAlive) return;

        curHealth -= damageAmount;
        anim.SetTrigger("Hit");

        //If health is zero, kill zombie
        if (curHealth <= 0)
        {
            onZombieDeath.Raise();
            isAlive = false;
            if (navAgent.enabled) navAgent.enabled = false;
            //Disable collision so body falls thru the floor
            col.isTrigger = true;

            anim.SetTrigger("Dead");
            src.PlayOneShot(die, 0.7f);

            //Spawn particle death effect
            instance = ParticlePoolManager.instance.deathPool.GetObject();
            if (instance != null)
            {
                instance.transform.localPosition = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
                instance.transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, 90f);
            }

            Invoke("Disable", 1.5f);
        }
    }

    //Return zombie to the object pool
    public void MoveToPool()
    {
        transform.SetParent(pool.transform);
    }
}
