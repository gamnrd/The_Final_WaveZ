using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectUI : MonoBehaviour
{
    [SerializeField] private GameObject stageSelectUI;

    private void Awake()
    {
        if (stageSelectUI == null) stageSelectUI = transform.Find("Stage").gameObject;
    }

    public void OnBack()
    {
        IdleManager.instance.menuActive = false;
        stageSelectUI.SetActive(false);
    }
}
