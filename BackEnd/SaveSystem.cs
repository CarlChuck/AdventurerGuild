using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    #region Singleton
    public static SaveSystem Instance;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion
    
    private const string SaveFileName = "adventurer_guild_save.json";
    private const string BackupFileName = "adventurer_guild_save.bak";
    
    private string SaveFilePath => Path.Combine(Application.persistentDataPath, SaveFileName);
    private string BackupFilePath => Path.Combine(Application.persistentDataPath, BackupFileName);
    
    public bool HasSaveFile => File.Exists(SaveFilePath);
    
    #region Public Save/Load Methods
    
    public bool SaveGame()
    {
        try
        {
            // Create backup of existing save file
            if (File.Exists(SaveFilePath))
            {
                File.Copy(SaveFilePath, BackupFilePath, true);
            }
            
            // Collect all game data
            GameSaveData saveData = CollectGameData();
            
            // Serialize to JSON
            string json = JsonUtility.ToJson(saveData, true);
            
            // Write to file
            File.WriteAllText(SaveFilePath, json);
            
            Debug.Log($"Game saved successfully to {SaveFilePath}");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save game: {e.Message}");
            
            // Restore backup if save failed
            if (File.Exists(BackupFilePath))
            {
                try
                {
                    File.Copy(BackupFilePath, SaveFilePath, true);
                    Debug.Log("Restored backup save file");
                }
                catch (Exception backupException)
                {
                    Debug.LogError($"Failed to restore backup: {backupException.Message}");
                }
            }
            
            return false;
        }
    }
    
    public bool LoadGame()
    {
        if (!File.Exists(SaveFilePath))
        {
            Debug.LogWarning("No save file found");
            return false;
        }
        
        try
        {
            // Read save file
            string json = File.ReadAllText(SaveFilePath);
            
            // Deserialize from JSON
            GameSaveData saveData = JsonUtility.FromJson<GameSaveData>(json);
            
            // Validate save data
            if (!ValidateSaveData(saveData))
            {
                Debug.LogError("Save data validation failed");
                return false;
            }
            
            // Apply save data to game
            ApplyGameData(saveData);
            
            // Refresh UI after loading
            if (UIManager.Instance != null)
            {
                UIManager.Instance.RefreshMissionLists();
            }
            
            Debug.Log("Game loaded successfully");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load game: {e.Message}");
            return false;
        }
    }
    
    public void DeleteSave()
    {
        try
        {
            if (File.Exists(SaveFilePath))
            {
                File.Delete(SaveFilePath);
                Debug.Log("Save file deleted");
            }
            
            if (File.Exists(BackupFilePath))
            {
                File.Delete(BackupFilePath);
                Debug.Log("Backup file deleted");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to delete save files: {e.Message}");
        }
    }
    
    #endregion
    
    #region Data Collection
    
    private GameSaveData CollectGameData()
    {
        GameSaveData saveData = new GameSaveData
        {
            saveTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            guildData = CollectGuildData(),
            adventurers = CollectAdventurerData(),
            items = CollectItemData(),
            missions = CollectMissionData(),
            market = CollectMarketData()
        };
        
        return saveData;
    }
    
    private GuildSaveData CollectGuildData()
    {
        if (Guild.Instance == null) return new GuildSaveData();
        
        return new GuildSaveData
        {
            guildName = Guild.Instance.GetGuildName(),
            gold = Guild.Instance.GetGold(),
            goldMil = Guild.Instance.GetGoldMil()
        };
    }
    
    private List<AdventurerSaveData> CollectAdventurerData()
    {
        List<AdventurerSaveData> adventurerData = new List<AdventurerSaveData>();
        
        if (Guild.Instance == null) return adventurerData;
        
        foreach (Adventurer adventurer in Guild.Instance.GetAdventurers())
        {
            if (adventurer == null) continue;
            
            AdventurerSaveData data = new AdventurerSaveData
            {
                name = adventurer.GetName(),
                level = adventurer.GetLevel(),
                experience = adventurer.GetExperience(),
                professionName = adventurer.GetProfession() ?? "",
                speciesName = adventurer.GetSpecies() ?? "",
                stats = new StatsSaveData
                {
                    combat = adventurer.GetCombat(),
                    healing = adventurer.GetHealing(),
                    social = adventurer.GetSocial(),
                    subterfuge = adventurer.GetSubterfuge(),
                    hunting = adventurer.GetHunting(),
                    magic = adventurer.GetMagic(),
                    craft = adventurer.GetCraft()
                },
                equipment = new EquipmentSaveData
                {
                    weaponId = adventurer.GetWeapon()?.GetComponent<UniqueId>()?.Id,
                    outfitId = adventurer.GetOutfit()?.GetComponent<UniqueId>()?.Id,
                    accessoryId = adventurer.GetAccessory()?.GetComponent<UniqueId>()?.Id
                }
            };
            
            adventurerData.Add(data);
        }
        
        return adventurerData;
    }
    
    private List<ItemSaveData> CollectItemData()
    {
        List<ItemSaveData> itemData = new List<ItemSaveData>();
        
        if (Guild.Instance == null) return itemData;
        
        foreach (Item item in Guild.Instance.GetItems())
        {
            if (item == null) continue;
            
            UniqueId uniqueId = item.GetComponent<UniqueId>();
            if (uniqueId == null)
            {
                // Add UniqueId component if missing
                uniqueId = item.gameObject.AddComponent<UniqueId>();
            }
            
            ItemSaveData data = new ItemSaveData
            {
                id = uniqueId.Id,
                name = item.GetItemName(),
                itemType = item.GetItemType().ToString(),
                quality = item.GetItemQuality().ToString(),
                value = item.GetGoldValue(),
                stats = new StatsSaveData
                {
                    combat = item.GetCombat(),
                    healing = item.GetHealing(),
                    social = item.GetSocial(),
                    subterfuge = item.GetSubterfuge(),
                    hunting = item.GetHunting(),
                    magic = item.GetMagic(),
                    craft = item.GetCraft()
                },
                isEquipped = item.IsEquipped(),
                equippedByAdventurer = item.GetEquippedByAdventurer()
            };
            
            itemData.Add(data);
        }
        
        return itemData;
    }
    
    private MissionSaveData CollectMissionData()
    {
        MissionSaveData missionData = new MissionSaveData
        {
            availableMissions = new List<MissionInstanceSaveData>(),
            inProgressMissions = new List<MissionInstanceSaveData>(),
            completedMissions = new List<MissionInstanceSaveData>()
        };
        
        if (MissionManager.Instance == null) return missionData;
        
        // Collect available missions
        foreach (Mission mission in MissionManager.Instance.GetTaskList())
        {
            if (mission != null)
            {
                missionData.availableMissions.Add(CreateMissionInstanceData(mission));
            }
        }
        
        // Collect in-progress missions
        foreach (Mission mission in MissionManager.Instance.GetInProgressList())
        {
            if (mission != null)
            {
                missionData.inProgressMissions.Add(CreateMissionInstanceData(mission));
            }
        }
        
        // Collect completed missions (limit to last 20)
        List<Mission> completedMissions = MissionManager.Instance.GetCompletedList();
        int startIndex = Mathf.Max(0, completedMissions.Count - 20);
        for (int i = startIndex; i < completedMissions.Count; i++)
        {
            Mission mission = completedMissions[i];
            if (mission != null)
            {
                missionData.completedMissions.Add(CreateMissionInstanceData(mission));
            }
        }
        
        return missionData;
    }
    
    private MissionInstanceSaveData CreateMissionInstanceData(Mission mission)
    {
        MissionInstanceSaveData data = new MissionInstanceSaveData
        {
            missionName = mission.GetMissionName(),
            missionLevel = mission.GetMissionLevel(),
            quality = mission.GetMissionQuality().ToString(),
            goldValue = mission.GetGoldValue(),
            maxAdventurers = mission.GetMaxAdventurers(),
            missionTime = mission.GetMissionTime(),
            state = mission.GetMissionState().ToString(),
            startTimestamp = mission.GetStartTimestamp(),
            assignedAdventurerNames = new List<string>()
        };
        
        // Get mission stats
        mission.GetStats(out int combat, out int healing, out int social, out int subterfuge, 
                        out int hunting, out int magic, out int craft, out IfCrafter craftType);
        
        data.stats = new StatsSaveData
        {
            combat = combat,
            healing = healing,
            social = social,
            subterfuge = subterfuge,
            hunting = hunting,
            magic = magic,
            craft = craft
        };
        
        // Get assigned adventurers
        foreach (Adventurer adventurer in mission.GetAdventurersOnMission())
        {
            if (adventurer != null)
            {
                data.assignedAdventurerNames.Add(adventurer.GetName());
            }
        }
        
        return data;
    }
    
    private MarketSaveData CollectMarketData()
    {
        // TODO: Implement when Markets system is expanded
        return new MarketSaveData
        {
            lastRefreshTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            refreshIntervalMinutes = 30
        };
    }
    
    #endregion
    
    #region Data Application
    
    private void ApplyGameData(GameSaveData saveData)
    {
        ApplyGuildData(saveData.guildData);
        ApplyAdventurerData(saveData.adventurers);
        ApplyItemData(saveData.items);
        RestoreEquipmentAssignments(saveData.adventurers);
        ApplyMissionData(saveData.missions);
        ApplyMarketData(saveData.market);
    }
    
    private void ApplyGuildData(GuildSaveData data)
    {
        if (Guild.Instance == null || data == null) return;
        
        Guild.Instance.SetGuildName(data.guildName ?? "Unknown Guild");
        Guild.Instance.SetGold(data.gold, data.goldMil);
    }
    
    private void ApplyAdventurerData(List<AdventurerSaveData> data)
    {
        if (data == null || Guild.Instance == null || CharGen.Instance == null) return;
        
        Guild.Instance.ClearAdventurers();
        
        foreach (AdventurerSaveData adventurerData in data)
        {
            if (adventurerData == null) continue;
            
            Adventurer recreatedAdventurer = CharGen.Instance.RecreateAdventurer(adventurerData, Guild.Instance.transform);
            if (recreatedAdventurer != null)
            {
                Guild.Instance.AddAdventurer(recreatedAdventurer);
            }
        }
    }
    
    private void ApplyItemData(List<ItemSaveData> data)
    {
        if (data == null || Guild.Instance == null || ItemGen.Instance == null) return;
        
        Guild.Instance.ClearItems();
        
        foreach (ItemSaveData itemData in data)
        {
            if (itemData == null) continue;
            
            Item recreatedItem = ItemGen.Instance.RecreateItem(itemData, Guild.Instance.transform);
            if (recreatedItem != null)
            {
                Guild.Instance.AddItemToInventory(recreatedItem);
            }
        }
    }
    
    private void ApplyMissionData(MissionSaveData data)
    {
        if (data == null || MissionManager.Instance == null) return;
        
        MissionManager.Instance.RestoreMissionLists(data);
    }
    
    private void ApplyMarketData(MarketSaveData data)
    {
        // TODO: Implement when Markets system is expanded
    }
    
    private void RestoreEquipmentAssignments(List<AdventurerSaveData> adventurerData)
    {
        if (adventurerData == null || Guild.Instance == null) return;
        
        foreach (AdventurerSaveData adventurerSave in adventurerData)
        {
            if (adventurerSave?.equipment == null) continue;
            
            Adventurer adventurer = FindAdventurerByName(adventurerSave.name);
            if (adventurer == null) continue;
            
            if (!string.IsNullOrEmpty(adventurerSave.equipment.weaponId))
            {
                Item weapon = FindItemById(adventurerSave.equipment.weaponId);
                if (weapon != null && weapon.GetItemType() == ItemType.Weapon)
                {
                    adventurer.EquipWeapon(weapon);
                }
            }
            
            if (!string.IsNullOrEmpty(adventurerSave.equipment.outfitId))
            {
                Item outfit = FindItemById(adventurerSave.equipment.outfitId);
                if (outfit != null && outfit.GetItemType() == ItemType.Outfit)
                {
                    adventurer.EquipOutfit(outfit);
                }
            }
            
            if (!string.IsNullOrEmpty(adventurerSave.equipment.accessoryId))
            {
                Item accessory = FindItemById(adventurerSave.equipment.accessoryId);
                if (accessory != null && accessory.GetItemType() == ItemType.Accessory)
                {
                    adventurer.EquipAccessory(accessory);
                }
            }
        }
    }
    
    private Adventurer FindAdventurerByName(string name)
    {
        if (string.IsNullOrEmpty(name) || Guild.Instance == null) return null;
        
        foreach (Adventurer adventurer in Guild.Instance.GetAdventurers())
        {
            if (adventurer != null && adventurer.GetName() == name)
                return adventurer;
        }
        return null;
    }
    
    private Item FindItemById(string id)
    {
        if (string.IsNullOrEmpty(id) || Guild.Instance == null) return null;
        
        foreach (Item item in Guild.Instance.GetItems())
        {
            if (item != null)
            {
                UniqueId uniqueId = item.GetComponent<UniqueId>();
                if (uniqueId != null && uniqueId.Id == id)
                    return item;
            }
        }
        return null;
    }
    
    #endregion
    
    #region Validation
    
    private bool ValidateSaveData(GameSaveData saveData)
    {
        if (saveData == null) return false;
        if (saveData.guildData == null) return false;
        if (saveData.adventurers == null) return false;
        if (saveData.items == null) return false;
        if (saveData.missions == null) return false;
        if (saveData.market == null) return false;
        
        return true;
    }
    
    #endregion
}

