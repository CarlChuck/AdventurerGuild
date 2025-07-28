using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NameGen : MonoBehaviour
{
    #region Singleton
    public static NameGen Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    private List<string> maleFirstNames;
    private List<string> femaleFirstNames;
    private List<string> lastNames;
    private bool namesLoaded = false;

    private void Start()
    {
        maleFirstNames = new List<string>();
        femaleFirstNames = new List<string>();
        lastNames = new List<string>();
        LoadAllNames();
    }
    private void LoadAllNames()
    {
        try
        {
            LoadNamesFromCSV("first_names", true);
            LoadNamesFromCSV("last_names", false);
            namesLoaded = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load name data: {e.Message}");
            namesLoaded = false;
        }
    }

    private void LoadNamesFromCSV(string fileName, bool isFirstNames)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(fileName);
        if (csvFile == null)
        {
            Debug.LogError($"Could not find {fileName}.csv in Resources folder");
            return;
        }

        string[] lines = csvFile.text.Split('\n');
        foreach (string line in lines)
        {
            if (string.IsNullOrEmpty(line.Trim()))
            {
                continue;
            }

            string[] values = line.Split(',');
            if (values.Length != 2)
            {
                continue;
            }

            if (isFirstNames)
            {
                maleFirstNames.Add(values[0].Trim());
                femaleFirstNames.Add(values[1].Trim());
            }
            else
            {
                lastNames.Add(values[0].Trim());
                lastNames.Add(values[1].Trim());
            }
        }
    }

    public string GenerateRandomName(bool isMale = true)
    {
        if (!namesLoaded || lastNames.Count == 0)
        {
            Debug.LogWarning("Names not loaded properly, using fallback");
            return "Unnamed Adventurer";
        }

        List<string> firstNameList = isMale ? maleFirstNames : femaleFirstNames;
        if (firstNameList.Count == 0)
        {
            Debug.LogWarning($"No {(isMale ? "male" : "female")} first names loaded");
            return "Unnamed Adventurer";
        }

        string firstName = firstNameList[Random.Range(0, firstNameList.Count)];
        string lastName = lastNames[Random.Range(0, lastNames.Count)];
        return firstName + " " + lastName;
    }

    public bool AreNamesLoaded()
    {
        return namesLoaded;
    }

}
