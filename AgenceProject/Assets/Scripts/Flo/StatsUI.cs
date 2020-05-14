using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{

    public static StatsUI instance;

    public GameObject content;
    public GameObject uiCanvas;

    void Awake()
    {

        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

    }

    public void Back()
    {
        uiCanvas.SetActive(true);
    }
    
    public void DropBtn()
    {
        PlayerData.instance.DeleteLevelData();
    }

    public void LoadLogs()
    {
        PlayerData.instance.content = content;
        PlayerData.instance.LoadLevelData();
    }

}
