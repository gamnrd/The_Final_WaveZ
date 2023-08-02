using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class ZombieMovement : MonoBehaviour
{
    public enum ZombieState : short { Pursuing, Wandering, Attacking };

    [Header("State")]
    public ZombieState zombieState = ZombieState.Wandering;

    [Header("Movement")]
    public float walkSpeed = 1;
    public float pursuitSpeed = 5;
    private Transform thisTransform;
    private Transform playerPos;
    [SerializeField] private CheckPlayerNear isPlayerNear;
    private float counter = 0;
    private Vector3 direction;

    [Header("Attack")]
    public int damage = 1;
    private bool hasAttacked = false;
    public float attackDistance = 2f;
    public float attackRate = 3f;
    public float lastAttackTime;
    private PlayerHealth playerHealth;

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
        health = GetComponent<ZombieHealth>();
        //navAgent.destination = pos.position;
        thisTransform = transform;
    }

    private void Start()
    {
        InvokeRepeating("CheckState", 0.5f, 0.5f);
        navAgent.speed = pursuitSpeed;  //Set speed when using nav mesh
    }

    public void OnEnable()
    {
        isPlayerNear.isPlayerNear = false;
        skinNum = Random.Range(0, 26);
        thisTransform.GetChild(skinNum).gameObject.SetActive(true); //Spawn with random zombie skin

        //Double check that play object isnt null
        if(playerPos == null) playerPos = GameObject.FindObjectOfType<PlayerHealth>().gameObject.transform; //GameObject.FindWithTag("Player").transform; //Get players transfor
        if(playerHealth == null) playerHealth = playerPos.gameObject.GetComponent<PlayerHealth>();

        //Delay spawned state to play animation and not attack right on spawn
        spawned = false;
        spawnCooldown = 1.3f;
        navAgent.enabled = false;

        //invoke spawned, invoke repeating move
    }


    public void OnDisable()
    {
        thisTransform.GetChild(skinNum).gameObject.SetActive(false);
        isPlayerNear.isPlayerNear = false;
    }

    
    // Update is called once per frame
    public void Update()
    {
        //If the zombie is dead
        if (!health.isAlive)
        {   
            //disable the nav agent to stop the body moving
            if (navAgent.enabled) navAgent.enabled = false;

            return;
        }

        //if just spawned and the player is within range, wait before pursuing
        if (!spawned)
        {
            spawnCooldown -= Time.deltaTime;
            if (spawnCooldown <= 0)
            {
                spawned = true;
            }
        }

        //CheckState();
        Move();
    }

    private void CheckState()
    {
        if (!health.isAlive) return;

        switch (zombieState)
        {
            case ZombieState.Pursuing:
                if (!isPlayerNear.isPlayerNear)
                    StartCoroutine(SetState(ZombieState.Wandering, 1f));
                break;

            case ZombieState.Wandering:
                if (isPlayerNear.isPlayerNear)
                    StartCoroutine(SetState(ZombieState.Pursuing, 1f));
                break;

            case ZombieState.Attacking:
                if (hasAttacked == false)
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

    public void Move()
    {
        if (!spawned) return;

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
            thisTransform.LookAt(navAgent.destination);
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
            thisTransform.position = Vector3.MoveTowards(thisTransform.position, direction, walkSpeed * Time.deltaTime);
            thisTransform.LookAt(direction);
        }
    }

    //After set time change direction of random wander
    public void ChangeDirection()
    {
        direction = new Vector3(thisTransform.position.x + Random.Range(-100, 100),
                                0,
                                thisTransform.position.z + Random.Range(-100, 100));
        //reset countdown to random between 5-20
        counter = Random.Range(5, 20);
    }

    //When colliding with player attack
    public void OnCollisionEnter(Collision other)
    {
        //Zombie damage
        if (other.gameObject.CompareTag("Player") && !hasAttacked && Time.time - lastAttackTime > attackRate)
        {
            Attack();
        }
    }

    private void OnCollisionStay(Collision other)
    {
        //Zombie damage
        if (other.gameObject.CompareTag("Player") && !hasAttacked && Time.time - lastAttackTime > attackRate)
        {
            Attack();
        }
    }

    void Attack()
    {
        if (!health.isAlive) return;

        zombieState = ZombieState.Attacking;
        hasAttacked = true;
        anim.SetBool("Attacking_b", true);
        anim.Play("Zombie_Attacking");
        Invoke("TryDamage", 0.26f);
        Invoke("DisableIsAttacking", 1f);
        lastAttackTime = Time.time;
    }

    void TryDamage()
    {
        if (Vector3.Distance(transform.position, playerPos.position) <= attackDistance)
        {
            playerHealth.DamagePlayer(damage);
            src.PlayOneShot(attack, 0.25f);
        }
    }

    void DisableIsAttacking()
    {
        hasAttacked = false;
        anim.SetBool("Attacking_b", false);
    }
}
