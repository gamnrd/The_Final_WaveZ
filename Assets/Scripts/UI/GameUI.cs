using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance = null;

    [Header("Game UI")]
    [SerializeField] private GameObject gameUI;

    [SerializeField] private int lives;
    [SerializeField] private TextMeshProUGUI livesTxt;

    [SerializeField] private Image healthImg;
    [SerializeField] private TextMeshProUGUI healthTxt;

    [SerializeField] private int score = 0;
    [SerializeField] private TextMeshProUGUI cashTxt;

    [SerializeField] private GameObject miniMap;

    [Header("Touch Controls")]
    [SerializeField] private GameObject moveJoystick;
    [SerializeField] private GameObject shootJoystick;
    [SerializeField] private GameObject pauseButton;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    
    private void Start()
    {   
        //Get all UI Elements if they are not set
        if (gameUI == null) gameUI = transform.Find("GameUI").GetComponent<RectTransform>().gameObject;
        if (healthImg == null) healthImg = transform.Find("/GameUI/HealthBar/Fill").GetComponent<Image>();
        if (livesTxt == null) livesTxt = transform.Find("GameUI/LivesTxt").GetComponent<TextMeshProUGUI>();
        if (healthTxt == null) healthTxt = transform.Find("GameUI/HealthBar/HealthTxt").GetComponent<TextMeshProUGUI>();
        if (cashTxt == null) cashTxt = transform.Find("GameUI/CashTxt").GetComponent<TextMeshProUGUI>();
        if (miniMap == null) miniMap = transform.Find("GameUI/MiniMap").GetComponent<RectTransform>().gameObject;
        if (moveJoystick == null) moveJoystick = transform.Find("GameUI/MoveJoystick").GetComponent<RectTransform>().gameObject;
        if (shootJoystick == null) shootJoystick = transform.Find("GameUI/ShootJoystick").GetComponent<RectTransform>().gameObject;
        if (pauseButton == null) pauseButton = transform.Find("GameUI/PauseButon").GetComponent<RectTransform>().gameObject;
    }

    public void ToggleTouchControls(bool toggle)
    {
        if (toggle)
        {
            moveJoystick.SetActive(true);
            shootJoystick.SetActive(true);
            pauseButton.SetActive(true);
            miniMap.SetActive(false);
        }
        else
        {
            moveJoystick.SetActive(false);
            shootJoystick.SetActive(false);
            pauseButton.SetActive(false);
            miniMap.SetActive(true);
        }
    }

    #region GameUI
    public void SetGameUI(bool toggle)
    {
        gameUI.SetActive(toggle);
    }


    public void UpdateHealthBars(int curHealth, int maxHealth)
    {
        healthImg.fillAmount = (float)curHealth / (float)maxHealth;
        healthTxt.text = "Health: " + curHealth + " / " + maxHealth + "";
    }

    public void UpdateLives(int curLives)
    {
        lives = curLives;
        livesTxt.text = lives + "x";
    }

    public void UpdateCashText(int cash)
    {
        cashTxt.text = "Cash: $" + cash;
    }

    public void AdjustScore(int adjustment)
    {
        score += adjustment;
        if (score <= 0)
        {
            score = 0;
        }
        cashTxt.text = "Score: " + score;
    }

    public int GetScore()
    {
        return score;
    }

    public void ToggleMiniMap(bool toggle)
    {
        miniMap.SetActive(toggle);
    }

    #endregion GameUI
}
