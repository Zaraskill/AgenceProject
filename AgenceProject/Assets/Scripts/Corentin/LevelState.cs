using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelState : MonoBehaviour
{
    //Shots
    public int shotsAllowed;

    //Ennemies
    public int numberEnnemies;
    private int ennemiesLeft;
    public int ennemiTest;

    //Stars score
    public int shotStarOne;
    public int shotStarTwo;
    public int shotStarThree;

    // Start is called before the first frame update
    void Start()
    {
        ennemiesLeft = numberEnnemies;
        GameManager.gameManager.GenerateLevel();
        ennemiTest = numberEnnemies;
    }

    public void BeatEnnemy()
    {
        ennemiesLeft--;
        ennemiTest--;
    }

    public bool hasNoEnnemiesLeft()
    {
        if (ennemiesLeft <= 0)
        {
            return true;
        }
        return false;
    }

    public int numberStars(int numberShotsDone)
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
