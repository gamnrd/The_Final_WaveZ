using UnityEngine;
using Unity;
using UnityEngine.AI;

public class SpawnZombie : MonoBehaviour
{
    [SerializeField] private float spawnCountDown = 3;
    [SerializeField] private float spawnTimer = 0;
    [SerializeField] private float despawnCountDown = 5;
    [SerializeField] private float deSpawntimer = 0;
    [SerializeField] private float distanceToSpawnAwayFromPlayer = 1;
    private Transform player;
    [SerializeField] private Vector3 spawnPos = Vector3.zero;
    private CheckPlayerNear IsPlayerNear;

    //Raycast
    private RaycastHit hit;
    private bool isSpawnPointInvalid = false;
    private Vector3 collision;
    [SerializeField]private LayerMask layer;
    [SerializeField] private Ray ray;


    [SerializeField] private Vector3 center;
    [SerializeField] private Vector3 size;

    private void OnDrawGizmosSelected()
    {
        //Draw zombie spawn area
        Gizmos.color = new Color(1, 0, 0, 0.1f);
        Gizmos.DrawCube(transform.localPosition + center, size);

        //Draw range that player needs to be within to spawn zombies
        Gizmos.color = new Color(0, 1, 0, 0.1f);
        //Gizmos.DrawSphere(transform.localPosition + center, spawnRange);
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        IsPlayerNear = GetComponent<CheckPlayerNear>();
    }

    void Start()
    {
        //Start timer
        spawnTimer = spawnCountDown;
        collision = transform.position;
        //layer = ~LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        //Check if player is within range of spawner
        if (IsPlayerNear.isPlayerNear)
        {
            //Ever couple seconds check if zombie can be spawned
            spawnTimer -= Time.deltaTime;

            //While player is within range reset despawn timer
            deSpawntimer = despawnCountDown;

            //After countdown spawn zombie
            if (spawnTimer <= 0)
            {
                Spawn();
            }
        }
        //Else if player is not within range and the spawner has currently spawned zombies
        else if (!IsPlayerNear.isPlayerNear && transform.childCount > 0)
        {
            //Countdown the time that player is out of range
            deSpawntimer -= Time.deltaTime;

            //After player has been far from spawner for set time, despawn zombies
            if (deSpawntimer <= 0)
            {
                Despawn();
            }
        }
    }

    private void Spawn()
    {
        //Create a random spawn point so long as it is too close to the player
        do
        {
            //Create a random spawn point within the spawn zone
            spawnPos = center + new Vector3(Random.Range(-size.x / 2, size.x / 2), 0, Random.Range(-size.z / 2, size.z / 2));

            //Use a raycast sphere with the zombies radius to check that the spawn point is clear of other objects
            ray = new Ray(transform.position + new Vector3(spawnPos.x, 10, spawnPos.z), Vector3.down);
            isSpawnPointInvalid = Physics.SphereCast(ray, 1f, out hit, 12, layer);

            //While the spawn point is not too close to the player and the spawn point is not inside another object
        } while (((Vector3.Distance(spawnPos, player.position)) < distanceToSpawnAwayFromPlayer) || isSpawnPointInvalid);
        
        //Check that zombie limit is not exceeded
        if (ZombieCounter.Instance.canSpawn())
        {
            //Spawn Zombie
            PoolableObject instance = ZombieCounter.Instance.zombiePool.GetObject();
            if (instance != null)
            {
                //Set zombie as child to spawner
                instance.transform.SetParent(transform, false);

                //Move zombie to spawn point
                instance.transform.localPosition = spawnPos;
            }
            ZombieCounter.Instance.incrementCount();
        }
        spawnTimer = spawnCountDown;
    }

    private void Despawn()
    {
        foreach (Transform child in transform)
        {
            ZombieCounter.Instance.decrementCount();
            child.GetComponent<ZombieMovement>().enabled = false;
            child.GetComponent<ZombieHealth>().Disable();
        }
    }

    private void DestroyAll()
    {
        foreach (Transform child in transform)
        {
            ZombieCounter.Instance.decrementCount();
            child.GetComponent<ZombieMovement>().enabled = false;//.Disable();
            child.GetComponent<ZombieHealth>().Disable();
        }
    }
}
