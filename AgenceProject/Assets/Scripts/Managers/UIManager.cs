﻿using System.Collections;
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
    public GameObject menuPause;
    public GameObject inGameUI;
    public GameObject statsMenu;
    public GameObject tutorialMessage;

    [Header("Main menu")]
    public GameObject wallpaper;

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
    private bool isDisplayInfos;

    [Header("Unlock Pages")]
    public Text objective;
    public Text warning;

    [Header("Options")]
    public GameObject soundButton;
    public GameObject musicButton;
    public Toggle graphicsSettings;
    private string options;

    [Header("Pause")]
    public Image backgroundPause;
    public GameObject displayPause;
    public GameObject displayReturn;
    public Button retryButton;
    public Button resumeButton;
    public GameObject pauseSoundButton;
    public GameObject pauseMusicButton;
    public List<Text> listTextShots;

    [Header("In Game")]
    public Text numberShots;
    public Button pauseButton;

    [Header("Results")]
    public Text resultsShots;
    public Text textResults;
    public Image imageStarsResults;
    public GameObject victoryButtonNext;
    public GameObject homeButton;
    public GameObject restartButton;
    public List<Image> stars;
    private int starsObtained;

    [Header("Stats")]
    public GameObject statContent;
    public Text statTotalTime;
    public Text statTotalRetry;

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
    }

    private void Start()
    {
        lockedPages = PlayerData.instance.GetPageLockData();
        if (lockedPages.Length == 0)
        {
            lockedPages = new bool[numberPagesTotal + 1];
            PlayerData.instance.pageLock = lockedPages;
            PlayerData.instance.SaveLevelData();
        }
        wallpaper.SetActive(true);
    }

