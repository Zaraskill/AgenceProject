using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager gameManager;

    public enum STATE_PLAY { inMenu,verificationThrow, waitingToThrow, checkMovement, inTutorial }

    public STATE_PLAY gameState;

    public static bool hasSoundCut = false;
    public static bool hasMusicCut = false;
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
            PlayerData.instance.LoadLevelData();
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
                break;
            case STATE_PLAY.verificationThrow:
                if (shootsDone == shootsAllowed && !LevelManager.levelManager.HasEnemy())
                {
                    VFXManager.instance.Alerte(false);
                    Time.timeScale = 0f;
                    EndLevel(false);
                }
                else if (shootsAllowed - shootsDone <= 1)
                {
                    VFXManager.instance.Alerte(true);
                }
                else
                {
                    gameState = STATE_PLAY.waitingToThrow;
                }
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
        PrepareLevel();
        isInGame = true;
        checkGm = FindObjectOfType<CheckListVelocity>();
        player = FindObjectOfType<PlayerController>();
        UIManager.uiManager.UndisplayLevelResults();
        UIManager.uiManager.UndisplayPause();
        if (LevelManager.levelManager.IsTutorial())
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
        player.dotStorage.SetActive(false);
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

        PlayerController.throwAllowed = false;
        checkGm.CheckMoving();
        gameState = STATE_PLAY.checkMovement;
    }

    public void PrepareLevel()
    {
        LevelManager.levelManager.ChargeLevel();
        shootsAllowed = LevelManager.levelManager.ShotsLevel();
        shootsDone = 0;
    }

    public void EndLevel(bool sideWin)
    {
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
        UIManager.uiManager.DisplayTutorial(SceneManager.GetActiveScene().buildIndex);
    }

    public void DeactivateTuto()
    {
        isInTutorial = false;
        gameState = STATE_PLAY.waitingToThrow;
        Time.timeScale = 1f;
    }

    #endregion

}
