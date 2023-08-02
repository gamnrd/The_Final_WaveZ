using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Shop", menuName = "Idle/Shop", order = 1)]
public class ShopObject : ScriptableObject
{
    public string shopName;
    public int shopUpgradeLevel;
    public bool isShopUnlocked;


    public List<UpgradeObject> upgrades = new List<UpgradeObject>();

}
