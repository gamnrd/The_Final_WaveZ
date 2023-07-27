using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance = null;
    private int totalCash = 0;
    private int maxHealth = 10;
    //private int lives = 3;
    GameData data;

    private void Awake()
    {
        if (instance == null)
            instance = this;


    }

    private void Start()
    {
        Invoke("LoadStats", 1f);
    }
    
    private void LoadStats()
    {
        //Load save data
        data = SaveSystem.Load();
        totalCash = data.totalCash;
        maxHealth = data.maxHealth;

        GameUI.Instance.UpdateCashText(totalCash);
    }

    public void AddCash(int amount)
    {
        totalCash = Mathf.Min(totalCash + amount, 999999999);
        GameUI.Instance.UpdateCashText(totalCash);
        data.totalCash = totalCash;
    }

    public int SpendCash(int amount)
    {
        //Not enough money to spend
        if (totalCash < amount)
            return -1;

        totalCash = Mathf.Max(totalCash - amount, 0);
        data.totalCash = totalCash;
        return 0;
    }

    public int GetCash()
    {
        return totalCash;
    }

    public void SaveStats()
    {
        data.totalCash = totalCash;
        data.maxHealth = maxHealth;
        SaveSystem.Save(data);
    }
}
