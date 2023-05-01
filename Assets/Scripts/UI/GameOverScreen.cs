using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    public static GameOverScreen Instance;

    [SerializeField] private GameObject gameOverUI;



    private void Awake()
    {
        Instance = this;
        if (gameOverUI == null) gameOverUI = transform.Find("GameOverUI").GetComponent<RectTransform>().gameObject;
    }


    public void GameOver()
    {
        GameUI.Instance.SetGameUI(false);
        gameOverUI.SetActive(true);
        gameOverUI.GetComponent<Animator>().enabled = true;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
