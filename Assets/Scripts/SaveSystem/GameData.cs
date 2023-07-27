using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameData
{
    //Money
    public int totalCash;

    //Player stats
    public int maxHealth;
    //public float defence;

    //public float fireRate;
    //public float gunDamage;
    //public float playerSpeed;


    //public bool[] levelUnlocked;
    //public bool[] charUnlocked;


    public GameData()
    {
        totalCash = 0;
        maxHealth = 10;
    }
}
