using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Menu")]
    public bool isPaused = false;
    
    void Start()
    {
        Time.timeScale = 1f;
    }

}
