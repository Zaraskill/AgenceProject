using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveDataFile (PlayerData level)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath, "Blobby.data");
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            SaveData data = new SaveData(level);

            formatter.Serialize(stream, data);
        }
    }

    public static SaveData LoadDataFile()
    {
        string path = Path.Combine(Application.persistentDataPath, "Blobby.data");
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            SaveData data;
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                data = formatter.Deserialize(stream) as SaveData;
            }
            
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in" + path);
            return null;
        }
    }

    public static void DeleteDataFile()
    {
        string path = Path.Combine(Application.persistentDataPath, "Blobby.data");
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Deleted");
        }
    }

}

// Local : C:\Users\[UserName]\AppData\LocalLow\AgenceProject\Blobby
