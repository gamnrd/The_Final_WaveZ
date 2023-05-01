using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WinScreen : MonoBehaviour
{
    public static WinScreen Instance;

    [SerializeField] private int playerScore = 0;
    [SerializeField] private int highScore = 0;
    [SerializeField] private GameObject gameWinUI;
    [SerializeField] private TextMeshProUGUI playerScoreTxt;
    [SerializeField] private TextMeshProUGUI highScoreTxt;


    private void Awake()
    {
        Instance = this;
        if (gameWinUI == null) gameWinUI = transform.Find("GameWinUI").GetComponent<RectTransform>().gameObject;
        if (playerScoreTxt == null) playerScoreTxt = transform.Find("GameWinUI/PlayerScoreTxt").GetComponent<TextMeshProUGUI>();
        if (highScoreTxt == null) highScoreTxt = transform.Find("GameWinUI/HighScoreTxt").GetComponent<TextMeshProUGUI>();
    }


    public void LevelComplete()
    {
        playerScore = GameUI.Instance.GetScore();
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (playerScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", playerScore);
        }
        Time.timeScale = 0;

        playerScoreTxt.text = "Your Score: " + playerScore;
        highScoreTxt.text = "High Score: " + highScore;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
