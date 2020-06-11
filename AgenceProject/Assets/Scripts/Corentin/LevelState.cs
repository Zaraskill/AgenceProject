using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelState : MonoBehaviour
{
    public bool isTutorial;
    public bool isIntroPlayer;
    public bool needCancelSlingshot;

    //Shots
    [Header("Shoots")]
    public int shootsAllowed;

    //Ennemies
    [Header("Enemies")]
    public int numberEnemies;
    private int enemiesLeft;
    public int enemiTest;

    //Stars score
    [Header("Stars score")]
    public int shotStarOne;
    public int shotStarTwo;
    public int shotStarThree;
    public int starsObtained;

    // Start is called before the first frame update
    void Start()
    {
        enemiesLeft = numberEnemies;
        enemiTest = numberEnemies;        
        GameManager.gameManager.GenerateLevel();
        this.shotStarOne = RulesSystem.GetLevelValueToInt(LevelManager.levelManager.currentLevel, 1);
        this.shotStarTwo = RulesSystem.GetLevelValueToInt(LevelManager.levelManager.currentLevel, 2);
        this.shotStarThree = RulesSystem.GetLevelValueToInt(LevelManager.levelManager.currentLevel, 3);
    }

    public void BeatEnnemy()
    {
        enemiesLeft--;
        enemiTest--;
    }

    public bool HasNoEnemiesLeft()
    {
        if (enemiesLeft <= 0)
        {
            return true;
        }
        return false;
    }

    public int NumberStars(int numberShotsDone)
    {
        if (numberShotsDone <= shotStarThree)
        {
            return 3;
        }
        else if (numberShotsDone > shotStarThree && numberShotsDone <= shotStarTwo)
        {
            return 2;
        }
        else if (numberShotsDone > shotStarThree && numberShotsDone <= shotStarOne)
        {
            return 1;
        }
        return 0;
    }
}
