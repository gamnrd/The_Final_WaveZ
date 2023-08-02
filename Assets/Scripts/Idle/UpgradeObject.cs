using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UpgradeType { Stat, Resource, OneTime}
public enum StatUpgradeType : short { None, Health, Defence, MoveSpeed, FireRate, BulletDamage };

[CreateAssetMenu(fileName = "Upgrade", menuName = "Idle/Upgrade", order = 2)]
public class UpgradeObject : ScriptableObject
{
    public UpgradeType upgradeType;

    [Header("Icon")]
    public Sprite upgradeIcon;

    [Header("Description")]
    public string upgradeName;
    public int upgradeCurLvl;
    public int upgradeNxtLvl;

    [Header("Cost")]
    public Sprite resourceIcon;
    public ResourceType resourceCostType;
    public int baseCost;
    public float priceMultiplier;
    public int upgradeLimit;
    
    [Header("Stat Upgrade")]
    public StatUpgradeType statUpgradeType;
    public float statIncreaseAmount;
    

}