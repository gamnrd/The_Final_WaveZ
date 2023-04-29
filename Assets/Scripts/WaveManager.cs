using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    [SerializeField] public int waveNum { get; set; }
    [SerializeField] public int waveMax { get; set; }
    [SerializeField] public int enemiesKilled { get; set; }
    [SerializeField] public int enemiesSpawned { get; set; }
    [SerializeField] public int spawnMax { get; set; }

    private int msgTimer = 4;
    private float timer;
    

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        timer = msgTimer;
        ResetWave();
    }

    private void ResetWave()
    {
        waveNum = 1;
        waveMax = 10;
        enemiesKilled = 0;
        enemiesSpawned = 0;
        spawnMax = 15;
        UIController.Instance.UpdateWaveTracker(enemiesKilled, spawnMax);
    }



    // Update is called once per frame
    public void ZombieKilled()
    {
        enemiesKilled++;
        if (enemiesKilled >= spawnMax)
        {
            //EndWave();
        }
        UIController.Instance.UpdateWaveTracker(enemiesKilled, spawnMax);
    }

    private void EndWave(int waveNum)
    {
        /*TODO
         * Display Wave complete message
         * 
         * Start timer for gap between waves
         * 
         * reset counts
         * 
         * timer ends
         * 
         * Display wave begin message 
         */
    }
}
