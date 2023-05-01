using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;

    [Header("Game UI")]
    [SerializeField] private GameObject gameUI;

    [SerializeField] private int lives;
    [SerializeField] private TextMeshProUGUI livesTxt;

    [SerializeField] private Image healthImg;
    [SerializeField] private TextMeshProUGUI healthTxt;

    [SerializeField] private int score = 0;
    [SerializeField] private TextMeshProUGUI scoreTxt;

    [SerializeField] private GameObject miniMap;

    [Header("Touch Controls")]
    [SerializeField] private bool usingTouch = false;
    [SerializeField] private GameObject moveJoystick;
    [SerializeField] private GameObject shootJoystick;


    private void Awake()
    {
        Instance = this;
        if(gameUI == null) gameUI = transform.Find("GameUI").GetComponent<RectTransform>().gameObject;
        if (healthImg == null) healthImg = transform.Find("GameUI/HealthBar/Fill").GetComponent<Image>();
        if (livesTxt == null) livesTxt = transform.Find("GameUI/LivesTxt").GetComponent<TextMeshProUGUI>();
        if (healthTxt == null) healthTxt = transform.Find("GameUI/HealthBar/HealthTxt").GetComponent<TextMeshProUGUI>();
        if (scoreTxt == null) scoreTxt = transform.Find("GameUI/ScoreTxt").GetComponent<TextMeshProUGUI>();
        if (miniMap == null) miniMap = transform.Find("GameUI/MiniMap").GetComponent<RectTransform>().gameObject;
        if (moveJoystick == null) moveJoystick = transform.Find("GameUI/MoveJoystick").GetComponent<RectTransform>().gameObject;
        if (shootJoystick == null) shootJoystick = transform.Find("GameUI/ShootJoystick").GetComponent<RectTransform>().gameObject;
    }

    private void Start()
    {
        if (usingTouch)
        {
            moveJoystick.SetActive(true);
            shootJoystick.SetActive(true);
        }
        else
        {
            moveJoystick.SetActive(false);
            shootJoystick.SetActive(false);
        }
    }

    #region GameUI
    public void SetGameUI(bool toggle)
    {
        gameUI.SetActive(toggle);
    }


    public void UpdateHealthBars(int curHealth, int maxHealth)
    {
        healthImg.fillAmount = (float)((float)curHealth / (float)maxHealth);
        healthTxt.text = "Health: " + curHealth + " / " + maxHealth + "";
    }

    public void UpdateLives(int curLives)
    {
        lives = curLives;
        livesTxt.text = lives + "x";
    }

    public void AdjustScore(int adjustment)
    {
        score += adjustment;
        if (score <= 0)
        {
            score = 0;
        }
        scoreTxt.text = "Score: " + score;
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
