﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager gameManager;

    public enum STATE_PLAY { inMenu,verificationThrow, waitingToThrow, checkMovement, inTutorial, introPlayer, levelResult }

    public STATE_PLAY gameState;

    public List<int> objectivesPages;

    public int shoot;
    private int shootsAllowed;
    private int shootsDone;

    [Header("BoolCheck status Dont touch")]
    public bool isInGame = false;
    public bool isInMenu = false;
    public bool isInIntroPlayer = false;
    private bool isInTutorial = false;    

    private CheckListVelocity checkGm;
    private PlayerController player;

    public void Awake()
    {
        if (gameManager != null && gameManager != this)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            gameManager = this;
        }
    }

    void Start() { 
        if (LocalisationSystem.isInit)
        {
            LocalisationSystem.Init();
        }
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            UIManager.uiManager.DisplayMainMenu();
        }
        LocalisationSystem.UpdateTextsUI();
    }

    public void StateChecking()
    {
        switch (gameState)
        {
            case STATE_PLAY.inMenu:
                break;
            case STATE_PLAY.inTutorial:
                break;
            case STATE_PLAY.verificationThrow:
                if (shootsDone - (LevelManager.levelManager.level.isIntroPlayer ? 1 : 0) == shootsAllowed && !LevelManager.levelManager.HasEnemy())
                    EndLevel(false);
                else if (shootsAllowed - shootsDone + (LevelManager.levelManager.level.isIntroPlayer ? 1 : 0) == 1)
                    VFXManager.instance.Alerte(true);
                else
                    gameState = STATE_PLAY.waitingToThrow;
                break;
            case STATE_PLAY.waitingToThrow:
                break;
            case STATE_PLAY.checkMovement:
                break;
            default:
                break;
        }        
    }

    public void GenerateLevel()
    {
        checkGm = FindObjectOfType<CheckListVelocity>();
        player = FindObjectOfType<PlayerController>();
        PrepareLevel();
        isInGame = true;        
        UIManager.uiManager.UndisplayLevelResults();
        UIManager.uiManager.UndisplayPause();
        if (LevelManager.levelManager.level.isIntroPlayer)
        {
            isInIntroPlayer = true;
            gameState = STATE_PLAY.introPlayer;
        }
        else if (LevelManager.levelManager.IsTutorial())
        {
            gameState = STATE_PLAY.inTutorial;
            ActivateTuto();
        }
        else
        {
            Time.timeScale = 1f;
            gameState = STATE_PLAY.waitingToThrow;           
        }
        UIManager.uiManager.DisplayInGameUI();
        isInMenu = false;
        StateChecking();
        UIManager.uiManager.UpdateShots(shootsAllowed);        
        
    }

    //Pause
    #region Pause Fonctions

    public void PauseGame()
    {
        player.dotStorage.SetActive(false);
        LevelManager.levelManager.PauseGame();
        Time.timeScale = 0f;
        gameState = STATE_PLAY.inMenu;
        isInMenu = true;
    }

    public void UnPauseGame()
    {
        gameState = STATE_PLAY.waitingToThrow;
        LevelManager.levelManager.UnpauseGame();
        Time.timeScale = 1f;
        isInMenu = false;
    }

    #endregion

    public void Shoot()
    {
        shootsDone++;
        UIManager.uiManager.UpdateShots(shootsAllowed - shootsDone + (LevelManager.levelManager.level.isIntroPlayer ? 1 : 0) );
        shoot++;
        PlayerController.throwAllowed = false;
        checkGm.CheckMoving();
        gameState = STATE_PLAY.checkMovement;
    }

    public int GetShootDone()
    {
        return shootsDone - (LevelManager.levelManager.level.isIntroPlayer ? 1 : 0);
    }

    public void PrepareLevel()
    {
        LevelManager.levelManager.ChargeLevel();
        shootsAllowed = LevelManager.levelManager.ShotsLevel();
        shootsDone = 0;
        if (LevelManager.levelManager.level.isIntroPlayer)
        {
            player.GetComponent<PlayerFirstMove>().InitIntroPlayer();
        }
    }

    public void EndLevel(bool sideWin)
    {
        PlayerController.throwAllowed = false;
        checkGm.StopCheck();
        VFXManager.instance.Alerte(false);
        gameState = STATE_PLAY.levelResult;
        UIManager.uiManager.DisplayLevelResults(sideWin, LevelManager.levelManager.ScoreResults(shootsDone - (LevelManager.levelManager.level.isIntroPlayer ? 1 : 0)));        
    }

    public void ChangeStatus()
    {
        if (LevelManager.levelManager.level.isIntroPlayer && isInIntroPlayer)
        {
            if (LevelManager.levelManager.IsTutorial())
            {
                GameManager.gameManager.gameState = GameManager.STATE_PLAY.inTutorial;
                GameManager.gameManager.ActivateTuto();
            }
            else
            {
                UIManager.uiManager.DisplayInGameUI();
                GameManager.gameManager.gameState = GameManager.STATE_PLAY.verificationThrow;
            }
            isInIntroPlayer = false;
        }
        else if (!(gameState == STATE_PLAY.levelResult))
        {
            GameManager.gameManager.gameState = GameManager.STATE_PLAY.verificationThrow;
        }
    }

    #region Tutorial Fonctions

    public void ActivateTuto()
    {
        isInTutorial = true;
        UIManager.uiManager.DisplayTutorial(SceneManager.GetActiveScene().buildIndex);
    }

    public void DeactivateTuto()
    {
        isInTutorial = false;
        UIManager.uiManager.UndisplayTutorial();
        gameState = STATE_PLAY.waitingToThrow;
        StateChecking();
    }

    #endregion

}
