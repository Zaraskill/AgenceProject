using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager uiManager;

    [Header("CanvasMenu")]
    public GameObject mainMenu;
    public GameObject levelMenu;
    public GameObject optionsMenu;
    public GameObject menuPause;
    public GameObject inGameUI;



    private void Awake()
    {
        if (uiManager != null)
        {
            Debug.LogError("Too many instances!");
        }
        else
        {
            uiManager = this;
            //DontDestroyOnLoad(this.gameObject);
        }
    } 

    #region Button Fonctions

    public void OnClickOptions()
    {
        UndisplayMainMenu();
        DisplayOptions();
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }

    public void OnClickPlay()
    {
        UndisplayMainMenu();
        DisplayLevelSelecter();
    }

    public void OnClickLevelone()
    {
        SceneManager.LoadScene("LevelOne_Exemple");
    }

    public void OnClickReturnOptions()
    {
        UndisplayOptions();
        DisplayMainMenu();
    }

    public void OnClickReturnLevelSelect()
    {
        UndisplayLevelSelecter();
        DisplayMainMenu();
    }

    public void OnClickPause()
    {
        GameManager.gameManager.PauseGame();
        UnDisplayInGameUI();
        DisplayPause();
    }

    public void OnClickResume()
    {
        UndisplayPause();
        DisplayInGameUI();
        GameManager.gameManager.UnPauseGame();
    }

    public void OnClickRetry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnClickReturnPause()
    {
        GameManager.gameManager.UnPauseGame();
        SceneManager.LoadScene("MenuScene_Exemple");
    }

    #endregion

    #region Display Fonctions

    //Options
    private void DisplayOptions()
    {
        optionsMenu.SetActive(true);
    }

    private void UndisplayOptions()
    {
        optionsMenu.SetActive(false);
    }

    //Main Menu
    public void DisplayMainMenu()
    {
        mainMenu.SetActive(true);
    }

    private void UndisplayMainMenu()
    {
        mainMenu.SetActive(false);
    }

    //Level Select
    private void DisplayLevelSelecter()
    {
        levelMenu.SetActive(true);
    }

    private void UndisplayLevelSelecter()
    {
        levelMenu.SetActive(false);
    }
    
    //Pause
    private void DisplayPause()
    {
        menuPause.SetActive(true);
    }

    private void UndisplayPause()
    {
        menuPause.SetActive(false);
    }

    //InGame UI
    private void DisplayInGameUI()
    {
        inGameUI.SetActive(true);
    }
    
    private void UnDisplayInGameUI()
    {
        inGameUI.SetActive(false);
    }

    //Level Results
    public void DisplayLevelResults(bool hasWin)
    {
        if (hasWin)
        {
            
        }
        else
        {

        }
    }

    #endregion
}
