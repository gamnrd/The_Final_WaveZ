using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieHealth : DestroyPoolableObject
{
    public int totalHealth = 3;
    private GameMode mode;

    

    //Sound
    public AudioSource src;
    public AudioClip die;
    private GameObject pool;
    private NavMeshAgent navAgent;
    private ZombieMovement movement;
    private Animator anim;

    //Death
    public bool isAlive;
    public int cashOnKill;
    private Rigidbody rb;
    private CapsuleCollider col;


    private void Awake()
    {
        pool = GameObject.Find("Zombie Pool");
        totalHealth = PlayerPrefs.GetInt("ZombieHealth", 3);
        navAgent = GetComponent<NavMeshAgent>();
        movement = GetComponent<ZombieMovement>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();

        //mode = GameManager.instance.gameMode;
    }

    public override void OnEnable()
    {
        //Set alive
        isAlive = true;
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        col.isTrigger = false;

        totalHealth = PlayerPrefs.GetInt("ZombieHealth", 3);
        //GetComponent<ZombieMovement>().enabled = true;
        movement.enabled = true;
        base.OnEnable();
    }
    public override void OnDisable()
    {
        isAlive = false;
        Invoke("MoveToPool", 0.1f);
        //GetComponent<ZombieMovement>().enabled = false;
        movement.enabled = false;
        base.OnDisable();
    }



    private void OnCollisionEnter(Collision other)
    {
        //Zombie damage
        if (other.gameObject.CompareTag("Bullet"))
        {
            //damage zombie
            DamageEnemy(1);
        }
    }


    //Zombie takes damage
    public void DamageEnemy(int damageAmount)
    {
        if (!isAlive) return;

        totalHealth -= damageAmount;
        //If health is zero, kill zombie
        if (totalHealth <= 0)
        {
            isAlive = false;
            rb.constraints = RigidbodyConstraints.FreezePosition;
            col.isTrigger = true;


            //movement.enabled = false;
            anim.SetTrigger("Dead");
            src.PlayOneShot(die, 0.7f);


            //TODO
            //PlayerStats.instance.AddCash(cashOnKill);
            GameUI.Instance.UpdateCashText(PlayerDataManager.instance.data.totalCash);
            PlayerDataManager.instance.AddResource(ResourceType.Cash, cashOnKill);


            if (GameManager.instance.gameMode == GameMode.Wave)
            {
                WaveZombieCounter.Instance.DecrementCount();
                WaveManager.Instance.ZombieKilled();
            }
            PoolableObject instance = ParticlePoolManager.instance.deathPool.GetObject();
            if (instance != null)
            {
                instance.transform.localPosition = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
                instance.transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, 90f);
            }



            Invoke("Disable", 1.5f);
            //Disable();
        }
    }

    public void MoveToPool()
    {
        transform.SetParent(pool.transform);
    }
}
