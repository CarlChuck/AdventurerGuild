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
    private List<string> missionAdjectives;
    private List<string> missionNouns;
    private bool namesLoaded = false;
    private bool missionNamesLoaded = false;

    private void Start()
    {
        maleFirstNames = new List<string>();
        femaleFirstNames = new List<string>();
        lastNames = new List<string>();
        missionAdjectives = new List<string>();
        missionNouns = new List<string>();
        LoadAllNames();
    }
    private void LoadAllNames()
    {
        try
        {
            LoadNamesFromCSV("first_names", true);
            LoadNamesFromCSV("last_names", false);
            LoadMissionNamesFromCSV("mission_names");
            namesLoaded = true;
            missionNamesLoaded = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load name data: {e.Message}");
            namesLoaded = false;
            missionNamesLoaded = false;
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

    private void LoadMissionNamesFromCSV(string fileName)
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

            missionAdjectives.Add(values[0].Trim());
            missionNouns.Add(values[1].Trim());
        }
    }

    public string GenerateRandomMissionName(MissionType missionType, Quality quality)
    {
        if (!missionNamesLoaded || missionAdjectives.Count == 0 || missionNouns.Count == 0)
        {
            Debug.LogWarning("Mission names not loaded properly, using fallback");
            return "Unnamed Mission";
        }

        // Get quality-appropriate adjectives
        List<string> availableAdjectives = GetAdjectivesForQuality(quality);
        if (availableAdjectives.Count == 0)
        {
            availableAdjectives = missionAdjectives; // Fallback to all adjectives
        }

        // Get type-appropriate nouns
        List<string> availableNouns = GetNounsForMissionType(missionType);
        if (availableNouns.Count == 0)
        {
            availableNouns = missionNouns; // Fallback to all nouns
        }

        string adjective = availableAdjectives[Random.Range(0, availableAdjectives.Count)];
        string noun = availableNouns[Random.Range(0, availableNouns.Count)];
        return $"{adjective} {noun}";
    }

    private List<string> GetAdjectivesForQuality(Quality quality)
    {
        List<string> filtered = new List<string>();

        switch (quality)
        {
            case Quality.Common:
                // Simple, basic descriptors
                string[] commonAdjectives = { "Simple", "Local", "Common", "Basic", "Standard", "Routine", "Regular", "Normal", "Peaceful", "Quiet" };
                foreach (string adj in commonAdjectives)
                {
                    if (missionAdjectives.Contains(adj))
                        filtered.Add(adj);
                }
                break;

            case Quality.Uncommon:
                // Slightly more interesting
                string[] uncommonAdjectives = { "Hidden", "Secret", "Mysterious", "Silver", "Iron", "Stone" };
                foreach (string adj in uncommonAdjectives)
                {
                    if (missionAdjectives.Contains(adj))
                        filtered.Add(adj);
                }
                break;

            case Quality.Masterwork:
                // Notable and significant
                string[] masterworkAdjectives = { "Golden", "Crystal", "Sacred", "Noble", "Royal", "Mystic", "Enchanted", "Magical" };
                foreach (string adj in masterworkAdjectives)
                {
                    if (missionAdjectives.Contains(adj))
                        filtered.Add(adj);
                }
                break;

            case Quality.Rare:
                // Dangerous and important
                string[] rareAdjectives = { "Ancient", "Cursed", "Dark", "Perilous", "Dire", "Dangerous", "Grim", "Fierce", "Arcane" };
                foreach (string adj in rareAdjectives)
                {
                    if (missionAdjectives.Contains(adj))
                        filtered.Add(adj);
                }
                break;

            case Quality.Legendary:
                // Epic and legendary
                string[] legendaryAdjectives = { "Legendary", "Epic", "Forgotten", "Lost", "Shadow", "Twisted", "Burning", "Frozen", "Bloody", "Brutal" };
                foreach (string adj in legendaryAdjectives)
                {
                    if (missionAdjectives.Contains(adj))
                        filtered.Add(adj);
                }
                break;
        }

        return filtered;
    }

    private List<string> GetNounsForMissionType(MissionType missionType)
    {
        List<string> filtered = new List<string>();

        switch (missionType)
        {
            case MissionType.Dungeon:
                string[] dungeonNouns = { "Dungeon", "Crypt", "Cavern", "Tomb", "Ruins", "Halls", "Depths", "Labyrinth" };
                AddAvailableNouns(filtered, dungeonNouns);
                break;

            case MissionType.Patrol:
            case MissionType.Defence:
                string[] patrolNouns = { "Patrol", "Guard", "Fortress", "Keep", "Territory", "Battlefield" };
                AddAvailableNouns(filtered, patrolNouns);
                break;

            case MissionType.Healing:
            case MissionType.Cure:
            case MissionType.HerbGather:
                string[] healingNouns = { "Sanctuary", "Shrine", "Chapel", "Altar", "Haven", "Grove" };
                AddAvailableNouns(filtered, healingNouns);
                break;

            case MissionType.Diplomacy:
            case MissionType.Entertain:
            case MissionType.Networking:
                string[] socialNouns = { "Negotiation", "Mission", "Quest", "Task", "Command" };
                AddAvailableNouns(filtered, socialNouns);
                break;

            case MissionType.Stealth:
            case MissionType.Smuggling:
            case MissionType.Larceny:
                string[] stealthNouns = { "Operation", "Infiltration", "Reconnaissance", "Surveillance", "Espionage" };
                AddAvailableNouns(filtered, stealthNouns);
                break;

            case MissionType.MonsterHunt:
            case MissionType.BountyHunt:
            case MissionType.Scouting:
                string[] huntNouns = { "Hunt", "Expedition", "Tracking", "Wilderness", "Frontier", "Investigation" };
                AddAvailableNouns(filtered, huntNouns);
                break;

            case MissionType.Enchantment:
            case MissionType.SpiritQuest:
            case MissionType.Divination:
                string[] magicNouns = { "Tower", "Laboratory", "Study", "Chamber", "Realm", "Trial" };
                AddAvailableNouns(filtered, magicNouns);
                break;

            case MissionType.MiningExpedition:
            case MissionType.OreRefinement:
            case MissionType.WoodcuttingExpedition:
            case MissionType.CraftingOrder:
                string[] craftNouns = { "Workshop", "Commission", "Order", "Request", "Creation", "Project", "Mine" };
                AddAvailableNouns(filtered, craftNouns);
                break;
        }

        return filtered;
    }

    private void AddAvailableNouns(List<string> filtered, string[] nouns)
    {
        foreach (string noun in nouns)
        {
            if (missionNouns.Contains(noun))
                filtered.Add(noun);
        }
    }

    public bool AreMissionNamesLoaded()
    {
        return missionNamesLoaded;
    }

}
