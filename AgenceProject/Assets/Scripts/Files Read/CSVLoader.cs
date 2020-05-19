using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class CSVLoader : MonoBehaviour
{

    private TextAsset textFile;
    private TextAsset localisationFile;
    private string[] lineSeperator = new string[] { "\r\n" };
    private char surround = '"';
    private string[] fieldSeperator = { "\",\"" };

    public void LoadCSV()
    {
        textFile = Resources.Load<TextAsset>("levelsInfos");
        localisationFile = Resources.Load<TextAsset>("localisation");
    }

    public List<string[]> GetLevelValues()
    {
        List<string[]> levelsInfos;

        string[] lines = textFile.text.Split(lineSeperator, StringSplitOptions.None);

        levelsInfos = new List<string[]>();

        Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

        for (int indexI = 0; indexI < lines.Length; indexI++)
        {
            string line = lines[indexI];
            string[] fields = CSVParser.Split(line);

            for(int IndexJ = 0; IndexJ <fields.Length; IndexJ++)
            {
                fields[IndexJ] = fields[IndexJ].TrimStart(' ', surround);
                fields[IndexJ] = fields[IndexJ].TrimEnd(surround);
            }
            if (fields.Length == 1 && fields[0] == "")
            {
                continue;
            }
            levelsInfos.Add(fields);
        }
        Debug.Log(levelsInfos);

        return levelsInfos;
    }

    public Dictionary<string, string> GetDictionaryValues(string attributeID)
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();

        string[] lines = localisationFile.text.Split(lineSeperator, StringSplitOptions.None);

        int attributeIndex = -1;

        string[] headers = lines[0].Split(fieldSeperator, StringSplitOptions.None);

        for (int index = 0 ; index<headers.Length ; index++)
        {
            if (headers[index].Contains(attributeID))
            {
                attributeIndex = index;
                break;
            }
        }

        Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

        for (int index = 1; index<lines.Length ; index++)
        {
            string line = lines[index];

            string[] fields = CSVParser.Split(line);

            for (int indexJ=0; indexJ<fields.Length; indexJ++)
            {
                fields[indexJ] = fields[indexJ].TrimStart(' ', surround);
                fields[indexJ] = fields[indexJ].TrimEnd(surround);
            }
            if (fields.Length == 1 && fields[0] == "")
            {
                continue;
            }
            if (fields.Length > attributeIndex)
            {
                var key = fields[0];

                if(dictionary.ContainsKey(key))
                {
                    continue;
                }

                var value = fields[attributeIndex];

                dictionary.Add(key, value);
            }
        }
        return dictionary;
    }

#if UNITY_EDITOR

    public void Add(string key, string value)
    {
        string appended = string.Format("\r\n\"{0}\",\"{1}\",\"\"", key, value);
        File.AppendAllText("Assets/Resources/localisation.txt", appended);

        UnityEditor.AssetDatabase.Refresh();
    }

    public void Add(string key, string valueEN, string valueFR)
    {
        string appended = string.Format("\n\"{0}\",\"{1}\",\"{2}\"", key, valueEN, valueFR);
        File.AppendAllText("Assets/Resources/localisation.txt", appended);

        UnityEditor.AssetDatabase.Refresh();
    }

    public void Remove(string key)
    {
        string[] lines = localisationFile.text.Split(lineSeperator, StringSplitOptions.None);

        string[] keys = new string[lines.Length];

        for(int index = 0; index < lines.Length; index++)
        {
            string line = lines[index];

            keys[index] = line.Split(fieldSeperator, StringSplitOptions.None)[0];
        }

        int indexKey = -1;

        for (int index = 0; index < keys.Length; index++)
        {
            if (keys[index].Contains(key))
            {
                indexKey = index;
                break;
            }
        }

        if (indexKey > -1)
        {
            string[] newLines;
            newLines = lines.Where(w => w != lines[indexKey]).ToArray();

            string replaced = string.Join(lineSeperator[0], newLines);
            File.WriteAllText("Assets/Resources/localisation.txt", replaced);
        }
        UnityEditor.AssetDatabase.Refresh();
    }

    public void Edit(string key, string value)
    {
        Remove(key);
        Add(key, value);
    }

#endif
}
