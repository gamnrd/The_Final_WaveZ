using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class IdleUI : MonoBehaviour
{
    public static IdleUI instance;

    [SerializeField] private GameObject IdleUIContainer;
    [SerializeField] private GameObject waveStageSelectContainer;

    [Header("Resource Text")]
    [SerializeField] private TextMeshProUGUI cashText;
    [SerializeField] private TextMeshProUGUI woodText;
    [SerializeField] private TextMeshProUGUI scrapsText;
    [SerializeField] private TextMeshProUGUI gasText;
    [SerializeField] private TextMeshProUGUI energyText;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        if (IdleUIContainer == null) IdleUIContainer = transform.Find("IdleUIContainer").gameObject;
        if (cashText == null) cashText = transform.Find("IdleUIContainer/Resource_Group/Status_Cash/Text_Value").GetComponent<TextMeshProUGUI>();
        if (woodText == null) woodText = transform.Find("IdleUIContainer/Resource_Group/Status_Wood/Text_Value").GetComponent<TextMeshProUGUI>();
        if (scrapsText == null) scrapsText = transform.Find("IdleUIContainer/Resource_Group/Status_Scraps/Text_Value").GetComponent<TextMeshProUGUI>();
        if (gasText == null) gasText = transform.Find("IdleUIContainer/Resource_Group/Status_Gas/Text_Value").GetComponent<TextMeshProUGUI>();
        if (energyText == null) energyText = transform.Find("IdleUIContainer/Resource_Group/Status_Energy/Text_Value").GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateResources();
    }

    public void UpdateResources()
    {
        cashText.text = PlayerDataManager.instance.data.totalCash.ToString("n0");
        woodText.text = PlayerDataManager.instance.data.totalWood.ToString("n0");
        scrapsText.text = PlayerDataManager.instance.data.totalScraps.ToString("n0");
        gasText.text = PlayerDataManager.instance.data.totalGas.ToString("n0");
        energyText.text = PlayerDataManager.instance.data.totalEnergy.ToString("n0");
    }

    public void Exit()
    {
        Debug.Log("Exit Pressed");
    }

    public void OnExplore()
    {
        waveStageSelectContainer.SetActive(true);
        IdleManager.instance.menuActive = true;
    }
}
