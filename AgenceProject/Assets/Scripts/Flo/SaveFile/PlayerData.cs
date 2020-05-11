using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    public static PlayerData playerData;

    public int[] levelNumber;
    public float[] timerNumber;
    public int[] starsNumber;
    public int[] retryNumber;

    public GameObject content;
    public List<Text> Contents;

    LevelManager lm;

    void Awake()
    {
        if (playerData != null)
        {
            Debug.LogError("too many instances");
        }
        else
        {
            playerData = this;
        }
    }

    void Start()
    {
        lm = gameObject.GetComponent<LevelManager>();
        
        CreateTableContent();
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
        levelNumber = data.levelNumber;
        timerNumber = data.timerNumber;
        starsNumber = data.starsNumber;
        retryNumber = data.retryNumber;
    }
    #endregion

    public void LocalUpdateData()
    {
        levelNumber[lm.currentLevel] = lm.currentLevel;
        timerNumber[lm.currentLevel] = lm.timerLevel;
        starsNumber[lm.currentLevel] = lm.starsObtained;
        retryNumber[lm.currentLevel] = lm.numberRetry;
    }

    public void CreateTableContent()
    {
        int i = 0;
        int j = 0;
        foreach (Transform childContent in content.transform)
        {
            foreach (Transform child in childContent)
            {
                Text cpt = child.GetComponent<Text>();
                switch (j)
                {
                    case 0:
                        cpt.text = levelNumber[i].ToString();
                        break;
                    case 1:
                        cpt.text = timerNumber[i].ToString();
                        break;
                    case 2:
                        cpt.text = starsNumber[i].ToString();
                        break;
                    case 3:
                        cpt.text = retryNumber[i].ToString();
                        break;
                }
                Contents.Add(cpt);
                
                j++;
            }
            i++;
            j = 0;
        }
    }
    
}

