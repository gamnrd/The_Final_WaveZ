using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [Header("Game UI")]
    public Image healthImg;
    public Text healthTxt;
    public int score = 0;    
    public Text livesTxt;
    public int lives;
    public Text scoreTxt;
    public Text waveTxt;
    public Text winScore;
    public Text winHighScore;

    [Header("UI")]
    public GameObject MenuScreen;
    public GameObject GameUI;

    [Header("Pause")]
    public GameObject pauseScreen;
    public GameObject aboutScreen;

    [Header("Main Menu")]
    public GameObject MainMenuScreen;
    public GameObject DifficultyScreen;
    public GameObject aboutMainScreen;

    [Header("Game Win")]
    public GameObject WinScreen;    
    
    [Header("Game Over")]
    public GameObject LoseScreen;

    [Header("Touch Controls")]
    [SerializeField] public bool usingTouch;


    private void Awake()
    {
        Instance = this;
    }

    #region GameUI
    public void SetGameUI(bool toggle)
    {
            GameUI.SetActive(toggle);
    }


    public void UpdateBars(int curHealth, int maxHealth)
    {
        healthImg.fillAmount = (float) ((float)curHealth / (float)maxHealth);
        healthTxt.text = "Health: " + curHealth + " / " + maxHealth + "";
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

    public void UpdateWaveTracker(int kills, int max)
    {
        waveTxt.text = kills + " / " + max;
    }

    public int GetScore()
    {
        return score;
    }

    #endregion GameUI

    #region Difficulty
    /*
      * easy - enemy health 1, player health 20
      * normal - enamy health 3, player 10
      * Insane - enamy 5, player 1*/
    public void Easy()
    {
        PlayerPrefs.SetInt("PlayerHealth", 20);
        PlayerPrefs.SetInt("ZombieHealth", 1);
        StartGame();
    }
    public void Normal()
    {
        PlayerPrefs.SetInt("PlayerHealth", 10);
        PlayerPrefs.SetInt("ZombieHealth", 3);
        StartGame();
    }

    public void Insane()
    {
        PlayerPrefs.SetInt("PlayerHealth", 1);
        PlayerPrefs.SetInt("ZombieHealth", 5);
        StartGame();
    }


    #endregion Difficulty

    #region PauseMenu
    public bool getPaused()
    {
        return MenuScreen.activeSelf;
    }

    //Pause Menu
    public void ResumeGame()
    {
        Time.timeScale = 1;
        SetScreens("unpause");
    }

    public void Back()
    {
        SetScreens("pause");
    }

    public void About()
    {
        SetScreens("about");
    }

    public void MainMenu()
    {
        SetGameUI(false);
        SetScreens("main");
        SceneManager.LoadScene("MainMenu");
    }

    #endregion PauseMenu

    #region MainMenu
    //Main menu

    public void StartGame()
    {
        Time.timeScale = 1;
        SetGameUI(true);
        SetScreens("pause");
        SetScreens("unpause");
        SceneManager.LoadScene("Game");
    }

    public void DifficultyMenu()
    {
        SetScreens("difficulty");
    }

    public void AboutMain()
    {
        SetScreens("mainabout");
    }

    public void BackMain()
    {
        SetScreens("main");
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void Win()
    {
        winScore.text = "Your Score: " + score;
        winHighScore.text = "High Score: " + PlayerPrefs.GetInt("HighScore", 0);
        SetScreens("win");
    }    
    
    public void Lose()
    {
        SetScreens("lose");
    }

    #endregion MainMenu

    public void SetScreens(string screenName)
    {
        pauseScreen.SetActive(false);
        aboutScreen.SetActive(false);
        MainMenuScreen.SetActive(false);
        aboutMainScreen.SetActive(false);
        DifficultyScreen.SetActive(false);
        WinScreen.SetActive(false);
        LoseScreen.SetActive(false);


        switch (screenName)
        {
            //Pause Menu
            case "unpause":
                MenuScreen.SetActive(false);
                break;
            case "pause":
                pauseScreen.SetActive(true);
                break;
            case "about":
                aboutScreen.SetActive(true);
                break;

            //Main Menu
            case "main":
                MainMenuScreen.SetActive(true);
                break;
            case "difficulty":
                DifficultyScreen.SetActive(true);
                break;
            case "mainabout":
                aboutMainScreen.SetActive(true);
                break;

            case "win":
                WinScreen.SetActive(true);
                break;
            case "lose":
                LoseScreen.SetActive(true);
                break;

            //unpause
            default:
                break;
        }
    }

}