#region Button Fonctions

    public void OnClickOptions()
    {
        UndisplayMainMenu();
        DisplayOptions();
        AudioManager.instance.Play("SFX_UI_Positif", false);
    }

    public void OnClickReturnOpt()
    {
        if (options == "opt")
        {
            UndisplayOptions();
            DisplayMainMenu();
        }
        else if (options == "lang")
        {
            UndisplayLanguageMenu();
            LanguageToOptions();
        }
        AudioManager.instance.Play("SFX_UI_Back", false);
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
            AudioManager.instance.Play("SFX_UI_Positif", false);
        }
        else
        {
            UndisplayCredits();
            DisplayMainMenu();
            AudioManager.instance.Play("SFX_UI_Back", false);
        }
    }

    public void OnClickPlay(bool display)
    {
        if (display)
        {
            UndisplayMainMenu();
            DisplayLevelSelecter();
            wallpaper.SetActive(false);
            AudioManager.instance.Play("SFX_UI_Validate", false);
        }
        else
        {
            UndisplayLevelSelecter();
            wallpaper.SetActive(true);
            DisplayMainMenu();
            AudioManager.instance.Play("SFX_UI_Back", false);
        }
    }

    public void OnClickLevel(int  levelSelected)
    {
        isDisplayInfos = true;
        int[] levels = PlayerData.instance.starsNumber;

        if (levelSelected + (8 * actualPage) == SceneManager.sceneCountInBuildSettings - 1 && !PlayerData.instance.pageLock[PlayerData.instance.pageLock.Length - 1])
        {
            DisplayUnlockPanel();
            objective.text = string.Format("{0}/{1}", NumberStarsUnlocked(PlayerData.instance.starsNumber), GameManager.gameManager.objectiveFinalLevel);
        }
        else if (levelSelected + (8 * actualPage) == 1 || levels[levelSelected + (8 * actualPage) - 1] != 0)
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
        AudioManager.instance.Play("SFX_UI_Positif", false);
    }

    public void OnClickPause(bool display)
    {
        if (display)
        {
            GameManager.gameManager.PauseGame();
            hasClickButton = true;            
            DisplayPause();
            AudioManager.instance.Play("SFX_UI_Positif", false);
        }
        else
        {
            GameManager.gameManager.UnPauseGame();
            UndisplayPause();
            AudioManager.instance.Play("SFX_UI_Back", false);
        }
    }

    public void OnClickRetry()
    {
        Time.timeScale = 1f;
        UnDisplayInGameUI();
        LevelLoader.instance.LoadLevel(SceneManager.GetActiveScene().buildIndex);
        LevelManager.levelManager.Retry();
        AudioManager.instance.Play("SFX_UI_Restart", false);
    }

    public void OnClickReturnPause()
    {
        retryButton.interactable = false;
        resumeButton.interactable = false;
        TweenManager.tweenManager.Play("outroDisplay");
        TweenManager.tweenManager.Play("introValidationReturn");
        AudioManager.instance.Play("SFX_UI_Back", false);
    }

    public void OnClickReturnMenu()
    {
        GameManager.gameManager.UnPauseGame();
        UndisplayPause();
        UnDisplayInGameUI();
        UndisplayLevelResults();
        DisplayLevelSelecter(LevelManager.levelManager.currentLevel);
        LevelLoader.instance.LoadLevel(0);
        AudioManager.instance.Play("SFX_UI_Negatif", false);

    }

    public void OnClickValidateReturn(bool back)
    {
        TweenManager.tweenManager.Play("outroValidationReturn");       
        retryButton.interactable = true;
        resumeButton.interactable = true;
        if (back)
        {
            PlayerData.instance.SaveLevelData();
            GameManager.gameManager.UnPauseGame();
            UndisplayPause();
            UnDisplayInGameUI();
            UndisplayLevelResults();
            UndisplayTutorial();
            DisplayLevelSelecter(LevelManager.levelManager.currentLevel);
            LevelLoader.instance.LoadLevel(0);
        }     
        else
        {
            TweenManager.tweenManager.Play("introDisplay");
        }
        AudioManager.instance.Play("SFX_UI_Positif", false);
    }

    public void OnClickNext()
    {
        if (SceneManager.GetActiveScene().buildIndex % 8 == 0)
        {
            if (PlayerData.instance.pageLock[SceneManager.GetActiveScene().buildIndex / 8 - 1])
            {
                Time.timeScale = 1f;
                UndisplayLevelResults();
                LevelLoader.instance.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                DisplayUnlockPanel();
                objective.text = string.Format("{0}/{1}", NumberStarsUnlocked(PlayerData.instance.starsNumber), GameManager.gameManager.objectivesPages[SceneManager.GetActiveScene().buildIndex / 8 - 1]);
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 2)
        {
            DisplayUnlockPanel();
            objective.text = string.Format("{0}/{1}", NumberStarsUnlocked(PlayerData.instance.starsNumber), GameManager.gameManager.objectiveFinalLevel);
        }
        else
        {
            Time.timeScale = 1f;
            LevelLoader.instance.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
        }
        AudioManager.instance.Play("SFX_UI_Positif", false);
    }

    public void OnClickReturnInfos()
    {
        UndisplayLevelInfos();
        isDisplayInfos = false;
        level = 0;
        AudioManager.instance.Play("SFX_UI_Back", false);
    }

    public void OnClickStartLevel()
    {
        UndisplayLevelInfos();
        UndisplayLevelSelecter();
        isDisplayInfos = false;
        LevelLoader.instance.LoadLevel(level);
        AudioManager.instance.Play("SFX_UI_Validate", false);
    }

    public void OnClickStat(bool key)
    {
        if (key)
        {
            UndisplayMainMenu();
            DisplayStats();
            PlayerData.instance.LoadLevelData();
            PlayerData.instance.UpdateTextContent(statContent);
            statTotalTime.text = "Total Time : " + PlayerData.instance.CalculTotalTime();
            statTotalRetry.text = "Total Retry : " + PlayerData.instance.CalculTotalRetry();
            AudioManager.instance.Play("SFX_UI_Positif", false);
        }
        else
        {
            UndisplayStats();
            DisplayMainMenu();
            AudioManager.instance.Play("SFX_UI_Back", false);
        }
    }

    public void OnClickDropData()
    {
        DisplayDropData();
    }

    public void OnClickSwitchLanguage(string key)
    {
        LocalisationSystem.SwitchLanguage(key);
        PlayerData.instance.SaveLevelData();
        AudioManager.instance.Play("SFX_UI_Positif", false);
    }

    public void OnClickCutMusic()
    {
        PlayerData.instance.parameter[2] = !PlayerData.instance.parameter[2];
        AudioManager.instance.CutMusic(PlayerData.instance.parameter[2]);
        RefreshToggleMusic();
        PlayerData.instance.SaveLevelData();
        if (PlayerData.instance.parameter[2])
            AudioManager.instance.Play("SFX_UI_Positif", false);
        else
            AudioManager.instance.Play("SFX_UI_Negatif", false);
    }    

    public void OnClickCutSound()
    {
        PlayerData.instance.parameter[3] = !PlayerData.instance.parameter[3];
        AudioManager.instance.CutSound(PlayerData.instance.parameter[3]);
        RefreshToggleSound();
        PlayerData.instance.SaveLevelData();

        if (PlayerData.instance.parameter[3])
            AudioManager.instance.Play("SFX_UI_Positif", false);
        else
            AudioManager.instance.Play("SFX_UI_Negatif", false);
    }    

    public void ToggleGraphics()
    {
        ApplicationSettings.ToggleGraphicSettings();
        PlayerData.instance.SaveLevelData();

        if (PlayerData.instance.parameter[0])
            AudioManager.instance.Play("SFX_UI_Positif", false);
        else
            AudioManager.instance.Play("SFX_UI_Negatif", false);
    }
    
    public void OnClickLanguage()
    {
        OptionsToLanguage();
        DisplayLanguageMenu();
        AudioManager.instance.Play("SFX_UI_Positif", false);
    }

    public void OnClickNextPage()
    {
        if (PlayerData.instance.pageLock[actualPage])
        {
            actualPage++;
            DisplayLevelSelecter();
        }
        else if (NumberStarsUnlocked(PlayerData.instance.starsNumber) >= GameManager.gameManager.objectivesPages[actualPage])
        {
            DisplayUnlockPanel();
            objective.text = string.Format("{0}/{1}", NumberStarsUnlocked(PlayerData.instance.starsNumber), GameManager.gameManager.objectivesPages[actualPage]);
        }
        AudioManager.instance.Play("SFX_UI_Positif", false);
    }

    public void OnClickPreviousPage()
    {
        actualPage--;
        DisplayLevelSelecter();
        AudioManager.instance.Play("SFX_UI_Positif", false);
    }

    public void OnClickUnlockPage(bool key)
    {
        if (key)
        {
            if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 2 && NumberStarsUnlocked(PlayerData.instance.starsNumber) >= GameManager.gameManager.objectiveFinalLevel)
            {
                PlayerData.instance.pageLock[PlayerData.instance.pageLock.Length - 1] = true;
                PlayerData.instance.SaveLevelData();
                UndisplayUnlockPanel();
                LevelLoader.instance.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else if (NumberStarsUnlocked(PlayerData.instance.starsNumber) >= GameManager.gameManager.objectivesPages[SceneManager.GetActiveScene().buildIndex>0 ? Mathf.CeilToInt(SceneManager.GetActiveScene().buildIndex / 8 -1) : 0])
            {
                PlayerData.instance.pageLock[actualPage] = true;
                PlayerData.instance.SaveLevelData();
                if (SceneManager.GetActiveScene().buildIndex > 0)
                {
                    UndisplayUnlockPanel();
                    LevelLoader.instance.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
                }
                else if (isDisplayInfos)
                {
                    UndisplayUnlockPanel();
                    isDisplayInfos = false;
                    PlayerData.instance.pageLock[PlayerData.instance.pageLock.Length - 1] = true;
                    PlayerData.instance.SaveLevelData();
                    UndisplayLevelSelecter();
                    DisplayLevelSelecter(SceneManager.sceneCountInBuildSettings - 1);
                }
                else
                {
                    OnClickNextPage();
                }
                UndisplayUnlockPanel();
            }
            else
            {
                warning.gameObject.SetActive(true);
            }
        }
        else
        {
            UndisplayUnlockPanel();
        }
        AudioManager.instance.Play("SFX_UI_Positif", false);
    }

    public void OnClickValidateDrop(bool key)
    {
        UndisplayDropData();
        if (key)
        {
            PlayerData.instance.DeleteLevelData();
            actualPage = 0;
            OnClickStat(false);
            AudioManager.instance.Play("SFX_UI_Positif", false);
        }
    }

#endregion

#region Display UI Fonctions

    #region Options

    private void DisplayOptions()
    {
        TweenManager.tweenManager.PlayMenuTween("introOptions");
        options = "opt";
    }

    private void UndisplayOptions()
    {
        TweenManager.tweenManager.PlayMenuTween("outroOptions");        
    }

    private void LanguageToOptions()
    {
        options = "opt";
        TweenManager.tweenManager.Play("introBackOptions");
        TweenManager.tweenManager.Play("introName");
        TweenManager.tweenManager.Play("introMusic");
        TweenManager.tweenManager.Play("introSound");
        TweenManager.tweenManager.Play("introButLang");
        TweenManager.tweenManager.Play("introProcess");
        TweenManager.tweenManager.Play("introRenderer");
        TweenManager.tweenManager.Play("introTextRenderer");
        TweenManager.tweenManager.Play("introTextLights");
    }

    public void RefreshToggleMusic()
    {
        if (!PlayerData.instance.parameter[2])
        {
            musicButton.GetComponent<Image>().sprite = dataResults.activatedMusic;
            pauseMusicButton.GetComponent<Image>().sprite = dataResults.activatedMusic;
        }
        else
        {
            musicButton.GetComponent<Image>().sprite = dataResults.deactivatedMusic;
            pauseMusicButton.GetComponent<Image>().sprite = dataResults.deactivatedMusic;
        }
    }

    public void RefreshToggleSound()
    {
        if (!PlayerData.instance.parameter[3])
        {
            soundButton.GetComponent<Image>().sprite = dataResults.activatedSound;
            pauseSoundButton.GetComponent<Image>().sprite = dataResults.activatedSound;
        }
        else
        {
            soundButton.GetComponent<Image>().sprite = dataResults.deactivatedSound;
            pauseSoundButton.GetComponent<Image>().sprite = dataResults.deactivatedSound;
        }
    }

    #endregion

    #region Credits

    private void DisplayCredits()
    {
        TweenManager.tweenManager.PlayMenuTween("introCredits");
    }

    private void UndisplayCredits()
    {
        TweenManager.tweenManager.PlayMenuTween("outroCredits");
    }

    #endregion

    #region Main Menu

    public void DisplayMainMenu()
    {
        mainMenu.SetActive(true);
        TweenManager.tweenManager.PlayMenuTween("introMainMenu");
    }

    private void UndisplayMainMenu()
    {
        TweenManager.tweenManager.PlayMenuTween("outroMainMenu");
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
                    buttonLevelSelecter[index % 8].GetComponent<Image>().sprite = dataResults.unlockedLevelOneStar;
                    break;
                case 2:
                    buttonLevelSelecter[index % 8].GetComponent<Image>().sprite = dataResults.unlockedLevelTwoStar;
                    break;
                case 3:
                    buttonLevelSelecter[index % 8].GetComponent<Image>().sprite = dataResults.unlockedLevelThreeStar;
                    break;
                default:
                    buttonLevelSelecter[index % 8].GetComponent<Image>().sprite = dataResults.unlockedLevelNoStar;
                    break;
            }            
            if (index != 0 && levels[index] == 0 && levels[index - 1] == 0)
            {
                lockedlevels[index % 8].GetComponent<Image>().sprite = dataResults.lockedLevel;
                lockedlevels[index % 8].SetActive(true);
            }
            else if (index == SceneManager.sceneCountInBuildSettings - 2 && !PlayerData.instance.pageLock[PlayerData.instance.pageLock.Length - 1])
            {
                lockedlevels[index % 8].SetActive(true);
                lockedlevels[index % 8].GetComponent<Image>().sprite = dataResults.unlockeableFinal;
            }
            else
            {
                lockedlevels[index % 8].SetActive(false);
            }
        }
        DisplayNextPageButton(levels);
        DisplayPreviousPageButton();
        TweenManager.tweenManager.PlayMenuTween("introLevelSelect");
    }

    public void DisplayLevelSelecter(int level)
    {
        actualPage = Mathf.CeilToInt( ((float)level) / 8) - 1;
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
        TweenManager.tweenManager.PlayMenuTween("outroLevelSelect");
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
        BlockLevelSelectButton();
        if (RulesSystem.GetLevelValueToInt(numberLevel, 1) > 1)
        {
            this.starOneCondition.GetComponent<TextLocaliserUI>().UpdateText("_multipleshotsstargoal");
        }
        else
        {
            this.starOneCondition.GetComponent<TextLocaliserUI>().UpdateText("_oneshotstargoal");
        }
        if (RulesSystem.GetLevelValueToInt(numberLevel, 2) > 1)
        {
            this.starTwoCondition.GetComponent<TextLocaliserUI>().UpdateText("_multipleshotsstargoal");
        }
        else
        {
            this.starTwoCondition.GetComponent<TextLocaliserUI>().UpdateText("_oneshotstargoal");
        }
        if (RulesSystem.GetLevelValueToInt(numberLevel, 3) > 1)
        {
            this.starThreeCondition.GetComponent<TextLocaliserUI>().UpdateText("_multipleshotsstargoal");
        }
        else
        {
            this.starThreeCondition.GetComponent<TextLocaliserUI>().UpdateText("_oneshotstargoal");
        }

        this.numberLevel.GetComponent<TextLocaliserUI>().UpdateText("_level");
        this.numberLevel.text = this.numberLevel.text + " " + numberLevel.ToString();
        starOneCondition.text = starOneCondition.text.Replace("X", RulesSystem.GetLevelValue(numberLevel, 1));
        starTwoCondition.text = starTwoCondition.text.Replace("X", RulesSystem.GetLevelValue(numberLevel, 2));
        starThreeCondition.text = starThreeCondition.text.Replace("X", RulesSystem.GetLevelValue(numberLevel, 3));
        DisplayNumberStars(numberLevel - 1, starsImage);
        TweenManager.tweenManager.PlayMenuTween("introInfos");

    }

    private void BlockLevelSelectButton()
    {
        Button[] buttonSelecter = levelMenu.GetComponentsInChildren<Button>();
        foreach (Button button in buttonSelecter)
        {
            button.interactable = false;
        }
    }

    private void UnblockLevelSelectButton()
    {
        Button[] buttonSelecter = levelMenu.GetComponentsInChildren<Button>();
        foreach (Button button in buttonSelecter)
        {
            button.interactable = true;
        }
    }

    public void UndisplayLevelInfos()
    {
        UnblockLevelSelectButton();
        TweenManager.tweenManager.PlayMenuTween("outroInfos");
    }

    #endregion

    #region Pause

    public void DisplayPause()
    {
        TweenManager.tweenManager.Play("goInPause");
        LocalisationNumberShots();     
        DisplayNumberShots();        
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
        TweenManager.tweenManager.PlayMenuTween("introPause");
        TweenManager.tweenManager.Play("introDisplay");
    }

    public void UndisplayPause()
    {
        TweenManager.tweenManager.PlayMenuTween("outroPause");
        TweenManager.tweenManager.Play("outroDisplay");
        TweenManager.tweenManager.Play("returnInGame");
        
    }

    private void LocalisationNumberShots()
    {
        displayPause.SetActive(true);
        if (LevelManager.levelManager.level.shotStarOne > 1)
        {
            listTextShots[0].GetComponent<TextLocaliserUI>().UpdateText("_multipleshotsstargoal");
        }
        else
        {
            listTextShots[0].GetComponent<TextLocaliserUI>().UpdateText("_oneshotstargoal");
        }
        if (LevelManager.levelManager.level.shotStarTwo > 1)
        {
            listTextShots[1].GetComponent<TextLocaliserUI>().UpdateText("_multipleshotsstargoal");
        }
        else
        {
            listTextShots[1].GetComponent<TextLocaliserUI>().UpdateText("_oneshotstargoal");
        }
        if (LevelManager.levelManager.level.shotStarThree > 1)
        {
            listTextShots[2].GetComponent<TextLocaliserUI>().UpdateText("_multipleshotsstargoal");
        }
        else
        {
            listTextShots[2].GetComponent<TextLocaliserUI>().UpdateText("_oneshotstargoal");
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
        LockUIButton(true);
        inGameUI.SetActive(false);
    }

    public void UpdateShots(int shots)
    {
        numberShots.text = " " + shots + " ";
    }

    public void LockUIButton(bool lockButton)
    {
        Button[] listButton = inGameUI.GetComponentsInChildren<Button>();
        foreach(Button button in listButton)
        {
            button.interactable = lockButton;
        }
    }

    #endregion

    #region Level Results

    public void DisplayLevelResults(bool hasWin, int starsUnlocked)
    {
        starsObtained = starsUnlocked;
        UnDisplayInGameUI();
        int index = SceneManager.GetActiveScene().buildIndex;
        if (hasWin)
        {
            AudioManager.instance.Play("SFX_UI_Victory", false);
            imageStarsResults.sprite = dataResults.victory;
            textResults.GetComponent<TextLocaliserUI>().UpdateText("_victory");
            LevelManager.levelManager.starsObtained = starsUnlocked;
            switch (starsUnlocked)
            {
                case 1:
                    stars[0].sprite = dataResults.star;
                    stars[1].sprite = dataResults.noStar;
                    stars[1].GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
                    stars[2].sprite = dataResults.noStar;
                    stars[2].GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
                    break;
                case 2:
                    stars[0].sprite = dataResults.star;
                    stars[1].sprite = dataResults.star;
                    stars[2].sprite = dataResults.noStar;
                    stars[2].GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
                    break;
                case 3:
                    stars[0].sprite = dataResults.star;
                    stars[1].sprite = dataResults.star;
                    stars[2].sprite = dataResults.star;
                    break;
                default:
                    break;
            }            
        }
        else
        {
            textResults.GetComponent<TextLocaliserUI>().UpdateText("_defeat");
            imageStarsResults.sprite = dataResults.defeat;
            starsObtained = 0;
            LevelManager.levelManager.starsObtained = 0;
            AudioManager.instance.Play("SFX_UI_Defeat", false);
        }

        if (GameManager.gameManager.GetShootDone() > 1)
        {
            resultsShots.GetComponent<TextLocaliserUI>().UpdateText("_resultmultipleshots");
        }
        else
        {
            resultsShots.GetComponent<TextLocaliserUI>().UpdateText("_resultoneshot");
        }

        if (PlayerData.instance != null)
            PlayerData.instance.SaveLevelData();
        
        resultsShots.text = resultsShots.text.Replace("X", GameManager.gameManager.GetShootDone().ToString());

        TweenManager.tweenManager.PlayMenuTween("introResults");
        StartCoroutine("Resultswin");        
    }

    public void UndisplayLevelResults()
    {
        victoryButtonNext.SetActive(false);
        restartButton.SetActive(false);
        homeButton.SetActive(false);        
        TweenManager.tweenManager.PlayMenuTween("outroResults");
        stars[0].gameObject.SetActive(false);
        stars[1].gameObject.SetActive(false);
        stars[2].gameObject.SetActive(false);
    }

    public void DisplayButton()
    {
        restartButton.SetActive(true);
        homeButton.SetActive(true);
        if (GameManager.gameManager.isVictory)
        {            
            if (SceneManager.GetActiveScene().buildIndex != SceneManager.sceneCountInBuildSettings - 1)
            {
                victoryButtonNext.SetActive(true);
            }
        }
    }

    IEnumerator Resultswin()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        TweenManager.tweenManager.PlayAnimStar(starsObtained);
    }

    #endregion

    #region Language

    public void DisplayLanguageMenu()
    {
        TweenManager.tweenManager.PlayMenuTween("introLanguage");
        options = "lang";
    }

    public void UndisplayLanguageMenu()
    {
        TweenManager.tweenManager.PlayMenuTween("outroLanguage");
    }

    private void OptionsToLanguage()
    {
        TweenManager.tweenManager.Play("outroBackOptions");
        TweenManager.tweenManager.Play("outroName");
        TweenManager.tweenManager.Play("outroMusic");
        TweenManager.tweenManager.Play("outroSound");
        TweenManager.tweenManager.Play("outroButLang");
        TweenManager.tweenManager.Play("outroProcess");
        TweenManager.tweenManager.Play("outroRenderer");
        TweenManager.tweenManager.Play("outroTextRenderer");
        TweenManager.tweenManager.Play("outroTextLights");
    }

    #endregion

    #region Stats

    public void DisplayStats()
    {
        TweenManager.tweenManager.PlayMenuTween("introStats");
    }

    public void UndisplayStats()
    {
        TweenManager.tweenManager.PlayMenuTween("outroStats");
    }

    #endregion

    #region Unlock

    private void DisplayUnlockPanel()
    {
        TweenManager.tweenManager.PlayMenuTween("introUnlock");
    }

    private void UndisplayUnlockPanel()
    {
        warning.gameObject.SetActive(false);
        TweenManager.tweenManager.PlayMenuTween("outroUnlock");
    }

    #region DropData

    private void DisplayDropData()
    {
        TweenManager.tweenManager.PlayMenuTween("introDelete");
    }

    private void UndisplayDropData()
    {
        TweenManager.tweenManager.PlayMenuTween("outroDelete");
    }

    #endregion

    #endregion

    #endregion

    #region Tutorial Fonctions

    public void DisplayTutorial(int level)
    {
        tutorialMessage.SetActive(true);
    }

    public void UndisplayTutorial()
    {
        tutorialMessage.SetActive(false);
    }

#endregion

}
