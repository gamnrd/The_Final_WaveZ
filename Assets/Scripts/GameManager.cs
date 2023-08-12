using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public enum GameMode : short {None, Wave, Story, Idle};
public enum FrameLimits { noLimit = 0, defaultFPS = -1, limit30 = 30, limit60 = 60, limit120 = 120 }


public class GameManager : MonoBehaviour
{

    public static GameManager instance { get; private set; }

    [Header("Scene Management")]
    [SerializeField] private bool gameInitialized = false;
    [SerializeField] private Camera loadingCamera;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadingBar;
    [SerializeField] private TextMeshProUGUI loadingText;

    [Header("Settings")]
    [SerializeField] private GameObject settingsScreen;

    public GameMode gameMode;
    public FrameLimits frameLimit;
    public int levelNum;

    public string currentSceneName;
    public string newSceneName;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            instance = this;
        }

        //DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = (int)frameLimit;
    }

    private void Start()
    {
        if (!gameInitialized)
        {
            LoadScene(this, "MainMenu");
            gameInitialized = true;
        }
    }


    #region SceneManagement
    public void LoadScene(Component sender, object data)
    {
        if (data is string sceneName)
        {
            loadingScreen.SetActive(true);
            loadingCamera.enabled = true;
            newSceneName = sceneName;
            AsyncOperation scene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            StartCoroutine(LoadingScreen(scene));
        }
    }      
    
    public void LoadLevel(Component sender, object data)
    {
        if (data is string sceneName)
        {
            loadingScreen.SetActive(true);
            loadingCamera.enabled = true;
            newSceneName = sceneName;
            AsyncOperation playerScene = SceneManager.LoadSceneAsync("WavePlayerScene", LoadSceneMode.Additive);
            AsyncOperation worldScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            StartCoroutine(LoadingScreen(playerScene, worldScene));
        }
    }

    public void UnloadScene(Component sender, object data)
    {
        if (data is string sceneName)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }
    }    
    
    public void UnloadLevel(Component sender, object data)
    {
        if (data is string sceneName)
        {
            SceneManager.UnloadSceneAsync("WavePlayerScene");
            SceneManager.UnloadSceneAsync(sceneName);
        }
    }

    IEnumerator LoadingScreen(AsyncOperation scene)
    {
        float loadProgess = 0;

        while (!scene.isDone)
        {
            loadProgess += scene.progress;
            loadingBar.value = loadProgess;
            loadingText.text = $"{loadProgess}%";
            yield return null;
        }
        loadingCamera.enabled = false;
        loadingScreen.SetActive(false);
        currentSceneName = newSceneName;
    }

    IEnumerator LoadingScreen(AsyncOperation playerScene, AsyncOperation worldScene)
    {
        float loadProgess = 0;

        while (!playerScene.isDone && !worldScene.isDone)
        {
            loadProgess += (playerScene.progress + worldScene.progress) / 2;
            loadingBar.value = loadProgess;
            loadingText.text = $"{loadProgess}%";
            yield return null;
        }
        loadingCamera.enabled = false;
        loadingScreen.SetActive(false);
        currentSceneName = newSceneName;
    }
    #endregion SceneManagement

    public void SetGameMode(Component sender, object data)
    {
        if (data is GameMode newMode)
            gameMode = newMode;
    }




    ////////////////////////////////////////////////////////////////////////
    /// 
    /// Settings
    ///
    ////////////////////////////////////////////////////////////////////////

    public void ToggleSettingsScreen(Component sender, object data)
    {
        if (data is bool toggle)
        {
            settingsScreen.SetActive(toggle);
        }
    }
}
