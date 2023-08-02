using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class GameData
{
    public const int RESOURCE_MAX = 999999;

    //Resources
    public int totalCash;
    public int totalWood;
    public int totalScraps;
    public int totalGas;
    public int totalEnergy;

    //Player stats
    public int maxHealth;
    public float defence;
    public float playerSpeed;
    public float fireRate;
    public float bulletDamage;

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
