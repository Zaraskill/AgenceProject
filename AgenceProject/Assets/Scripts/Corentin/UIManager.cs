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
    public GameObject levelInfos;
    public GameObject optionsMenu;
    public GameObject menuPause;
    public GameObject inGameUI;
    public GameObject tutorialMessage;

    private Button[] buttonLevelSelecter;

    [Header("Level Infos")]
    public Text numberLevel;
    public Text starOneCondition;
    public Text starTwoCondition;
    public Text starThreeCondition;
    private int level;

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
        if (uiManager == null || uiManager == this)
        {
            uiManager = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
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

    public void OnClickLevel(int  levelSelected)
    {
        DisplayLevelInfos(levelSelected);
        level = levelSelected;
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
        UnDisplayInGameUI();
        DisplayPause();
        GameManager.gameManager.PauseGame();
    }

    public void OnClickResume()
    {
        UndisplayPause();
        DisplayInGameUI();
        GameManager.gameManager.UnPauseGame();
    }

    public void OnClickRetry()
    {
        LevelLoader.instance.LoadLevel(SceneManager.GetActiveScene().buildIndex);        
    }

    public void OnClickReturnPause()
    {
        GameManager.gameManager.UnPauseGame();
        GameManager.gameManager.isInGame = false;
        UndisplayPause();
        UndisplayLevelResults();
        DisplayMainMenu();
        LevelLoader.instance.LoadLevel(0);
    }

    public void OnClickNext()
    {
        LevelLoader.instance.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnClickReturnInfos()
    {
        UndisplayLevelInfos();
        level = 0;
    }

    public void OnClickStartLevel()
    {
        UndisplayLevelInfos();
        UndisplayLevelSelecter();
        LevelLoader.instance.LoadLevel(level);
    }

    public void OnClickStat()
    {
        SceneManager.LoadScene("Stats");
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
    public void DisplayLevelSelecter()
    {
        levelMenu.SetActive(true);
        //SaveData data = SaveSystem.LoadDataFile();
        //buttonLevelSelecter = levelMenu.GetComponentsInChildren<Button>();
        //int index;
        //for (index = 0; index < buttonLevelSelecter.Length; index++)
        //{
        //    if (index <= data.LevelsDatas.Length)
        //    {
        //        buttonLevelSelecter[index].GetComponent<Image>().sprite = dataResults.UnlockedLevel;
        //    }
        //    else
        //    {
        //        buttonLevelSelecter[index].GetComponent<Image>().sprite = dataResults.LockedLevel;
        //    }
        //}
    }

    public void UndisplayLevelSelecter()
    {
        levelMenu.SetActive(false);
    }
    
    //Level Infos
    public void DisplayLevelInfos(int numberLevel)
    {
        levelInfos.SetActive(true);
        this.numberLevel.text = numberLevel.ToString();
        this.starOneCondition.text = GameManager.GetLevelValue(numberLevel, 1);
        this.starTwoCondition.text = GameManager.GetLevelValue(numberLevel, 2);
        this.starThreeCondition.text = GameManager.GetLevelValue(numberLevel, 3);
    }

    public void UndisplayLevelInfos()
    {
        levelInfos.SetActive(false);
    }

    //Pause
    public void DisplayPause()
    {
        menuPause.SetActive(true);
    }

    public void UndisplayPause()
    {
        menuPause.SetActive(false);
    }

    //InGame UI
    public void DisplayInGameUI()
    {
        inGameUI.SetActive(true);
    }
    
    public void UnDisplayInGameUI()
    {
        inGameUI.SetActive(false);
    }

    public void UpdateShots(int shots)
    {
        numberShots.text = " " + shots + " ";
    }

    //Level Results
    public void DisplayLevelResults(bool hasWin, int starsUnlocked)
    {
        UnDisplayInGameUI();
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
