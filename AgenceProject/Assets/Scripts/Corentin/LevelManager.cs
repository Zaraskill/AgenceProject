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
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log("Mode : " + mode);
        if (scene.buildIndex - 1 != currentLevel && currentLevel < 4)
        {
            numberRetry = 0;
        }
        timeActive = false;
        timerLevel = 0f;
        UpdateLevelValues(scene.buildIndex);
        PlayerData.instance.lm = this;
        if (scene.buildIndex == 0)
        {
            Button button = GameObject.Find("StatButton").GetComponent<Button>();
            button.onClick.AddListener(delegate () {
                GameObject.Find("StatUI").SetActive(true);
            });
        }
    }
    
    void OnDisable()
    {
        Debug.Log("OnDisable");
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
        currentLevel = id - 1;
        if (currentLevel >= 0)
        {
            timeActive = true;
        }
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
    
}

