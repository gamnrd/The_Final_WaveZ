using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Level Button", menuName = "Idle/Level", order = 3)]
public class WaveLevelObject : ScriptableObject
{
    public int levelNum;
    public Sprite image;
    public string levelSubText;
    public string levelMainText;
    public string levelSceneName;

    public List<GameObject> rewards = new List<GameObject>();
}