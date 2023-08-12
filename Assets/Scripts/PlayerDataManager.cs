using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType : short { Cash, Wood, Scraps, Gas, Energy };
//public enum StatType : short { Health, Defence, Speed, BulletDamage, FireRate };

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager instance;
    public GameData data;

    [SerializeField] private bool deleteDataOnPlay = false;
    [SerializeField] private bool maxResource = false;

    [SerializeField] private GameEvent resourcesChanged;
    [SerializeField] private GameEvent statsChanged;

    //Resources
    private const int RESOURCE_MAX = GameData.RESOURCE_MAX;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        if (deleteDataOnPlay)
        {
            SaveSystem.DeleteSave();
        }
        if (maxResource)
        {
            MaxResources();
        }

        LoadStats();
    }

    public void LoadStats()
    {
        //Load save data
        data = SaveSystem.Load();
    }

    public void SaveStats()
    {
        SaveSystem.Save(data);
    }

    public void DeleteSave()
    {
        SaveSystem.DeleteSave();
        data = SaveSystem.Load();
        resourcesChanged.Raise();
        statsChanged.Raise();
    }


    ////////////////////////////////////////////////////////////////////////
    /// 
    /// Resources
    ///
    ////////////////////////////////////////////////////////////////////////

    public void MaxResources()
    {
        data.totalCash = RESOURCE_MAX;
        data.totalWood = RESOURCE_MAX;
        data.totalScraps = RESOURCE_MAX;
        data.totalGas = RESOURCE_MAX;
        data.totalEnergy = RESOURCE_MAX;
        SaveSystem.Save(data);
        resourcesChanged.Raise();
    }

    public void AddResource(ResourceType type, int amount)
    {
        switch (type)
        {
            case ResourceType.Cash:
                data.totalCash = Mathf.Min(data.totalCash + amount, RESOURCE_MAX);
                break;

            case ResourceType.Wood:
                data.totalWood = Mathf.Min(data.totalWood + amount, RESOURCE_MAX);
                break;

            case ResourceType.Scraps:
                data.totalScraps = Mathf.Min(data.totalScraps + amount, RESOURCE_MAX);
                break;

            case ResourceType.Gas:
                data.totalGas = Mathf.Min(data.totalGas + amount, RESOURCE_MAX);
                break;

            case ResourceType.Energy:
                data.totalEnergy = Mathf.Min(data.totalEnergy + amount, RESOURCE_MAX);
                break;

            default:
                break;
        }
    }


    //Gets the resource type, checks that amount is under balance, then subtracts spent amount and saves
    public bool SpendResource(ResourceType type, int amount)
    {
        switch (type)
        {
            case ResourceType.Cash:
                if (data.totalCash < amount)
                    return false;
                data.totalCash = Mathf.Max(data.totalCash - amount, 0);
                break;

            case ResourceType.Wood:
                if (data.totalWood < amount)
                    return false;
                data.totalWood = Mathf.Max(data.totalWood - amount, 0);
                break;

            case ResourceType.Scraps:
                if (data.totalScraps < amount)
                    return false;
                data.totalScraps = Mathf.Max(data.totalScraps - amount, 0);
                break;

            case ResourceType.Gas:
                if (data.totalGas < amount)
                    return false;
                data.totalGas = Mathf.Max(data.totalGas - amount, 0);
                break;

            case ResourceType.Energy:
                if (data.totalEnergy < amount)
                    return false;
                data.totalEnergy = Mathf.Max(data.totalEnergy - amount, 0);
                break;

            default:
                return false;
        }

        SaveSystem.Save(data);
        return true;
    }

    //Gets the resource type, checks that amount is under balance, then subtracts spent amount and saves
    public bool CheckForResource(ResourceType type, int amount)
    {
        switch (type)
        {
            case ResourceType.Cash:
                if (data.totalCash < amount)
                    return false;
                else
                    return true;

            case ResourceType.Wood:
                if (data.totalWood < amount)
                    return false;
                else
                    return true;

            case ResourceType.Scraps:
                if (data.totalScraps < amount)
                    return false;
                else
                    return true;

            case ResourceType.Gas:
                if (data.totalGas < amount)
                    return false;
                else
                    return true;

            case ResourceType.Energy:
                if (data.totalEnergy < amount)
                    return false;
                else
                    return true;

            default:
                return false;
        }
    }

    ////////////////////////////////////////////////////////////////////////
    /// 
    /// Stats
    ///
    ////////////////////////////////////////////////////////////////////////
    public void AddStat(StatUpgradeType type, float amount)
    {
        switch (type)
        {
            case StatUpgradeType.Health:
                data.maxHealth = Mathf.Min(data.maxHealth + amount, 100);
                break;

            case StatUpgradeType.Defence:
                data.defence = Mathf.Min(data.defence + amount, 75);
                break;

            case StatUpgradeType.PlayerSpeed:
                data.playerSpeed = Mathf.Min(data.playerSpeed + amount, 30);
                break;

            case StatUpgradeType.BulletDamage:
                data.bulletDamage = Mathf.Min(data.bulletDamage + amount, 20);
                break;

            case StatUpgradeType.FireRate:
                data.fireRate = Mathf.Max(data.fireRate - amount, 0.1f);
                break;

            default:
                break;
        }
    }

    ////////////////////////////////////////////////////////////////////////
    /// 
    /// Upgrades
    ///
    ////////////////////////////////////////////////////////////////////////
    public int GetUpgradeLvl(StatUpgradeType type)
    {
        switch (type)
        {
            case StatUpgradeType.Health:
                return data.healthLvl;

            case StatUpgradeType.Defence:
                return data.defenceLvl;

            case StatUpgradeType.PlayerSpeed:
                return data.speedLvl;

            case StatUpgradeType.BulletDamage:
                return data.bulletDamageLvl;

            case StatUpgradeType.FireRate:
                return data.fireRateLvl;

            default:
                return -1;
        }
    }

    public void BuyUpgrade(StatUpgradeType type)
    {
        switch (type)
        {
            case StatUpgradeType.Health:
                data.healthLvl++;
                break;

            case StatUpgradeType.Defence:
                data.defenceLvl++;
                break;

            case StatUpgradeType.PlayerSpeed:
                data.speedLvl++;
                break;

            case StatUpgradeType.BulletDamage:
                data.bulletDamageLvl++;
                break;

            case StatUpgradeType.FireRate:
                data.fireRateLvl++;
                break;

            default:
                break;
        }
    }


    ////////////////////////////////////////////////////////////////////////
    /// 
    /// Levels
    ///
    ////////////////////////////////////////////////////////////////////////
    
    public void UnlockLevel(int levelToUnlock)
    {
        if (levelToUnlock > 4)
            return;

        data.levelUnlocked[levelToUnlock] = true;
    }
}
