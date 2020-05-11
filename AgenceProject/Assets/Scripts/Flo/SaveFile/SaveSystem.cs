using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveDataFile (PlayerData level)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath, "Blobby.data");
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(level);
        
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData LoadDataFile()
    {
        string path = Path.Combine(Application.persistentDataPath, "Blobby.data");
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in" + path);
            return null;
        }
    }

}
