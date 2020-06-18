using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalisationSystem : MonoBehaviour
{

    public enum LANGUAGE { English, French }
    public static LANGUAGE language;

    private static Dictionary<string, string> localisedEN;
    private static Dictionary<string, string> localisedFR;

    public static bool isInit;

    public static CSVLoader csvLoader;

    private void Awake()
    {
        if (!isInit)
        {
            Init();
        }
    }

    public static void Init()
    {
        csvLoader = new CSVLoader();
        csvLoader.LoadCSV();


        UpdateDictionaries();

        language = (LANGUAGE) PlayerData.instance.language;

        isInit = true;
    }

    public static void UpdateDictionaries()
    {
        localisedEN = csvLoader.GetDictionaryValues("en");
        localisedFR = csvLoader.GetDictionaryValues("fr");
    }

    public static void SwitchLanguage(string lang)
    {
        switch (lang)
        {
            case "en" :
                language = LANGUAGE.English;
                PlayerData.instance.language = 0;
                break;
            case "fr":
                language = LANGUAGE.French;
                PlayerData.instance.language = 1;
                break;
            default:
                break;
        }
        TextLocaliserUI[] texts = UIManager.uiManager.gameObject.GetComponentsInChildren<TextLocaliserUI>();
        foreach(TextLocaliserUI text in texts)
        {
            text.UpdateText();
        }
    }

    public static Dictionary<string, string> GetDictionaryForEditor()
    {
        if (!isInit)
        {
            Init();
        }

        return localisedEN;
    }

    public static string GetLocalisedValue(string key)
    {
        if (!isInit)
        {
            Init();
        }

        string value = key;

        switch (language)
        {
            case LANGUAGE.English:
                localisedEN.TryGetValue(key, out value);
                break;
            case LANGUAGE.French:
                localisedFR.TryGetValue(key, out value);
                break;
        }

        return value;
    }
#if UNITY_EDITOR
    public static void Add(string key, string value)
    {
        if (value.Contains("\""))
        {
            value.Replace('"', '\"');
        }

        if (csvLoader == null)
        {
            csvLoader = new CSVLoader();
        }

        csvLoader.LoadCSV();
        csvLoader.Add(key, value);
        csvLoader.LoadCSV();

        UpdateDictionaries();
    }

    public static void Replace(string key, string value)
    {
        if (value.Contains("\""))
        {
            value.Replace('"', '\"');
        }

        if (csvLoader == null)
        {
            csvLoader = new CSVLoader();
        }

        csvLoader.LoadCSV();
        csvLoader.Edit(key, value);
        csvLoader.LoadCSV();

        UpdateDictionaries();
    }

    public static void Remove(string key)
    {
        if (csvLoader == null)
        {
            csvLoader = new CSVLoader();
        }

        csvLoader.LoadCSV();
        csvLoader.Remove(key);
        csvLoader.LoadCSV();

        UpdateDictionaries();
    }
#endif
}
