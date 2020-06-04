[System.Serializable]
public class SaveData
{

    public int[] levelNumber;
    public float[] timerNumber;
    public int[] starsNumber;
    public int[] retryNumber;
    public bool[] pageLock;
    public int language;

    public SaveData(PlayerData data)
    {
        levelNumber = data.levelNumber;
        timerNumber = data.timerNumber;
        starsNumber = data.starsNumber;
        retryNumber = data.retryNumber;
        pageLock = data.pageLock;
        language = data.language;


    }

    public void LoadLevelData()
    {
        SaveData data = SaveSystem.LoadDataFile();
        levelNumber = data.levelNumber;
        timerNumber = data.timerNumber;
        starsNumber = data.starsNumber;
        retryNumber = data.retryNumber;
        pageLock = data.pageLock;
        language = data.language;
    }

}


