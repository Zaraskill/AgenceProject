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

    //Stars score
    public int shotStarOne;
    public int shotStarTwo;
    public int shotStarThree;

    // Start is called before the first frame update
    void Start()
    {
        ennemiesLeft = numberEnnemies;    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BeatEnnemy()
    {
        ennemiesLeft--;
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
