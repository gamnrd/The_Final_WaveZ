using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private float spawnCountDownMin = 2;
    [SerializeField] private float spawnCountDownMax = 5;
    [SerializeField] private float lastSpawnTime = 0;
    [SerializeField] private Vector3 spawnPos = Vector3.zero;
    private int spawnAttemptLimt = 3;
    private int spawnAttempts = 0;
    private ZombieStats stats = new ZombieStats();

    //Raycast
    private bool isSpawnPointInvalid = false;
    [SerializeField] private LayerMask layer;
    [SerializeField] private Ray ray;


    [SerializeField] private Vector3 center;
    [SerializeField] private Vector3 size;

    private void OnDrawGizmosSelected()
    {
        //Draw zombie spawn area
        Gizmos.color = new Color(1, 0, 0, 0.1f);
        Gizmos.DrawCube(transform.localPosition + center, size);
    }


    void Start()
    {
        InvokeRepeating("CheckForSpawn", 3f, 1f);
    }

    private void CheckForSpawn()
    {
        if ((Time.time - lastSpawnTime > Random.Range(spawnCountDownMin, spawnCountDownMax)) && WaveManager.Instance.CanSpawn())
        {
            Spawn();
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
                return;
            }

            //While the spawn point is not too close to the player and the spawn point is not inside another object
        } while (isSpawnPointInvalid);

        //Spawn Zombie
        PoolableObject instance = WaveManager.Instance.zombiePool.GetObject();
        if (instance != null)
        {
            instance.GetComponent<ZombieHealth>().stats = stats;
            instance.GetComponent<ZombieHealth>().enabled = true;

            instance.GetComponent<ZombieMovement>().stats = stats;

            //Set zombie as child to spawner
            instance.transform.SetParent(transform, false);

            //Move zombie to spawn point
            instance.transform.localPosition = new Vector3(spawnPos.x, 0, spawnPos.z);
            lastSpawnTime = Time.time;
        }
        WaveManager.Instance.EnemySpawned(1);
    }

    public void SetStats(Component sender, object data)
    {
        if (data is ZombieStats newStats)
        {
            stats = newStats;
        }
    }
}