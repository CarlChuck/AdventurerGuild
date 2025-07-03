using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NameGen : MonoBehaviour
{
    private List<string> firstNames;
    private List<string> lastNames;

    private void Start()
    {
        firstNames = new List<string>();
        lastNames = new List<string>();
    }
    private void LoadNamesFromCSV(string filePath)
    {
        using StreamReader reader = new StreamReader(filePath);
        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            string[] values = line.Split(',');
            if (values.Length != 2)
            {
                continue;
            }

            firstNames.Add(values[0].Trim());
            lastNames.Add(values[1].Trim());
        }
    }

    private string GenerateRandomName()
    {
        if (firstNames.Count == 0 || lastNames.Count == 0)
        {
            return "No names loaded";
        }

        string firstName = firstNames[Random.Range(0, firstNames.Count)];
        string lastName = lastNames[Random.Range(0, lastNames.Count)];
        return firstName + " " + lastName;
    }

}
