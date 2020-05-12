using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CSVLoader : MonoBehaviour
{

    private TextAsset textFile;
    private char lineReturn = '\r';
    private char lineSeperator = '\n';
    private char surround = '"';
    private string[] fielSeperator = { "\",\"" };

    public void LoadCSV()
    {
        textFile = Resources.Load<TextAsset>("levelsInfos");
        Debug.Log("text init");
    }

    public List<string[]> GetLevelValues()
    {
        List<string[]> levelsInfos;

        string[] lines = textFile.text.Split(new char[] { lineReturn, lineSeperator }, StringSplitOptions.None);

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
}
