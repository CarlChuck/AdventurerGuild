using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission : MonoBehaviour
{
    private string missionName;
    private MissionSO missionBase;
    private MissionType missionType;
    private int missionLevel;
    private Quality missionRarity;
    private int difficultyValue;
    private int combat;
    private int healing;
    private int social;
    private int subterfuge;
    private int hunting;
    private int magic;
    private int craft;
    private IfCrafter craftType;
    private int goldValue;
    private List<Item> itemRewards;
    private int maxAdventurers;
    private List<Adventurer> adventurersOnMission;
    private Adventurer adventurerSlot1;
    private Adventurer adventurerSlot2;
    private Adventurer adventurerSlot3;
    private Adventurer adventurerSlot4;
    private int missionTime;
    private MissionState missionState;
    private float startTime;
    private bool hasStarted;

    #region Setters
    public void SetMission(MissionSO theMission)
    {
        missionBase = theMission;
        missionType = theMission.mType;
        SetMissionName();
        SetMissionLevel();
        SetMissionQuality();
        SetMaxAdventurers();
        SetMissionStats();
        SetMissionRewards();
        CalculateDifficultyValue();
        SetMissionTime();
        craftType = theMission.ifCrafter;
        missionState = MissionState.Available;
        adventurersOnMission = new();
        adventurerSlot1 = null;
        adventurerSlot2 = null;
        adventurerSlot3 = null;
        adventurerSlot4 = null;
    }
    private void SetMissionQuality()
    {
        int rand = Random.Range(1, 1001);
        if (rand <= 700)
        {
            missionRarity = Quality.Common;
        }
        else if (rand <= 900)
        {
            missionRarity = Quality.Uncommon;
        }
        else if (rand <= 975)
        {
            missionRarity = Quality.Masterwork;
        }
        else if (rand <= 995)
        {
            missionRarity = Quality.Rare;
        }
        else
        {
            missionRarity = Quality.Legendary;
        }
    }
    private void SetMissionLevel()
    {
        int topAdvLevel = Guild.Instance.GetHighestLevelAdventurer();
        if (topAdvLevel > 1)
        {
            missionLevel = Random.Range(1, topAdvLevel + 1);
        }
        else
        {
            missionLevel = 1;
        }
    }
    private void CalculateDifficultyValue()
    {
        difficultyValue = combat + healing + social + subterfuge + hunting + magic + craft;
    }
    private void SetMissionName()
    {
        if (NameGen.Instance != null && NameGen.Instance.AreMissionNamesLoaded())
        {
            missionName = NameGen.Instance.GenerateRandomMissionName(missionType, missionRarity);
        }
        else
        {
            missionName = "Unnamed Mission";
        }
    }
    private void SetMissionStats()
    {
        float multiplier = 0;
        switch (missionRarity)
        {
            case Quality.Common:
                multiplier = 1;
                break;
            case Quality.Uncommon:
                multiplier = 1.2f;
                break;
            case Quality.Masterwork:
                multiplier = 1.5f;
                break;
            case Quality.Rare:
                multiplier = 2f;
                break;
            case Quality.Legendary:
                multiplier = 3f;
                break;
        }
        combat = (int)(GetStatBasedOnStartingValue(missionBase.combat) * missionLevel * maxAdventurers * multiplier);
        healing = (int)(GetStatBasedOnStartingValue(missionBase.healing) * missionLevel * maxAdventurers * multiplier);
        social = (int)(GetStatBasedOnStartingValue(missionBase.social) * missionLevel * maxAdventurers * multiplier);
        subterfuge = (int)(GetStatBasedOnStartingValue(missionBase.subterfuge) * missionLevel * maxAdventurers * multiplier);
        hunting = (int)(GetStatBasedOnStartingValue(missionBase.hunting) * missionLevel * maxAdventurers * multiplier);
        magic = (int)(GetStatBasedOnStartingValue(missionBase.magic) * missionLevel * maxAdventurers * multiplier);
        craft = (int)(GetStatBasedOnStartingValue(missionBase.craft) * missionLevel * maxAdventurers * multiplier);
    }
    private void SetMissionRewards()
    {
        // Calculate base gold reward based on difficulty and quality
        float qualityMultiplier = missionRarity switch
        {
            Quality.Common => 1f,
            Quality.Uncommon => 1.5f,
            Quality.Masterwork => 2f,
            Quality.Rare => 3f,
            Quality.Legendary => 5f,
            _ => 1f
        };
        
        goldValue = (int)(difficultyValue * qualityMultiplier * 10); // Base gold calculation
        itemRewards = new List<Item>(); // Initialize empty, rewards added on completion
    }
    private void SetMaxAdventurers()
    {
        int rand = Random.Range(1, 11);
        if (missionType == MissionType.Dungeon || missionType == MissionType.Patrol || missionType == MissionType.Defence || missionType == MissionType.MonsterHunt 
            || missionType == MissionType.MiningExpedition || missionType == MissionType.WoodcuttingExpedition)
        {
            if (rand < 3)
            {
                maxAdventurers = 2;
            }
            else if (rand < 7)
            {
                maxAdventurers = 3;
            }
            else
            {
                maxAdventurers = 4;
            }
        }
        else if (missionType == MissionType.CraftingOrder)
        {
            maxAdventurers = 1;
        }
        else
        {
            if (rand < 8)
            {
                maxAdventurers = 1;
            }
            else
            {
                maxAdventurers = 2;
            }
        }
    }
    private void SetMissionTime()
    {
        // If ScriptableObject has a mission time set, use it, otherwise assign defaults
        missionTime = missionBase.missionTime > 0 ? missionBase.missionTime : GetBaseMissionDuration();
                    
        // Scale duration based on mission level and rarity
        float difficultyMultiplier = 1.0f + (missionLevel * 0.1f);
        float rarityMultiplier = missionRarity switch
        {
            Quality.Common => 1.0f,
            Quality.Uncommon => 1.2f,
            Quality.Masterwork => 1.5f,
            Quality.Rare => 2.0f,
            Quality.Legendary => 3.0f,
            _ => 1.0f
        };
            
        missionTime = (int)(missionTime * difficultyMultiplier * rarityMultiplier);
        missionTime = Random.Range((int)(missionTime * 0.7f), (int)(missionTime * 1.3f)); // Randomize time by 2 minutes
    }
    
    private int GetBaseMissionDuration()
    {
        // Base durations in seconds for different mission types
        return missionType switch
        {
            // Quick missions (2-5 minutes)
            MissionType.CraftingOrder => Random.Range(120, 300),
            MissionType.HerbGather => Random.Range(180, 300),
            MissionType.Cure => Random.Range(120, 240),
            
            // Medium missions (5-10 minutes)  
            MissionType.Healing => Random.Range(300, 600),
            MissionType.Entertain => Random.Range(240, 480),
            MissionType.Diplomacy => Random.Range(360, 600),
            MissionType.Networking => Random.Range(300, 540),
            MissionType.Stealth => Random.Range(300, 600),
            MissionType.Smuggling => Random.Range(420, 720),
            MissionType.Larceny => Random.Range(240, 480),
            MissionType.Enchantment => Random.Range(360, 600),
            MissionType.Divination => Random.Range(300, 540),
            MissionType.OreRefinement => Random.Range(300, 600),
            
            // Long missions (8-15 minutes)
            MissionType.Patrol => Random.Range(480, 900),
            MissionType.Defence => Random.Range(600, 900),
            MissionType.Dungeon => Random.Range(600, 900),
            MissionType.MonsterHunt => Random.Range(540, 840),
            MissionType.BountyHunt => Random.Range(480, 780),
            MissionType.Scouting => Random.Range(420, 720),
            MissionType.SpiritQuest => Random.Range(600, 900),
            MissionType.MiningExpedition => Random.Range(540, 840),
            MissionType.WoodcuttingExpedition => Random.Range(480, 780),
            
            // Default fallback
            _ => Random.Range(300, 600)
        };
    }
    #endregion
    #region Getters
    public int GetStatBasedOnStartingValue(int valueToPass)
    {
        int valueToExit = valueToPass switch
        {
            1 => Random.Range(1, 4),
            2 => Random.Range(2, 6),
            3 => Random.Range(3, 8),
            4 => Random.Range(4, 10),
            5 => Random.Range(5, 12),
            _ => 1
        };
        return valueToExit;
    }
    public string GetMissionName()
    {
        return missionName;
    }
    public int GetMissionDifficultyValue()
    {
        return difficultyValue;
    }
    public Quality GetMissionQuality()
    {
        return missionRarity;
    }
    public MissionType GetMissionType()
    {
        return missionType;
    }
    public int GetMissionLevel()
    {
        return missionLevel;
    }
    public MissionState GetMissionState()
    {
        return missionState;
    }
    public void GetStats(out int OutCombat, out int OutHealing, out int OutSocial, out int OutSubterfuge, out int OutHunting, out int OutMagic, out int OutCraft, out IfCrafter OutCraftType)
    {
        OutCombat = combat;
        OutHealing = healing;
        OutSocial = social;
        OutSubterfuge = subterfuge;
        OutHunting = hunting;
        OutMagic = magic;
        OutCraft = craft;
        OutCraftType = craftType;
    }
    public int GetGoldValue()
    {
        return goldValue;
    }
    public List<Item> GetRewards()
    {
        return itemRewards;
    }
    public List<Adventurer> GetAdventurersOnMission()
    {
        return adventurersOnMission;
    }
    public int GetMaxAdventurers()
    {
        return maxAdventurers;
    }
    public int GetMissionTime()
    {
        return missionTime;
    }
    #endregion
    #region MissionProgression
    public void AssignAdventurer(Adventurer adventurerToAssign)
    {
        if (missionState != MissionState.Available)
        {
            return;
        }

        if (adventurerSlot1 == null)
        {
            adventurerSlot1 = adventurerToAssign;
        }
        else if (adventurerSlot2 == null)
        {
            adventurerSlot2 = adventurerToAssign;
        }
        else if (adventurerSlot3 == null)
        {
            adventurerSlot3 = adventurerToAssign;
        }
        else if (adventurerSlot4 == null)
        {
            adventurerSlot4 = adventurerToAssign;
        }
            
        // Update the adventurers list to keep it in sync
        UpdateAdventurersOnMissionList();
    }
    public void RemoveAdventurer(Adventurer adventurerToRemove)
    {
        if (missionState != MissionState.Available)
        {
            return;
        }

        if (adventurerSlot1 == adventurerToRemove)
        {
            adventurerSlot1 = null;
        }
        else if (adventurerSlot2 == adventurerToRemove)
        {
            adventurerSlot2 = null;
        }
        else if (adventurerSlot3 == adventurerToRemove)
        {
            adventurerSlot3 = null;
        }
        else if (adventurerSlot4 == adventurerToRemove)
        {
            adventurerSlot4 = null;
        }
            
        // Update the adventurers list to keep it in sync
        UpdateAdventurersOnMissionList();
    }
    public void BeginMission()
    {
        missionState = MissionState.InProgress;
        startTime = Time.time;
        hasStarted = true;
        // Update adventurers list from slots
        UpdateAdventurersOnMissionList();
    }
    
    public void BeginMissionWithTimestamp(long startTimestamp)
    {
        missionState = MissionState.InProgress;
        hasStarted = true;
        
        // Convert Unix timestamp back to Unity time
        long currentTimestamp = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        float elapsedRealTime = currentTimestamp - startTimestamp;
        startTime = Time.time - elapsedRealTime;
        
        // Update adventurers list from slots
        UpdateAdventurersOnMissionList();
        
        // Check if mission completed while game was closed
        if (GetRemainingTime() <= 0f)
        {
            OnMissionEnd();
        }
    }
    public void OnMissionEnd()
    {
        missionState = MissionState.Completed;
        // Note: Adventurers remain assigned to completed missions for reward distribution
        // They will be filtered out from available adventurers until mission is cleared from completed list
    }
    public float GetRemainingTime()
    {
        if (!hasStarted || missionState != MissionState.InProgress)
        {
            return 0f;
        }

        float elapsedTime = Time.time - startTime;
        float remainingTime = missionTime - elapsedTime;
        return Mathf.Max(0f, remainingTime);
    }
    
    public bool IsTimeExpired()
    {
        return hasStarted && missionState == MissionState.InProgress && GetRemainingTime() <= 0f;
    }
    
    public float GetElapsedTime()
    {
        if (!hasStarted)
        {
            return 0f;
        }

        return Time.time - startTime;
    }
    
    public long GetStartTimestamp()
    {
        if (!hasStarted || missionState != MissionState.InProgress)
        {
            return 0;
        }
        
        // Calculate actual start timestamp from current time and elapsed time
        long currentTimestamp = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        long actualStartTimestamp = currentTimestamp - (long)GetElapsedTime();
        return actualStartTimestamp;
    }
    
    private void UpdateAdventurersOnMissionList()
    {
        adventurersOnMission.Clear();
        if (adventurerSlot1 != null)
        {
            adventurersOnMission.Add(adventurerSlot1);
        }

        if (adventurerSlot2 != null)
        {
            adventurersOnMission.Add(adventurerSlot2);
        }

        if (adventurerSlot3 != null)
        {
            adventurersOnMission.Add(adventurerSlot3);
        }

        if (adventurerSlot4 != null)
        {
            adventurersOnMission.Add(adventurerSlot4);
        }
    }
    
    public void CalculateTeamStats(out int teamCombat, out int teamHealing, out int teamSocial, out int teamSubterfuge, out int teamHunting, out int teamMagic, out int teamCraft)
    {
        teamCombat = 0;
        teamHealing = 0;
        teamSocial = 0;
        teamSubterfuge = 0;
        teamHunting = 0;
        teamMagic = 0;
        teamCraft = 0;
        
        foreach (Adventurer adventurer in adventurersOnMission)
        {
            if (!adventurer)
            {
                continue;
            }

            teamCombat += adventurer.GetCombat();
            teamHealing += adventurer.GetHealing();
            teamSocial += adventurer.GetSocial();
            teamSubterfuge += adventurer.GetSubterfuge();
            teamHunting += adventurer.GetHunting();
            teamMagic += adventurer.GetMagic();
            teamCraft += adventurer.GetCraft();
        }
    }
    
    public int GetAssignedAdventurerCount()
    {
        return adventurersOnMission.Count;
    }
    
    public MissionResult CalculateMissionSuccess()
    {
        // Calculate team stats
        CalculateTeamStats(out int teamCombat, out int teamHealing, out int teamSocial, out int teamSubterfuge, out int teamHunting, out int teamMagic, out int teamCraft);
        
        float totalSuccessRate = 0f;
        int relevantStats = 0;
        
        // Check each stat requirement against team capabilities
        if (combat > 0)
        {
            totalSuccessRate += Mathf.Min(teamCombat / (float)combat, 2f);
            relevantStats++;
        }
        if (healing > 0)
        {
            totalSuccessRate += Mathf.Min(teamHealing / (float)healing, 2f);
            relevantStats++;
        }
        if (social > 0)
        {
            totalSuccessRate += Mathf.Min(teamSocial / (float)social, 2f);
            relevantStats++;
        }
        if (subterfuge > 0)
        {
            totalSuccessRate += Mathf.Min(teamSubterfuge / (float)subterfuge, 2f);
            relevantStats++;
        }
        if (hunting > 0)
        {
            totalSuccessRate += Mathf.Min(teamHunting / (float)hunting, 2f);
            relevantStats++;
        }
        if (magic > 0)
        {
            totalSuccessRate += Mathf.Min(teamMagic / (float)magic, 2f);
            relevantStats++;
        }
        if (craft > 0)
        {
            totalSuccessRate += Mathf.Min(teamCraft / (float)craft, 2f);
            relevantStats++;
        }
        
        // Calculate final success percentage
        float finalSuccessRate = relevantStats > 0 ? (totalSuccessRate / relevantStats) * 100f : 0f;
        
        // Determine success grade
        MissionGrade grade;
        if (finalSuccessRate >= 150f)
        {
            grade = MissionGrade.CriticalSuccess;
        }
        else if (finalSuccessRate >= 100f)
        {
            grade = MissionGrade.Success;
        }
        else if (finalSuccessRate >= 75f)
        {
            grade = MissionGrade.PartialSuccess;
        }
        else if (finalSuccessRate >= 50f)
        {
            grade = MissionGrade.Failure;
        }
        else
        {
            grade = MissionGrade.CriticalFailure;
        }

        return new MissionResult
        {
            grade = grade,
            successRate = finalSuccessRate,
            teamCombat = teamCombat,
            teamHealing = teamHealing,
            teamSocial = teamSocial,
            teamSubterfuge = teamSubterfuge,
            teamHunting = teamHunting,
            teamMagic = teamMagic,
            teamCraft = teamCraft
        };
    }
    
    public MissionRewards CalculateRewards(MissionResult result)
    {
        float goldMultiplier = result.grade switch
        {
            MissionGrade.CriticalSuccess => 1.5f,
            MissionGrade.Success => 1f,
            MissionGrade.PartialSuccess => 0.75f,
            MissionGrade.Failure => 0.25f,
            MissionGrade.CriticalFailure => 0f,
            _ => 0f
        };
        
        int rewardGold = (int)(goldValue * goldMultiplier);
        int experiencePerAdventurer = result.grade switch
        {
            MissionGrade.CriticalSuccess => missionLevel * 150,
            MissionGrade.Success => missionLevel * 100,
            MissionGrade.PartialSuccess => missionLevel * 75,
            MissionGrade.Failure => missionLevel * 25,
            MissionGrade.CriticalFailure => 0,
            _ => 0
        };
        
        // Determine item rewards based on success grade and mission quality
        List<Item> rewardItems = new List<Item>();
        bool shouldGrantItem = result.grade switch
        {
            MissionGrade.CriticalSuccess => true,
            MissionGrade.Success => Random.Range(0f, 1f) < 0.7f,
            MissionGrade.PartialSuccess => Random.Range(0f, 1f) < 0.4f,
            MissionGrade.Failure => false,
            MissionGrade.CriticalFailure => false,
            _ => false
        };
        
        if (shouldGrantItem)
        {
            // Generate item with quality based on mission success and rarity
            Quality itemQuality = DetermineRewardItemQuality(result.grade);
            // Note: Actual item generation would require ItemGen integration
            // For now, we'll just track that an item should be granted
        }
        
        return new MissionRewards
        {
            gold = rewardGold,
            experiencePerAdventurer = experiencePerAdventurer,
            items = rewardItems,
            adventurersToReward = new List<Adventurer>(adventurersOnMission)
        };
    }
    
    private Quality DetermineRewardItemQuality(MissionGrade grade)
    {
        float[] qualityChances = grade switch
        {
            MissionGrade.CriticalSuccess => new float[] { 0.2f, 0.3f, 0.3f, 0.15f, 0.05f }, // Higher chance for better items
            MissionGrade.Success => new float[] { 0.4f, 0.35f, 0.2f, 0.05f, 0.0f },
            MissionGrade.PartialSuccess => new float[] { 0.6f, 0.3f, 0.1f, 0.0f, 0.0f },
            _ => new float[] { 1.0f, 0.0f, 0.0f, 0.0f, 0.0f }
        };
        
        float roll = Random.Range(0f, 1f);
        float cumulative = 0f;
        
        for (int i = 0; i < qualityChances.Length; i++)
        {
            cumulative += qualityChances[i];
            if (roll <= cumulative)
            {
                return (Quality)i;
            }
        }
        
        return Quality.Common;
    }
    #endregion
}
public enum MissionType
{
    Dungeon, Patrol, Defence,
    Healing, Cure, HerbGather,
    Diplomacy, Entertain, Networking,
    Stealth, Smuggling, Larceny,
    MonsterHunt, BountyHunt, Scouting,
    Enchantment, SpiritQuest, Divination,
    MiningExpedition, OreRefinement, WoodcuttingExpedition, CraftingOrder
}
public enum MissionState { Available, InProgress, Completed}

public enum MissionGrade 
{ 
    CriticalFailure, 
    Failure, 
    PartialSuccess, 
    Success, 
    CriticalSuccess 
}

[System.Serializable]
public struct MissionResult
{
    public MissionGrade grade;
    public float successRate;
    public int teamCombat;
    public int teamHealing;
    public int teamSocial;
    public int teamSubterfuge;
    public int teamHunting;
    public int teamMagic;
    public int teamCraft;
}

[System.Serializable]
public struct MissionRewards
{
    public int gold;
    public int experiencePerAdventurer;
    public List<Item> items;
    public List<Adventurer> adventurersToReward;
}