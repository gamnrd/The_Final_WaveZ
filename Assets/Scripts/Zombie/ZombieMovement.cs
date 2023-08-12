using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using System.Collections;

[Serializable]
public class ZombieStats
{
    public float walkSpeed = 1;
    public float pursuitSpeed = 5;
    public float damage = 1.0f;
    public float attackRate = 3f;
    public float maxHealth = 3.0f;
}

public class ZombieMovement : MonoBehaviour
{
    public enum ZombieState : short { Pursuing, Wandering, Attacking };

    [SerializeField] public ZombieStats stats = new ZombieStats();

    [Header("State")]
    public ZombieState zombieState = ZombieState.Wandering;

    [Header("Movement")]
    private Transform thisTransform;
    private Transform playerPos;
    [SerializeField] private CheckPlayerNear isPlayerNear;
    private float counter = 0;
    private Vector3 direction;

    [Header("Attack")]
    private bool hasAttacked = false;
    private bool playerInRange = false;
    public float attackDistance = 2f;

    public float lastAttackTime;
    private PlayerHealth playerHealth;
    [SerializeField] private GameEvent hitPlayer;

    private int skinNum;
    private bool spawned = false;
    private float spawnCooldown = 1.5f;

    //Walk aimlesly


    [Header("Sound")]
    public AudioSource src;
    public AudioClip attack;

    private NavMeshAgent navAgent;
    private Animator anim;
    private ZombieHealth health;

    private void Awake()
    {
        isPlayerNear = GetComponentInChildren<CheckPlayerNear>();
        anim = GetComponent<Animator>();    //Get animator
        playerPos = GameObject.FindObjectOfType<PlayerHealth>().gameObject.transform; //Get players transfor
        playerHealth = playerPos.gameObject.GetComponent<PlayerHealth>();
        navAgent = GetComponent<NavMeshAgent>();   //Get nav mesh
        navAgent.enabled = false;
        health = GetComponent<ZombieHealth>();
        //navAgent.destination = pos.position;
        thisTransform = transform;
    }

    private void Start()
    {
        navAgent.updateRotation = true;
    }

    private IEnumerator CheckStateCorutine()
    {
        while (true)
        {
            CheckState();
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void OnEnable()
    {
        isPlayerNear.isPlayerNear = false;
        skinNum = UnityEngine.Random.Range(0, 26);
        thisTransform.GetChild(skinNum).gameObject.SetActive(true); //Spawn with random zombie skin

        //Double check that play object isnt null
        if (playerPos == null) playerPos = GameObject.FindObjectOfType<PlayerHealth>().gameObject.transform;
        if(playerHealth == null) playerHealth = playerPos.gameObject.GetComponent<PlayerHealth>();

        //Delay spawned state to play animation and not attack right on spawn
        spawned = false;
        spawnCooldown = 1.3f;
        navAgent.enabled = false;

        StartCoroutine(CheckStateCorutine());
    }


    public void OnDisable()
    {
        thisTransform.GetChild(skinNum).gameObject.SetActive(false);
        isPlayerNear.isPlayerNear = false;
        navAgent.enabled = false;
        spawned = false;

        StopCoroutine(CheckStateCorutine());
    }

    
    // Update is called once per frame
    public void Update()
    {
        //If the zombie is dead
        if (!health.isAlive) return;

        //if just spawned and the player is within range, wait before pursuing
        if (!spawned)
        {
            spawnCooldown -= Time.deltaTime;
            if (spawnCooldown <= 0)
            {
                health.curHealth = stats.maxHealth;
                navAgent.speed = UnityEngine.Random.Range(stats.pursuitSpeed - 0.5f, stats.pursuitSpeed + 0.5f);
                StartCoroutine(SetState(ZombieState.Wandering, 0f));
                spawned = true;
            }
        }
        else
        {
            Move();
        }       
    }


    //Check the zombies current state and act accordingly
    private void CheckState()
    {
        if (!health.isAlive) return;

        switch (zombieState)
        {
            //If the zombie is currently pursuing the player but they are out of range, change to wandering
            case ZombieState.Pursuing:
                if (!isPlayerNear.isPlayerNear)
                    StartCoroutine(SetState(ZombieState.Wandering, 1f));
                break;

            //If the zombie is currently wandering and the player gets in range, change to pursuing
            case ZombieState.Wandering:
                if (isPlayerNear.isPlayerNear)
                    StartCoroutine(SetState(ZombieState.Pursuing, 1f));
                break;

            //if the zombie has just attacked
            case ZombieState.Attacking:
                StartCoroutine(SetState(ZombieState.Pursuing, 0.5f));
                break;

            default:
                break;
        }
    }

    IEnumerator SetState(ZombieState newState, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        zombieState = newState;
    }


    //Reset zombies to a wanering state
    public void ResetState()
    {
        StartCoroutine(SetState(ZombieState.Wandering, 0.1f));
    }

    public void Move()
    {
        //If the player is within range, move towards them
        if (zombieState == ZombieState.Pursuing)
        {
            //Look at and move towards player
            if (navAgent.enabled == false)
            {
                navAgent.enabled = true;
                anim.speed = 3; //Increase animation speed for run
            }

            navAgent.destination = playerPos.position;
            return;
        }

        //else if player out of range, wander around aimlessly
        if (zombieState == ZombieState.Wandering)
        {
            counter -= Time.deltaTime;

            if (navAgent.enabled == true)
            {
                anim.speed = 1.5f; //Decrease animation speed for walk
                navAgent.ResetPath();
                navAgent.enabled = false;   //Nav mesh only used when pursuing player
            }

            //After countdown change direction
            if (counter <= 0)
            {
                ChangeDirection();
            }
            thisTransform.position = Vector3.MoveTowards(thisTransform.position, direction, stats.walkSpeed * Time.deltaTime);
            thisTransform.LookAt(direction);
        }

    }

    //After set time change direction of random wander
    public void ChangeDirection()
    {
        direction = new Vector3(thisTransform.position.x + UnityEngine.Random.Range(-50, 50),
                                0,
                                thisTransform.position.z + UnityEngine.Random.Range(-50, 50));
        //reset countdown to random between 5-10
        counter = UnityEngine.Random.Range(5, 10);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (!health.isAlive) return;

        if (zombieState == ZombieState.Wandering && other.gameObject.layer == 0)
            ChangeDirection();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!playerInRange)
                playerInRange = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //If the zombie is dead, is attacking or has recently attacked, return;
        if (!health.isAlive || hasAttacked || Time.time - lastAttackTime < stats.attackRate) return;

        //try to attack player
        if (other.gameObject.CompareTag("Player"))
            Attack();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (playerInRange)
                playerInRange = false;
        }
    }


    void Attack()
    {
        //Change to attack state
        StartCoroutine(SetState(ZombieState.Attacking, 0.0f));
        hasAttacked = true;

        //Set attack animation
        anim.SetBool("Attacking_b", true);
        anim.Play("Zombie_Attacking");

        //Start attack functions and record attack time
        Invoke("TryDamage", 0.26f);
        Invoke("DisableIsAttacking", 1f);
        lastAttackTime = Time.time;
    }

    void TryDamage()
    {
        //If the player is still within range of the attack when the animation would hit, damage player
        if (playerInRange)
        {
            hitPlayer.Raise(this, stats.damage);
            src.PlayOneShot(attack, 0.25f);
        }
    }

    //End attack
    void DisableIsAttacking()
    {
        hasAttacked = false;
        anim.SetBool("Attacking_b", false);
    }
}
