using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { set; get; }

    public SaveState state;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
        Load();
    }

    // Save State
    public void Save()
    {
        PlayerPrefs.SetString("save",  Helper.Serialize<SaveState>(state));
    }

    // Load Save State
    public void Load()
    {
        if (PlayerPrefs.HasKey("save"))
        {
            state = Helper.Deserialize<SaveState>(PlayerPrefs.GetString("save"));
        }
        else
        {
            state = new SaveState();
            Save();
            Debug.Log("Save file Create !");
        }
    }

    // Reset Save
    public void ResetSave()
    {
        PlayerPrefs.DeleteKey("save");
    }

}
