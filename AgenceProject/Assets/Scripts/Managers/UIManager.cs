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
    public GameObject levelsPlayable;
    public GameObject levelInfos;
    public GameObject optionsMenu;
    public GameObject languageMenu;
    public GameObject menuPause;
    public GameObject inGameUI;
    public GameObject tutorialMessage;

    private Button[] buttonLevelSelecter;
    private int numberPagesTotal;
    private int actualPage = 0;

    [Header("Tutorial")]
    public GameObject nextButton;
    public GameObject previousButton;
    public GameObject gotItButton;
    private int levelTuto;
    private int index = 0;
    private Dictionary<int, List<Sprite>> tutorials;

    [Header("Level Select")]
    public Button nextPageButton;
    public Button previousPageButton;

    [Header("Level Infos")]
    public Text numberLevel;
    public Text starOneCondition;
    public Text starTwoCondition;
    public Text starThreeCondition;
    public GameObject starsImage;
    private int level;

    [Header("Pause")]
    public GameObject displayPause;
    public GameObject displayReturn;

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
        numberPagesTotal = (int) Mathf.Ceil((SceneManager.sceneCountInBuildSettings - 1) / 8);
        gameObject.SetActive(true);
        tutorials = new Dictionary<int, List<Sprite>>();
        tutorials.Add(1, dataResults.firstTuto);
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
        int[] levels = PlayerData.instance.starsNumber;

        if (levelSelected + (8 * actualPage) == 1 || levels[levelSelected + (8 * actualPage) - 1] != 0)
        {
            DisplayLevelInfos(levelSelected + (8 * actualPage));
            level = levelSelected + (8 * actualPage);
        }
        else if ( levels[levelSelected + (8 * actualPage) - 1] == 0)
        {
            if (levels[levelSelected + (8 * actualPage) - 2] != 0)
            {
                DisplayLevelInfos(levelSelected + (8 * actualPage));
                level = levelSelected + (8 * actualPage);
            }
                
        }
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
        DisplayPause();        
    }

    public void OnClickResume()
    {
        UndisplayPause();
        GameManager.gameManager.UnPauseGame();
    }

    public void OnClickRetry()
    {
        Time.timeScale = 1f;
        LevelLoader.instance.LoadLevel(SceneManager.GetActiveScene().buildIndex);
        LevelManager.levelManager.numberRetry++;
    }

    public void OnClickReturnPause()
    {
        displayPause.SetActive(false);
        displayReturn.SetActive(true);
    }

    public void OnClickReturnMenu()
    {
        GameManager.gameManager.UnPauseGame();
        GameManager.gameManager.isInGame = false;
        UndisplayPause();
        UnDisplayInGameUI();
        UndisplayLevelResults();
        DisplayMainMenu();
        LevelLoader.instance.LoadLevel(0);
    }

    public void OnClickValidateReturn()
    {
        GameManager.gameManager.UnPauseGame();
        GameManager.gameManager.isInGame = false;
        UndisplayPause();
        UnDisplayInGameUI();
        UndisplayLevelResults();
        DisplayMainMenu();
        LevelLoader.instance.LoadLevel(0);
    }

    public void OnClickUnvalidateReturn()
    {
        displayReturn.SetActive(false);
        displayPause.SetActive(true);
    }

    public void OnClickNext()
    {
        Time.timeScale = 1f;
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
        StatsUI.instance.LoadLogs();
        StatsUI.instance.uiCanvas = this.gameObject;
    }

    public void OnClickSwitchLanguage(string key)
    {
        LocalisationSystem.SwitchLanguage(key);
        languageMenu.SetActive(false);
        languageMenu.SetActive(true);
    }

    public void OnClickCutMusic()
    {

    }

    public void OnClickCutSound()
    {

    }

    public void OnClickLanguage()
    {
        UndisplayOptions();
        DisplayLanguageMenu();
    }

    public void OnClickReturnLang()
    {
        UndisplayLanguageMenu();
        DisplayOptions();
    }

    public void OnClickNextPage()
    {
        actualPage++;
        DisplayLevelSelecter();
    }

    public void OnClickPreviousPage()
    {
        actualPage--;
        DisplayLevelSelecter();
    }

    //A modifier pour automatiser/////////////
    public void OnClickEndTuto()
    {
        UndisplayTutorial();
        DisplayInGameUI();
        GameManager.gameManager.DeactivateTuto();
    }

    public void OnClickNextTuto()
    {
        tutorialMessage.GetComponent<Image>().sprite = tutorials[levelTuto][index + 1];
        if (index == 0)
        {
            previousButton.SetActive(true);
        }
        index++;
        if (index == tutorials[levelTuto].Count - 1)
        {
            nextButton.SetActive(false);
            gotItButton.SetActive(true);
        }
        
    }

    public void OnClickPreviousTuto()
    {
        tutorialMessage.GetComponent<Image>().sprite = tutorials[levelTuto][index - 1];
        if (index == tutorials[levelTuto].Count - 1)
        {
            gotItButton.SetActive(false);
            nextButton.SetActive(true);
        }
        index--;
        if (index == 0)
        {
            previousButton.SetActive(false);

        }
    }
    ////////////////////////

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

        buttonLevelSelecter = levelsPlayable.GetComponentsInChildren<Button>();
        int[] levels = PlayerData.instance.starsNumber;
        int index;

        for (index = 8*actualPage; index < 8 * (actualPage + 1); index++)
        {
            if (index > (SceneManager.sceneCountInBuildSettings - 1))
            {
                buttonLevelSelecter[index%8].gameObject.SetActive(false);
                continue;
            }
            else
            {
                buttonLevelSelecter[index%8].gameObject.SetActive(true);
                buttonLevelSelecter[index%8].GetComponentInChildren<Text>().text = (index+1).ToString();
            }
            buttonLevelSelecter[index%8].GetComponent<Image>().sprite = dataResults.UnlockedLevel;
            if(index > 0)
            {
                if (levels[index] == 0 && levels[index - 1] == 0)
                {
                    buttonLevelSelecter[index%8].GetComponent<Image>().sprite = dataResults.LockedLevel;
                }
            }
            DisplayNumberStars(index, buttonLevelSelecter[index%8].gameObject);
        }
        DisplayNextPageButton();
        DisplayPreviousPageButton();
    }

    private void DisplayNextPageButton()
    {
        if (actualPage + 1 < numberPagesTotal)
        {
            nextPageButton.gameObject.SetActive(true);
        }
        else
        {
            nextPageButton.gameObject.SetActive(false);
        }
    }

    private void DisplayPreviousPageButton()
    {
        if (actualPage > 0)
        {
            previousPageButton.gameObject.SetActive(true);
        }
        else
        {
            previousPageButton.gameObject.SetActive(false);
        }
    }

    private void DisplayNumberStars(int level, GameObject targetDisplay)
    {
        Image[] sprites = targetDisplay.GetComponentsInChildren<Image>();
        int stars = PlayerData.instance.starsNumber[level];
        switch (stars)
        {
            case 1:
                sprites[1].sprite = dataResults.VictoryOneStar;
                sprites[2].sprite = dataResults.DefeatZeroStar;
                sprites[3].sprite = dataResults.DefeatZeroStar;
                break;
            case 2:
                sprites[1].sprite = dataResults.VictoryOneStar;
                sprites[2].sprite = dataResults.VictoryTwoStar;
                sprites[3].sprite = dataResults.DefeatZeroStar;
                break;
            case 3:
                sprites[1].sprite = dataResults.VictoryOneStar;
                sprites[2].sprite = dataResults.VictoryTwoStar;
                sprites[3].sprite = dataResults.VictoryThreeStar;
                break;
            default:
                sprites[1].sprite = dataResults.DefeatZeroStar;
                sprites[2].sprite = dataResults.DefeatZeroStar;
                sprites[3].sprite = dataResults.DefeatZeroStar;
                break;
        }
        
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
        this.starOneCondition.text = RulesSystem.GetLevelValue(numberLevel, 1);
        this.starTwoCondition.text = RulesSystem.GetLevelValue(numberLevel, 2);
        this.starThreeCondition.text = RulesSystem.GetLevelValue(numberLevel, 3);
        DisplayNumberStars(numberLevel - 1, starsImage);
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
            LevelManager.levelManager.starsObtained = starsUnlocked;
            switch (starsUnlocked)
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
            LevelManager.levelManager.starsObtained = 0;
        }
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            victoryButtonNext.SetActive(false);
        }

        if (PlayerData.instance != null)
            PlayerData.instance.SaveLevelData();
    }

    public void UndisplayLevelResults()
    {
        victoryButtonNext.SetActive(false);
        resultsDisplay.SetActive(false);
    }

    //Language
    public void DisplayLanguageMenu()
    {
        languageMenu.SetActive(true);
    }

    public void UndisplayLanguageMenu()
    {
        languageMenu.SetActive(false);
    }

    #endregion

    #region Tutorial Fonctions

    public void DisplayTutorial(int level)
    {
        index = 0;
        levelTuto = level;
        tutorialMessage.SetActive(true);
        tutorialMessage.GetComponent<Image>().sprite = tutorials[level][0];
    }

    public void UndisplayTutorial()
    {
        previousButton.SetActive(false);
        gotItButton.SetActive(false);
        nextButton.SetActive(true);
        tutorialMessage.SetActive(false);
        index = 0;
    }

    #endregion
}
