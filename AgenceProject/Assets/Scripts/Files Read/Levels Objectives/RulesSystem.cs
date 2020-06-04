using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulesSystem : MonoBehaviour
{

    private static List<string[]> rulesSet;

    public static bool isInit;
    public static CSVLoader csvLoader;

    public static void Init()
    {
        csvLoader = new CSVLoader();
        csvLoader.LoadCSV();

        LoadStars();

        isInit = true;
    }

    public static void LoadStars()
    {
        rulesSet = new List<string[]>();
        rulesSet = csvLoader.GetLevelValues();
    }

    public static string GetLevelValue(int level, int star)
    {
        if (!isInit)
        {
            Init();
        }

        return rulesSet[level - 1][star - 1];
    }

    public static int GetLevelValueToInt(int level, int star)
    {
        if (!isInit)
        {
            Init();
        }

        return int.Parse(rulesSet[level - 1][star - 1]);
    }

}
