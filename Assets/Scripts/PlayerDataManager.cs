using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType : short { Cash, Wood, Scraps, Gas, Energy };

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager instance;
    public GameData data;

    [SerializeField] private bool deleteDataOnPlay = false;

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



    ////////////////////////////////////////////////////////////////////////
    /// 
    /// Resources
    ///
    ////////////////////////////////////////////////////////////////////////

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


}
