using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    AsyncOperation scene;
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Image loadingBar;

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

    public void StartGame()
    {
        scene = SceneManager.LoadSceneAsync("Game");
        startScreen.SetActive(false);
        loadingScreen.SetActive(true);
        StartCoroutine(LoadingScreen());
        
    }

    IEnumerator LoadingScreen()
    {
        float loadProgess = 0;

        while(!scene.isDone)
        {
            loadProgess += scene.progress;
            loadingBar.fillAmount = loadProgess;
            yield return null;
        }
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
