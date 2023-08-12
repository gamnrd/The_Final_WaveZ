using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UpgradeType { Stat, Resource, OneTime}
public enum StatUpgradeType : short { None, Health, Defence, PlayerSpeed, FireRate, BulletDamage };

[System.Serializable]
public class UpgradeCost
{ 
    public ResourceType resourceCostType;
    public int baseCost;
    public int priceMultiplier;
}

[CreateAssetMenu(fileName = "Upgrade", menuName = "Idle/Upgrade", order = 2)]
public class UpgradeObject : ScriptableObject
{
    public UpgradeType upgradeType;

    [Header("Icon")]
    public Sprite upgradeIcon;

    [Header("Description")]
    public string upgradeName;
    public int upgradeLimit;

    [Header("Cost")]
    public UpgradeCost[] cost;

    [Header("Stat Upgrade")]
    public StatUpgradeType statUpgradeType;
    public float statIncreaseAmount;
    

}