using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveZombieCounter : MonoBehaviour
{
    public static WaveZombieCounter Instance = null;
    [SerializeField] private int zombieCount = 0;
    [SerializeField] private int zombiesSpawned = 0;
    [SerializeField] private int zombieMax = 100;

    //Object Pool
    public ObjectPool zombiePool;
    [SerializeField] private ZombieHealth zombiePrefab;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        zombiePool = ObjectPool.CreateInstance(zombiePrefab, zombieMax, "Zombie Pool", transform);
        //TODO set max each wave, keep track of total zombies spawned, not present zombies
    }

    //Return the number of currently spawned zombies
    public int GetCount()
    {
        return zombieCount;
    }
    //Increment the count of currently spawned zombies
    public void IncrementCount()
    {
        zombieCount++;
    }
    //decrement the count of currently spawned zombies
    public void DecrementCount()
    {
        zombieCount--;
    }
    //Check that the spawned limit is not exceeded
    public bool CanSpawn()
    {
        if (zombieCount < zombieMax)
        {
            return true;
        }
        else if (zombieCount >= zombieMax)
        {
            return false;
        }
        return false;
    }

    public void EndWave()
    {
        if (zombiesSpawned == zombieMax && zombieCount <= 0)
        {
            //end wave
        }
    }
}


