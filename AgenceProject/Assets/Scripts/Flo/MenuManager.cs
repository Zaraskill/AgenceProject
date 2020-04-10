using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    
    public void selectLevel(int levelId)
    {
        Debug.Log(levelId);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
