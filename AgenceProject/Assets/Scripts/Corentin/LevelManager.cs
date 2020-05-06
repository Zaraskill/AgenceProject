﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static LevelManager levelManager;

    public int currentLevel;
    public int starsObtained;
    public int numberRetry;
    public float timerLevel;
    public LevelState level;

    void Awake()
    {
        if(levelManager != null)
        {
            Debug.LogError("too many instances");
        }
        else
        {
            levelManager = this;
            //DontDestroyOnLoad(this.gameObject);
        }        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnemyDeath()
    {
        level.BeatEnnemy();
        if (level.HasNoEnemiesLeft())
        {
            GameManager.gameManager.EndLevel(true);
        }
    }

    public bool HasEnemy()
    {
        return level.HasNoEnemiesLeft();
    }

    public int ScoreResults(int numberShotsDone)
    {
        return level.numberStars(numberShotsDone);
    }

    public int ShotsLevel()
    {
        return level.shootsAllowed;
    }

    public void ChargeLevel()
    {
        level = FindObjectOfType<LevelState>();
    }

    public void CheckMovement()
    {
        level.CheckMoving();
    }

    #region Save & load
    public void SaveLevelData()
    {
        SaveSystem.SaveDataFile(this);
    }
    /*
    public void LoadLevelData()
    {
        SaveData data = SaveSystem.LoadDataFile();

        //
    }
    */
    #endregion

}
