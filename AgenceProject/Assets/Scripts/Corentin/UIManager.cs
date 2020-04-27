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
    public GameObject tutorialMessage;

    [Header("In Game")]
    public Text numberShots;
    //Results
    [Header("Results")]
    public GameObject resultsDisplay;
    public Image imageTextResults;
    public Image imageStarsResults;
    public GameObject victoryButtonNext;


    public FlexibleUIData dataResults;



    private void Awake()
    {
        if (uiManager == null)
        {
            uiManager = this;
            DontDestroyOnLoad(this.gameObject);
        }
        gameObject.SetActive(true);
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
        UndisplayPause();
        UndisplayLevelResults();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnClickReturnPause()
    {
        GameManager.gameManager.UnPauseGame();
        SceneManager.LoadScene("MenuScene_Exemple");
    }

    #endregion

    #region Display UI Fonctions

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
    public void DisplayLevelResults(bool hasWin, int starsUnlocked)
    {
        resultsDisplay.SetActive(true);
        if (hasWin)
        {
            victoryButtonNext.SetActive(true);
            imageTextResults.sprite = dataResults.VictoryText;
            switch(starsUnlocked)
            {
                case 1:
                    imageStarsResults.sprite = dataResults.VictoryOneStar;
                    break;
                case 2:
                    imageStarsResults.sprite = dataResults.VictoryTwoStar;
                    break;
                case 3:
                    imageStarsResults.sprite = dataResults.VictoryThreeStar;
                    break;
                default:
                    break;
            }
        }
        else
        {
            imageTextResults.sprite = dataResults.DefeatText;
            imageStarsResults.sprite = dataResults.DefeatZeroStar;
        }
    }

    public void UndisplayLevelResults()
    {
        victoryButtonNext.SetActive(false);
        resultsDisplay.SetActive(false);
    }

    //In game
    public void UpdateShots(int shots)
    {
        Debug.Log("text");
        numberShots.text = " " + shots + " ";
    }

    #endregion

    #region Tutorial Fonctions

    public void DisplayTutorial()
    {
        tutorialMessage.SetActive(true);
    }

    public void UndisplayTutorial()
    {
        tutorialMessage.SetActive(false);
    }

    #endregion
}
