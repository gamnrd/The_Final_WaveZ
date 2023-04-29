using UnityEngine;
using Unity;
using UnityEngine.AI;

public class SpawnZombie : MonoBehaviour
{
    [SerializeField] private float spawnCountDown = 3;
    [SerializeField] private float timer = 0;
    [SerializeField] private float spawnRange = 70;
    [SerializeField] private float despawnRange = 90;
    [SerializeField] private float distanceToPlayer;
    [SerializeField] private float distanceToSpawnAwayFromPlayer = 1;
    private Transform player;
    [SerializeField] private Vector3 spawnPos;



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
    }

    void Start()
    {
        //Start timer
        timer = spawnCountDown;
    }

    // Update is called once per frame
    void Update()
    {
        //Check if player is within range of spawner
        if (CheckForPlayer())
        {
            //Ever couple seconds check if need to spawn zombie
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                Spawn();
            }
        }
        else
        {
            Despawn();
        }
    }


    //Check if player is within range of the spawner
    private bool CheckForPlayer()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.position);
        //Check if player is nearby - only spawn when player is nearby
        if (distanceToPlayer < spawnRange)
        {
            return true;
        }
        return false;
    }

   /* private bool GetSpawnPosition(Collider collider, Vector3 point)
    {
        return collider. (point);
    }*/

    private void Spawn()
    {
        //Create a random spawn point so long as it is too close to the player
        do
        {
            spawnPos = center + new Vector3(Random.Range(-size.x / 2, size.x / 2), 0, Random.Range(-size.z / 2, size.z / 2));
        } while ((Vector3.Distance(spawnPos, player.position)) < distanceToSpawnAwayFromPlayer);
        



        //Check that zombie limit is not exceeded
        if (ZombieCounter.Instance.canSpawn())
        {
            //Spawn Zombie
            PoolableObject instance = ZombieCounter.Instance.zombiePool.GetObject();
            
            if (instance != null)
            {
                instance.transform.SetParent(transform, false);
                instance.transform.localPosition = spawnPos;
            }
            ZombieCounter.Instance.incrementCount();
        }
        timer = spawnCountDown;
    }

    private void Despawn()
    {
        //If player is too far, destory remaining spawned zombies
        if (distanceToPlayer > despawnRange)
        {
            foreach (Transform child in transform)
            {
                ZombieCounter.Instance.decrementCount();
                //Destroy(child.gameObject);
                child.GetComponent<ZombieMovement>().enabled = false;//.Disable();
                child.GetComponent<ZombieHealth>().Disable();
            }
        }
    }

    private void DestroyAllZombies()
    {
        foreach (Transform child in transform)
        {
            ZombieCounter.Instance.decrementCount();
            //Destroy(child.gameObject);
            child.GetComponent<ZombieMovement>().enabled = false;//.Disable();
            child.GetComponent<ZombieHealth>().Disable();
        }
    }
}
