using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    
    public void selectLevel(int levelId)
    {
        Debug.Log(levelId);
        GameManager.IntLevel(levelId);
        SceneManager.LoadScene("Flo_Main");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
