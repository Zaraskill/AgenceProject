using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;

    public int[] levelNumber;
    public float[] timerNumber;
    public int[] starsNumber;
    public int[] retryNumber;
    public bool[] pageLock;

    public GameObject content;

    public LevelManager lm;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        //lm = gameObject.GetComponent<LevelManager>();
    }

    #region Save & load
    public void SaveLevelData()
    {
        LocalUpdateData();
        SaveSystem.SaveDataFile(this);
    }
    public void LoadLevelData()
    {
        SaveData data = SaveSystem.LoadDataFile();
        if (data == null)
            return;

        levelNumber = data.levelNumber;
        timerNumber = data.timerNumber;
        starsNumber = data.starsNumber;
        retryNumber = data.retryNumber;
        pageLock = data.pageLock;

        UpdateTextContent(content);
    }
    public bool[] GetPageLockData()
    {
        SaveData data = SaveSystem.LoadDataFile();
        if (data == null)
            return null;

        return data.pageLock;
    }
    #endregion

    public void LocalUpdateData()
    {
        if (lm.currentLevel == -1)
        {
            return;
        }
        levelNumber[lm.currentLevel] = lm.currentLevel;
        timerNumber[lm.currentLevel] += lm.timerLevel;
        if (lm.starsObtained > starsNumber[lm.currentLevel])
        {
            starsNumber[lm.currentLevel] = lm.starsObtained;
        }
        retryNumber[lm.currentLevel] += lm.numberRetry;

        lm.timerLevel = 0;
        lm.numberRetry = 0;
    }

    public void UpdateTextContent(GameObject parent)
    {
        int i = 0;
        int j = 0;
        foreach (Transform childContent in parent.transform)
        {
            foreach (Transform child in childContent)
            {
                Text cpt = child.GetComponent<Text>();
                switch (j)
                {
                    case 0:
                        cpt.text = "Lvl : " + (1 + i).ToString();
                        break;
                    case 1:
                        cpt.text = "Time : " + TimerConvert(timerNumber[i]);
                        break;
                    case 2:
                        cpt.text = "Stars : " + starsNumber[i].ToString();
                        break;
                    case 3:
                        cpt.text = "Retry : " + retryNumber[i].ToString();
                        break;
                }
                
                j++;
            }
            i++;
            j = 0;
        }
    }

    public string TimerConvert(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60);

        string trueTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        return trueTime;
    }

    #region Delete Data File
    public void DeleteLevelData()
    {
        SaveSystem.DeleteDataFile();
        ResetValues();
        UpdateTextContent(content);
    }

    public void ResetValues()
    {
        for (int i = 0; i < levelNumber.Length; i++)
        {
            levelNumber[i] = 0;
            timerNumber[i] = 0;
            starsNumber[i] = 0;
            retryNumber[i] = 0;
        }
    }
    #endregion

}

