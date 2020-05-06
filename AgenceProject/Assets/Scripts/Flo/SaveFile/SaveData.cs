using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData : MonoBehaviour
{

    public int levelNumber;
    public int timerNumber;
    public int retryNumber;

    public SaveData(LevelState level)
    {
        //levelNumber = level.levelNumber;
        //timerNumber = level.timeNumber;
        //retryNumber = level.timeNumber;
    }


    /*** A mettre dans le script 'LevelState' ***
    public void SaveLevelData()
    {
        SaveSystem.SaveDataFile(this);
    }

    public void LoadLevelData()
    {
        SaveData data = SaveSystem.LoadDataFile();

        //levelNumber = data.levelNumber;
        //timerNumber = data.timeNumber;
        //retryNumber = data.timeNumber;
    }
    *********************************************/
}
