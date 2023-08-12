using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleManager : MonoBehaviour
{
    public static IdleManager instance;
    public bool menuActive;



    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

}
