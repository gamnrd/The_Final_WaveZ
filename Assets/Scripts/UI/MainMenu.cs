using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
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
        AsyncOperation scene = SceneManager.LoadSceneAsync("Game");
        startScreen.SetActive(false);
        loadingScreen.SetActive(true);
        StartCoroutine(LoadingScreen(scene));
    }

    public void StartWave()
    {
        AsyncOperation playerScene = SceneManager.LoadSceneAsync("WavePlayerScene");
        AsyncOperation worldScene = SceneManager.LoadSceneAsync("Wave_Test", LoadSceneMode.Additive);
        startScreen.SetActive(false);
        loadingScreen.SetActive(true);
        StartCoroutine(LoadingScreen(playerScene, worldScene));
    }

    IEnumerator LoadingScreen(AsyncOperation scene)
    {
        float loadProgess = 0;

        while(!scene.isDone)
        {
            loadProgess += scene.progress;
            loadingBar.fillAmount = loadProgess;
            yield return null;
        }
    }

    IEnumerator LoadingScreen(AsyncOperation playerScene, AsyncOperation worldScene)
    {
        float loadProgess = 0;

        while (!playerScene.isDone && !worldScene.isDone)
        {
            loadProgess += (playerScene.progress + worldScene.progress) / 2;
            loadingBar.fillAmount = loadProgess;
            yield return null;
        }
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
