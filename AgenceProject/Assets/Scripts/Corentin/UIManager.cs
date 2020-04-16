using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //
    public static UIManager uiManager;


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
}
