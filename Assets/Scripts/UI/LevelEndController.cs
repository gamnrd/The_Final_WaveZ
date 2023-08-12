using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelEndController : MonoBehaviour
{
    [Header("Game Over")]
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject gameOverBackground;
    [SerializeField] private TextMeshProUGUI gameOverWavesCleared;

    [Header("Level Compleated")]
    [SerializeField] private GameObject victoryCanvas;
    [SerializeField] private GameObject victoryBackground;
    [SerializeField] private TextMeshProUGUI victoryWavesCleared;

    [Header("Rewards")]
    [SerializeField] private GameObject rewardBoxPrefab;
    [SerializeField] private List<RewardBoxContents> rewards = new List<RewardBoxContents>();
    [SerializeField] private RectTransform victoryRewardParent;
    [SerializeField] private RectTransform gameOverRewardParent;
    [SerializeField] private Sprite[] rewardIcons;

    [Header("Events")]
    [SerializeField] private GameEvent onRespawnPlayer;
    [SerializeField] private GameEvent toggleGameUI;
    [SerializeField] private GameEvent loadScene;
    [SerializeField] private GameEvent unloadLevel;

    private void Awake()
    {
        if (gameOverCanvas == null) gameOverCanvas = transform.Find("GameOverScreen/GameOverCanvas").GetComponent<RectTransform>().gameObject;
        if (gameOverBackground == null) gameOverBackground = transform.Find("GameOverScreen/GameOverCanvas/GameOverBackground").GetComponent<RectTransform>().gameObject;
        if (gameOverWavesCleared == null) gameOverWavesCleared = transform.Find("GameOverScreen/GameOverCanvas/GameOverUI/Text_SubTitle").GetComponent<TextMeshProUGUI>();

        if (victoryCanvas == null) victoryCanvas = transform.Find("GameWinScreen/GameWinCanvas").GetComponent<RectTransform>().gameObject;
        if (victoryBackground == null) victoryBackground = transform.Find("GameWinScreen/GameWinCanvas/GameWinBackground").GetComponent<RectTransform>().gameObject;
        if (victoryWavesCleared == null) victoryWavesCleared = transform.Find("GameWinScreen/GameWinCanvas/GameWinUI/Text_SubTitle").GetComponent<TextMeshProUGUI>();
    }

    public void CalculateRewards(bool victory)
    {
        WaveData waveData = WaveManager.Instance.waveData;
        //int curLevel = SceneManager.GetActiveScene().buildIndex - 3;
        int curLevel = GameManager.instance.levelNum;
        Debug.Log(curLevel);

        //Cash
        RewardBoxContents cashReward = new RewardBoxContents();
        cashReward.qty = ((waveData.waveNum - 1) * 50) + waveData.enemiesKilled;
        cashReward.icon = rewardIcons[0];
        cashReward.type = ResourceType.Cash;
        rewards.Add(cashReward);
   
        
        //Wood
        if (curLevel == 2)
        {
            RewardBoxContents woodReward = new RewardBoxContents();
            woodReward.qty = ((waveData.waveNum - 1) * 10) + (waveData.enemiesKilled / 2);
            woodReward.icon = rewardIcons[1];
            woodReward.type = ResourceType.Wood;
            rewards.Add(woodReward);
        }        
        
        //Scraps
        if (curLevel == 3)
        {
            RewardBoxContents scrapsReward = new RewardBoxContents();
            scrapsReward.qty = ((waveData.waveNum - 1) * 10) + (waveData.enemiesKilled / 2);
            scrapsReward.icon = rewardIcons[2];
            scrapsReward.type = ResourceType.Scraps;
            rewards.Add(scrapsReward);
        }        
        
        //Gas
        if (curLevel == 4)
        {
            RewardBoxContents gasReward = new RewardBoxContents();
            gasReward.qty = ((waveData.waveNum - 1) * 10) + (waveData.enemiesKilled / 2);
            gasReward.icon = rewardIcons[2];
            gasReward.type = ResourceType.Gas;
            rewards.Add(gasReward);
        }        
        
        //Energy
        if (curLevel == 5)
        {
            RewardBoxContents energyReward = new RewardBoxContents();
            energyReward.qty = ((waveData.waveNum - 1) * 10) + (waveData.enemiesKilled / 2);
            energyReward.icon = rewardIcons[2];
            energyReward.type = ResourceType.Energy;
            rewards.Add(energyReward);
        }

        foreach (RewardBoxContents _reward in rewards)
        {
            if (victory)
            {
                RewardBox instance = Instantiate(rewardBoxPrefab, Vector3.zero, Quaternion.identity, victoryRewardParent).GetComponent<RewardBox>();
                instance.SetupRewardBox(_reward);
            }
            else
            {
                RewardBox instance = Instantiate(rewardBoxPrefab, Vector3.zero, Quaternion.identity, gameOverRewardParent).GetComponent<RewardBox>();
                instance.SetupRewardBox(_reward);
            }

        }
    }


    public void GameOver()
    {
        CalculateRewards(false);
        toggleGameUI.Raise(this, false);
        gameOverCanvas.SetActive(true);
        gameOverBackground.GetComponent<Animator>().enabled = true;
        gameOverWavesCleared.text = $"{WaveManager.Instance.waveData.waveNum - 1}x Waves Cleared!";
        Time.timeScale = 0.5f;
    }

    public void LevelCompleated()
    {
        CalculateRewards(true);
        toggleGameUI.Raise(this, false);
        victoryCanvas.SetActive(true);
        victoryBackground.GetComponent<Animator>().enabled = true;
        victoryWavesCleared.text = $"{WaveManager.Instance.waveData.waveNum - 1}x Waves Cleared!";
        Time.timeScale = 0f;
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        unloadLevel.Raise(this, GameManager.instance.currentSceneName);
        loadScene.Raise(this, "MainMenu");
    }

    public void ReturnToBase()
    {
        GetRewards(false);
        Time.timeScale = 1f;
        unloadLevel.Raise(this, GameManager.instance.currentSceneName);
        loadScene.Raise(this, "Idle_Base");
    }

    private void GetRewards(bool doubled)
    {
        foreach (RewardBoxContents _reward in rewards)
        {
            if (doubled)
                _reward.qty *= 2;

            PlayerDataManager.instance.AddResource(_reward.type, _reward.qty);
            PlayerDataManager.instance.SaveStats();
        }
    }

    public void AdDoubleRewards()
    {
        GetRewards(true);
        Time.timeScale = 1f;
        unloadLevel.Raise(this, GameManager.instance.currentSceneName);
        loadScene.Raise(this, "Idle_Base");
    }

    public void UseAntidote()
    {
        /*
         * check if there are antidotes
         * use antidote respawn
         * 
         * find way to disable button instead based on antidotes
         */

        Time.timeScale = 1f;
        toggleGameUI.Raise();
        gameOverCanvas.SetActive(false);
        gameOverBackground.GetComponent<Animator>().enabled = false;
        onRespawnPlayer.Raise();
    }
}
