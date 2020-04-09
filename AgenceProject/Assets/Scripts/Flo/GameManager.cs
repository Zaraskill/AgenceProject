using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Menu")]
    public GameObject PausePanel;
    
    void Start()
    {
        Time.timeScale = 1f;
    }
    
    void Update()
    {
        
    }

    public void Pause()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
