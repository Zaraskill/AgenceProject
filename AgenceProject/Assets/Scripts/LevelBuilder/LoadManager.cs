using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LoadManager : MonoBehaviour
{

    [HideInInspector]
    public bool playerPlaced = false;
    [HideInInspector]
    public bool saveLoadMenuOpen = false;
    [Header("Load Level")]
    public InputField levelNameLoad;
    public Animator loadUIAnimation;
    public Text levelMessage;
    public Animator messageAnim;

    [Header("Manager")]
    public ManagerScript ms;

    private LevelEditor level;
    private bool saveLoadPositionIn = false;

    private void Start()
    {
        ms.player.GetComponent<Rigidbody>().isKinematic = true;
        ChooseLoad();
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
        Debug.Log("DONE !");
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
        ms.player.GetComponent<Rigidbody>().isKinematic = false;
    }

    // create Object
    void InstantiateObjects(int id, int i)
    {
        GameObject newObj;

        newObj = Instantiate(ms.Objects[id], level.editorObjects[i].pos, Quaternion.identity);
        newObj.transform.rotation = level.editorObjects[i].rot;
        newObj.transform.parent = ms.level_Platforms.transform;
        newObj.layer = 9;
        newObj.name = "Platform_" + id;

        //Add editor object component and feed data.
        EditorObject eo = newObj.AddComponent<EditorObject>();
        eo.data.pos = newObj.transform.position;
        eo.data.rot = newObj.transform.rotation;
        eo.data.objectType = (EditorObject.ObjectType)id;
    }

    public void ChooseLoad()
    {
        if (saveLoadPositionIn == false)
        {
            loadUIAnimation.SetTrigger("SaveLoadIn"); // slide menu into screen
            saveLoadPositionIn = true; // indicate menu on screen
            saveLoadMenuOpen = true; // indicate load menu open, prevent camera movement.
        }
        else
        {
            loadUIAnimation.SetTrigger("SaveLoadOut"); // slide menu off screen
            saveLoadPositionIn = false; // indicate menu off screen
            saveLoadMenuOpen = false; // indicate load menu off screen, allow camera movement.
        }
    }
}
