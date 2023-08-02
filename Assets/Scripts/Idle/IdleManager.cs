using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IdleManager : MonoBehaviour
{
    public static IdleManager instance;
    public bool menuActive;



    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void StartWave()
    {
        AsyncOperation playerScene = SceneManager.LoadSceneAsync("WavePlayerScene");
        AsyncOperation worldScene = SceneManager.LoadSceneAsync("Wave_World1_Hospital", LoadSceneMode.Additive);
    }
}
