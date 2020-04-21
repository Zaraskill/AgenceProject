using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelState : MonoBehaviour
{

    public int numberEnnemies;
    private int ennemiesLeft;

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
}
