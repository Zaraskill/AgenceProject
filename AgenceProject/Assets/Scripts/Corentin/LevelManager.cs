using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static LevelManager levelManager;

    private int currentLevel;
    public LevelState level;

    void Awake()
    {
        if(levelManager != null)
        {
            Debug.LogError("too many instances");
        }
        else
        {
            levelManager = this;
            //DontDestroyOnLoad(this.gameObject);
        }        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnemyDeath()
    {
        level.BeatEnnemy();
        if (level.hasNoEnemiesLeft())
        {
            GameManager.gameManager.EndLevel(true);
        }
    }

    public int ScoreResults(int numberShotsDone)
    {
        return level.numberStars(numberShotsDone);
    }

    public int ShotsLevel()
    {
        return level.shootsAllowed;
    }

    public void ChargeLevel()
    {
        level = FindObjectOfType<LevelState>();
    }

    public void CheckMovement()
    {
        level.CheckMoving();
    }
}
