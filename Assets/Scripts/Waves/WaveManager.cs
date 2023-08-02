using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance = null;

    [SerializeField] private int waveNum;
    [SerializeField] public int waveMax;
    [SerializeField] public int enemiesToKill;
    [SerializeField] public int enemiesSpawned;
    [SerializeField] public int spawnMax;

    public TextMeshProUGUI waveTrackerText;

    private int msgTimer = 4;
    private float timer;
    

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        timer = msgTimer;
        FirstWave();
    }

    private void FirstWave()
    {
        waveNum = 1;
        waveMax = 10;
        enemiesSpawned = 0;
        spawnMax = 10;
        enemiesToKill = spawnMax;
        UpdateWaveText(waveNum, enemiesToKill, spawnMax);
    }

    public void EnemySpawned(int amount)
    {
        enemiesSpawned += amount;
    }

    // Update is called once per frame
    public void ZombieKilled()
    {
        enemiesToKill--;
        UpdateWaveText(waveNum, enemiesToKill, spawnMax);

        if (enemiesToKill <= 0)
        {
            EndWave();
        }
        
    }

    public void UpdateWaveText(int wave, int kills, int zombieMax)
    {
        waveTrackerText.text = $"Wave: {wave}\nZombies: {kills} / {zombieMax}";  
    }

    public bool CanSpawn()
    {
        return enemiesSpawned < spawnMax;
    }

    private void EndWave()
    {
        waveNum++;
        enemiesSpawned = 0;
        spawnMax += 5;
        enemiesToKill = spawnMax;
        UpdateWaveText(waveNum, enemiesToKill, spawnMax);
        PlayerDataManager.instance.SaveStats();

        if (waveNum > waveMax)
        {
            //wwaves complete
        }
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
