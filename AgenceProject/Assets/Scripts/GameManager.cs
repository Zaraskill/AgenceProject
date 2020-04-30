using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager gameManager;

    //public enum STATE_PLAY { inMenu, waitingToThrow, }

    public int shoot;
    private int shootsAllowed;
    private int shootsDone;
    private bool isInTutorial = false;
    public bool isInGame = false;

    public void Awake()
    {
        if (gameManager != null)
        {
            Debug.LogError("Too many instances!");
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            gameManager = this;
        }
    }

    void Start ()
    {
        
    }

    void Update()
    {
        if (isInTutorial)
        {
            GenerateLevel();
        }
        if (isInGame)
        {
            if (shootsDone == shootsAllowed)
            {
                EndLevel(false);
            }
        }
        
    }

    public void GenerateLevel()
    {
        PrepareLevel();
        shootsAllowed = LevelManager.levelManager.ShotsLevel();
        shootsDone = 0;
        isInGame = true;
        UIManager.uiManager.UpdateShots(shootsAllowed);
        UIManager.uiManager.UndisplayLevelResults();
        UIManager.uiManager.UndisplayPause();
        UIManager.uiManager.DisplayInGameUI();
    }

    //Pause
    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1f;
    }

    public void Shoot()
    {
        shootsDone++;
        UIManager.uiManager.UpdateShots(shootsAllowed - shootsDone);
        shoot++;
    }

    public void PrepareLevel()
    {
        LevelManager.levelManager.ChargeLevel();
        //UIManager.uiManager.DisplayInGameUI();
    }

    public void EndLevel(bool sideWin)
    {
        //Debug.Log("ending");
        UIManager.uiManager.DisplayLevelResults(sideWin, LevelManager.levelManager.ScoreResults(shootsDone));
    }

    #region Tutorial Fonctions

    public void ActivateTuto()
    {
        isInTutorial = true;
        Time.timeScale = 0f;
        UIManager.uiManager.DisplayTutorial();
    }

    public void DeactivateTuto()
    {
        isInTutorial = false;
        UIManager.uiManager.UndisplayTutorial();
        Time.timeScale = 1f;
    }

    #endregion

}