// Serializable data structures for save system
[System.Serializable]
public class GameSaveData
{
    public long saveTimestamp;
    public GuildSaveData guildData;
    public List<AdventurerSaveData> adventurers;
    public List<ItemSaveData> items;
    public MissionSaveData missions;
    public MarketSaveData market;
}

[System.Serializable]
public class GuildSaveData
{
    public string guildName;
    public int gold;
    public int goldMil;
}

[System.Serializable]
public class AdventurerSaveData
{
    public string name;
    public int level;
    public int experience;
    public string professionName;
    public string speciesName;
    public StatsSaveData stats;
    public EquipmentSaveData equipment;
}

[System.Serializable]
public class ItemSaveData
{
    public string id;
    public string name;
    public string itemType; // Weapon, Outfit, Accessory
    public string quality; // Common, Uncommon, Masterwork, Rare, Legendary
    public int value;
    public StatsSaveData stats;
    public bool isEquipped;
    public string equippedByAdventurer; // Name of adventurer who has this equipped
}

[System.Serializable]
public class StatsSaveData
{
    public int combat;
    public int healing;
    public int social;
    public int subterfuge;
    public int hunting;
    public int magic;
    public int craft;
}

[System.Serializable]
public class EquipmentSaveData
{
    public string weaponId;
    public string outfitId;
    public string accessoryId;
}

[System.Serializable]
public class MissionSaveData
{
    public List<MissionInstanceSaveData> availableMissions;
    public List<MissionInstanceSaveData> inProgressMissions;
    public List<MissionInstanceSaveData> completedMissions;
}

[System.Serializable]
public class MissionInstanceSaveData
{
    public string missionName;
    public int missionLevel;
    public string quality;
    public int goldValue;
    public int maxAdventurers;
    public int missionTime;
    public string state; // Available, InProgress, Completed
    public long startTimestamp; // Unix timestamp when mission started
    public List<string> assignedAdventurerNames;
    public StatsSaveData stats;
}

[System.Serializable]
public class MarketSaveData
{
    public long lastRefreshTimestamp;
    public int refreshIntervalMinutes;
    // TODO: Add specific market data when Markets system is expanded
}
