using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager gameManager;

    [Header("Menu")]
    public bool isPaused = false;
    public GameObject pausePanel;
    public GameObject victoryPanel;
    public GameObject defeatPanel;

    [Header("Values")]
    public int ennemisLeft;
    public int shootsLeft;

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
        Time.timeScale = 1f;
    }

    //Fonction de pause
    public void Pause()
    {
        if(isPaused == false)
        {
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            pausePanel.SetActive(false);
        }

        isPaused = !isPaused;
    }

    // Fonction à lancé à chaque tir [fonction de test]
    public void ShootTakeDown()
    {
        shootsLeft--;
    }

    // Fonction qui se lance quand un ennemi meur
    public void EnnemiTakeDown ()
    {
        ennemisLeft--;

        if(ennemisLeft < 0)
        {
            UIManager.uiManager.DisplayLevelResults(true);
        }
    }

    // Fonction qui se lance après que tous les tirs ont été effectuer et que plus aucuns éléments dans la scène ne bouge
    public void CheckEnnemiAlive()
    {
        if (shootsLeft <= 0 && ennemisLeft != 0)
        {
            UIManager.uiManager.DisplayLevelResults(false);
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1;
    }

}
