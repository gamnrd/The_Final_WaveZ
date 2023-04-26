using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    private int playerScore;
    public GameObject pauseMenu;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            LevelComplete();
        }
    }

    private void LevelComplete()
    {
        /*
        playerScore = UIController.Instance.GetScore();
        int highscore = PlayerPrefs.GetInt("HighScore", 0);
        if (playerScore > highscore)
        {
            PlayerPrefs.SetInt("HighScore", playerScore);
        }
        pauseMenu.SetActive(true);
        UIController.Instance.Win();
        Time.timeScale = 0;*/
    }
}
