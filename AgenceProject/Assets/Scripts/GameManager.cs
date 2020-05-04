using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager gameManager;

    public enum STATE_PLAY { inMenu,verificationThrow, waitingToThrow, checkMovement }
    public STATE_PLAY gameState;

    public int shoot;
    private int shootsAllowed;
    private int shootsDone;
    private bool isInTutorial = false;
    public bool isInGame = false;

    public void Awake()
    {
        if (gameManager != null && gameManager != this)
        {
            Destroy(gameObject);
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
        switch (gameState)
        {
            case STATE_PLAY.inMenu:
                break;
            case STATE_PLAY.verificationThrow:
                if (shootsDone == shootsAllowed)
                {
                    EndLevel(false);
                }
                else
                {
                    gameState = STATE_PLAY.waitingToThrow;
                }
                break;
            case STATE_PLAY.waitingToThrow:
                break;
            case STATE_PLAY.checkMovement:
                //LevelManager.levelManager.CheckMovement();
                break;
            default:
                break;
        }
        if (isInTutorial)
        {
            GenerateLevel();
        }
        //if (isInGame)
        //{
        //    if (shootsDone == shootsAllowed)
        //    {
        //        EndLevel(false);
        //    }
        //}
        
    }

    public void GenerateLevel()
    {
        PrepareLevel();
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
        //gameState = STATE_PLAY.checkMovement;
    }

    public void PrepareLevel()
    {
        LevelManager.levelManager.ChargeLevel();
        shootsAllowed = LevelManager.levelManager.ShotsLevel();
        shootsDone = 0;
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
