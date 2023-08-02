using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    public static GameOverScreen Instance;

    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject gameOverBackground;


    private void Awake()
    {
        Instance = this;
        if (gameOverCanvas == null) gameOverCanvas = transform.Find("GameOverCanvas").GetComponent<RectTransform>().gameObject;
        if (gameOverBackground == null) gameOverBackground = transform.Find("GameOverCanvas/GameOverBackground").GetComponent<RectTransform>().gameObject;
    }


    public void GameOver()
    {
        GameUI.Instance.SetGameUI(false);
        gameOverCanvas.SetActive(true);
        gameOverBackground.GetComponent<Animator>().enabled = true;
        Time.timeScale = 0.5f;
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void ReturnToBase()
    {
        //Getrewardstotals
        Time.timeScale = 1f;
        SceneManager.LoadScene("Idle_Base", LoadSceneMode.Single);
    }

    private void GetRewardTotals()
    {

    }

    public void AdContinue()
    {

    }
}
