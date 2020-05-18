using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager gameManager;

    public enum STATE_PLAY { inMenu,verificationThrow, waitingToThrow, checkMovement, inTutorial }

    public STATE_PLAY gameState;

    public int shoot;
    private int shootsAllowed;
    private int shootsDone;
    private bool isInTutorial = false;
    public bool isInGame = false;

    public bool isInMenu = false;

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
            case STATE_PLAY.inTutorial:
                UIManager.uiManager.DisplayTutorial();
                break;
            case STATE_PLAY.verificationThrow:
                if (shootsDone == shootsAllowed && !LevelManager.levelManager.HasEnemy())
                {
                    Time.timeScale = 0f;
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
                PlayerController.throwAllowed = false;
                checkGm.CheckMoving();
                break;
            default:
                break;
        }
        //if (isInTutorial)
        //{
        //    GenerateLevel();
        //}
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
        checkGm = FindObjectOfType<CheckListVelocity>();
        player = FindObjectOfType<PlayerController>();
        UIManager.uiManager.UndisplayLevelResults();
        UIManager.uiManager.UndisplayPause();
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            gameState = STATE_PLAY.inTutorial;
            ActivateTuto();
        }
        else
        {
            Time.timeScale = 1f;
            gameState = STATE_PLAY.waitingToThrow;
            UIManager.uiManager.DisplayInGameUI();
            
        }
        isInMenu = false;
        UIManager.uiManager.UpdateShots(shootsAllowed);        
        
    }

    //Pause
    public void PauseGame()
    {
        Time.timeScale = 0f;
        gameState = STATE_PLAY.inMenu;
    }

    public void UnPauseGame()
    {
        gameState = STATE_PLAY.waitingToThrow;
        Time.timeScale = 1f;
    }

    public void Shoot()
    {
        shootsDone++;
        UIManager.uiManager.UpdateShots(shootsAllowed - shootsDone);
        shoot++;
        gameState = STATE_PLAY.checkMovement;
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
        checkGm.StopCheck();
        gameState = STATE_PLAY.inMenu;
        isInMenu = true;
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
        gameState = STATE_PLAY.waitingToThrow;
        Time.timeScale = 1f;
    }

    #endregion

}
