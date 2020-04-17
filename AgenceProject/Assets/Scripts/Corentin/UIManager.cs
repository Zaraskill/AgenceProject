using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager uiManager;

    [Header("CanvasMenu")]
    public Canvas menu;
    public GameObject mainMenu;
    public GameObject levelMenu;
    public GameObject menuPause;
    public GameObject inGameUI;
    public Button button;



    private void Awake()
    {
        if (uiManager != null)
        {
            Debug.LogError("Too many instances!");
        }
        else
        {
            uiManager = this;
            DontDestroyOnLoad(this.gameObject);
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

    public void DisplayMainMenu()
    {

    }

    public void DisplayPause()
    {

    }

    public void DisplayLevelResults(bool hasWin)
    {
        if (hasWin)
        {
            levelMenu.SetActive(true);
        }
        else
        {
            menuPause.SetActive(true);
        }
    }
}
