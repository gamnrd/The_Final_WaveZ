using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    public static PauseScreen Instance;
    [SerializeField] private bool isPaused = false;

    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject mainScreen;

    [Header("Events")]
    [SerializeField] private GameEvent toggleGameUI;
    [SerializeField] private GameEvent toggleSettings;
    [SerializeField] private GameEvent loadScene;
    [SerializeField] private GameEvent unloadLevel;



    private void Awake()
    {
        Instance = this;
        if (pauseUI == null) pauseUI = transform.Find("PauseCanvas").GetComponent<RectTransform>().gameObject;
        if (mainScreen == null) mainScreen = transform.Find("PauseCanvas/Pause_Main").GetComponent<RectTransform>().gameObject;
    }

    private void Start()
    {
        ResumeGame();
    }

    #region PauseMenu
    public bool GetPaused()
    {
        return isPaused;
    }

    //Pause Menu
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
        toggleGameUI.Raise(this, false);
        pauseUI.gameObject.SetActive(true);
        mainScreen.gameObject.SetActive(true);
    }    
    
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        pauseUI.SetActive(false);
        mainScreen.SetActive(false);
        toggleGameUI.Raise(this, true);
    }

    public void ReturnToBase()
    {
        Time.timeScale = 1;
        unloadLevel.Raise(this, GameManager.instance.currentSceneName);
        loadScene.Raise(this, "Idle_Base");
        //AsyncOperation scene = SceneManager.LoadSceneAsync("Idle_Base");
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        unloadLevel.Raise(this, GameManager.instance.currentSceneName);
        loadScene.Raise(this, "MainMenu");
        //SceneManager.LoadScene("MainMenu");
    }

    public void Settings()
    {
        toggleSettings.Raise(this, true);
    }

    #endregion PauseMenu
}
