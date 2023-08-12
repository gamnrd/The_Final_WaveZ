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

    [Header("Stats Text")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI defenceText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI bulletDamageText;
    [SerializeField] private TextMeshProUGUI fireRateText;

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

        if (healthText == null) healthText = transform.Find("ShopCanvas/ShopScreen/PlayerStatsPanel/Health/Value").GetComponent<TextMeshProUGUI>();
        if (defenceText == null) defenceText = transform.Find("ShopCanvas/ShopScreen/PlayerStatsPanel/Defence/Value").GetComponent<TextMeshProUGUI>();
        if (speedText == null) speedText = transform.Find("ShopCanvas/ShopScreen/PlayerStatsPanel/Speed/Value").GetComponent<TextMeshProUGUI>();
        if (bulletDamageText == null) bulletDamageText = transform.Find("ShopCanvas/ShopScreen/PlayerStatsPanel/BulletDamage/Value").GetComponent<TextMeshProUGUI>();
        if (fireRateText == null) fireRateText = transform.Find("ShopCanvas/ShopScreen/PlayerStatsPanel/FireRate/Value").GetComponent<TextMeshProUGUI>();


        if (upgradeParent == null) upgradeParent = transform.Find("ShopCanvas/ShopScreen/UpgradesPanel/Content/UpgradeContainer").GetComponent<RectTransform>();

    }

    public void SetupShop(Component sender, object data)
    {
        if (data is Shop _shop)
        {
            IdleManager.instance.menuActive = true;
            shopCanvas.SetActive(true);
            shopTitleText.text = _shop.shopObject.shopName;
            SetupPlayerStats();

            foreach (UpgradeObject _upgrade in _shop.shopObject.upgrades)
            {
                UpgradeButton obj = Instantiate(btnPrefab, Vector3.zero, Quaternion.identity, upgradeParent.gameObject.transform);
                obj.upgrade = _upgrade;
                obj.CreateButton();
            }
        }
    }

    public void SetupPlayerStats()
    {
        healthText.text = PlayerDataManager.instance.data.maxHealth.ToString();
        defenceText.text = PlayerDataManager.instance.data.defence.ToString();
        speedText.text = PlayerDataManager.instance.data.playerSpeed.ToString();
        bulletDamageText.text = PlayerDataManager.instance.data.bulletDamage.ToString();
        fireRateText.text = PlayerDataManager.instance.data.fireRate.ToString();
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
}
