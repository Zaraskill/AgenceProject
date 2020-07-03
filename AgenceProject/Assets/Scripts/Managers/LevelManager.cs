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
    public int currentScene;
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
        timeActive = false;
        currentScene = scene.buildIndex;

        PlayerData.instance.lm = this;
        UpdateLevelValues(scene.buildIndex);

        ApplicationSettings.SystemeGraphicAuto(true);
        StartCoroutine(LateLoadStart(scene.buildIndex));
    }

    IEnumerator LateLoadStart(int id)
    {
        yield return new WaitForSeconds(0.1f);
        AudioManager.instance.IntiAudio(id);
    }
    
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        if (timeActive == true && currentScene != 0)
        {
            PlayerData.instance.timerNumber[currentLevel - 1] += Time.deltaTime;
        }
    }

    public void Retry()
    {
        PlayerData.instance.retryNumber[currentLevel - 1]++;
    }

    void UpdateLevelValues(int id)
    {
        currentLevel = id;
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

    public void PauseGame()
    {
        timeActive = false;
    }

    public void UnpauseGame()
    {
        timeActive = true;
    }
    
}

