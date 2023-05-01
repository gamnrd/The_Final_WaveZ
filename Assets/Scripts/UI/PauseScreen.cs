using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    public static PauseScreen Instance;
    [SerializeField] private bool isPaused = false;

    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject mainScreen;
    [SerializeField] private GameObject aboutScreen;

    private void Awake()
    {
        Instance = this;
        if (pauseUI == null) pauseUI = transform.Find("PauseUI").GetComponent<RectTransform>().gameObject;
        if (mainScreen == null) mainScreen = transform.Find("PauseUI/MainScreen").GetComponent<RectTransform>().gameObject;
        if (aboutScreen == null) aboutScreen = transform.Find("PauseUI/AboutScreen").GetComponent<RectTransform>().gameObject;
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
        GameUI.Instance.SetGameUI(false);
        pauseUI.gameObject.SetActive(true);
        mainScreen.gameObject.SetActive(true);
        aboutScreen.gameObject.SetActive(false);
    }    
    
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        pauseUI.SetActive(false);
        mainScreen.SetActive(false);
        aboutScreen.SetActive(false);
        GameUI.Instance.SetGameUI(true);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    #endregion PauseMenu
}
