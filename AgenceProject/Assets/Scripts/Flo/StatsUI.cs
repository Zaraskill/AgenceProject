using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{

    public static StatsUI instance;

    public GameObject content;
    public GameObject global;
    public GameObject uiCanvas;
    public Text totalTime;
    public Text totalRetry;

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
        Back();
    }

    public void LoadLogs()
    {
        PlayerData.instance.content = content;
        PlayerData.instance.LoadLevelData();
    }

    public void GlobalsStats()
    {
        PlayerData.instance.UpdateTextContent(global);
        totalTime.text = "Total Time : " + CalculTotalTime();
        totalRetry.text = "Total Retry : " + CalculTotalRetry();
    }

    public string CalculTotalTime()
    {
        float total = 0;

        for (int i = 0; i < PlayerData.instance.levelNumber.Length; i++)
        {
            total += PlayerData.instance.timerNumber[i];
        }
        
        return PlayerData.instance.TimerConvert(total);
    }

    public int CalculTotalRetry()
    {
        int total = 0;

        for (int i = 0; i < PlayerData.instance.levelNumber.Length; i++)
        {
            total += PlayerData.instance.retryNumber[i];
        }

        return total;
    }

}
