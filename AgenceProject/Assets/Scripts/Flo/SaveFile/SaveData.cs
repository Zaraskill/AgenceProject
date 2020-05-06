using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData : MonoBehaviour
{
    public Level[] LevelsDatas;

    public SaveData(LevelManager level)
    {
        LevelsDatas[level.currentLevel].levelNumber = level.currentLevel;
        LevelsDatas[level.currentLevel].timerNumber = level.timerLevel;
        LevelsDatas[level.currentLevel].starsNumber = level.starsObtained;
        LevelsDatas[level.currentLevel].retryNumber = level.numberRetry;
    }
    
}

public class Level
{
    public int levelNumber;
    public float timerNumber;
    public int starsNumber;
    public int retryNumber;
}
