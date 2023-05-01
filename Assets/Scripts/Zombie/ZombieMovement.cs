using UnityEngine;
using UnityEngine.AI;

public class ZombieMovement : MonoBehaviour
{
    //Pursue player
    private Transform playerPos;
    [SerializeField] private CheckPlayerNear isPlayerNear;
    public float walkSpeed = 1;
    public float pursuitSpeed = 5;

    //Walk aimlesly
    private float counter = 0;
    private Vector3 direction;

    //Sound
    public AudioSource src;
    public AudioClip attack;

    private int skinNum;

    private NavMeshAgent navAgent;
    private Animator anim;

    private void Awake()
    {
        isPlayerNear = GetComponentInChildren<CheckPlayerNear>();
        anim = GetComponent<Animator>();    //Get animator
        playerPos = GameObject.FindWithTag("Player").transform; //Get players transfor
        navAgent = GetComponent<NavMeshAgent>();   //Get nav mesh

    }

    private void Start()
    {
        //pursuiSpeed = playerprefs zomspeed
        navAgent.speed = pursuitSpeed;  //Set speed when using nav mesh
    }

    public void OnEnable()
    {
        //navAgent.speed = pursuitSpeed;  //Set speed when using nav mesh
        isPlayerNear.isPlayerNear = false;
        skinNum = Random.Range(0, 26);
        transform.GetChild(skinNum).gameObject.SetActive(true); //Spawn with random zombie skin
    }

    public void OnDisable()
    {
        transform.GetChild(skinNum).gameObject.SetActive(false);
        isPlayerNear.isPlayerNear = false;
    }

    
    // Update is called once per frame
    public void Update()
    {
        Move();
    }

    public void Move()
    {
        
        //If the player is within range, move towards them
        if (isPlayerNear.isPlayerNear)
        {
            //Look at and move towards player
            navAgent.enabled = true;
            navAgent.destination = playerPos.position; // new Vector3(playerPos.position.x, playerPos.position.y, playerPos.position.z);
            transform.LookAt(navAgent.destination);
            anim.speed = 3; //Increase animation speed for run
        }
        //else if player out of range, wander around aimlessly
        else
        {
            counter -= Time.deltaTime;
            anim.speed = 1.5f; //Decrease animation speed for walk
            navAgent.enabled = false;   //Nav mesh only used when pursuing player
            //After countdown change direction
            if (counter <= 0)
            {
                ChangeDirection();
            }
            transform.position = Vector3.MoveTowards(transform.position, direction, walkSpeed * Time.deltaTime);
            transform.LookAt(direction);
        }
    }

    //After set time change direction of random wander
    public void ChangeDirection()
    {
        direction = new Vector3(transform.position.x + Random.Range(-100, 100),
                                0, 
                                transform.position.z + Random.Range(-100, 100));
        //reset countdown to random between 5-20
        counter = Random.Range(5, 20);
    }

    //When colliding with player attack
    public void OnCollisionEnter(Collision other)
    {
        //Zombie damage
        if (other.gameObject.CompareTag("Player"))
        {
            //damage zombie
            anim.speed = 1;
            anim.SetBool("Attacking_b", true);
            anim.Play("Zombie_Attacking");

            //Only if zombie is currently attacking
            if (anim.GetBool("Attacking_b") == true)
            {
                //Damage player
                other.gameObject.GetComponent<PlayerHealth>().DamagePlayer(1);
            }

            //If zombie attack animation is over, set zombie to not attacking
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Zombie_Eating"))
            {
                anim.SetBool("Attacking_b", false);
            }
            src.PlayOneShot(attack, 0.5f);
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        //If zombie spawns in a no spawn area, move back to the spawner
        if (other.gameObject.CompareTag("NoSpawn"))
        {
            transform.localPosition = Vector3.zero;
        }
    }
}
