using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Serializable]
public class RewardBoxContents
{
    public Sprite icon = null;
    public int qty = 0;
    public ResourceType type;
}

public class RewardBox : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemQty;

    private void Awake()
    {
        if (itemIcon == null) itemIcon = transform.Find("ItemIcon").GetComponent<Image>();
        if (itemQty == null) itemQty = transform.Find("QtyTxt").GetComponent<TextMeshProUGUI>();
    }

    public void SetupRewardBox(RewardBoxContents contents)
    {
        itemIcon.sprite = contents.icon;
        itemQty.text = contents.qty.ToString();
    }
}
