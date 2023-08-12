using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Serializable]
public class WaveData
{
    public int waveNum = 1;
    public int waveMax = 100;
    public int spawnMax = 10;
    public int enemiesToKill = 10;
    public int enemiesKilled = 0;
}


public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    [Header("Zombie Pool")]
    public ObjectPool zombiePool;
    [SerializeField] int zombiePoolMax = 100;
    [SerializeField] ZombieHealth zombiePrefab;

    [Header("Wave Stats")]
    [SerializeField] public WaveData waveData = new WaveData();
    [SerializeField] int enemiesSpawned;
    [SerializeField] ZombieStats zomStats = new ZombieStats();

    private float timeBetweenWaves = 5.0f;

    [SerializeField] private GameObject healthPackPrefab;

    [Header("Events")]
    [SerializeField] GameEvent updateWaveText;
    [SerializeField] GameEvent updateZombieStats;
    [SerializeField] private GameEvent levelCompleated;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        zombiePool = ObjectPool.CreateInstance(zombiePrefab, zombiePoolMax, "Zombie Pool", transform);
        
    }

    // Start is called before the first frame update
    void Start()
    {
        FirstWave();
    }

    private void FirstWave()
    {
        waveData.waveNum = 1;
        waveData.waveMax = 100;
        enemiesSpawned = 0;
        waveData.spawnMax = 10;
        waveData.enemiesToKill = waveData.spawnMax;
        updateWaveText.Raise(this, waveData);
    }

    public void EnemySpawned(int amount)
    {
        enemiesSpawned += amount;
    }

    // Update is called once per frame
    public void ZombieKilled()
    {
        waveData.enemiesToKill--;
        waveData.enemiesKilled++;
        updateWaveText.Raise(this, waveData);

        if (waveData.enemiesToKill <= 0)
        {
            Invoke("EndWave", timeBetweenWaves);
        }
    }

    public bool CanSpawn()
    {
        return enemiesSpawned < waveData.spawnMax;
    }

    

    private void EndWave()
    {
        waveData.waveNum++;
        enemiesSpawned = 0;
        waveData.spawnMax += 5;
        waveData.enemiesToKill = waveData.spawnMax;
        updateWaveText.Raise(this, waveData);
        PlayerDataManager.instance.SaveStats();
        
        //Make zombies stronger every 5 waves
        if (waveData.waveNum % 5 == 0)
        {
            Debug.Log("wave 5");
            zomStats.damage += 0.5f;
            zomStats.maxHealth += 0.5f;

            updateZombieStats.Raise(this, zomStats);
        }

        if (waveData.waveNum % 10 == 0)
        {
            zomStats.pursuitSpeed += 1.0f;
            Instantiate(healthPackPrefab, Vector3.zero, Quaternion.identity, transform);
        }


            if (waveData.waveNum > waveData.waveMax)
        {
            levelCompleated.Raise();


            //PlayerDataManager.instance.UnlockLevel(GameManager.instance.levelNum);
            
            
            
            
            //wwaves complete
            //ulock next level
            //Victory screen
        }
        /*TODO
         * Display Wave complete message
         * 
         * Display wave begin message 
         */
    }
}
