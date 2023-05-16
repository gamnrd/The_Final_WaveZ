using UnityEngine;
using Unity;
using UnityEngine.AI;

public class SpawnZombie : MonoBehaviour
{
    [SerializeField] private float spawnCountDown = 3;
    [SerializeField] private float spawnTimer = 0;
    [SerializeField] private float despawnCountDown = 5;
    [SerializeField] private float deSpawntimer = 0;
    [SerializeField] private Vector3 spawnPos = Vector3.zero;
    private CheckPlayerNear IsPlayerNear;
    private int spawnAttemptLimt = 3;
    private int spawnAttempts = 0;

    //Raycast
    private bool isSpawnPointInvalid = false;
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
        //Gizmos.DrawSphere(transform.localPosition + center, 70);
    }

    private void Awake()
    {
        IsPlayerNear = GetComponent<CheckPlayerNear>();
    }

    void Start()
    {
        //Start timer
        spawnTimer = spawnCountDown;
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
        spawnAttempts = 0;

        //Create a random spawn point so long as it is too close to the player
        do
        {
            //Track how many attempts are made to find a valid spawn position
            spawnAttempts++;
        
            //Create a random spawn point within the spawn zone
            spawnPos = center + new Vector3(Random.Range(-size.x / 2, size.x / 2), 0, Random.Range(-size.z / 2, size.z / 2));

            //Use a raycast sphere with the zombies radius to check that the spawn point is clear of other objects
            ray = new Ray(transform.position + new Vector3(spawnPos.x, 10, spawnPos.z), Vector3.down);
            //Start from Ray pos, radius, output hit, distance, layers to hit
            isSpawnPointInvalid = Physics.SphereCast(ray, 0.5f, 12, layer);


            //If there have been too many unsuccesful attempts to find a valid spawn pos
            if (spawnAttempts > spawnAttemptLimt)
            {
                Debug.Log($"{gameObject.name} had {spawnAttemptLimt} failed spawn attempts");
                spawnAttempts = 0;
                spawnTimer = spawnCountDown;
                return;
            }

            //While the spawn point is not too close to the player and the spawn point is not inside another object
        } while (isSpawnPointInvalid);


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

    [ContextMenu(itemName: "Combine Meshes")]
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
