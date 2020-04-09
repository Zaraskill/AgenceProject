using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ManagerScript : MonoBehaviour
{
    // Hide these variables from Unity editor.
    [HideInInspector]
    public bool playerPlaced = false;
    [HideInInspector]
    public bool saveLoadMenuOpen = false;

    public Animator itemUIAnimation;
    public Animator optionUIAnimation;
    public Animator saveUIAnimation;
    public Animator loadUIAnimation;
    public MouseScript user;
    public Slider rotSlider;
    public GameObject rotUI;
    public InputField levelNameSave;
    public InputField levelNameLoad;
    public Text levelMessage;
    public Animator messageAnim;

    [Header("GameObjects")]
    public GameObject player;

    [Header("Prefabs")]
    public GameObject[] Objects;

    [Header("Groupe")]
    public GameObject level_Platforms;
    
    private bool itemPositionIn = true;
    private bool optionPositionIn = true;
    private bool saveLoadPositionIn = false;
    private LevelEditor level;
    

    void Start()
    {
        Time.timeScale = 1f;

        rotSlider.onValueChanged.AddListener(delegate { RotationValueChange(); });
        CreateEditor();
    }

    LevelEditor CreateEditor()
    {
        level = new LevelEditor();
        level.editorObjects = new List<EditorObject.Data>();
        return level;
    }
    
    void RotationValueChange()
    {
        user.rotObject.transform.eulerAngles = new Vector2(0, rotSlider.value);
        user.rotObject.GetComponent<EditorObject>().data.rot = user.rotObject.transform.rotation;
    }

    /// <summary>
    /// Selecting menus
    /// </summary>
    public void SlideItemMenu()
    {
        if (itemPositionIn == false)
        {
            itemUIAnimation.SetTrigger("ItemMenuIn");
            itemPositionIn = true;
        }
        else
        {
            itemUIAnimation.SetTrigger("ItemMenuOut"); 
            itemPositionIn = false;
        }
    }

    public void SlideOptionMenu()
    {
        if (optionPositionIn == false)
        {
            optionUIAnimation.SetTrigger("OptionMenuIn");
            optionPositionIn = true;
        }
        else
        {
            optionUIAnimation.SetTrigger("OptionMenuOut");
            optionPositionIn = false;
        }
    }

    public void ChooseSave()
    {
        if (saveLoadPositionIn == false)
        {
            saveUIAnimation.SetTrigger("SaveLoadIn");
            saveLoadPositionIn = true;
            saveLoadMenuOpen = true;
        }
        else
        {
            saveUIAnimation.SetTrigger("SaveLoadOut");
            saveLoadPositionIn = false;
            saveLoadMenuOpen = false; 
        }
    }

    public void ChooseLoad()
    {
        if (saveLoadPositionIn == false)
        {
            loadUIAnimation.SetTrigger("SaveLoadIn");
            saveLoadPositionIn = true;
            saveLoadMenuOpen = true;
        }
        else
        {
            loadUIAnimation.SetTrigger("SaveLoadOut");
            saveLoadPositionIn = false;
            saveLoadMenuOpen = false;
        }
    }

    public void ChooseQuit()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Choosing an object
    /// </summary>
    public void ChoosePlatform(int id)
    {
        user.itemOption = (MouseScript.ItemList)id;
        user.itemOptionID = id;
    }

    /// <summary>
    /// Choosing an option for level manipulation
    /// </summary>
    public void ChooseCreate()
    {
        user.manipulateOption = MouseScript.LevelManipulation.Create;
        //user.mr.enabled = true;
        rotUI.SetActive(false);
    }

    public void ChooseRotate()
    {
        user.manipulateOption = MouseScript.LevelManipulation.Rotate;
        //user.mr.enabled = false;
        rotUI.SetActive(true);
    }

    public void ChooseDestroy()
    {
        user.manipulateOption = MouseScript.LevelManipulation.Destroy;
        //user.mr.enabled = false;
        rotUI.SetActive(false);
    }

    

    // Saving a level
    public void SaveLevel()
    {
        // Gather all objects with EditorObject component
        EditorObject[] foundObjects = FindObjectsOfType<EditorObject>();
        foreach (EditorObject obj in foundObjects)
            level.editorObjects.Add(obj.data); // add these objects to the list of editor objects

        string json = JsonUtility.ToJson(level); // write the level data to json
        string folder = Application.dataPath + "/LevelData/"; // create a folder
        string levelFile = "";

        //set a default file name if no name given
        if (levelNameSave.text == "")
            levelFile = "new_level.json";
        else
            levelFile = levelNameSave.text + ".json";

        //Create new directory if LevelData directory does not yet exist.
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        string path = Path.Combine(folder, levelFile); // set filepath

        //Overwrite file with same name, if applicable
        if (File.Exists(path))
            File.Delete(path);

        // create and save file
        File.WriteAllText(path, json); 

        //Remove save menu
        saveUIAnimation.SetTrigger("SaveLoadOut");
        saveLoadPositionIn = false;
        saveLoadMenuOpen = false;
        levelNameSave.text = ""; // clear input field
        levelNameSave.DeactivateInputField(); // remove focus from input field.

        //Display message
        levelMessage.text = levelFile + " saved to LevelData folder.";
        messageAnim.Play("MessageFade", 0, 0);
    }


    // Loading a level
    public void LoadLevel()
    {
        string folder = Application.dataPath + "/LevelData/";
        string levelFile = "";

        //set a default file name if no name given
        if (levelNameLoad.text == "")
            levelFile = "new_level.json";
        else
            levelFile = levelNameLoad.text + ".json";

        string path = Path.Combine(folder, levelFile); // set filepath

        if (File.Exists(path)) // if the file could be found in LevelData
        {
            // The objects currently in the level will be deleted
            EditorObject[] foundObjects = FindObjectsOfType<EditorObject>();
            foreach (EditorObject obj in foundObjects)
                Destroy(obj.gameObject);

            playerPlaced = false; // since objects are being destroyed, go ahead and say player placed is false

            string json = File.ReadAllText(path); // provide text from json file
            level = JsonUtility.FromJson<LevelEditor>(json); // level information filled from json file
            CreateFromFile(); // create objects from level data.
        }
        else // if file could not be found.
        {
            loadUIAnimation.SetTrigger("SaveLoadOut"); // remove menu
            saveLoadPositionIn = false; // indicate menu not on screen
            saveLoadMenuOpen = false; // indicate camera can move.
            levelMessage.text = levelFile + " could not be found!"; // send message
            messageAnim.Play("MessageFade", 0, 0);
            levelNameLoad.DeactivateInputField(); // remove focus from input field
        }
    }

    // create objects based on data within level.
    void CreateFromFile()
    {
        for (int i = 0; i < level.editorObjects.Count; i++)
        {
            if (level.editorObjects[i].objectType == EditorObject.ObjectType.Platform_1)
            {
                InstantiateObjects(0, i);
            }
            else if (level.editorObjects[i].objectType == EditorObject.ObjectType.Platform_2)
            {
                InstantiateObjects(1, i);
            }
            else if (level.editorObjects[i].objectType == EditorObject.ObjectType.Platform_3)
            {
                InstantiateObjects(2, i);
            }
            else if (level.editorObjects[i].objectType == EditorObject.ObjectType.Platform_4)
            {
                InstantiateObjects(3, i);
            }
        }

        //Clear level box
        levelNameLoad.text = "";
        levelNameLoad.DeactivateInputField(); // remove focus from input field

        loadUIAnimation.SetTrigger("SaveLoadOut"); // slide load menu off screen
        saveLoadPositionIn = false; // indicate load menu off screen
        saveLoadMenuOpen = false; // allow camera movement.

        //Display message
        levelMessage.text = "Level loading...done.";
        messageAnim.Play("MessageFade", 0, 0);
    }

    // create Object
    void InstantiateObjects(int id, int i)
    {
        GameObject newObj;

        newObj = Instantiate(Objects[id], level.editorObjects[i].pos, Quaternion.identity);
        newObj.transform.rotation = level.editorObjects[i].rot;
        newObj.transform.parent = level_Platforms.transform;
        newObj.layer = 9;
        newObj.name = "Platform_" + id;

        //Add editor object component and feed data.
        EditorObject eo = newObj.AddComponent<EditorObject>();
        eo.data.pos = newObj.transform.position;
        eo.data.rot = newObj.transform.rotation;
        eo.data.objectType = (EditorObject.ObjectType)id;
    }
}
