using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{

    public int[] levelNumber;
    public float[] timerNumber;
    public int[] starsNumber;
    public int[] retryNumber;

    public SaveData(PlayerData data)
    {
        levelNumber = data.levelNumber;
        timerNumber = data.timerNumber;
        starsNumber = data.starsNumber;
        retryNumber = data.retryNumber;
        
    }

    public void LoadLevelData()
    {
        SaveData data = SaveSystem.LoadDataFile();
        levelNumber = data.levelNumber;
        timerNumber = data.timerNumber;
        starsNumber = data.starsNumber;
        retryNumber = data.retryNumber;
    }

}


