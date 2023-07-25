using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode : short { Wave, Story};

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public GameMode gameMode;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

}
