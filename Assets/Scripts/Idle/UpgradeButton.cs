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

    [Header("Description")]
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI upgradeCurLvl;
    [SerializeField] private TextMeshProUGUI upgradeNxtLvl;
    [SerializeField] private TextMeshProUGUI statChange;
    [SerializeField] private Image costIcon;
    [SerializeField] private TextMeshProUGUI upgradeCost;

    private void Awake()
    {
        //CreateButton();
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClicked);
    }

    public void CreateButton()
    {
        buttonIcon.sprite = upgrade.upgradeIcon;
        buttonIconLvl.text = $"LV.{upgrade.upgradeCurLvl}";

        title.text = upgrade.upgradeName;
        upgradeCurLvl.text = $"LV.{upgrade.upgradeCurLvl}";
        upgradeNxtLvl.text = $"LV.{upgrade.upgradeCurLvl + 1}";
        
        costIcon.sprite = upgrade.resourceIcon;
        upgradeCost.text = upgrade.baseCost.ToString("n0");

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
        ShopUI.instance.Test(gameObject);
    }
}
