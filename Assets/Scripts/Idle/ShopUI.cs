using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUI : MonoBehaviour
{
    public static ShopUI instance;
    [SerializeField] private GameObject shopCanvas;
    [SerializeField] private UpgradeButton btnPrefab;

    [SerializeField] private TextMeshProUGUI shopTitleText;

    [Header("Resource Text")]
    [SerializeField] private TextMeshProUGUI cashText;
    [SerializeField] private TextMeshProUGUI woodText;
    [SerializeField] private TextMeshProUGUI scrapsText;
    [SerializeField] private TextMeshProUGUI gasText;
    [SerializeField] private TextMeshProUGUI energyText;

    [Header("Upgrades")]
    [SerializeField] private RectTransform upgradeParent;


    private void Awake()
    {
        if (instance == null)
            instance = this;

        shopCanvas = transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        //Get all UI Elements if they are not set
        if (shopTitleText == null) shopTitleText = transform.Find("ShopCanvas/ShopScreen/TopBar/ShopTitleText").GetComponent<TextMeshProUGUI>();

        if (cashText == null) cashText = transform.Find("ShopCanvas/ShopScreen/Resources_LeftMenu/Cash/Text_Value").GetComponent<TextMeshProUGUI>();
        if (woodText == null) woodText = transform.Find("ShopCanvas/ShopScreen/Resources_LeftMenu/Wood/Text_Value").GetComponent<TextMeshProUGUI>();
        if (scrapsText == null) scrapsText = transform.Find("ShopCanvas/ShopScreen/Resources_LeftMenu/Scraps/Text_Value").GetComponent<TextMeshProUGUI>();
        if (gasText == null) gasText = transform.Find("ShopCanvas/ShopScreen/Resources_LeftMenu/Gas/Text_Value").GetComponent<TextMeshProUGUI>();
        if (energyText == null) energyText = transform.Find("ShopCanvas/ShopScreen/Resources_LeftMenu/Energy/Text_Value").GetComponent<TextMeshProUGUI>();

        if (upgradeParent == null) upgradeParent = transform.Find("ShopCanvas/ShopScreen/UpgradesPanel/Content/UpgradeContainer").GetComponent<RectTransform>();

    }

    public void SetupShop(Shop _shop)
    {
        if (_shop == null)
            return;

        IdleManager.instance.menuActive = true;
        shopCanvas.SetActive(true);
        shopTitleText.text = _shop.shopObject.shopName;
        cashText.text = PlayerDataManager.instance.data.totalCash.ToString("n0");
        woodText.text = PlayerDataManager.instance.data.totalWood.ToString("n0");
        scrapsText.text = PlayerDataManager.instance.data.totalScraps.ToString("n0");
        gasText.text = PlayerDataManager.instance.data.totalGas.ToString("n0");
        energyText.text = PlayerDataManager.instance.data.totalEnergy.ToString("n0");

        foreach (UpgradeObject _upgrade in _shop.shopObject.upgrades)
        {
            UpgradeButton obj = Instantiate(btnPrefab, Vector3.zero, Quaternion.identity, upgradeParent.gameObject.transform);
            obj.upgrade = _upgrade;
            obj.CreateButton();
        }
    }

    public void CloseShop()
    {
        IdleManager.instance.menuActive = false;
        shopCanvas.SetActive(false);

        foreach (Transform child in upgradeParent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    /*
    public void OpenShop(ShopObject shop)
    {
        SetupShop();
    }*/

    public void Test(GameObject btn)
    {
        if (btn.TryGetComponent<UpgradeButton>(out UpgradeButton upgradeButton))
        {
            Debug.Log(upgradeButton.upgrade.upgradeName);
        }
        
    }
}
