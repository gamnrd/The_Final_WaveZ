using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameData
{
    //Money
    public int totalMoney;

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
        totalMoney = 0;
        maxHealth = 10;


    }
}
