using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{ 
    public float spawnDelay = 3;
    [SerializeField] private float timer = 0;
    [SerializeField] private bool playerNear = false;
    private float spawnRange = 70;
    [SerializeField] private float distanceToPlayer;

    void Start()
    {
        //Start timer
        timer = spawnDelay;
    }

    // Update is called once per frame
    void Update()
    {
        //Check if player is within range of spawner
        CheckForPlayer();

        if (playerNear == true && WaveManager.Instance.enemiesSpawned < WaveManager.Instance.spawnMax)
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
            //Despawn();
        }
    }

    private void CheckForPlayer()
    {
        distanceToPlayer = Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
        //Check if player is nearby - only spawn when player is nearby
        if (distanceToPlayer < spawnRange)
        {
            playerNear = true;
        }
        else
        {
            playerNear = false;
        }
    }

    private void Spawn()
    { 
        //Spawn Zombie
        Instantiate(Resources.Load("Zombie"), gameObject.transform.position, gameObject.transform.rotation, this.gameObject.transform);
        WaveManager.Instance.enemiesSpawned++;
        timer = spawnDelay;
    }

    private void Despawn()
    {
        //If player is too far, destory remaining spawned zombies
        if (distanceToPlayer > 90)
        {
            foreach (Transform child in transform)
            {
                ZombieCounter.Instance.decrementCount();
                GameObject.Destroy(child.gameObject);
            }
        }
    }

    private void DestroyAllZombies()
    {
        foreach (Transform child in transform)
        {
            ZombieCounter.Instance.decrementCount();
            GameObject.Destroy(child.gameObject);
        }
    }
}
