using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Menu")]
    public bool isPaused = false;
    public GameObject pausePanel;
    public GameObject victoryPanel;
    public GameObject defeatPanel;

    [Header("Values")]
    public int ennemiNumber;
    public int shootNumber;

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
        shootNumber--;
    }

    // Fonction qui se lance quand un ennemi meur
    public void EnnemiTakeDown ()
    {
        ennemiNumber--;

        if(ennemiNumber < 0)
        {
            Victory();
        }
    }

    // Fonction qui se lance après que tous les tirs ont été effectuer et que plus aucuns éléments dans la scène ne bouge
    public void CheckEnnemiAlive()
    {
        if (shootNumber == 0 && ennemiNumber != 0)
        {
            Defeat();
        }
    }

    // Fonction de victoire
    public void Victory()
    {
        victoryPanel.SetActive(true);

        //
    }

    // Fonction de défaite
    public void Defeat()
    {
        defeatPanel.SetActive(true);

        //
    }

}
