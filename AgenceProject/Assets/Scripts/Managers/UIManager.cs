using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager uiManager;

    public static bool hasClickButton = false;

    [Header("CanvasMenu")]
    public GameObject mainMenu;
    public GameObject levelMenu;
    public GameObject levelsPlayable;
    public GameObject levelInfos;
    public GameObject optionsMenu;
    public GameObject creditsMenu;
    public GameObject languageMenu;
    public GameObject menuPause;
    public GameObject inGameUI;
    public GameObject tutorialMessage;

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
    public Text numberStars;
    public List<GameObject> lockedlevels;

    private Button[] buttonLevelSelecter;
    private int numberPagesTotal;
    private int actualPage = 0;

    [Header("Level Infos")]
    public Text numberLevel;
    public Text starOneCondition;
    public Text starTwoCondition;
    public Text starThreeCondition;
    public GameObject starsImage;
    private int level;

    [Header("Options")]
    public GameObject soundButton;
    public GameObject musicButton;

    [Header("Pause")]
    public Image backgroundPause;
    public TextLocaliserUI textPause;
    public GameObject displayPause;
    public GameObject displayReturn;
    public Button retryButton;
    public Button resumeButton;

    [Header("In Game")]
    public Text numberShots;
    public Button pauseButton;

    [Header("Results")]
    public GameObject resultsDisplay;
    public Text textResults;
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

    public void OnClickOptions(bool display)
    {
        if (display)
        {
            UndisplayMainMenu();
            DisplayOptions();
        }
        else
        {
            UndisplayOptions();
            DisplayMainMenu();
        }        
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }

    public void OnClickCredits(bool display)
    {
        if (display)
        {
            UndisplayMainMenu();
            DisplayCredits();
        }
        else
        {
            UndisplayCredits();
            DisplayMainMenu();
        }
    }

    public void OnClickPlay(bool display)
    {
        if (display)
        {
            UndisplayMainMenu();
            DisplayLevelSelecter();
        }
        else
        {
            UndisplayLevelSelecter();
            DisplayMainMenu();
        }
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

    public void OnClickPause()
    {
        hasClickButton = true;
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
        UnDisplayInGameUI();
        LevelLoader.instance.LoadLevel(SceneManager.GetActiveScene().buildIndex);
        LevelManager.levelManager.numberRetry++;
    }

    public void OnClickReturnPause()
    {
        backgroundPause.sprite = dataResults.pauseNoStar;
        retryButton.interactable = false;
        resumeButton.interactable = false;
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

    public void OnClickValidateReturn(bool back)
    {
        displayReturn.SetActive(false);
        displayPause.SetActive(true);
        retryButton.interactable = true;
        resumeButton.interactable = true;
        if (back)
        {
            GameManager.gameManager.UnPauseGame();
            GameManager.gameManager.isInGame = false;
            UndisplayPause();
            UnDisplayInGameUI();
            UndisplayLevelResults();
            DisplayMainMenu();
            LevelLoader.instance.LoadLevel(0);
        }     
        else
        {
            DisplayPause();
        }
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
        GameManager.hasMusicCut = !GameManager.hasMusicCut;
        AudioManager.instance.CutMusic(GameManager.hasMusicCut);
        if (!GameManager.hasMusicCut)
        {
            musicButton.GetComponent<Image>().sprite = dataResults.ActivatedMusic;
        }
        else
        {
            musicButton.GetComponent<Image>().sprite = dataResults.DeactivatedMusic;
        }
    }

    public void OnClickCutSound()
    {
        GameManager.hasSoundCut = !GameManager.hasSoundCut;
        AudioManager.instance.CutSound(GameManager.hasSoundCut);
        if (!GameManager.hasSoundCut)
        {
            soundButton.GetComponent<Image>().sprite = dataResults.ActivatedSound;
        }
        else
        {
            soundButton.GetComponent<Image>().sprite = dataResults.DeactivatedSound;
        }
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

#endregion

#region Display UI Fonctions

    #region Options

    private void DisplayOptions()
    {
        optionsMenu.SetActive(true);
    }

    private void UndisplayOptions()
    {
        optionsMenu.SetActive(false);
    }

    #endregion

    #region Credits

    private void DisplayCredits()
    {
        creditsMenu.SetActive(true);
    }

    private void UndisplayCredits()
    {
        creditsMenu.SetActive(false);
    }

    #endregion

    #region Main Menu

    public void DisplayMainMenu()
    {
        mainMenu.SetActive(true);
    }

    private void UndisplayMainMenu()
    {
        mainMenu.SetActive(false);
    }

    #endregion

    #region Level Select

    public void DisplayLevelSelecter()
    {
        levelMenu.SetActive(true);
        buttonLevelSelecter = levelsPlayable.GetComponentsInChildren<Button>();
        int[] levels = PlayerData.instance.starsNumber;
        numberStars.text = NumberStarsUnlocked(levels).ToString();
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
            switch (levels[index])
            {
                case 1:
                    buttonLevelSelecter[index % 8].GetComponent<Image>().sprite = dataResults.UnlockedLevelOneStar;
                    break;
                case 2:
                    buttonLevelSelecter[index % 8].GetComponent<Image>().sprite = dataResults.UnlockedLevelTwoStar;
                    break;
                case 3:
                    buttonLevelSelecter[index % 8].GetComponent<Image>().sprite = dataResults.UnlockedLevelThreeStar;
                    break;
                default:
                    buttonLevelSelecter[index % 8].GetComponent<Image>().sprite = dataResults.UnlockedLevelNoStar;
                    break;
            }            
            if (index != 0 && levels[index] == 0 && levels[index - 1] == 0)
            {
                lockedlevels[index % 8].SetActive(true);
            }
            else
            {
                lockedlevels[index % 8].SetActive(false);
            }
        }
        DisplayNextPageButton();
        DisplayPreviousPageButton();
    }

    private int NumberStarsUnlocked(int[] starsLevel)
    {
        int stars = 0;
        for (int index = 0; index < starsLevel.Length; index++)
        {
            stars += starsLevel[index];
        }
        return stars;
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
        Image sprites = targetDisplay.GetComponent<Image>();
        int stars = PlayerData.instance.starsNumber[level];
        switch (stars)
        {
            case 1:
                sprites.sprite = dataResults.oneStar;
                break;
            case 2:
                sprites.sprite = dataResults.twoStar;
                break;
            case 3:
                sprites.sprite = dataResults.threeStar;
                break;
            default:
                sprites.sprite = dataResults.zeroStar;
                break;
        }
        
    }

    public void UndisplayLevelSelecter()
    {
        levelMenu.SetActive(false);
    }

    #endregion

    #region Level Infos

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

    #endregion

    #region Pause

    public void DisplayPause()
    {
        menuPause.SetActive(true);
        displayReturn.SetActive(false);
        displayPause.SetActive(true);
        pauseButton.gameObject.SetActive(false);
        switch (PlayerData.instance.starsNumber[level - 1])
        {
            case 1:
                backgroundPause.sprite = dataResults.pauseOneStar;
                break;
            case 2:
                backgroundPause.sprite = dataResults.pauseTwoStar;
                break;
            case 3:
                backgroundPause.sprite = dataResults.pauseThreeStar;
                break;
            default:
                backgroundPause.sprite = dataResults.pauseZeroStar;
                break;
        }

    }

    public void UndisplayPause()
    {
        menuPause.SetActive(false);
        pauseButton.gameObject.SetActive(true);
    }

    #endregion

    #region In Game

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

    #endregion

    #region Level Results

    public void DisplayLevelResults(bool hasWin, int starsUnlocked)
    {
        UnDisplayInGameUI();
        resultsDisplay.SetActive(true);
        if (hasWin)
        {
            victoryButtonNext.SetActive(true);
            textResults.text = "Victory";
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
            textResults.text = "Defeat";
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

    #endregion

    #region Language

    public void DisplayLanguageMenu()
    {
        languageMenu.SetActive(true);
    }

    public void UndisplayLanguageMenu()
    {
        languageMenu.SetActive(false);
    }

    #endregion

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

    //public bool HasMouseOverButton()
    //{
    //    Button[] listButton = GetComponentsInChildren<Button>();
    //    foreach(Button button in listButton)
    //    {
    //        if (button.IsActive())
    //        {
    //            if ()
    //            {
    //                return true;
    //            }
    //        }
    //    }
    //    return false;
    //}
}
