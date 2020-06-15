using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    public static LevelManager levelManager;

    public int currentLevel;
    public int starsObtained;
    public int numberRetry;
    public float timerLevel;
    bool timeActive;

    public LevelState level;

    void Awake()
    {
        if (levelManager == null || levelManager == this)
        {
            levelManager = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        gameObject.SetActive(true);
    }
    
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex - 1 != currentLevel && currentLevel < 4)
        {
            numberRetry = 0;
        }
        timeActive = false;
        timerLevel = 0f;
        UpdateLevelValues(scene.buildIndex);
        PlayerData.instance.lm = this;
        PlayLevelMusic(scene.buildIndex);
    }
    
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        if (timeActive == true)
        {
            timerLevel += Time.deltaTime;
        }
    }

    void UpdateLevelValues(int id)
    {
        currentLevel = id;
        if (currentLevel >= 0)
        {
            timeActive = true;
        }
    }

    /* Music List :
     * 
     * Music_Main_Theme
     * Music_The_Experiment
     * Music_The_Laboratory
     * 
     */
    void PlayLevelMusic(int id)
    {
        if (id == 0)
            AudioManager.instance.PlayMusic("Music_Main_Theme");
        else if (id >= 1 && id < 17)
            AudioManager.instance.PlayMusic("Music_The_Experiment");
        else if (id >= 17)
            AudioManager.instance.PlayMusic("Music_The_Laboratory");
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
        return level.NumberStars(numberShotsDone);
    }

    public int ShotsLevel()
    {
        return level.shootsAllowed;
    }

    public void ChargeLevel()
    {
        level = FindObjectOfType<LevelState>();
    }

    public bool IsTutorial()
    {
        return level.isTutorial;
    }
    
}

