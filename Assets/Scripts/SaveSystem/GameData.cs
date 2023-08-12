using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameData
{
    public const int RESOURCE_MAX = 999999;

    [Header("Resources")]
    public int totalCash;
    public int totalWood;
    public int totalScraps;
    public int totalGas;
    public int totalEnergy;

    [Header("Player Stats")]
    public float maxHealth;
    public float defence;
    public float playerSpeed;
    public float fireRate;
    public float bulletDamage;

    [Header("Upgrade levels")]
    public int healthLvl;
    public int defenceLvl;
    public int speedLvl;
    public int bulletDamageLvl;
    public int fireRateLvl;


    //Level Unlocks
    public bool[] levelUnlocked;
    //public bool[] charUnlocked;


    public GameData()
    {
        totalCash = 0;
        totalWood = 0;
        totalScraps = 0;
        totalGas = 0;
        totalEnergy = 0;

        maxHealth = 10;
        defence = 0;
        playerSpeed = 20;
        bulletDamage = 1;
        fireRate = 0.5f;

        healthLvl = 0;
        defenceLvl = 0;
        speedLvl = 0;
        bulletDamageLvl = 0;
        fireRateLvl = 0;

    levelUnlocked = new bool[] { true, false, false, false, false };
    }

    //Add resources from one game data to another
    public static GameData operator +(GameData a, GameData b)
    {
        a.totalCash = Mathf.Min(a.totalCash + b.totalCash, RESOURCE_MAX);
        a.totalWood = Mathf.Min(a.totalWood + b.totalWood, RESOURCE_MAX);
        a.totalScraps = Mathf.Min(a.totalScraps + b.totalScraps, RESOURCE_MAX);
        a.totalGas = Mathf.Min(a.totalGas + b.totalGas, RESOURCE_MAX);
        a.totalEnergy = Mathf.Min(a.totalEnergy + b.totalEnergy, RESOURCE_MAX);


        return a;
    }
}
