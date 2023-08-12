using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveLevelButton : MonoBehaviour
{
    public WaveLevelObject levelData;

    
    [SerializeField] private Image levelImg;
    [SerializeField] private TextMeshProUGUI badgeLvlNum;
    [SerializeField] private Image badgeLvlLock;
    [SerializeField] private TextMeshProUGUI LvlNum;
    [SerializeField] private TextMeshProUGUI LvlName;
    [SerializeField] private RectTransform rewardsContainer;
    [SerializeField] private GameEvent loadLevel;
    [SerializeField] private GameEvent unloadScene;
    private Button button;

    private void Awake()
    {
        if (levelImg == null) levelImg = transform.Find("LevelImage").GetComponent<Image>();
        if (badgeLvlNum == null) badgeLvlNum = transform.Find("Badge/LvlNumTxt").GetComponent<TextMeshProUGUI>();
        if (badgeLvlLock == null) badgeLvlLock = transform.Find("Badge/LockedIcon").GetComponent<Image>();
        if (LvlNum == null) LvlNum = transform.Find("Text 1").GetComponent<TextMeshProUGUI>();
        if (LvlName == null) LvlName = transform.Find("Text 2").GetComponent<TextMeshProUGUI>();
        if (rewardsContainer == null) rewardsContainer = transform.Find("Rewards").GetComponent<RectTransform>();
        if (button == null) button = GetComponent<Button>();
    }

    private void Start()
    {
        levelImg.sprite = levelData.image;
        badgeLvlNum.text = levelData.levelNum.ToString();
        LvlNum.text = levelData.levelSubText;
        LvlName.text = levelData.levelMainText;

        foreach (GameObject reward in levelData.rewards)
        {
            Instantiate(reward, Vector3.zero, Quaternion.identity, rewardsContainer);
        }

        CheckLevelUnlocked();
    }

    public void StartLevel()
    {
        GameManager.instance.levelNum = levelData.levelNum;
        unloadScene.Raise(this, "Idle_Base");
        loadLevel.Raise(this, levelData.levelSceneName);
    }

    public void CheckLevelUnlocked()
    {
        if (PlayerDataManager.instance.data.levelUnlocked[levelData.levelNum - 1])
        {
            button.interactable = true;
            badgeLvlLock.enabled = false;
            badgeLvlNum.enabled = true;
        }
        else
        {
            button.interactable = false;
            badgeLvlLock.enabled = true;
            badgeLvlNum.enabled = false;
        }
    }
}
