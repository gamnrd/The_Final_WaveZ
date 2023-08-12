using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance = null;

    [Header("Game UI")]
    [SerializeField] private GameObject gameUI;

    [Header("Health Bar")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private Image healthBarFill;

    [SerializeField] private GameObject miniMap;

    [Header("Touch Controls")]
    [SerializeField] private GameObject moveJoystick;
    [SerializeField] private GameObject shootJoystick;
    [SerializeField] private GameObject pauseButton;

    [Header("Wave UI")]
    [SerializeField] private GameObject waveCountdown;
    [SerializeField] private TextMeshProUGUI waveCountdownTxt;
    [SerializeField] private float waveCountdownTimer;
    [SerializeField] private TextMeshProUGUI waveTracker;
    [SerializeField] private TextMeshProUGUI killCountTxt;


    [SerializeField] private GameEvent onGamePause;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    
    private void Start()
    {   
        //Get all UI Elements if they are not set
        if (gameUI == null) gameUI = transform.Find("GameUIContainer").GetComponent<RectTransform>().gameObject;
        if (healthBar == null) healthBar = transform.Find("GameUIContainer/NonStaticCanvas/HPSlider").GetComponent<Slider>();
        if (miniMap == null) miniMap = transform.Find("GameUIContainer/MiniMap").GetComponent<RectTransform>().gameObject;
        if (moveJoystick == null) moveJoystick = transform.Find("GameUIContainer/MoveJoystick").GetComponent<RectTransform>().gameObject;
        if (shootJoystick == null) shootJoystick = transform.Find("GameUIContainer/ShootJoystick").GetComponent<RectTransform>().gameObject;
        if (pauseButton == null) pauseButton = transform.Find("GameUIContainer/PauseButon").GetComponent<RectTransform>().gameObject;

        if (waveCountdown == null) waveCountdown = transform.Find("GameUIContainer/NonStaticCanvas/WaveCountdown").GetComponent<RectTransform>().gameObject;
        if (waveTracker == null) waveTracker = transform.Find("GameUIContainer/NonStaticCanvas/WaveTracker/WaveText").GetComponent<TextMeshProUGUI>();
        if (waveCountdownTxt == null) waveCountdownTxt = transform.Find("GameUIContainer/NonStaticCanvas/WaveCountdown/TimerText").GetComponent<TextMeshProUGUI>();
        if (killCountTxt == null) killCountTxt = transform.Find("GameUIContainer/NonStaticCanvas/KillCounter").GetComponent<TextMeshProUGUI>();
    }

    public void ToggleTouchControls(Component sender, object data)
    {
        if (data is bool toggle)
        {
            moveJoystick.SetActive(toggle);
            shootJoystick.SetActive(toggle);
            pauseButton.SetActive(toggle);
            miniMap.SetActive(false);
        }
    }

    #region GameUI
    public void ToggleGameUI(Component sender, object data)
    {
        if (data is bool toggle)
        {
            gameUI.SetActive(toggle);
        }
    }

    
    public void UpdateHealthBars(Component sender, object data)
    {
        if (data is PlayerHealthStats stats)
        {
            healthBar.maxValue = stats._maxHealth;
            healthBar.value = stats._curHealth;
            //Change the health bar closer to red as health goes down
            healthBarFill.color = new Color(healthBarFill.color.r, stats._curHealth / stats._maxHealth, healthBarFill.color.b);
        }
        
    }

    public void UpdateWaveUI(Component sender, object data)
    {
        if (data is WaveData waveData)
        {
            waveTracker.text = $"Wave {waveData.waveNum}\n<size=32>Enemies {waveData.enemiesToKill} / {waveData.spawnMax}";
            killCountTxt.text = $"{waveData.enemiesKilled}";
        }
    }

    public void UpdateCashText(int cash)
    {
        //cashTxt.text = "Cash: $" + cash;
    }

    public void AdjustScore(int adjustment)
    {/*
        score += adjustment;
        if (score <= 0)
        {
            score = 0;
        }
        cashTxt.text = "Score: " + score;*/
    }

    public int GetScore()
    {
        return 0;
    }

    public void ToggleMiniMap(bool toggle)
    {
        miniMap.SetActive(false);
    }

    public void OnPausePressed()
    {
        onGamePause.Raise();
    }

    #endregion GameUI
}

