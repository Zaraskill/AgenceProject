using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelState : MonoBehaviour
{
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

    //Objects in map
    [Header("Objects")]
    public GameObject pushableComponent;
    public GameObject destructibleComponent;
    public List<Rigidbody2D> children;

    private Rigidbody2D player;

    // Start is called before the first frame update
    void Start()
    {
        if (pushableComponent != null)
        {
            foreach (Transform childComponent in pushableComponent.transform)
            {
                foreach (Transform child in childComponent)
                {
                    children.Add(child.GetComponent<Rigidbody2D>());
                }
            }
        }
        if (destructibleComponent != null)
        {
            foreach (Transform child in destructibleComponent.transform)
            {
                children.Add(child.GetComponent<Rigidbody2D>());
            }
        }
        player = FindObjectOfType<PlayerController>().GetComponent<Rigidbody2D>();
        enemiesLeft = numberEnemies;
        enemiTest = numberEnemies;
        GameManager.gameManager.GenerateLevel();
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

    #region Check Movement Fonctions
    
    public void CheckMoving()
    {
        StartCoroutine(Checking());
    }

    IEnumerator Checking()
    {
        yield return new WaitForSeconds(1f);
        bool active = true;
        while (active)
        {
            if (player.velocity.magnitude <= 0.01f)
            {
                int objectDoesntSleep = 0;

                foreach (Rigidbody2D child in children)
                {
                    if (child != null && child.velocity.magnitude > 0.01f)
                        objectDoesntSleep++;
                }

                if (objectDoesntSleep == 0)
                {
                    active = false;
                }
            }

            yield return new WaitForSeconds(1f);
        }
        GameManager.gameManager.gameState = GameManager.STATE_PLAY.verificationThrow;
    }
    #endregion
}
