using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    [Header("Prefabs")]
    public GameObject[] Objects;

    [Header("Groupe")]
    public GameObject level_Platforms;

    private LevelEditor level;

    // Start is called before the first frame update
    void Start()
    {
        LoadLevel(GameManager.currentlevel.ToString());
    }

    // Loading a level
    public void LoadLevel(string levelNameLoad)
    {
        string folder = Application.dataPath + "/LevelData/";
        string levelFile = "";

        //set a default file name if no name given
        if (levelNameLoad == "")
            levelFile = "new_level.json";
        else
            levelFile = levelNameLoad + ".json";

        string path = Path.Combine(folder, levelFile); // set filepath

        if (File.Exists(path)) // if the file could be found in LevelData
        {
            // The objects currently in the level will be deleted
            EditorObject[] foundObjects = FindObjectsOfType<EditorObject>();
            foreach (EditorObject obj in foundObjects)
                Destroy(obj.gameObject);

            //playerPlaced = false; // since objects are being destroyed, go ahead and say player placed is false

            string json = File.ReadAllText(path); // provide text from json file
            level = JsonUtility.FromJson<LevelEditor>(json); // level information filled from json file
            CreateFromFile(); // create objects from level data.
        }
        else // if file could not be found.
        {
            /*
            loadUIAnimation.SetTrigger("SaveLoadOut"); // remove menu
            saveLoadPositionIn = false; // indicate menu not on screen
            saveLoadMenuOpen = false; // indicate camera can move.
            levelMessage.text = levelFile + " could not be found!"; // send message
            messageAnim.Play("MessageFade", 0, 0);
            levelNameLoad.DeactivateInputField(); // remove focus from input field
            */
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

        /*
        //Clear level box
        levelNameLoad.text = "";
        levelNameLoad.DeactivateInputField(); // remove focus from input field

        loadUIAnimation.SetTrigger("SaveLoadOut"); // slide load menu off screen
        saveLoadPositionIn = false; // indicate load menu off screen
        saveLoadMenuOpen = false; // allow camera movement.

        //Display message
        levelMessage.text = "Level loading...done.";
        messageAnim.Play("MessageFade", 0, 0);
        */
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
