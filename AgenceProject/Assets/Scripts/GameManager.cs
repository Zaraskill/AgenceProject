using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager gameManager;

    public int shoot;
    private int shootsAllowed;
    private int shootsDone;
    private bool isInTutorial = false;

    public void Awake()
    {
        if (gameManager != null)
        {
            Debug.LogError("Too many instances!");
        }
        else
        {
            //DontDestroyOnLoad(this.gameObject);
            gameManager = this;
        }
    }

    void Start ()
    {
        shootsAllowed = LevelManager.levelManager.ShotsLevel();
    }

    void Update()
    {
        if (shootsDone > shootsAllowed)
        {
            EndLevel(false);
        }
    }

    ////Fonction de pause
    //public void Pause()
    //{
    //    if(isPaused == false)
    //    {
    //        Time.timeScale = 0f;
    //        pausePanel.SetActive(true);
    //    }
    //    else
    //    {
    //        Time.timeScale = 1f;
    //        pausePanel.SetActive(false);
    //    }

    //    isPaused = !isPaused;
    //}

    //// Fonction à lancé à chaque tir [fonction de test]
    //public void ShootTakeDown()
    //{
    //    shootsLeft--;
    //}

    //// Fonction qui se lance quand un ennemi meur
    //public void EnnemiTakeDown ()
    //{
    //    ennemisLeft--;

    //    if(ennemisLeft < 0)
    //    {
    //        UIManager.uiManager.DisplayLevelResults(true);
    //    }
    //}

    //// Fonction qui se lance après que tous les tirs ont été effectuer et que plus aucuns éléments dans la scène ne bouge
    //public void CheckEnnemiAlive()
    //{
    //    if (shootsLeft <= 0 && ennemisLeft != 0)
    //    {
    //        UIManager.uiManager.DisplayLevelResults(false);
    //    }
    //}

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
        shoot++;
    }

    public void PrepareLevel()
    {

    }

    public void EndLevel(bool sideWin)
    {
        Debug.Log("ending");
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
