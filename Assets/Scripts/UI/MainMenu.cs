using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private GameEvent loadScene;
    [SerializeField] private GameEvent setGameMode;
    [SerializeField] private GameEvent unloadScene;
    [SerializeField] private GameEvent toggleSettingsUI;
 
    
    public void OnStartClicked()
    {
        setGameMode.Raise(this, GameMode.Idle);
        unloadScene.Raise(this, "MainMenu");
        loadScene.Raise(this, "Idle_Base");
    }

    public void OnSettingsClicked()
    {
        toggleSettingsUI.Raise(this, true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
