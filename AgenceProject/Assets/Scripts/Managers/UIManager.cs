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
    public Dictionary<int, List<Sprite>> tutorials;

    [Header("Level Select")]
    public Button nextPageButton;
    public Button previousPageButton;
    public Text numberStars;
    public List<GameObject> lockedlevels;
    public GameObject lockPage;
    public Text objectivePage;

    private Button[] buttonLevelSelecter;
    private int numberPagesTotal;
    private int actualPage = 0;
    private bool[] lockedPages;

    [Header("Level Infos")]
    public Text numberLevel;
    public Text starOneCondition;
    public Text starTwoCondition;
    public Text starThreeCondition;
    public GameObject starsImage;
    private int level;

    [Header("Unlock Pages")]
    public GameObject lockPanel;
    public Text objective;


    [Header("Options")]
    public GameObject soundButton;
    public GameObject musicButton;

    [Header("Pause")]
    public Image backgroundPause;
    public GameObject displayPause;
    public GameObject displayReturn;
    public Button retryButton;
    public Button resumeButton;
    public List<Text> listTextShots;

    [Header("In Game")]
    public Text numberShots;
    public Button pauseButton;

    [Header("Results")]
    public GameObject resultsDisplay;
    public Text resultsShots;
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
        numberPagesTotal = Mathf.CeilToInt((SceneManager.sceneCountInBuildSettings - 1) / 8);
        tutorials = new Dictionary<int, List<Sprite>>();
        tutorials.Add(1, dataResults.firstTuto);
    }

    private void Start()
    {
        lockedPages = PlayerData.instance.GetPageLockData();
        if (lockedPages == null)
        {
            lockedPages = new bool[numberPagesTotal];
            PlayerData.instance.pageLock = lockedPages;
            PlayerData.instance.SaveLevelData();
        }
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

    public void OnClickPause(bool display)
    {
        if (display)
        {
            hasClickButton = true;
            GameManager.gameManager.PauseGame();
            DisplayPause();
        }
        else
        {
            UndisplayPause();
            GameManager.gameManager.UnPauseGame();
        }              
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
        menuPause.SetActive(false);
        menuPause.SetActive(true);
    }

    public void OnClickReturnMenu()
    {
        GameManager.gameManager.UnPauseGame();
        GameManager.gameManager.isInGame = false;
        UndisplayPause();
        UnDisplayInGameUI();
        UndisplayLevelResults();
        DisplayLevelSelecter(LevelManager.levelManager.currentLevel);
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
            DisplayLevelSelecter(LevelManager.levelManager.currentLevel);
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
        PlayerData.instance.SaveLevelData();
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

    public void OnClickLanguage(bool display)
    {
        if (display)
        {
            UndisplayOptions();
            DisplayLanguageMenu();
        }
        else
        {
            UndisplayLanguageMenu();
            DisplayOptions();
        }
        
    }

    public void OnClickNextPage()
    {
        if (lockedPages[actualPage])
        {
            actualPage++;
            DisplayLevelSelecter();
        }
        else if (NumberStarsUnlocked(PlayerData.instance.starsNumber) >= GameManager.gameManager.objectivesPages[actualPage])
        {
            lockPanel.SetActive(true);
            objective.text = string.Format("{0}/{1}", NumberStarsUnlocked(PlayerData.instance.starsNumber), GameManager.gameManager.objectivesPages[actualPage]);
        }      
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
        index++;
        previousButton.SetActive(true);                
        if (index == tutorials[levelTuto].Count - 1)
        {
            nextButton.SetActive(false);
            gotItButton.SetActive(true);
        }
        
    }

    public void OnClickPreviousTuto()
    {
        index--;
        tutorialMessage.GetComponent<Image>().sprite = tutorials[levelTuto][index];
        if (index == tutorials[levelTuto].Count - 2)
        {
            gotItButton.SetActive(false);
            nextButton.SetActive(true);
        }        
        else if (index == 0)
        {
            previousButton.SetActive(false);

        }
    }

    public void OnClickUnlockPage(bool key)
    {
        if (key)
        {
            lockedPages[actualPage] = true;
            PlayerData.instance.pageLock = lockedPages;
            PlayerData.instance.SaveLevelData();
        }
        lockPanel.SetActive(false);
        DisplayLevelSelecter();
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
        DisplayNextPageButton(levels);
        DisplayPreviousPageButton();
    }

    public void DisplayLevelSelecter(int level)
    {
        actualPage = Mathf.CeilToInt(level / 8) - 1;
        DisplayLevelSelecter();
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

    private void DisplayNextPageButton(int[] levels)
    {
        if (actualPage + 1 < numberPagesTotal)
        {
            lockPage.SetActive(false);
            nextPageButton.gameObject.SetActive(true);
            
            if (PlayerData.instance.pageLock[actualPage])
            {
                nextPageButton.GetComponent<Image>().sprite = dataResults.unlockedPage;
            }
            else if (NumberStarsUnlocked(levels) >= GameManager.gameManager.objectivesPages[actualPage])
            {
                lockPage.SetActive(true);
                objectivePage.text = string.Format("{0}/{1}", NumberStarsUnlocked(levels), GameManager.gameManager.objectivesPages[actualPage]);
                nextPageButton.GetComponent<Image>().sprite = dataResults.unlockeablePage;
            }
            else
            {
                lockPage.SetActive(true);
                objectivePage.text = string.Format("{0}/{1}", NumberStarsUnlocked(levels), GameManager.gameManager.objectivesPages[actualPage]);
                nextPageButton.GetComponent<Image>().sprite = dataResults.lockedPage;
            }
        }
        else
        {
            lockPage.SetActive(false);
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

    private bool CanUnlockPage()
    {

        return false;
    }

    #endregion

    #region Level Infos

    public void DisplayLevelInfos(int numberLevel)
    {
        if (RulesSystem.GetLevelValueToInt(numberLevel, 1) > 1)
        {
            this.starOneCondition.GetComponent<TextLocaliserUI>().localisedString = "_multipleshotsstargoal";
        }
        else
        {
            this.starOneCondition.GetComponent<TextLocaliserUI>().localisedString = "_oneshotstargoal";
        }
        if (RulesSystem.GetLevelValueToInt(numberLevel, 2) > 1)
        {
            this.starTwoCondition.GetComponent<TextLocaliserUI>().localisedString = "_multipleshotsstargoal";
        }
        else
        {
            this.starTwoCondition.GetComponent<TextLocaliserUI>().localisedString = "_oneshotstargoal";
        }
        if (RulesSystem.GetLevelValueToInt(numberLevel, 3) > 1)
        {
            this.starThreeCondition.GetComponent<TextLocaliserUI>().localisedString = "_multipleshotsstargoal";
        }
        else
        {
            this.starThreeCondition.GetComponent<TextLocaliserUI>().localisedString = "_oneshotstargoal";
        }
        levelInfos.SetActive(true);
        this.numberLevel.text = this.numberLevel.text + " " + numberLevel.ToString();
        starOneCondition.text = starOneCondition.text.Replace("X", RulesSystem.GetLevelValue(numberLevel, 1));
        starTwoCondition.text = starTwoCondition.text.Replace("X", RulesSystem.GetLevelValue(numberLevel, 2));
        starThreeCondition.text = starThreeCondition.text.Replace("X", RulesSystem.GetLevelValue(numberLevel, 3));
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
        LocalisationNumberShots();
        displayPause.SetActive(true);
        DisplayNumberShots();
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

    private void LocalisationNumberShots()
    {
        if (LevelManager.levelManager.level.shotStarOne > 1)
        {
            listTextShots[0].GetComponent<TextLocaliserUI>().localisedString = "_multipleshotsstargoal";
        }
        else
        {
            listTextShots[0].GetComponent<TextLocaliserUI>().localisedString = "_oneshotstargoal";
        }
        if (LevelManager.levelManager.level.shotStarTwo > 1)
        {
            listTextShots[1].GetComponent<TextLocaliserUI>().localisedString = "_multipleshotsstargoal";
        }
        else
        {
            listTextShots[1].GetComponent<TextLocaliserUI>().localisedString = "_oneshotstargoal";
        }
        if (LevelManager.levelManager.level.shotStarThree > 1)
        {
            listTextShots[2].GetComponent<TextLocaliserUI>().localisedString = "_multipleshotsstargoal";
        }
        else
        {
            listTextShots[2].GetComponent<TextLocaliserUI>().localisedString = "_oneshotstargoal";
        }
    }

    private void DisplayNumberShots()
    {
        listTextShots[0].text = listTextShots[0].text.Replace("X", LevelManager.levelManager.level.shotStarOne.ToString());
        listTextShots[1].text = listTextShots[1].text.Replace("X", LevelManager.levelManager.level.shotStarTwo.ToString());
        listTextShots[2].text = listTextShots[2].text.Replace("X", LevelManager.levelManager.level.shotStarThree.ToString());
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
        int index = SceneManager.GetActiveScene().buildIndex;


        if (hasWin)
        {
            if (index % 8 == 0)
            {
                if (PlayerData.instance.pageLock[index / 8 - 1])
                {
                    victoryButtonNext.SetActive(true);
                }
            }
            else
            {
                victoryButtonNext.SetActive(true);
            }
            textResults.GetComponent<TextLocaliserUI>().localisedString = "_victory";
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
            textResults.GetComponent<TextLocaliserUI>().localisedString = "_defeat";
            imageStarsResults.sprite = dataResults.DefeatZeroStar;
            LevelManager.levelManager.starsObtained = 0;
        }
        if (GameManager.gameManager.GetShootDone() > 1)
        {
            resultsShots.GetComponent<TextLocaliserUI>().localisedString = "_resultmultipleshots";
        }
        else
        {
            resultsShots.GetComponent<TextLocaliserUI>().localisedString = "_resultoneshot";
        }
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            victoryButtonNext.SetActive(false);
        }

        if (PlayerData.instance != null)
            PlayerData.instance.SaveLevelData();
        resultsDisplay.SetActive(true);
        resultsShots.text = resultsShots.text.Replace("X", GameManager.gameManager.GetShootDone().ToString());
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

}
