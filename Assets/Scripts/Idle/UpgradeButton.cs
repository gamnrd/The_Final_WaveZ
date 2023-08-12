using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeButton : MonoBehaviour
{
    public UpgradeObject upgrade;
    private Button button;

    [Header("Icon")]
    [SerializeField] private Image buttonIcon;
    [SerializeField] private TextMeshProUGUI buttonIconLvl;
    [SerializeField] private Sprite[] resourceIcons;

    [Header("Description")]
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private int curLvl;
    [SerializeField] private TextMeshProUGUI upgradeCurLvl;
    [SerializeField] private TextMeshProUGUI upgradeNxtLvl;
    [SerializeField] private TextMeshProUGUI statChange;

    [Header("Cost")]
    [SerializeField] private Image costIcon1;
    [SerializeField] private TextMeshProUGUI cost1Text;
    [SerializeField] private GameObject cost2Object;
    [SerializeField] private Image costIcon2;
    [SerializeField] private TextMeshProUGUI cost2Text;
    [SerializeField] private int upgradeTeir;

    [Header("Events")]
    [SerializeField] private GameEvent statsChanged;
    [SerializeField] private GameEvent resourcesChanged;
    [SerializeField] private GameEvent refreshButton;

    //Price integers
    private int cost1;
    private int cost2;
    private int totalcost1 = 0;
    private int totalcost2 = 0;
    
    private void Awake()
    {
        //CreateButton();
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClicked);
    }

    public void CreateButton()
    {
        curLvl = PlayerDataManager.instance.GetUpgradeLvl(upgrade.statUpgradeType);
        buttonIcon.sprite = upgrade.upgradeIcon;
        buttonIconLvl.text = $"LV.{curLvl}";

        title.text = upgrade.upgradeName;

        upgradeCurLvl.text = $"LV.{curLvl}";
        upgradeNxtLvl.text = $"LV.{curLvl + 1}";
        
        //Cash cost
        costIcon1.sprite = resourceIcons[0];
        cost1 = (upgrade.cost[0].baseCost + (curLvl * upgrade.cost[0].priceMultiplier));
        totalcost1 += cost1;
        cost1Text.text = cost1.ToString();
        if (PlayerDataManager.instance.CheckForResource(upgrade.cost[0].resourceCostType, cost1) == false)
            button.interactable = false;
        else
            button.interactable = true;

        //Based on teir, add second resource cost
        upgradeTeir = curLvl / upgrade.upgradeLimit;
        if (upgradeTeir > 0 && upgradeTeir < 5)
        {
            cost2Object.SetActive(true);

            //Second cost
            costIcon2.sprite = resourceIcons[upgradeTeir];
            cost2 = (upgrade.cost[upgradeTeir].baseCost + ((curLvl - (upgradeTeir * upgrade.upgradeLimit)) * upgrade.cost[upgradeTeir].priceMultiplier));
            cost2Text.text = cost2.ToString();
            totalcost2 += cost2;

            if (PlayerDataManager.instance.CheckForResource(upgrade.cost[upgradeTeir].resourceCostType, cost2) == false)
                button.interactable = false;
            else
                button.interactable = true;

        }
        else if(upgradeTeir >= 5)
            UpgradeMaxed();
        

        if (upgrade.upgradeType == UpgradeType.Stat)
        {
            statChange.text = $"{upgrade.statUpgradeType.ToString()} + {upgrade.statIncreaseAmount}";
        }

        if (upgrade.upgradeType == UpgradeType.Resource)
        {

        }

        if (upgrade.upgradeType == UpgradeType.OneTime)
        {

        }
    }

    public void ButtonClicked()
    {
        //Confirm player has funds
        if (PlayerDataManager.instance.CheckForResource(upgrade.cost[0].resourceCostType, cost1) == false)
            return;

        if (upgradeTeir > 0 && upgradeTeir < 5 && PlayerDataManager.instance.CheckForResource(upgrade.cost[upgradeTeir].resourceCostType, cost2) == false)
            return;


            //Spend resource(s) for upgrade
            PlayerDataManager.instance.SpendResource(upgrade.cost[0].resourceCostType, cost1);
        if (upgradeTeir > 0 && upgradeTeir < 5)
        {
            PlayerDataManager.instance.SpendResource(upgrade.cost[upgradeTeir].resourceCostType, cost2);
        }
        resourcesChanged.Raise();
        //Change saved upgrade level
        PlayerDataManager.instance.BuyUpgrade(upgrade.statUpgradeType);
        //Apply stat change
        PlayerDataManager.instance.AddStat(upgrade.statUpgradeType, upgrade.statIncreaseAmount);
        statsChanged.Raise();
        //Save Player Data
        PlayerDataManager.instance.SaveStats();

        //Refresh button
        refreshButton.Raise();
    }

    //When an upgrade has been maxed out, diable upgrade button and change text to indicate maxed
    public void UpgradeMaxed()
    {
        button.interactable = false;
        costIcon1.enabled = false;
        cost1Text.text = "";
        cost2Object.SetActive(false);

        buttonIconLvl.text = "Max";

        title.text = upgrade.upgradeName;

        upgradeCurLvl.text = "";
        upgradeNxtLvl.text = "";

        Debug.Log($"{totalcost1}   {totalcost2}");
    }
}
