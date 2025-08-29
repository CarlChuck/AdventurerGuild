using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guild : MonoBehaviour
{
    #region Singleton
    public static Guild Instance;
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
        items = new();
        adventurers = new();
    }
    #endregion
    private string guildName;
    private int gold;
    private int goldMil;

    [SerializeField] private List<Item> items;
    [SerializeField] private List<Adventurer> adventurers;
    public Stat statPrefab;

    public void StartGuild(string gName)
    {
        ClearAdventurers();
        ClearItems();
        guildName = gName;
        gold = 1000;
        goldMil = 0;
        AddStartingAdventurers();
        AddStartingItems();
        Markets.Instance.InitialiseMarket();
        MissionManager.Instance.InitaliseMissions();
    }
    public void LoadSavedGuild()
    {
        if (SaveSystem.Instance != null && SaveSystem.Instance.HasSaveFile)
        {
            bool loadSuccess = SaveSystem.Instance.LoadGame();
            if (loadSuccess)
            {
                Debug.Log("Guild loaded from save file");
                // Initialize systems after loading
                Markets.Instance.InitialiseMarket();
                MissionManager.Instance.InitaliseMissions();
            }
            else
            {
                Debug.LogWarning("Failed to load save file, starting new guild");
                StartNewGuild();
            }
        }
        else
        {
            Debug.Log("No save file found, starting new guild");
            StartNewGuild();
        }
    }
    
    private void StartNewGuild()
    {
        guildName = "New Guild";
        gold = 1000;
        goldMil = 0;
        AddStartingAdventurers();
        AddStartingItems();
        Markets.Instance.InitialiseMarket();
        MissionManager.Instance.InitaliseMissions();
    }

    #region Setters
    public void SetGuildName(string newName)
    {
        guildName = newName;
        TriggerAutoSave();
    }
    public void SetStartingGold(int startGold)
    {
        gold = startGold;
        goldMil = 0;
    }
    
    public void SetGold(int newGold, int newGoldMil)
    {
        gold = newGold;
        goldMil = newGoldMil;
        UIManager.Instance?.SetGoldAmount(goldMil, gold);
    }
    #endregion
    #region Getters
    public string GetGuildName()
    {
        return guildName;
    }
    public int GetGold()
    {
        return gold;
    }
    public int GetGoldMil()
    {
        return goldMil;
    }
    public string GetGoldAsString()
    {
        return goldMil.ToString() + gold.ToString();
    }
    public List<Item> GetItems()
    {
        return items;
    }
    public List<Item> GetWeapons()
    {
        List<Item> weapList = new();
        foreach (Item item in items)
        {
            if (item.GetItemType() == ItemType.Weapon)
            {
                weapList.Add(item);
            }
        }
        return weapList;
    }
    public List<Item> GetOutfits()
    {
        List<Item> outfitList = new();
        foreach (Item item in items)
        {
            if (item.GetItemType() == ItemType.Outfit)
            {
                outfitList.Add(item);
            }
        }
        return outfitList;
    }
    public List<Item> GetAccessories()
    {
        List<Item> accessList = new();
        foreach (Item item in items)
        {
            if (item.GetItemType() == ItemType.Accessory)
            {
                accessList.Add(item);
            }
        }
        return accessList;
    }
    public List<Adventurer> GetAdventurers()
    {
        return adventurers;
    }
    public int GetHighestLevelAdventurer()
    {
        int levelToReturn = 1;
        foreach (Adventurer adv in adventurers)
        {
            int advLevel = adv.GetLevel();
            if (advLevel > levelToReturn)
            {
                levelToReturn = advLevel;
            }
        }
        return levelToReturn;
    }
    
    public List<Adventurer> GetAdventurersOnActiveMissions()
    {
        List<Adventurer> busyAdventurers = new List<Adventurer>();
        
        if (MissionManager.Instance != null)
        {
            List<Mission> activeMissions = MissionManager.Instance.GetInProgressList();
            List<Mission> completedMissions = MissionManager.Instance.GetCompletedList();
            
            // Check in-progress missions
            foreach (Mission mission in activeMissions)
            {
                List<Adventurer> missionAdventurers = mission.GetAdventurersOnMission();
                foreach (Adventurer adventurer in missionAdventurers)
                {
                    if (adventurer != null && !busyAdventurers.Contains(adventurer))
                    {
                        busyAdventurers.Add(adventurer);
                    }
                }
            }
            
            // Check completed missions (adventurers may still be "returning")
            foreach (Mission mission in completedMissions)
            {
                List<Adventurer> missionAdventurers = mission.GetAdventurersOnMission();
                foreach (Adventurer adventurer in missionAdventurers)
                {
                    if (adventurer != null && !busyAdventurers.Contains(adventurer))
                    {
                        busyAdventurers.Add(adventurer);
                    }
                }
            }
        }
        
        return busyAdventurers;
    }
    
    public List<Adventurer> GetAvailableAdventurers()
    {
        List<Adventurer> availableAdventurers = new List<Adventurer>();
        List<Adventurer> busyAdventurers = GetAdventurersOnActiveMissions();
        
        foreach (Adventurer adventurer in adventurers)
        {
            if (!busyAdventurers.Contains(adventurer))
            {
                availableAdventurers.Add(adventurer);
            }
        }
        
        return availableAdventurers;
    }
    #endregion
    #region Adventurers
    public void AddAdventurer(Adventurer adventurer)
    {
        adventurers.Add(adventurer);
        adventurer.transform.SetParent(transform);
        TriggerAutoSave();
    }
    public void RemoveAdventurer(Adventurer adventurer)
    {
        adventurers.Remove(adventurer);
    }
    public void ClearAdventurers()
    {
        if (adventurers.Count > 0)
        {
            foreach (Adventurer adventurer in adventurers)
            {
                Destroy(adventurer.gameObject);
            }
            adventurers.Clear();
        }
    }
    private void AddStartingAdventurers()
    {
        int startingCharacters = 10;
        for (int i = 0; i < startingCharacters; i++)
        {
            Adventurer newAdventurer = CharGen.Instance.GenerateStarterCharacter(transform);
            adventurers.Add(newAdventurer);
        }
    }
    #endregion
    #region Inventory
    public void AddItemToInventory(Item item)
    {
        items.Add(item);
        item.transform.SetParent(transform);
        TriggerAutoSave();
    }
    public void RemoveItemFromInventory(Item item)
    {
        items.Remove(item);
    }
    private void AddStartingItems()
    {
        int startingItems = 5;
        for (int i = 0; i < startingItems; i++)
        {
            Item newItem = ItemGen.Instance.GenerateStarterItem(transform);
            items.Add(newItem);
        }
    }
    public void DestroyItem(Item item)
    {
        Destroy(item.gameObject);
    }
    public void ClearItems()
    {
        if (items.Count > 0)
        {
            foreach (Item item in items)
            {
                Destroy(item.gameObject);
            }
            items.Clear();
        }
    }
    #endregion
    #region Gold
    public void AddGold(int goldToAdd)
    {
        gold += goldToAdd;
        int mil = 1000000;
        if (gold > mil)
        {
            gold -= mil;
            goldMil += 1;
        }
        UIManager.Instance.SetGoldAmount(GetGoldMil(),GetGold());
    }
    public void RemoveGold(int goldToTake)
    {
        int mil = 1000000;
        if (goldMil > 0)
        {
            gold += mil;
            goldMil -= 1;
        }
        gold -= goldToTake;
        if (gold > mil)
        {
            gold -= mil;
            goldMil += 1;
        }
        UIManager.Instance.SetGoldAmount(GetGoldMil(), GetGold());
    }
    #endregion
    #region Mission Rewards
    public void DistributeMissionRewards(MissionRewards rewards)
    {
        // Add gold to guild treasury
        if (rewards.gold > 0)
        {
            AddGold(rewards.gold);
        }
        
        // Add experience to adventurers who participated
        foreach (Adventurer adventurer in rewards.adventurersToReward)
        {
            if (adventurer != null && rewards.experiencePerAdventurer > 0)
            {
                adventurer.AddExperience(rewards.experiencePerAdventurer);
            }
        }
        
        // Add items to guild inventory
        if (rewards.items != null)
        {
            foreach (Item item in rewards.items)
            {
                if (item != null)
                {
                    AddItemToInventory(item);
                }
            }
        }
        
        // TODO: Trigger UI notification for mission completion rewards
        TriggerAutoSave();
    }
    #endregion
    
    #region Save System Integration
    private void TriggerAutoSave()
    {
        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.SaveGame();
        }
    }
    
    public void ManualSave()
    {
        TriggerAutoSave();
    }
    #endregion
}
