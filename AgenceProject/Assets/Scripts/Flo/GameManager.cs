using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Menu")]
    public GameObject PausePanel;
    public bool isPaused = false;

    static public int currentlevel;
    
    void Start()
    {
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        isPaused = true;
        PausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        isPaused = false;
        PausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void MenuBtn()
    {
        SceneManager.LoadScene("Flo_Menu");
    }

    static public void IntLevel(int levelname)
    {
        currentlevel = levelname;
    }

}
